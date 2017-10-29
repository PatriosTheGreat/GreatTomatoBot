using System.Net;
using System.Text.RegularExpressions;
using TomatoBot.Model;

namespace TomatoBot.Services
{
	public abstract class BloombergCurrencyProvider : ICurrencyToAnotherRateProvider
	{
		public abstract Currency Source { get; }

		public abstract Currency Target { get; }
		
		public CurrencyRate GetRate()
		{
			var urlToCurrency = $"{BasePath}{Target}{Source}{PathPostfix}";

			var currencyString = new WebClient().DownloadString(urlToCurrency);
			var currencyGroup = PriceRegex.Match(currencyString);
			if (currencyGroup.Length < 2)
			{
				return new CurrencyRate(0, CurrencyRateDirection.None, Target);
			}

			return new CurrencyRate(
				double.Parse(currencyGroup.Groups[1].ToString()), 
				currencyString.Contains("price-container down") ? CurrencyRateDirection.Down : CurrencyRateDirection.None,
				Target);
		}

		private const string BasePath = "https://www.bloomberg.com/quote/";
		private const string PathPostfix = ":CUR";
		private static readonly Regex PriceRegex = new Regex("<div class=\"price\">([.0-9]+?)</div>");
	}
}