using TomatoBot.Model;

namespace TomatoBot.Services
{
	public interface ICurrencyToAnotherRateProvider
	{
		Currency Source { get; }

		Currency Target { get; }

		CurrencyRate GetRate();
	}
}
