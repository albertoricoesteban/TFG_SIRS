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

    public class RolRepository : Repository<Rol>, IRolRepository
    {
        public RolRepository(ApplicationDbContext context)
           : base(context)
        {
        }

        // Obtener un Rol por su Id con AsNoTracking
        public Rol GetById(int id)
        {
            return _dbSet.AsNoTracking()
                         .FirstOrDefault(r => r.Id == id);
        }

        // Método para agregar un nuevo Rol
        public void Add(Rol rol)
        {
            _dbSet.Add(rol);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para actualizar un Rol existente
        public void Update(Rol rol)
        {
            _dbSet.Update(rol);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para eliminar un Rol por su Id
        public void Delete(int id)
        {
            var rol = _dbSet.Find(id); // Busca el Rol por Id
            if (rol != null)
            {
                _dbSet.Remove(rol); // Elimina el Rol si existe
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Método para obtener todos los Roles
        public IEnumerable<Rol> GetAllRoles()
        {
            return _dbSet.AsNoTracking().ToList();
        }

    }
}
