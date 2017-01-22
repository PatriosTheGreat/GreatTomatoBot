﻿using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public class GetTotalConversationScoreCommand : ScoreCommandBase, ICommandWithHelpLine
    {
        public GetTotalConversationScoreCommand(ScoreRepository repository) : base(repository)
        {
        }

        public string CommandName => "getTotalScore";

        public string Description => "отображает общий счет в чате";

        public string Sample => "/score";

        public override bool CanExecute(Activity activity)
        {
            return base.CanExecute(activity) && CommandUserRegex.IsMatch(activity.Text);
        }

        public override string ExecuteAndGetResponse(Activity activity)
        {
            var scores = ScoreRepository.GetScoresInConversation(activity.Conversation.Id);
            var userScopes = string.Join(ActivityExtension.NewLine, scores.OrderByDescending(score => score.Score).Select(score => score.PersonalScore()));
            return $"{userScopes}{ActivityExtension.NewLine}Всего {scores.Sum(score => score.Score)}";
        }
        
        private static readonly Regex CommandUserRegex = new Regex("(/|(@GreatTomatoBot ))(счет|score)");
    }
}