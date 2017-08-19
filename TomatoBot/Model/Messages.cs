using System;

namespace TomatoBot.Model
{
	public class Messages
	{
		public Messages(
			string conversationId, 
			string userId, 
			int wordsCount, 
			DateTime time, 
			int smilesCount, 
			int attachmentsCount, 
			int linksCount, 
			int messageLength, 
			int? replyToId)
		{
			ConversationId = conversationId;
			UserId = userId;
			WordsCount = wordsCount;
			Time = time;
			SmilesCount = smilesCount;
			AttachmentsCount = attachmentsCount;
			LinksCount = linksCount;
			MessageLength = messageLength;
			ReplyToId = replyToId;
		}

		public string ConversationId { get; }

		public string UserId { get; }

		public int WordsCount { get; }

		public DateTime Time { get; }

		public int SmilesCount { get; }

		public int AttachmentsCount { get; }

		public int LinksCount { get; }

		public int MessageLength { get; }

		public int? ReplyToId { get; }
	}
}