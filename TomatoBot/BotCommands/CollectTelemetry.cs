using TomatoBot.Repository;
using System;
using System.Linq;
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
			_messagesRepository.AddMessage(
				new Messages(
					activity.ConversationId, 
					activity.FromUser.UserId, 
					activity.Words.Length, 
					DateTime.UtcNow,
					smilesCount: activity.Smiles.Length,
					attachmentsCount: activity.AttachmentsCount,
					linksCount: activity.Links.Length,
					messageLength: activity.Words.Sum(word => word.Length),
					replyToId: activity.ReplyTo?.Id));
			return string.Empty;
		}

		private readonly MessagesRepository _messagesRepository;
	}
}