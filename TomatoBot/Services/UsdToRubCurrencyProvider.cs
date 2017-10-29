using TomatoBot.Model;

namespace TomatoBot.Services
{
	public class UsdToRubCurrencyProvider : BloombergCurrencyProvider
	{
		public override Currency Source => Currency.USD;

		public override Currency Target => Currency.RUB;
	}
}