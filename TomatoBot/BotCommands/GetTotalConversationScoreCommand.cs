﻿using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public class GetTotalConversationScoreCommand : PersonalBotCommandBase
    {
        public GetTotalConversationScoreCommand(ScoreRepository repository)
        {
            _repository = repository;
        }

        public override bool CanExecute(Activity activity)
        {
            return base.CanExecute(activity) && CommandUserRegex.IsMatch(activity.Text);
        }

        public override string ExecuteAndGetResponce(Activity activity)
        {
            var scores = _repository.GetScoresInConversation(activity.Conversation.Id);
            var userScopes = string.Join(", ", scores.Select(score => score.PersonalScore()));
            return $"{userScopes}, Всего {scores.Sum(score => score.Score)}";
        }

        private readonly ScoreRepository _repository;

        private static readonly Regex CommandUserRegex =
            new Regex("(/|(@GreatTomatoBot ))((общий счет)|(total score))");
    }
}