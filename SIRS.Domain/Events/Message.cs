using System;

using MediatR;

namespace SIRS.Domain.Events;

public abstract class Message : IRequest<bool>
{
    public string MessageType { get; protected set; }

    public Guid AggregateId { get; protected set; }

    protected Message()
    {
        MessageType = GetType().Name;
    }
}
