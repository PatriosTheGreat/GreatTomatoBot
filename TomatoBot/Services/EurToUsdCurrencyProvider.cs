using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class EurToUsdCurrencyProvider : BloombergCurrencyProvider
	{
		public override Currency Source => Currency.EUR;

		public override Currency Target => Currency.USD;
	}
}