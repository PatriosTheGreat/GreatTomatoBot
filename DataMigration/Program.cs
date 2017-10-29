using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using TomatoBot.Model;
using TomatoBot.Repository;
using TomatoBot.Services;

namespace DataMigration
{
	class Program
	{
		static void Main(string[] args)
		{
			var userRepo = new UsersRepository();

			Console.WriteLine(userRepo.GetUserById(258).Identity);
			//var usersRepository = new UsersRepository();
		//	foreach(var data in dataSet)
		//	{
		//	}
			Currency currency;
			Console.WriteLine(GetRate().Rate);
			Console.WriteLine(GetRate().RateDirection);
			Console.WriteLine(TryGetCurrency("EUR", out currency));
			Console.WriteLine(currency);
			// Console.ReadLine();
			var jsonString =
				new WebClient().DownloadString(
					$"{BasePath2}?{StartParameter}={DateTime.UtcNow.AddDays(-10):yyyy-MM-dd}&{EndParameter}={DateTime.UtcNow:yyyy-MM-dd}");
					var responce = JsonConvert.DeserializeObject<CoinDeskResponce>(jsonString);

			var lastRating = responce.bpi.Last().Value;
			var beforeLastRating = responce.bpi.Reverse().Take(2).Last().Value;

			Console.WriteLine(lastRating);
			Console.WriteLine(beforeLastRating);
			Console.ReadLine();
		}

		private const string BasePath2 = "https://api.coindesk.com/v1/bpi/historical/close.json";
		private const string StartParameter = "start";
		private const string EndParameter = "end";

		private sealed class CoinDeskResponce
		{
			public Dictionary<DateTime, double> bpi { get; set; }
		}

		private static bool TryGetCurrency(string currencyString, out Currency currency)
		{
			return Enum.TryParse(currencyString.ToUpper(), out currency);
		}

		public static CurrencyRate GetRate()
		{
			var latest = GetRate(DateTime.Now);
			var beforeLatest = GetRate(latest.Date.AddDays(-1));

			return new CurrencyRate(
				latest.Rate,
				latest.Rate < beforeLatest.Rate ? CurrencyRateDirection.Down : CurrencyRateDirection.None,
				Target);
		}

		public static Currency Source = Currency.EUR;
		public static Currency Target = Currency.RUB;

		private static CurrencyResult GetRate(DateTime date)
		{
			var fixerResult = GetRateFromFixer(date);
			if (fixerResult.rates.ContainsKey(Source.ToString()) && fixerResult.rates.ContainsKey(Target.ToString()))
			{
				var sourceRate = fixerResult.rates[Source.ToString()];
				var targetRate = fixerResult.rates[Target.ToString()];
				return new CurrencyResult(targetRate / sourceRate, fixerResult.date);
			}

			if (Target == Currency.EUR)
			{
				return new CurrencyResult(1 / fixerResult.rates[Source.ToString()], fixerResult.date);
			}

			return new CurrencyResult(fixerResult.rates.Last().Value, fixerResult.date);
		}

		private static FixerResultJson GetRateFromFixer(DateTime date)
		{
			var jsonString = new WebClient().DownloadString($"{BasePath}/{date:yyyy-MM-dd}?{Symbols}={Source},{Target}");
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
