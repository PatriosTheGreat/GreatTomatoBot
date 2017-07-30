using System;

namespace TomatoBot.Model
{
	public sealed class Memeses
	{
		public Memeses(
			int id,
			DateTime sendTime,
			int userId,
			string hash,
			string conversationId)
		{
			Id = id;
			SendTime = sendTime;
			UserId = userId;
			Hash = hash;
			ConversationId = conversationId;
		}

		public int Id { get; }

		public DateTime SendTime { get; }

		public int UserId { get; }

		public string Hash { get; }

		public string ConversationId { get; }
	}
}