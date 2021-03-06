﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NTextCat;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public class DetermineWrongLayoutCommand : IBotCommand
    {
        public DetermineWrongLayoutCommand(UsersRepository usersRepository)
        {
			_usersRepository = usersRepository;
            _naiveBayesLanguageIdentifier =
                new NaiveBayesLanguageIdentifierFactory().Load(
                    GetType().Assembly.GetManifestResourceStream(NgramsEmbeddedFileName));
        }

        public bool CanExecute(MessageActivity activity)
        {
            return !activity.IsMessageForBot() && IsEnglishIncorrect(string.Join(" ", activity.Words));
        }

        public string ExecuteAndGetResponse(MessageActivity activity)
        {
			activity.FromUser.Score++;
			_usersRepository.SetScoreForUser(activity.ConversationId, activity.FromUser.UserId, activity.FromUser.Score);
            return $"{activity.FromUser.PersonalScore()}{ActivityExtension.NewLine}Вы, возможно, имели ввиду:{ActivityExtension.NewLine}{GetOriginalText(string.Join(" ", activity.Words))}";
        }

        private static string GetOriginalText(string wrongLayoutText) =>
            new string(wrongLayoutText.Select(ch => SwitchLayout.ContainsKey(ch) ? SwitchLayout[ch] : ch).ToArray());
        
        private bool IsEnglishIncorrect(string message)
        {
            if (GetWords(message, RussianWordRegex).Any())
            {
                return false;
            }

            var text = string.Join(" ", GetWords(message, EnglishWordRegex));

            var languageRates =
                _naiveBayesLanguageIdentifier.Identify(text).Select(rate => rate.Item1.Iso639_2T).ToArray();
            return Array.IndexOf(languageRates, EnglishIsoCode) > Array.IndexOf(languageRates, RussianWrongIsoCode);
        }

        private static IEnumerable<string> GetWords(string message, Regex languageRegex)
            => from object word in languageRegex.Matches(message) select word.ToString();

        private readonly UsersRepository _usersRepository;
        private readonly NaiveBayesLanguageIdentifier _naiveBayesLanguageIdentifier;

        private static readonly Regex EnglishWordRegex = new Regex("[a-zA-Z\\]\\];',\\.\\{\\}\\:<>`~\"]+",
            RegexOptions.Compiled);

        private static readonly Regex RussianWordRegex = new Regex("[а-яА-Я]+", RegexOptions.Compiled);
        private const string NgramsEmbeddedFileName = "TomatoBot.Core14.profile.xml";
        private const string EnglishIsoCode = "eng";
        private const string RussianWrongIsoCode = "rusWrong";

        private static readonly char[] SmileFirstSymbols = {':', ';', '='};

        private static Dictionary<char, char> SwitchLayout = new Dictionary<char, char>
        {
            ['q'] = 'й', ['w'] = 'ц', ['e'] = 'у', ['r'] = 'к', ['t'] = 'е', ['y'] = 'н', ['u'] = 'г', ['i'] = 'ш', ['o'] = 'щ', ['p'] = 'з', ['a'] = 'ф', ['s'] = 'ы', ['d'] = 'в',
            ['f'] = 'а', ['g'] = 'п', ['h'] = 'р', ['j'] = 'о', ['k'] = 'л', ['l'] = 'д', ['z'] = 'я', ['x'] = 'ч', ['c'] = 'с', ['v'] = 'м', ['b'] = 'и', ['n'] = 'т', ['m'] = 'ь', 
            ['Q'] = 'Й', ['W'] = 'Ц', ['E'] = 'У', ['R'] = 'К', ['T'] = 'Е', ['Y'] = 'Н', ['U'] = 'Г', ['I'] = 'Ш', ['O'] = 'Щ', ['P'] = 'З', ['A'] = 'Ф', ['S'] = 'Ы', ['D'] = 'В',
            ['F'] = 'А', ['G'] = 'П', ['H'] = 'Р', ['J'] = 'О', ['K'] = 'Л', ['L'] = 'Д', ['Z'] = 'Я', ['X'] = 'Ч', ['C'] = 'С', ['V'] = 'М', ['B'] = 'И', ['N'] = 'Т', ['M'] = 'Ь',
            [','] = 'б', ['<'] = 'Б', ['.'] = 'ю', ['>'] = 'Ю', ['&'] = '?', [';'] = 'ж', [':'] = 'Ж', ['\''] = 'э', ['"'] = 'Э', ['['] = 'х', ['{'] = 'Х', [']'] = 'ъ', ['}'] = 'Ъ',
            ['`'] = 'ё', ['~'] = 'Ё', ['?'] = ','
        };

        public static char[] SmileFirstSymbols1
        {
            get
            {
                return SmileFirstSymbols;
            }
        }
    }
}