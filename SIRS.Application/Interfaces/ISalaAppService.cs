using SIRS.Application.ViewModels;
using System.Collections.Generic;

namespace SIRS.Application.Interfaces
{
    public interface ISalaAppService
    {
        SalaViewModel GetById(int id);
        void Add(SalaViewModel sala);
        void Update(SalaViewModel sala);
        void Delete(int id);
        IEnumerable<SalaViewModel> SearchByDescripcion(string descripcion);
        IEnumerable<SalaViewModel> GetByEstado(string estado);
        IEnumerable<SalaViewModel> GetAll();
        IEnumerable<SalaViewModel> GetSalasByFilter(string nombreCorto, int capacidad, int edificioId);
        IEnumerable<SalaViewModel> GetByEdificioId(int edificioId);
        IEnumerable<SalaViewModel> GetSalasByFilterWithEdificioNombre();
    }
}
