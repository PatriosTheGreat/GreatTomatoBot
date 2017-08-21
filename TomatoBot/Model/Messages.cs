using System;

namespace TomatoBot.Model
{
	public class Messages
	{
		public Messages(
			int userId, 
			int wordsCount, 
			DateTime time, 
			int smilesCount, 
			int attachmentsCount, 
			int linksCount, 
			int messageLength, 
			int? replyToId)
		{
			UserId = userId;
			WordsCount = wordsCount;
			Time = time;
			SmilesCount = smilesCount;
			AttachmentsCount = attachmentsCount;
			LinksCount = linksCount;
			MessageLength = messageLength;
			ReplyToId = replyToId;
		}
		
		public int UserId { get; }

		public int WordsCount { get; }

		public DateTime Time { get; }

		public int SmilesCount { get; }

		public int AttachmentsCount { get; }

		public int LinksCount { get; }

		public int MessageLength { get; }

		public int? ReplyToId { get; }
	}
}