using TomatoBot.BotCommands;

namespace TomatoBot.Model
{
	public sealed class MessageActivity
	{
		public MessageActivity(
			string[] links, 
			string message,
			ChannelUserData channelData,
			Users fromUser, 
			Users replyTo, 
			int attachmentsCount, 
			string[] smiles, 
			string[] words, 
			string fromUserId, 
			string conversationId)
		{
			Links = links;
			Message = message ?? string.Empty;
			ChannelData = channelData;
			FromUser = fromUser;
			ReplyTo = replyTo;
			AttachmentsCount = attachmentsCount;
			Smiles = smiles;
			Words = words;
			FromUserId = fromUserId;
			ConversationId = conversationId;
		}

		public string[] Links { get; }

		public string Message { get; }

		public ChannelUserData ChannelData { get; }

		public string FromUserId { get; }

		public string ConversationId { get; }

		public Users FromUser { get; }

		public Users ReplyTo { get; }

		public int AttachmentsCount { get; }

		public string[] Smiles { get; }

		public string[] Words { get; }
		
		public bool IsMessageForBot() => Message.StartsWith("/") || Message.Contains(ActivityExtension.BotName);
	}
}