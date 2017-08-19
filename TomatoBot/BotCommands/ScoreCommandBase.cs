using System.Linq;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public abstract class ScoreCommandBase : IBotCommand
    {
        protected ScoreCommandBase(UsersRepository userRepository)
        {
			UserRepository = userRepository;
        }

        public virtual bool CanExecute(MessageActivity activity) => activity.IsMessageForBot();

        public abstract string ExecuteAndGetResponse(MessageActivity activity);

        protected UsersRepository UserRepository { get; }

        protected Users GetScoreForUserOrNull(MessageActivity activity)
        {
            return UserRepository.GetUser(activity.ConversationId, GetUserNameFromMessageOrNull(activity));
        }

        private string GetUserNameFromMessageOrNull(MessageActivity activity)
        {
            return activity.Message.Split(' ').Select(word => word.Trim().Trim('@')).FirstOrDefault(
                word => UserRepository.IsUserExists(activity.ConversationId, word));
        }
    }
}