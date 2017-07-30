namespace TomatoBot.Model
{
	public class Users
	{
		public Users(
			int id,
			string nickname,
			string firstName,
			string userId,
			string conversationId,
			int score)
		{
			Id = id;
			Nickname = nickname;
			FirstName = firstName;
			UserId = userId;
			ConversationId = conversationId;
			Score = score;
		}

		public int Id { get; }

		public string Nickname { get; }

		public string FirstName { get; }

		public string UserId { get; }

		public string ConversationId { get; }

		public int Score { get; }

		public string Identity => string.IsNullOrEmpty(Nickname) ? FirstName : Nickname;

		public string PersonalScore()
		{
			var userName = "неизвестный";
			if (!string.IsNullOrEmpty(Nickname))
			{
				userName = Nickname;
			}
			else if (!string.IsNullOrEmpty(FirstName))
			{
				userName = FirstName;
			}

			return $"{userName} {Score}";
		}
	}
}