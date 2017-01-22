using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;
using System.Linq;

namespace TomatoBot.BotCommands
{
    public sealed class GetScoreForUserCommand : ScoreCommandBase, ICommandWithHelpLine
    {
        public GetScoreForUserCommand(ScoreRepository scoreRepository) : base(scoreRepository)
        {
        }

        public string CommandName => "getScoreForUser";

        public string Description => "отображает счет относительно определенного участника чата";

        public string Sample => "/score @UserName";

        public override bool CanExecute(Activity activity)
        {
            return base.CanExecute(activity) && CommandUserRegex.IsMatch(activity.Text);
        }

        public override string ExecuteAndGetResponse(Activity activity)
        {
            var userScore = GetScoreForUserOrNull(activity);

            if (userScore != null)
            {
                var totalScore = ScoreRepository.GetScoresInConversation(activity.Conversation.Id)
                    .Sum(score => score.Score);

                return $"{userScore.PersonalScore()}, У всех остальных {totalScore - userScore.Score}";
            }

            return string.Empty;
        }
        
        private static readonly Regex CommandUserRegex =
            new Regex($"(/|(@GreatTomatoBot ))((счет)|(score)) {ActivityExtension.UserNameRegex}");
    }
}