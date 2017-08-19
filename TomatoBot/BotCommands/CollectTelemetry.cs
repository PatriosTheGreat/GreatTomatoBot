using TomatoBot.Repository;
using System;
using TomatoBot.Model;

namespace TomatoBot.BotCommands
{
	public class CollectTelemetry : IBotCommand
	{
		public CollectTelemetry(MessagesRepository messagesRepository)
		{
			_messagesRepository = messagesRepository;
		}

		public bool CanExecute(MessageActivity activity) => true;

		public string ExecuteAndGetResponse(MessageActivity activity)
		{
			_messagesRepository.AddMessage(new Messages(activity.ConversationId, activity.FromUserId, activity.Words.Length, DateTime.UtcNow));
			return string.Empty;
		}

		private readonly MessagesRepository _messagesRepository;
	}
}