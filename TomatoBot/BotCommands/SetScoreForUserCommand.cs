using System.Linq;
using System.Text.RegularExpressions;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class SetScoreForUserCommand : ScoreCommandBase, ICommandWithHelpLine
    {
        public SetScoreForUserCommand(UsersRepository userRepository) : base(userRepository)
        {
        }

        public string CommandName => "setScoreForUser";

        public string Description => "устанавливает счет для определенного пользователя чата";

        public string Sample => "/set score @UserName 0";

        public override bool CanExecute(MessageActivity activity)
        {
            return base.CanExecute(activity) && SetScoreUserRegex.IsMatch(activity.Message) && GetScoreToSet(activity.Message) != -1;
        }

        public override string ExecuteAndGetResponse(MessageActivity activity)
        {
            var userScore = GetScoreForUserOrNull(activity);

            if (userScore != null)
            {
				UserRepository.SetScoreForUser(activity.ConversationId, userScore.UserId, GetScoreToSet(activity.Message));
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