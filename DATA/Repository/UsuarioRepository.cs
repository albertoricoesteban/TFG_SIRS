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
        public IEnumerable<Usuario> SearchByFilter(string? username = null, string? nombre = null, string? apellido1 = null, string? apellido2 = null, string? email = null, DateTime? fechaRegistro = null, int? rolId = null)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            // Filtro por username, si es proporcionado
            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(u => u.Username.Contains(username));
            }

            // Filtro por apellido1, si es proporcionado
            if (!string.IsNullOrEmpty(apellido1))
            {
                query = query.Where(u => u.Apellido1.Contains(apellido1));
            }

            // Filtro por apellido2, si es proporcionado
            if (!string.IsNullOrEmpty(apellido2))
            {
                query = query.Where(u => u.Apellido2.Contains(apellido2));
            }

            // Filtro por email, si es proporcionado
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email.Contains(email));
            }

            // Filtro por fecha de registro, si es proporcionada
            if (fechaRegistro.HasValue)
            {
                query = query.Where(u => u.FechaRegistro == fechaRegistro.Value);
            }

            // Filtro por rolId, si es proporcionado
            if (rolId.HasValue)
            {
                query = query.Where(u => u.RolId == rolId.Value);
            }

            return query.ToList();
        }
        public bool UserExistsByUsername(string username)
        {
            return _dbSet.AsNoTracking().Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
        public bool UserExistsByEmail(string email)
        {
            return _dbSet.AsNoTracking().Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
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
