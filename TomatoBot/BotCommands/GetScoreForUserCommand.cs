using System.Text.RegularExpressions;
using TomatoBot.Repository;
using System.Linq;
using TomatoBot.Model;

namespace TomatoBot.BotCommands
{
    public sealed class GetScoreForUserCommand : ScoreCommandBase, ICommandWithHelpLine
    {
        public GetScoreForUserCommand(UsersRepository userRepository) : base(userRepository)
        {
        }

        public string CommandName => "getScoreForUser";

        public string Description => "отображает счет относительно определенного участника чата";

        public string Sample => "/score @UserName";

        public override bool CanExecute(MessageActivity activity)
        {
            return base.CanExecute(activity) && CommandUserRegex.IsMatch(activity.Message);
        }

        public override string ExecuteAndGetResponse(MessageActivity activity)
        {
            var totalScore = UserRepository.GetScoresInConversation(activity.ConversationId)
                .Sum(score => score.Score);

            return $"{activity.FromUser.PersonalScore()}, У всех остальных {totalScore - activity.FromUser.Score}";
        }
        
        private static readonly Regex CommandUserRegex =
            new Regex($"(/|(@GreatTomatoBot ))((счет)|(score)) {ActivityExtension.UserNameRegex}");
    }
}