using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public sealed class IncrementScoreForUserCommand : PersonalBotCommandBase
    {
        public IncrementScoreForUserCommand(ScoreRepository scoreRepository) : base(scoreRepository)
        {   
        }

        public override bool CanExecute(Activity activity)
        {
            return base.CanExecute(activity) && ValidateIncrementUserCommandRegex.IsMatch(activity.Text);
        }

        public override string ExecuteAndGetResponse(Activity activity)
        {
            var userScore = GetScoreForUserOrNull(activity);

            if (userScore != null)
            {
                ScoreRepository.SetScoreForUser(activity.Conversation.Id, userScore.UserId, userScore.Score + 1);
                return userScore.PersonalScore();
            }

            return string.Empty;
        }
        
        private static readonly Regex ValidateIncrementUserCommandRegex = 
            new Regex("(/|(@GreatTomatoBot)) (((увеличить счет)|(increment score)) @?([a-zA-Z0-9]+))|(@?([a-zA-Z0-9]+) [+])");
    }
}