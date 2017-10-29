using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class GbpToEurCurrencyProvider : FixerBasedCurrencyProvider
	{
		public override Currency Source => Currency.GBP;

		public override Currency Target => Currency.EUR;
	}
}