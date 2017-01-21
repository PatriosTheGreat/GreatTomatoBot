using System;
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
            var filteredText = FilterText(activity.Text);
            return !activity.IsMessageForBot() && IsEnglishIncorrect(filteredText);
        }

        public string ExecuteAndGetResponse(Activity activity)
        {
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
            var filteredWords =
                activityText.Split().Select(word => word.Trim()).Where(word => !ActivityExtension.IsUrl(word) && !IsSmile(word));
            return string.Join(" ", filteredWords);
        }

        private static bool IsSmile(string word) => word.Length == 2 && (word.First() == ':' || word.First() == ';');

        // ToDo: Исправить определение неправильного английского слова
        private bool IsRussianIncorrect(string message) => IsIncorrectLanguage(message, RussianWordRegex, RussianIsoCode, EnglishWrongIsoCode);

        private bool IsEnglishIncorrect(string message) => IsIncorrectLanguage(message, EnglisgWordRegex, EnglishIsoCode, RussianWrongIsoCode);

        private bool IsIncorrectLanguage(string message, Regex languageRegex, string languageIso, string wrongLanguageIso)
        {
            var text = string.Join(" ", GetWords(message, languageRegex));

            var lenguageRates = _naiveBayesLanguageIdentifier.Identify(text).Select(rate => rate.Item1.Iso639_2T).ToArray();
            return Array.IndexOf(lenguageRates, languageIso) > Array.IndexOf(lenguageRates, wrongLanguageIso);
        }

        private static IEnumerable<string> GetWords(string message, Regex languageRegex) => from object word in languageRegex.Matches(message) select word.ToString();

        private readonly ScoreRepository _repository;
        private readonly NaiveBayesLanguageIdentifier _naiveBayesLanguageIdentifier;
        private static readonly Regex EnglisgWordRegex = new Regex("[a-zA-Z\\]\\];',\\.\\{\\}\\:<>`~\"]+", RegexOptions.Compiled);
        private static readonly Regex RussianWordRegex = new Regex("[а-яА-Я]+", RegexOptions.Compiled);
        private const string NgramsEmbeddedFileName = "TomatoBot.Core14.profile.xml";
        private const string RussianIsoCode = "rus";
        private const string EnglishIsoCode = "eng";
        private const string RussianWrongIsoCode = "rusWrong";
        private const string EnglishWrongIsoCode = "engWrong";
    }
}