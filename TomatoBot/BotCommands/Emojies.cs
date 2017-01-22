﻿namespace TomatoBot.BotCommands
{
    public static class Emojies
    {
        public const string UsaFlag = "🇺🇸";
        public const string RussianFlag = "🇷🇺";
        public const string GermanyFlag = "🇩🇪";
        public const string EnglishFlag = "🇬🇧";
        public const string IrelandFlag = "🇮🇪";
        public const string KoreaFlag = "🇰🇷";
        public const string JapanFlag = "🇯🇵";

        public static string GetFlag(string country)
        {
            switch (country)
            {
                case "en":
                    return EnglishFlag;
                case "ru":
                    return RussianFlag;
                case "de":
                    return GermanyFlag;
                case "ko":
                    return KoreaFlag;
                default:
                    return country;
            }
        }
    }
}