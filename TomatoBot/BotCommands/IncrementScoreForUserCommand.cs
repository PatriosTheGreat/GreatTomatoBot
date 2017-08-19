using System.Text.RegularExpressions;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class IncrementScoreForUserCommand : ScoreCommandBase, ICommandWithHelpLine
    {
        public IncrementScoreForUserCommand(UsersRepository userRepository) : base(userRepository)
        {
        }

        public string CommandName => "incrementScoreForUser";

        public string Description => "увеличивает счет определенному участнику чата";

        public string Sample => "@GreatTomatoBot @UserName +";

        public override bool CanExecute(MessageActivity activity)
        {
            return base.CanExecute(activity) && ValidateIncrementUserCommandRegex.IsMatch(activity.Message);
        }

        public override string ExecuteAndGetResponse(MessageActivity activity)
        {
            var userScore = GetScoreForUserOrNull(activity);

            if (userScore != null)
            {
				UserRepository.SetScoreForUser(activity.FromUser.ConversationId, userScore.UserId, userScore.Score + 1);
                return userScore.PersonalScore();
            }

            return string.Empty;
        }
        
        private static readonly Regex ValidateIncrementUserCommandRegex = 
            new Regex($"(/|(@GreatTomatoBot)) (((увеличить счет)|(increment score)) {ActivityExtension.UserNameRegex}|{ActivityExtension.UserNameRegex} [+])");
    }
}