using SIRS.Domain.Commands;
using SIRS.Domain.Events;

namespace SIRS.Domain.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command)
            where T : Command;

        Task RaiseEvent<T>(T @event)
            where T : Event;
    }

}
