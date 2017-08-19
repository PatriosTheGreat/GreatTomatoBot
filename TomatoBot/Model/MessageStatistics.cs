using TomatoBot.BotCommands;

namespace TomatoBot.Model
{
	public sealed class MessageStatistics
	{
		public MessageStatistics(
			int messagesCount, 
			int wordsCount, 
			int smilesCount, 
			int attachmentsCount,
			double? averageWordsLength, 
			string firstName, 
			string nickname)
		{
			MessagesCount = messagesCount;
			WordsCount = wordsCount;
			SmilesCount = smilesCount;
			AttachmentsCount = attachmentsCount;
			AverageWordsLength = averageWordsLength;
			FirstName = firstName;
			Nickname = nickname;
		}

		public int MessagesCount { get; }

		public int WordsCount { get; }

		public int SmilesCount { get; }

		public int AttachmentsCount { get; }

		public double? AverageWordsLength { get; }

		public string FirstName { get; }

		public string Nickname { get; }

		public string GetUserStatistics()
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

			return $"{userName}{ActivityExtension.NewLine}{MessagesCount}_{WordsCount}_{SmilesCount}_{AttachmentsCount}_{AverageWordsLength?.ToString("F2") ?? "0"}";
		}
	}
}