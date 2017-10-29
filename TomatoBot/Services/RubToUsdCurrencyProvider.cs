using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class RubToUsdCurrencyProvider : BloombergCurrencyProvider
	{
		public override Currency Source => Currency.RUB;

		public override Currency Target => Currency.USD;
	}
}