using System.Linq;
using System.Text.RegularExpressions;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public class GetTotalConversationScoreCommand : ScoreCommandBase, ICommandWithHelpLine
    {
        public GetTotalConversationScoreCommand(UsersRepository userRepository) : base(userRepository)
        {
        }

        public string CommandName => "getTotalScore";

        public string Description => "отображает общий счет в чате";

        public string Sample => "/score";

        public override bool CanExecute(MessageActivity activity)
        {
            return base.CanExecute(activity) && CommandUserRegex.IsMatch(activity.Message);
        }

        public override string ExecuteAndGetResponse(MessageActivity activity)
        {
            var scores = UserRepository.GetScoresInConversation(activity.FromUser.UserId);
            var userScopes = string.Join(ActivityExtension.NewLine, scores.OrderByDescending(score => score.Score).Select(score => score.PersonalScore()));
            return $"{userScopes}{ActivityExtension.NewLine}Всего {scores.Sum(score => score.Score)}";
        }
        
        private static readonly Regex CommandUserRegex = new Regex("(/|(@GreatTomatoBot ))(счет|score)");
    }
}