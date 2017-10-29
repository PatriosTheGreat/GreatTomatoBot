using System;
using System.Linq;
using TomatoBot.Model;
using TomatoBot.Services;

namespace TomatoBot.BotCommands
{
    public sealed class GetCurrencyCommand : IBotCommand, ICommandWithHelpLine
    {
	    public GetCurrencyCommand()
	    {
		    var rubToUsdProvider = new RubToUsdCurrencyProvider();
			var usdToRubProvider = new UsdToRubCurrencyProvider();
			var rubToEurProvider = new RubToEurCurrencyProvider();
			var eurToRubProvider = new EurToRubCurrencyProvider();
			var usdToEurProvider = new UsdToEurCurrencyProvider();
			var eurToUsdProvider = new EurToUsdCurrencyProvider();
			var rubToGbpProvider = new RubToGbpCurrencyProvider();
			var gbpToRubProvider = new GbpToRubCurrencyProvider();
			var eurToGbpProvider = new EurToGbpCurrencyProvider();
			var gbpToEurProvider = new GbpToEurCurrencyProvider();
			var usdToGbpProvider = new UsdToGbpCurrencyProvider();
			var gbpToUsdProvider = new GbpToUsdCurrencyProvider();

		    _currencyToAnotherRateProviders = new ICurrencyToAnotherRateProvider[]
		    {
			    rubToUsdProvider,
			    usdToRubProvider,
			    rubToEurProvider,
			    eurToRubProvider,
			    usdToEurProvider,
			    eurToUsdProvider,
			    rubToGbpProvider,
			    gbpToRubProvider,
			    eurToGbpProvider,
			    gbpToEurProvider,
			    usdToGbpProvider,
			    gbpToUsdProvider
		    };

			_currencyRatingProviders = new ICurrencyRatingProvider[]
			{
				new CurrencyRatingProvider(Currency.RUB, rubToUsdProvider, rubToEurProvider, rubToGbpProvider),
				new CurrencyRatingProvider(Currency.USD, usdToRubProvider, usdToEurProvider, usdToGbpProvider),
				new CurrencyRatingProvider(Currency.EUR, eurToUsdProvider, eurToRubProvider, eurToGbpProvider),
				new CurrencyRatingProvider(Currency.GBP, gbpToUsdProvider, gbpToEurProvider, gbpToRubProvider), 
			};
	    }

        public bool CanExecute(MessageActivity activity) => 
			activity.IsMessageForBot() &&
				(TryGetCurrency(activity.Message, out Currency currency) || TryGetCurrencyToCurrency(activity.Message, out currency, out currency));
		
        public string CommandName => "getCurrency";

        public string Description => "отображает курс валюты";

        public string Sample => "/eur2rub";

        public string ExecuteAndGetResponse(MessageActivity activity)
        {
	        Currency sourceCurrency;
	        if (TryGetCurrency(activity.Message, out sourceCurrency))
	        {
		        var ratings = _currencyRatingProviders.Single(provider => provider.Source == sourceCurrency).GetRates();
		        return string.Join(ActivityExtension.NewLine, ratings.Select(GetRatingString));
	        }

	        Currency targetCurrency;
			if(TryGetCurrencyToCurrency(activity.Message, out sourceCurrency, out targetCurrency))
			{
				var rating =
					_currencyToAnotherRateProviders.Single(
						provider => provider.Source == sourceCurrency && provider.Target == targetCurrency).GetRate();
				return GetRatingString(rating);
			}

			return string.Empty;
        }

	    private static string GetRatingString(CurrencyRate rate)
	    {
			var postfix = rate.RateDirection == CurrencyRateDirection.Down ? "Снижение!" : "";
			return $"{CurrencySymbols.GetSymbol(rate.Target)} {rate.Rate:N3} {postfix}";
		}

	    private static bool TryGetCurrency(string currencyString, out Currency currency)
	    {
			return Enum.TryParse(currencyString.TrimStart('/').ToUpper(), out currency);
		}

		private static bool TryGetCurrencyToCurrency(string currencyString, out Currency source, out Currency target)
		{
			source = Currency.EUR;
			target = Currency.EUR;

			var parts = currencyString.Split('2');
			if (parts.Length != 2)
			{
				return false;
			}

			var sourceGotten = TryGetCurrency(parts[0], out source);
			var targetGotten = TryGetCurrency(parts[1], out target);
			return sourceGotten && targetGotten;
		}

		private readonly ICurrencyRatingProvider[] _currencyRatingProviders;
	    private readonly ICurrencyToAnotherRateProvider[] _currencyToAnotherRateProviders;
    }
}