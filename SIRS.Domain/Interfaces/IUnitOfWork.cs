using System;

namespace SIRS.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    bool Commit();
}
