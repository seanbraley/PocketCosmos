
using System;

public interface IView
{
    IViewController Controller { get; }
    void LogDebug(string message);
    void LogError(string message);
    void LogError(Exception e);
    void LogInfo(string message);
    void Disconnected(string message);
}

