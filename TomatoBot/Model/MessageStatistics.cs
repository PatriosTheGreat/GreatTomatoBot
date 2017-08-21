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

		public string GetDetailedStatistics()
		{
			return $"`{GetSpaces(MessagesCount)}{GetSpaces(WordsCount)}{GetSpaces(SmilesCount)}{GetSpaces(AttachmentsCount)}{GetSpaces(AverageWordsLength?.ToString("F2") ?? "0")}`  {GetUserName()} ";
		}

		public string GetStatistics() =>
			$"`{GetSpaces(MessagesCount)}{GetSpaces(WordsCount)}{GetSpaces(AttachmentsCount)}`  {GetUserName()} ";

		public string GetUserName()
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

			return userName;
		}

		private static string GetSpaces<T>(T number)
		{
			const int MaxWidth = 5;
			var delta = MaxWidth - number.ToString().Length;
			return number + new string(' ', delta < 1 ? 1 : delta);
		}
	}
}