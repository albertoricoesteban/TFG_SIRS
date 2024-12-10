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

    public class EdificioRepository : Repository<Edificio>, IEdificioRepository
    {
        public EdificioRepository(ApplicationDbContext context)
       : base(context)
        {
        }

        // Obtener un Edificio por su Id
        public Edificio GetById(int id)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(c => c.Id == id);
        }

        // Obtener todos los Edificios
        public IEnumerable<Edificio> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        // Agregar un nuevo Edificio
        public void Add(Edificio edificio)
        {
            _dbSet.Add(edificio);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Actualizar un Edificio existente
        public void Update(Edificio edificio)
        {
            _dbSet.Update(edificio);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Eliminar un Edificio por su Id
        public void Delete(int id)
        {
            var edificio = _dbSet.Find(id);
            if (edificio != null)
            {
                _dbSet.Remove(edificio);
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Buscar edificios por un criterio (ejemplo: por nombre)
        public IEnumerable<Edificio> SearchByName(string name)
        {
            return _dbSet.AsNoTracking()
                         .Where(e => e.Descripcion.Contains(name))
                         .ToList();
        }

        public IEnumerable<Edificio> GetEdificiosByFilter(string descripcion, string direccion)
        {
            var query = _dbSet.AsQueryable(); if (!string.IsNullOrEmpty(descripcion)) { query = query.Where(e => e.Descripcion.ToUpper().Contains(descripcion.ToUpper())); }
            if (!string.IsNullOrEmpty(direccion)) { query = query.Where(e => e.Direccion.ToUpper().Contains(direccion.ToUpper())); }
            return query.ToList();
        }
    }
}
