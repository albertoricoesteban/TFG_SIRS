﻿using Microsoft.EntityFrameworkCore;
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

    public class ReservaRepository : Repository<Reserva>, IReservaRepository
    {
        public ReservaRepository(ApplicationDbContext context)
          : base(context)
        {
        }

        // Obtener una Reserva por su Id con AsNoTracking
        public Reserva GetById(int id)
        {
            return _dbSet.AsNoTracking()
                         .Include(r => r.Sala) // Incluye la relación con Sala
                         .Include(r => r.Usuario) // Incluye la relación con Usuario
                         .FirstOrDefault(r => r.Id == id);
        }

        // Método para agregar una nueva Reserva
        public void Add(Reserva reserva)
        {
            _dbSet.Add(reserva);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para actualizar una Reserva existente
        public void Update(Reserva reserva)
        {
            _dbSet.Update(reserva);
            _db.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Método para eliminar una Reserva por su Id
        public void Delete(int id)
        {
            var reserva = _dbSet.Find(id); // Busca la Reserva por Id
            if (reserva != null)
            {
                _dbSet.Remove(reserva); // Elimina la Reserva si existe
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Método para obtener Reservas por Sala
        public IEnumerable<Reserva> GetBySala(int salaId)
        {
            return _dbSet.AsNoTracking()
                         .Include(r => r.Sala) // Incluye la relación con Sala
                         .Where(r => r.SalaId == salaId)
                         .ToList();
        }

        // Método para obtener Reservas por Usuario
        public IEnumerable<Reserva> GetByUsuario(int usuarioId)
        {
            return _dbSet.AsNoTracking()
                         .Include(r => r.Usuario) // Incluye la relación con Usuario
                         .Where(r => r.UsuarioId == usuarioId)
                         .ToList();
        }

        // Método para obtener todas las Reservas en una fecha específica
        public IEnumerable<Reserva> GetByFecha(DateTime fecha)
        {
            return _dbSet.AsNoTracking()
                         .Include(r => r.Sala) // Incluye la relación con Sala
                         .Include(r => r.Usuario) // Incluye la relación con Usuario
                         .Where(r => r.FechaReserva == fecha.Date)
                         .ToList();
        }

    }
}
