namespace TomatoBot.BotCommands
{
    public interface ICommandWithHelpLine
    {
        string CommandName { get; }

        string Description { get; }

        string Sample { get; }
    }
}
