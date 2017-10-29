using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class UsdToGbpCurrencyProvider : FixerBasedCurrencyProvider
	{
		public override Currency Source => Currency.USD;

		public override Currency Target => Currency.GBP;
	}
}