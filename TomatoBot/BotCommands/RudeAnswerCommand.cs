using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public sealed class RudeAnswerCommand : PersonalBotCommandBase
    {
        public override string ExecuteAndGetResponce(Activity activity)
        {
            return "Твоя мама случаем не прихоится сестрой твоему папе?";
        }
    }
}