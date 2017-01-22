using System.Linq;
using Microsoft.Bot.Connector;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public abstract class ScoreCommandBase : IBotCommand
    {
        protected ScoreCommandBase(ScoreRepository scoreRepository)
        {
            ScoreRepository = scoreRepository;
        }

        public virtual bool CanExecute(Activity activity) => activity.IsMessageForBot();

        public abstract string ExecuteAndGetResponse(Activity activity);

        protected ScoreRepository ScoreRepository { get; }

        protected MemberScore GetScoreForUserOrNull(IMessageActivity activity)
        {
            return ScoreRepository.GetScoreForUser(activity.Conversation.Id, GetUserNameFromMessageOrNull(activity));
        }

        private string GetUserNameFromMessageOrNull(IMessageActivity activity)
        {
            return activity.Text.Split(' ').Select(word => word.Trim().Trim('@')).FirstOrDefault(
                word => ScoreRepository.IsUserExists(activity.Conversation.Id, word));
        }
    }
}