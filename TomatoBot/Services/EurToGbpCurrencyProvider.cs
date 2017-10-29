using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class EurToGbpCurrencyProvider : FixerBasedCurrencyProvider
	{
		public override Currency Source => Currency.EUR;

		public override Currency Target => Currency.GBP;
	}
}