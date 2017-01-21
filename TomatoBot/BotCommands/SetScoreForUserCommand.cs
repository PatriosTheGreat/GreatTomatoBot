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
            return base.CanExecute(activity) && SetScoreUserRegex.IsMatch(activity.Text) && GetScoreToSet(activity.Text) != -1;
        }

        public override string ExecuteAndGetResponse(Activity activity)
        {
            var userScore = GetScoreForUserOrNull(activity);

            if (userScore != null)
            {
                ScoreRepository.SetScoreForUser(activity.Conversation.Id, userScore.UserId, GetScoreToSet(activity.Text));
                return userScore.PersonalScore();
            }

            return string.Empty;
        }

        private static int GetScoreToSet(string text)
        {
            int score;
            if (!int.TryParse(text.Split(' ').Last(), out score))
            {
                return -1;
            }

            return score;
        }
        
        private static readonly Regex SetScoreUserRegex =
            new Regex($"(/|(@GreatTomatoBot ))((выставить счет)|(set score)) {ActivityExtension.UserNameRegex} ([0-9]+)");
    }
}