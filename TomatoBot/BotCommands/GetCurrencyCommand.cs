using System;
using System.Collections.Generic;
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
			
			var btcToUsdProvier = new BtcToUsdCurrencyProvider();

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
				gbpToUsdProvider,
				btcToUsdProvier
			};

			_currencyRatingProviders = new ICurrencyRatingProvider[]
			{
				new CurrencyRatingProvider(Currency.RUB, rubToUsdProvider, rubToEurProvider, rubToGbpProvider),
				new CurrencyRatingProvider(Currency.USD, usdToRubProvider, usdToEurProvider, usdToGbpProvider),
				new CurrencyRatingProvider(Currency.EUR, eurToUsdProvider, eurToRubProvider, eurToGbpProvider),
				new CurrencyRatingProvider(Currency.GBP, gbpToUsdProvider, gbpToEurProvider, gbpToRubProvider),
				new CurrencyRatingProvider(Currency.BTC, btcToUsdProvier) 
			};
		}

		public bool CanExecute(MessageActivity activity) =>
			activity.IsMessageForBot() && 
			new CurrencyRequest(activity.Message, _currencyToAnotherRateProviders, _currencyRatingProviders).CanProvider;

		public string CommandName => "getCurrency";

		public string Description => "отображает курс валюты";

		public string Sample => "/eur2rub";

		public string ExecuteAndGetResponse(MessageActivity activity)
		{
			var currencyRequest = new CurrencyRequest(activity.Message, _currencyToAnotherRateProviders, _currencyRatingProviders);
			return
				currencyRequest.IsFullInformationRequested
					? string.Join(ActivityExtension.NewLine, currencyRequest.CurrencyRatingProvider.GetRates().Select(GetRatingString))
					: GetRatingString(currencyRequest.CurrencyToAnotherRateProvider.GetRate());
		}

		private static string TrimBotName(string message)
		{
			var nameStart = message.IndexOf("@", StringComparison.Ordinal);
			return nameStart == -1 ? message : message.Substring(0, nameStart);
		}

		private static string GetRatingString(CurrencyRate rate)
		{
			var postfix = rate.RateDirection == CurrencyRateDirection.Down ? "Снижение!" : "";
			return $"{CurrencySymbols.GetSymbol(rate.Target)} {rate.Rate:N3} {postfix}";
		}

		private readonly ICurrencyRatingProvider[] _currencyRatingProviders;
		private readonly ICurrencyToAnotherRateProvider[] _currencyToAnotherRateProviders;

		private sealed class CurrencyRequest
		{
			public CurrencyRequest(
				string requestMessage, 
				IEnumerable<ICurrencyToAnotherRateProvider> currencyToAnotherRateProviders, 
				IEnumerable<ICurrencyRatingProvider> currencyRatingProviders)
			{
				var requestWihoutBotName = TrimBotInformation(requestMessage);
				var parts = requestWihoutBotName.Split('2');
				if (parts.Length != 2)
				{
					InitializeFullInformationRequest(requestWihoutBotName, currencyRatingProviders);
				}
				else
				{
					InitializeCurrencyoCurrencyRequest(parts[0], parts[1], currencyToAnotherRateProviders);
				}
			}

			public bool IsFullInformationRequested { get; private set; }

			public bool CanProvider { get; private set; }

			public ICurrencyToAnotherRateProvider CurrencyToAnotherRateProvider { get; private set; }

			public ICurrencyRatingProvider CurrencyRatingProvider { get; private set; }

			private void InitializeCurrencyoCurrencyRequest(string sourceRequest, string targetRequest, IEnumerable<ICurrencyToAnotherRateProvider> currencyToAnotherRateProviders)
			{
				IsFullInformationRequested = false;
				if (TryGetCurrency(sourceRequest, out Currency source) &&
						TryGetCurrency(targetRequest, out Currency target) &&
						TryInitializeCurrencyToCurrencyProvider(source, target, currencyToAnotherRateProviders))
				{
					CanProvider = true;
				}
				else
				{
					CanProvider = false;
				}
			}

			private void InitializeFullInformationRequest(string request, IEnumerable<ICurrencyRatingProvider> currencyRatingProviders)
			{
				IsFullInformationRequested = true;
				if (TryGetCurrency(request, out Currency source) && TryInitializeCurrencyRatingProvider(source, currencyRatingProviders))
				{
					CanProvider = true;
				}
				else
				{
					CanProvider = false;
				}
			}

			private static string TrimBotInformation(string request)
			{
				var nameStart = request.IndexOf("@", StringComparison.Ordinal);
				return (nameStart == -1 ? request : request.Substring(0, nameStart)).TrimStart('/');
			}

			private bool TryInitializeCurrencyRatingProvider(Currency source, IEnumerable<ICurrencyRatingProvider> currencyRatingProviders)
			{
				var selectedProvider = currencyRatingProviders.SingleOrDefault(provider => provider.Source == source);

				if (selectedProvider == null)
				{
					return false;
				}

				CurrencyRatingProvider = selectedProvider;
				return true;
			}

			private bool TryInitializeCurrencyToCurrencyProvider(
				Currency source, 
				Currency target, 
				IEnumerable<ICurrencyToAnotherRateProvider> currencyToAnotherRateProviders)
			{
				var selectedProvider =
					currencyToAnotherRateProviders.SingleOrDefault(provider => provider.Source == source && provider.Target == target);

				if (selectedProvider == null)
				{
					return false;
				}

				CurrencyToAnotherRateProvider = selectedProvider;
				return true;
			}

			private static bool TryGetCurrency(string currencyString, out Currency currency) => 
				Enum.TryParse(TrimBotName(currencyString).ToUpper(), out currency);
		}
	}
}