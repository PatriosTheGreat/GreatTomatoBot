using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public sealed class SetScoreForUserCommand : PersonalBotCommandBase
    {
        public SetScoreForUserCommand(ScoreRepository scoreRepository) : base(scoreRepository)
        {
        }

        public override bool CanExecute(Activity activity)
        {
            int score;
            return base.CanExecute(activity) && SetScoreUserRegex.IsMatch(activity.Text) && int.TryParse(activity.Text.Split(' ').Last(), out score);
        }

        public override string ExecuteAndGetResponce(Activity activity)
        {
            var userScore = GetScoreForUserOrNull(activity);

            if (userScore != null)
            {
                int score;

                if(int.TryParse(activity.Text.Split(' ').Last(), out score))
                {
                    ScoreRepository.SetScoreForUser(activity.Conversation.Id, userScore.UserId, score);
                    return userScore.PersonalScore();
                }
            }

            return string.Empty;
        }
        
        private static readonly Regex SetScoreUserRegex =
            new Regex("(/|(@GreatTomatoBot ))((выставить счет)|(set score)) @?([a-zA-Z0-9]+) ([0-9]+)");
    }
}