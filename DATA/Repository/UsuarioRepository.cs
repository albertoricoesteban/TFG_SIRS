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

    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context)
       : base(context)
        {
        }

        // Obtener un Usuario por su Id con AsNoTracking
        public Usuario GetById(int id)
        {
            return _dbSet.AsNoTracking()
                         .Include(u => u.Rol) // Incluye la relación con Rol
                         .FirstOrDefault(u => u.Id == id);
        }

        // Método para agregar un nuevo Usuario
        public void Add(Usuario usuario)
        {
            _dbSet.Add(usuario);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para actualizar un Usuario existente
        public void Update(Usuario usuario)
        {
            _dbSet.Update(usuario);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para eliminar un Usuario por su Id
        public void Delete(int id)
        {
            var usuario = _dbSet.Find(id); // Busca el Usuario por Id
            if (usuario != null)
            {
                _dbSet.Remove(usuario); // Elimina el Usuario si existe
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Método personalizado para buscar Usuarios por nombre
        public IEnumerable<Usuario> SearchByName(string name)
        {
            return _dbSet.AsNoTracking()
                         .Where(u => u.Nombre.Contains(name))
                         .ToList();
        }

        // Método para obtener Usuarios por Rol
        public IEnumerable<Usuario> GetByRol(string rolNombre)
        {
            return _dbSet.AsNoTracking()
                         .Include(u => u.Rol) // Incluye la relación con Rol
                         .Where(u => u.Rol.Nombre == rolNombre) // Filtra por el nombre del Rol
                         .ToList();
        }
        // Método para obtener todos los usuarios
        public IEnumerable<Usuario> GetAllUsuarios()
        {
            return _dbSet.AsNoTracking().ToList();
        }
    }
}
