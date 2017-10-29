using System.Linq;
using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class CurrencyRatingProvider : ICurrencyRatingProvider
	{
		public CurrencyRatingProvider(
			Currency source,
			params ICurrencyToAnotherRateProvider[] currencyToAnotherRateProviders)
		{
			Source = source;
			_currencyToAnotherRateProviders = currencyToAnotherRateProviders;
		}

		public Currency Source { get; }

		public CurrencyRate[] GetRates()
		{
			return _currencyToAnotherRateProviders.Select(provider => provider.GetRate()).ToArray();
		}

		private readonly ICurrencyToAnotherRateProvider[] _currencyToAnotherRateProviders;
	}
}