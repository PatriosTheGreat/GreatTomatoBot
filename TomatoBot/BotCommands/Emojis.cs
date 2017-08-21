namespace TomatoBot.BotCommands
{
    public static class Emojis
    {
        public const string UsaFlag = "🇺🇸";
        public const string RussianFlag = "🇷🇺";
        public const string GermanyFlag = "🇩🇪";
        public const string EnglishFlag = "🇬🇧";
        public const string IrelandFlag = "🇮🇪";
        public const string KoreaFlag = "🇰🇷";
        public const string JapanFlag = "🇯🇵";
        public const string FrenchFlag = "🇫🇷";
	    public const string Envelop = "✉";
	    public const string Message = "📝";
	    public const string Attach = "📎";
	    public const string Smile = "😄";
	    public const string Statistics = "📈";

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
                case "fr":
                    return FrenchFlag;
                default:
                    return country;
            }
        }
    }
}