namespace JsonHelper.UserInterface
{
    public interface ICommandsExecutor
    {
        ConsoleCommand FindCommandByName(string name);
        string[] GetAvailableCommandName();
        Task Execute(string[] args);
    }
}