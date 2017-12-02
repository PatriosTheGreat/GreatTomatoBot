using System.Globalization;
using System.Net;
using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class BtcToUsdCurrencyProvider : ICurrencyToAnotherRateProvider
	{
		public Currency Source => Currency.BTC;

		public Currency Target => Currency.USD;

		public CurrencyRate GetRate() =>
			double.TryParse(new WebClient().DownloadString(BasePath), NumberStyles.Any, CultureInfo.InvariantCulture, out double result)
				? new CurrencyRate(1 / result, CurrencyRateDirection.None, Target)
				: new CurrencyRate(0, CurrencyRateDirection.None, Target);
		
		private const string BasePath = "https://blockchain.info/tobtc?currency=USD&value=1";
	}
}