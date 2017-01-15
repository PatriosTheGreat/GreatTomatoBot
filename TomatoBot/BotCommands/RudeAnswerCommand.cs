using System;
using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public sealed class RudeAnswerCommand : IBotCommand
    {
        public string ExecuteAndGetResponce(Activity activity)
        {
            switch (_random.Next(0, 6))
            {
                case 0:
                    return "Твоя мама случаем не прихоится сестрой твоему папе?";
                case 1:
                    return "Хватит меня угнетать, ты теломразь!";
                case 2:
                    return "Что ты делаешь со своей жизнью?";
                case 3:
                    return "Ты же не ожидаешь от меня ответа на это?";
                case 4:
                    return "Эх, надо было пойти во флот…";
                case 5:
                    return "Начнём с того, что ты пидоглазое мудило.";
                default:
                    return "Не знаю что ещё добавить.";
            }
        }

        public bool CanExecute(Activity activity)
        {
            return activity.IsAdressToBot();
        }

        private readonly Random _random = new Random();
    }
}