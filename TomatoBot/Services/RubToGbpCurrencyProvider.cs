using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class RubToGbpCurrencyProvider : FixerBasedCurrencyProvider
	{
		public override Currency Source => Currency.RUB;

		public override Currency Target => Currency.GBP;
	}
}