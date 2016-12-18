using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public sealed class SetScoreForUserCommand : PersonalBotCommandBase
    {
        public SetScoreForUserCommand(ScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public override bool CanExecute(Activity activity)
        {
            return base.CanExecute(activity) && SetScoreUserRegex.IsMatch(activity.Text);
        }

        public override string ExecuteAndGetResponce(Activity activity)
        {
            var matchingGroupds = SetScoreUserRegex.Match(activity.Text);
            var userInfo = matchingGroupds.Groups[matchingGroupds.Groups.Count - 2].ToString();
            var userScore = _scoreRepository.GetScoreForUser(activity.Conversation.Id, userInfo);

            if (userScore != null)
            {
                int newScore;
                if (int.TryParse(matchingGroupds.Groups[matchingGroupds.Groups.Count - 1].ToString(), out newScore))
                {
                    _scoreRepository.SetScoreForUser(activity.Conversation.Id, userScore.UserId, newScore);
                    return userScore.PersonalScore();
                }
            }

            return string.Empty;
        }

        private readonly ScoreRepository _scoreRepository;
        private static readonly Regex SetScoreUserRegex =
            new Regex("(/|(@GreatTomatoBot ))((выставить счет)|(set score)) @?([a-zA-Z0-9]+) ([0-9]+)");
    }
}