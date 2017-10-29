using TomatoBot.Model;

namespace TomatoBot.Services
{
	public class RubToEurCurrencyProvider : BloombergCurrencyProvider
	{
		public override Currency Source => Currency.RUB;
		
		public override Currency Target => Currency.EUR;
	}
}