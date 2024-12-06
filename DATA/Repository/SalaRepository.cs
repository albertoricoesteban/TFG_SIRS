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

    public class SalaRepository : Repository<Sala>, ISalaRepository
    {
        public SalaRepository(ApplicationDbContext context)
       : base(context)
        {
        }
        // Obtener una Sala por su Id con AsNoTracking
        public Sala GetById(int id)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(s => s.Id == id);
        }

        // Método para agregar una nueva Sala
        public void Add(Sala sala)
        {
            _dbSet.Add(sala);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para actualizar una Sala existente
        public void Update(Sala sala)
        {
            _dbSet.Update(sala);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para eliminar una Sala por su Id
        public void Delete(int id)
        {
            var sala = _dbSet.Find(id); // Busca la Sala por Id
            if (sala != null)
            {
                _dbSet.Remove(sala); // Elimina la Sala si existe
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Método personalizado para buscar Salas por un criterio (descripcion)
        public IEnumerable<Sala> SearchByDescripcion(string descripcion)
        {
            return _dbSet.AsNoTracking()
                         .Where(s => s.Descripcion.Contains(descripcion))
                         .ToList();
        }

        // Método personalizado para obtener Salas por estado sala
        public IEnumerable<Sala> GetByEstado(string estado)
        {
            return _dbSet.AsNoTracking()
                     .Include(s => s.EstadoSala) // Incluye la relación con EstadoSala
                     .Where(s => s.EstadoSala.Descripcion == estado) // Filtra por el nombre del estado
                     .ToList();
        }
        // Método para obtener todos las salas
        public IEnumerable<Sala> GetAllSalas()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public IEnumerable<Sala> GetSalasByFilter(string nombreCorto, int capacidad, int edificioId)
        {
            var query = _dbSet.AsQueryable(); if (!string.IsNullOrEmpty(nombreCorto)) { query = query.Where(e => e.Descripcion.Contains(nombreCorto)); }
            if (capacidad>0) { query = query.Where(e => e.Capacidad.Equals(capacidad)); }
            if (edificioId> 0) { query = query.Where(e => e.EdificioId.Equals(edificioId)); }
            return query.ToList();
        }
        // Método para obtener las salas con el nombre del edificio entre paréntesis 
        public IEnumerable<Sala> GetSalasByFilterWithEdificioNombre()
        {
            return _dbSet.AsNoTracking()
                   .Include(s => s.Edificio) // Incluye la relación con EstadoSala
                   .ToList();
        }

        // Método personalizado para obtener Salas por edificio
        public IEnumerable<Sala> GetByEdificioId(int EdificioId)
        {
            return _dbSet.AsNoTracking()
                     .Include(s => s.EstadoSala) // Incluye la relación con EstadoSala
                     .Where(s => s.EstadoSala.Id == 1 && s.EdificioId== EdificioId) // Filtra por sala disponible y edificioId
                     
                     .ToList();
        }
    }
} 
