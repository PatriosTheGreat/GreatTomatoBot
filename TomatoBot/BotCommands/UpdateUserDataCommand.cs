using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class UpdateUserDataCommand : IBotCommand
    {
        public UpdateUserDataCommand(UsersRepository usersRepository)
        {
			_usersRepository = usersRepository;
        }

        public bool CanExecute(MessageActivity activity) => true;

        public string ExecuteAndGetResponse(MessageActivity activity)
        {
			_usersRepository.UpdateUserData(
                activity.ConversationId, 
                activity.FromUserId,
                activity.ChannelData.UserFirstName, 
                activity.ChannelData.UserNickname);

            return string.Empty;
        }

        private readonly UsersRepository _usersRepository;
    }
}