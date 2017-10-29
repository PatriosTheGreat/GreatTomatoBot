using TomatoBot.Model;

namespace TomatoBot.Services
{
	public interface ICurrencyRatingProvider
	{
		Currency Source { get; }

		CurrencyRate[] GetRates();
	}
}
