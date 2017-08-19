using System.Linq;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
	public sealed class GetTotalStatistics : IBotCommand, ICommandWithHelpLine
	{
		public GetTotalStatistics(MessagesRepository messagesRepository)
		{
			_messagesRepository = messagesRepository;
		}

		public string CommandName => "getTotalStatistics";

		public string Description => "отображает полную статистику чата за весь период наблюдений";

		public string Sample => "/totalstatistics";

		public bool CanExecute(MessageActivity activity) => activity.Message.StartsWith("/totalstatistics");

		public string ExecuteAndGetResponse(MessageActivity activity)
		{
			var totalStatistics = _messagesRepository.GetTotalStatistics(activity.ConversationId);
			var userStatistics = string.Join(ActivityExtension.NewLine, totalStatistics.Select(statistics => statistics.GetDetailedStatistics()));
			return $"·						msg	word	smile	att	avg{ActivityExtension.NewLine}{ActivityExtension.NewLine}{userStatistics}{ActivityExtension.NewLine}Всего сообщений {totalStatistics.Sum(statistics => statistics.MessagesCount)}";
		}

		private readonly MessagesRepository _messagesRepository;
	}
}
