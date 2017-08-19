using System.Linq;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
	public sealed class GetDailyStatistics : IBotCommand, ICommandWithHelpLine
	{
		public GetDailyStatistics(MessagesRepository messagesRepository)
		{
			_messagesRepository = messagesRepository;
		}

		public string CommandName => "getDailyStatistics";

		public string Description => "отображает статистику чата за день";

		public string Sample => "/dailystatistics";

		public bool CanExecute(MessageActivity activity) => activity.Message.StartsWith("/dailystatistics");

		public string ExecuteAndGetResponse(MessageActivity activity)
		{
			var totalStatistics = _messagesRepository.GetTotalStatistics(activity.ConversationId);
			var userStatistics = string.Join(ActivityExtension.NewLine, totalStatistics.Select(statistics => statistics.GetStatistics()));
			return $"Сообщений Слов Атачей{ActivityExtension.NewLine}{userStatistics}{ActivityExtension.NewLine}Всего сообщений {totalStatistics.Sum(statistics => statistics.MessagesCount)}";
		}

		private readonly MessagesRepository _messagesRepository;
	}
}
