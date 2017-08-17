using Microsoft.Bot.Connector;
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

		public bool CanExecute(Activity activity) => true;

		public string ExecuteAndGetResponse(Activity activity)
		{
			_messagesRepository.AddMessage(new Messages(activity.Conversation.Id, activity.From.Id, CountWords(activity.Text), DateTime.UtcNow));
			return string.Empty;
		}

		private int CountWords(string text)
		{
			text = text.Trim();
			int wordCount = 0, index = 0;

			while (index < text.Length)
			{
				// check if current char is part of a word
				while (index < text.Length && !char.IsWhiteSpace(text[index]))
					index++;

				wordCount++;

				// skip whitespace until next word
				while (index < text.Length && char.IsWhiteSpace(text[index]))
					index++;
			}

			return wordCount;
		}

		private readonly MessagesRepository _messagesRepository;
	}
}