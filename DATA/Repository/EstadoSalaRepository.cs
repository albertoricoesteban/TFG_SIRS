using Microsoft.EntityFrameworkCore;
using SIRS.Data.Context;
using SIRS.Domain.Interfaces;
using SIRS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRS.Data.Repository
{

    public class EstadoSalaRepository : Repository<EstadoSala>, IEstadoSalaRepository
    {
        public EstadoSalaRepository(ApplicationDbContext context)
        : base(context)
        {
        }

        // Obtener un EstadoSala por su Id con AsNoTracking
        public EstadoSala GetById(int id)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        // Método para agregar un nuevo EstadoSala
        public void Add(EstadoSala estadoSala)
        {
            _dbSet.Add(estadoSala);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para actualizar un EstadoSala existente
        public void Update(EstadoSala estadoSala)
        {
            _dbSet.Update(estadoSala);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para eliminar un EstadoSala por su Id
        public void Delete(int id)
        {
            var estadoSala = _dbSet.Find(id); // Busca el EstadoSala por Id
            if (estadoSala != null)
            {
                _dbSet.Remove(estadoSala); // Elimina el EstadoSala si existe
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Método para obtener todos los Estados de Sala
        public IEnumerable<EstadoSala> GetAllEstados()
        {
            return _dbSet.AsNoTracking().ToList();
        }

    }
}
