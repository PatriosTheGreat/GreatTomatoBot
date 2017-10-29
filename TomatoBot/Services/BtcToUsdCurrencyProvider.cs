using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class BtcToUsdCurrencyProvider : ICurrencyToAnotherRateProvider
	{
		public Currency Source => Currency.BTC;

		public Currency Target => Currency.USD;

		public CurrencyRate GetRate()
		{
			var jsonString = 
				new WebClient().DownloadString(
					$"{BasePath}?{StartParameter}={DateTime.UtcNow.AddDays(-10):yyyy-MM-dd}&{EndParameter}={DateTime.UtcNow:yyyy-MM-dd}");
			var responce = JsonConvert.DeserializeObject<CoinDeskResponce>(jsonString);

			var lastRating = responce.bpi.Last().Value;
			var beforeLastRating = responce.bpi.Reverse().Take(2).Last().Value;

			return new CurrencyRate(
				lastRating, 
				lastRating < beforeLastRating ? CurrencyRateDirection.Down : CurrencyRateDirection.None,
				Target);
		}

		private const string BasePath = "https://api.coindesk.com/v1/bpi/historical/close.json";
		private const string StartParameter = "start";
		private const string EndParameter = "end";

		private sealed class CoinDeskResponce
		{
			public Dictionary<DateTime, double> bpi { get; set; }
		}
	}
}