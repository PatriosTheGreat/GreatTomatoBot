using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public sealed class GetCurrencyCommand : IBotCommand, ICommandWithHelpLine
    {
        public bool CanExecute(Activity activity)
        {
            return activity.IsMessageForBot() && GetOperationType(activity.Text) != CurrencyOperationType.None;
        }

        public string CommandName => "getCurrency";

        public string Description => "отображает курс евро или доллара к рублю";

        public string Sample => "/eurrub";

        public string ExecuteAndGetResponse(Activity activity)
        {
            var response = string.Empty;
            string urlToCurrency;
            switch (GetOperationType(activity.Text))
            {
                case CurrencyOperationType.UsdToRub:
                    urlToCurrency = "https://www.bloomberg.com/quote/USDRUB:CUR";
                    break;
                case CurrencyOperationType.EurToRub:
                    urlToCurrency = "https://www.bloomberg.com/quote/EURRUB:CUR";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var currencyString = new WebClient().DownloadString(urlToCurrency);
            var currencyGroup = PriceRegex.Match(currencyString);
            if (currencyGroup.Length < 2)
            {
                return response;
            }

            response = currencyGroup.Groups[1].ToString();

            if (currencyString.Contains("price-container down"))
            {
                response += " #СлаваРоссии!";
            }

            return response;
        }

        private static CurrencyOperationType GetOperationType(string message)
        {
            var lowerMessage = message.ToLower();
            if (!RubWords.Any(lowerMessage.Contains))
            {
                return CurrencyOperationType.None;
            }

            if (UsdWords.Any(lowerMessage.Contains))
            {
                return CurrencyOperationType.UsdToRub;
            }

            if (EurWords.Any(lowerMessage.Contains))
            {
                return CurrencyOperationType.EurToRub;
            }

            return CurrencyOperationType.None;
        }

        private static readonly Regex PriceRegex = new Regex("<div class=\"price\">([.0-9]+?)</div>");
        private static readonly string[] UsdWords = { "usd", "доллар", "бакс" };
        private static readonly string[] RubWords = { "рубл", "rub" };
        private static readonly string[] EurWords = { "eur", "евро" };

        private enum CurrencyOperationType
        {
            UsdToRub,
            EurToRub,
            None
        }
    }
}