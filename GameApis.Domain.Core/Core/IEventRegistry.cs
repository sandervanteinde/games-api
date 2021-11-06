namespace GameApis.Domain.Core;

public interface IEventRegistry
{
    Type GetEventType(string eventName);
    void RegisterEventType(string eventName, Type eventType);
}