using Microsoft.Bot.Connector;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class UpdateUserDataCommand : IBotCommand
    {
        public UpdateUserDataCommand(UsersRepository usersRepository)
        {
			_usersRepository = usersRepository;
        }

        public bool CanExecute(Activity activity) => true;

        public string ExecuteAndGetResponse(Activity activity)
        {
            var userChannelData = new ChannelUserData(activity.ChannelData?.ToString());
			_usersRepository.UpdateUserData(
                activity.Conversation.Id, 
                activity.From.Id,
                userChannelData.UserFirstName, 
                userChannelData.UserNickname);

            return string.Empty;
        }

        private readonly UsersRepository _usersRepository;
    }
}