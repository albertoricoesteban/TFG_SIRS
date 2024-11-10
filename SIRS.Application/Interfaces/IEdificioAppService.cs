using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRS.Application.Interfaces
{
    public interface IEdificioAppService 
    {
        EdificioViewModel GetById(int id);
        IEnumerable<EdificioViewModel> GetAll();
        void Add(EdificioViewModel edificio);
        void Update(EdificioViewModel edificio);
        void Delete(int id);
        IEnumerable<EdificioViewModel> SearchByName(string name);
    }
}
