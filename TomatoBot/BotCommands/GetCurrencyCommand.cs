using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using TomatoBot.Model;

namespace TomatoBot.BotCommands
{
    public sealed class GetCurrencyCommand : IBotCommand, ICommandWithHelpLine
    {
        public bool CanExecute(MessageActivity activity)
        {
            return activity.IsMessageForBot() && GetOperationType(activity.Message) != CurrencyOperationType.None;
        }

        public string CommandName => "getCurrency";

        public string Description => "отображает курс евро или доллара к рублю или евро к доллару";

        public string Sample => "/eurrub";

        public string ExecuteAndGetResponse(MessageActivity activity)
        {
            var type = GetOperationType(activity.Message);
            if (type == CurrencyOperationType.RubToEny)
            {
                return
                    $"{GetCurrency(CurrencyOperationType.EurToRub)}{ActivityExtension.NewLine}{GetCurrency(CurrencyOperationType.UsdToRub)}";
            }

            return GetCurrency(type);
        }

        private string GetCurrency(CurrencyOperationType type)
        {
            var response = string.Empty;
            string urlToCurrency;
            switch (type)
            {
                case CurrencyOperationType.UsdToRub:
                    urlToCurrency = "https://www.bloomberg.com/quote/USDRUB:CUR";
                    break;
                case CurrencyOperationType.EurToRub:
                    urlToCurrency = "https://www.bloomberg.com/quote/EURRUB:CUR";
                    break;
                case CurrencyOperationType.EurToUsd:
                    urlToCurrency = "https://www.bloomberg.com/quote/EURUSD:CUR";
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
                response += $" {GetPostfix(type)}!";
            }

            return response;
        }

        private static string GetPostfix(CurrencyOperationType type)
        {
            switch (type)
            {
                case CurrencyOperationType.EurToUsd:
                    return "#СлаваТрампу!";
                default:
                    return "#СлаваРоссии!";
            }
        }

        private static CurrencyOperationType GetOperationType(string message)
        {
            var lowerMessage = message.ToLower();
            var rubExists = RubWords.Any(lowerMessage.Contains);
            var eurExists = EurWords.Any(lowerMessage.Contains);
            var usdExists = UsdWords.Any(lowerMessage.Contains);

            if (rubExists && usdExists)
            {
                return CurrencyOperationType.UsdToRub;
            }

            if (rubExists && eurExists)
            {
                return CurrencyOperationType.EurToRub;
            }

            if (usdExists && eurExists)
            {
                return CurrencyOperationType.EurToUsd;
            }

            if (rubExists)
            {
                return CurrencyOperationType.RubToEny;
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
            EurToUsd,
            RubToEny,
            None
        }
    }
}