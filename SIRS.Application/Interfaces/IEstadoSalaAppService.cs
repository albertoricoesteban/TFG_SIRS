using SIRS.Application.ViewModels;
using System.Collections.Generic;

namespace SIRS.Application.Interfaces
{
    public interface IEstadoSalaAppService
    {
        EstadoSalaViewModel GetById(int id);
        void Add(EstadoSalaViewModel estadoSala);
        void Update(EstadoSalaViewModel estadoSala);
        void Delete(int id);
        IEnumerable<EstadoSalaViewModel> GetAllEstados();
    }
}
