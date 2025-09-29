using System;

public interface IGameConsole : IService
{
    void ShowMessage(string _msg);
    void AddCommand(CGameConsoleCommand _command);
    void RemoveCommand(string _cmdName);
    void ExecuteCommand(string _cmd);
    void Show();
    void Hide();
}