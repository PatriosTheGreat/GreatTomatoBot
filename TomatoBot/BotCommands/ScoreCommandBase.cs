using System.Linq;
using Microsoft.Bot.Connector;
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

        public virtual bool CanExecute(Activity activity) => activity.IsMessageForBot();

        public abstract string ExecuteAndGetResponse(Activity activity);

        protected UsersRepository UserRepository { get; }

        protected Users GetScoreForUserOrNull(IMessageActivity activity)
        {
            return UserRepository.GetUser(activity.Conversation.Id, GetUserNameFromMessageOrNull(activity));
        }

        private string GetUserNameFromMessageOrNull(IMessageActivity activity)
        {
            return activity.Text.Split(' ').Select(word => word.Trim().Trim('@')).FirstOrDefault(
                word => UserRepository.IsUserExists(activity.Conversation.Id, word));
        }
    }
}