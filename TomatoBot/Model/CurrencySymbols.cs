using System;

namespace TomatoBot.Model
{
	public static class CurrencySymbols
	{
		public const string Rub = "₽";
		public const string Usd = "$";
		public const string Eur = "€";
		public const string Gbp = "£";
		public const string Btc = "B";

		public static string GetSymbol(Currency currency)
		{
			switch (currency)
			{
				case Currency.RUB:
					return Rub;
				case Currency.USD:
					return Usd;
				case Currency.EUR:
					return Eur;
				case Currency.GBP:
					return Gbp;
				default:
					throw new ArgumentOutOfRangeException(nameof(currency), currency, null);
			}
		}
	}
}