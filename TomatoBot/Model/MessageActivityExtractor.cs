using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.BotCommands;
using TomatoBot.Repository;

namespace TomatoBot.Model
{
	public sealed class MessageActivityExtractor
	{
		public MessageActivityExtractor(UsersRepository usersRepository)
		{
			_usersRepository = usersRepository;
		}

		public MessageActivity Extract(Activity activity)
		{
			var smiles = new List<string>();
			var urls = new List<string>();
			var words = new List<string>();
			if (!string.IsNullOrEmpty(activity.Text))
			{
				foreach (var word in activity.Text.Split())
				{
					var urlsInWord = GetUrlsInMessage(word).ToArray();
					if (urlsInWord.Length > 0)
					{
						urls.AddRange(urlsInWord);
					}
					else if (IsSmile(word))
					{
						smiles.Add(word);
					}
					else
					{
						words.Add(word);
					}
				}
			}

			var userChannelData = new ChannelUserData(activity.ChannelData?.ToString());

			_usersRepository.UpdateUserData(
				activity.Conversation.Id,
				activity.From.Id,
				userChannelData.UserFirstName,
				userChannelData.UserNickname);
			
			return new MessageActivity(
				urls.ToArray(),
				activity.Text,
				_usersRepository.GetUser(activity.Conversation.Id, activity.From.Id),
				string.IsNullOrEmpty(userChannelData.ReplyToId) ? null : _usersRepository.GetUser(activity.Conversation.Id, userChannelData.ReplyToId),
				activity.Attachments.Count,
				smiles.ToArray(),
				words.ToArray(),
				activity.Conversation.Id);
		}

		private static bool IsSmile(string word) => (word.Length == 2 && SmileFirstSymbols.Contains(word.First())) || (word.Length == 3 && word[1] == '_');
		
		private static IEnumerable<string> GetUrlsInMessage(string messages)
		{
			var matches = UrlRegex.Matches(messages);
			return from object match in matches select match.ToString();
		}

		private readonly UsersRepository _usersRepository;
		private static readonly Regex UrlRegex = new Regex(
			@"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?",
			RegexOptions.Compiled);
		private static readonly char[] SmileFirstSymbols = { ':', ';', '=' };
	}
}