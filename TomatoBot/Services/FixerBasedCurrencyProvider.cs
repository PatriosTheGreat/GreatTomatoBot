using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using TomatoBot.Model;

namespace TomatoBot.Services
{
	public abstract class FixerBasedCurrencyProvider : ICurrencyToAnotherRateProvider
	{
		public abstract Currency Source { get; }

		public abstract Currency Target { get; }

		public CurrencyRate GetRate()
		{
			var latest = GetRate(DateTime.Now);
			var beforeLatest = GetRate(latest.Date.AddDays(-1));

			return new CurrencyRate(
				latest.Rate,
				latest.Rate < beforeLatest.Rate ? CurrencyRateDirection.Down : CurrencyRateDirection.None,
				Target);
		}

		private CurrencyResult GetRate(DateTime date)
		{
			var fixerResult = GetRateFromFixer(date);
			if (fixerResult.rates.ContainsKey(Source.ToString()) && fixerResult.rates.ContainsKey(Target.ToString()))
			{
				var sourceRate = fixerResult.rates[Source.ToString()];
				var targetRate = fixerResult.rates[Target.ToString()];
				return new CurrencyResult(sourceRate / targetRate, fixerResult.date);
			}

			if (fixerResult.rates.ContainsKey(Target.ToString()))
			{
				return new CurrencyResult(1 / fixerResult.rates[Target.ToString()], fixerResult.date);
			}
			
			return new CurrencyResult(fixerResult.rates.Last().Value, fixerResult.date);
		}

		private FixerResultJson GetRateFromFixer(DateTime date)
		{
			var jsonString = new WebClient().DownloadString($"{BasePath}/{date:yyyy-MM-dd}?{Symbols}={Target},{Source}");
			return JsonConvert.DeserializeObject<FixerResultJson>(jsonString);
		}

		private const string BasePath = "http://api.fixer.io/";
		private const string Symbols = "symbols";

		private class CurrencyResult
		{
			public CurrencyResult(double rate, DateTime date)
			{
				Rate = rate;
				Date = date;
			}

			public double Rate { get; }

			public DateTime Date { get; }
		}

		private class FixerResultJson
		{
			public Dictionary<string, double> rates { get; set; }

			public DateTime date { get; set; }
		}
	}
}