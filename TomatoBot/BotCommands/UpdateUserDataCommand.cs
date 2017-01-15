using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public sealed class UpdateUserDataCommand : IBotCommand
    {
        public UpdateUserDataCommand(ScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public bool CanExecute(Activity activity) => true;

        public string ExecuteAndGetResponse(Activity activity)
        {
            var userChannelData = new ChannelUserData(activity.ChannelData?.ToString());
            _scoreRepository.UpdateUserData(
                activity.Conversation.Id, 
                activity.From.Id,
                userChannelData.UserFirstName, 
                userChannelData.UserNickname);

            return string.Empty;
        }

        private readonly ScoreRepository _scoreRepository;
    }
}