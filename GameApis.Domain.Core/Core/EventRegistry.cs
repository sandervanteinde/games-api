namespace GameApis.Domain.Core;

internal class EventRegistry : IEventRegistry
{
    private readonly Dictionary<string, Type> _eventTypes = new Dictionary<string, Type>();

    public Type GetEventType(string eventName)
    {
        if (!_eventTypes.TryGetValue(eventName, out var type))
        {
            throw new ArgumentException($"The event type {eventName} does not exist.", nameof(eventName));
        }

        return type;
    }

    public void RegisterEventType(string eventName, Type eventType)
    {
        _eventTypes.Add(eventName, eventType);
    }
}
