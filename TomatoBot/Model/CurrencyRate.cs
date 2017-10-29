namespace TomatoBot.Model
{
	public class CurrencyRate
	{
		public CurrencyRate(double rate, CurrencyRateDirection rateDirection, Currency target)
		{
			Rate = rate;
			RateDirection = rateDirection;
			Target = target;
		}

		public double Rate { get; }

		public Currency Target { get; }

		public CurrencyRateDirection RateDirection { get; }
	}
}