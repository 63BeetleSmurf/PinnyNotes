using System.Collections.Concurrent;

namespace PinnyNotes.WpfUi.Services;

public class MessengerService
{
    private readonly ConcurrentDictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<TMessage>(Action<TMessage> handler)
    {
        Type messageType = typeof(TMessage);
        _subscribers.AddOrUpdate(
            messageType,
            _ => [handler],
            (_, handlers) => {
                lock (handlers)
                {
                    handlers.Add(handler);
                    return handlers;
                }
            }
        );
    }

    public void Unsubscribe<TMessage>(Action<TMessage> handler)
    {
        Type messageType = typeof(TMessage);
        if (_subscribers.TryGetValue(messageType, out var handlers))
            lock (handlers)
                handlers.Remove(handler);
    }

    public void Publish<TMessage>(TMessage message)
    {
        Type messageType = typeof(TMessage);
        if (_subscribers.TryGetValue(messageType, out var handlers))
        {
            List<Delegate> snapshot;
            lock (handlers)
                snapshot = [..handlers];

            foreach (Delegate? handler in snapshot)
                ((Action<TMessage>)handler)(message);
        }
    }
}
