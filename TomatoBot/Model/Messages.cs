using System;

namespace TomatoBot.Model
{
	public class Messages
	{
		public Messages(string conversationId, string userId, int wordsCount, DateTime time)
		{
			ConversationId = conversationId;
			UserId = userId;
			WordsCount = wordsCount;
			Time = time;
		}

		public string ConversationId { get; }

		public string UserId { get; }

		public int WordsCount { get; }

		public DateTime Time { get; }
	}
}