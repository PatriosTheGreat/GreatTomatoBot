using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using NTextCat;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public class DetermineWrongLayoutCommand : IBotCommand
    {
        public DetermineWrongLayoutCommand(ScoreRepository repository)
        {
            _repository = repository;
            _naiveBayesLanguageIdentifier = new NaiveBayesLanguageIdentifierFactory().Load(GetType().Assembly.GetManifestResourceStream(NgramsEmbeddedFileName));
        }

        public bool CanExecute(Activity activity)
        {
            return GetWords(activity.Text).Any() && !activity.IsMessageForBot();
        }

        public string ExecuteAndGetResponse(Activity activity)
        {
            if (!IsAnyWordIncorrect(FilterText(activity.Text)))
            {
                return string.Empty;
            }

            var userInfo = _repository.GetScoreForUser(activity.Conversation.Id, activity.From.Name);
            if (userInfo != null)
            {
                _repository.SetScoreForUser(activity.Conversation.Id, userInfo.UserId, userInfo.Score + 1);
                return userInfo.PersonalScore();
            }

            return string.Empty;
        }

        private static string FilterText(string activityText)
        {
            foreach (var url in ActivityExtension.GetUrlsInMessage(activityText))
            {
                activityText = activityText.Replace(url, string.Empty);
            }

            return activityText;
        }

        private bool IsAnyWordIncorrect(string message)
        {
            return GetWords(message).Any(word =>
            {
                var language = _naiveBayesLanguageIdentifier.Identify(word).First().Item1.Iso639_2T;
                return language == "rusWrong" || language == "engWrong";
            });
        }

        private static IEnumerable<string> GetWords(string message)
        {
            return from object word in WordRegex.Matches(message) select word.ToString();
        }

        private readonly ScoreRepository _repository;
        private readonly NaiveBayesLanguageIdentifier _naiveBayesLanguageIdentifier;
        private static readonly Regex WordRegex = new Regex("[a-zA-Zа-яА-Я]+", RegexOptions.Compiled);
        private const string NgramsEmbeddedFileName = "TomatoBot.Core14.profile.xml";
    }
}