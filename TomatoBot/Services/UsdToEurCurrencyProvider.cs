using TomatoBot.Model;

namespace TomatoBot.Services
{
	public sealed class UsdToEurCurrencyProvider : BloombergCurrencyProvider
	{
		public override Currency Source => Currency.USD;

		public override Currency Target => Currency.EUR;
	}
}