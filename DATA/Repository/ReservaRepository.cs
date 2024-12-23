﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public IEnumerable<Reserva> GetReservasByFilters(int salaId, DateTime? fechaReserva, TimeSpan? horaInicio, int? usuarioId)
        {
            var query = _dbSet
                .Include(r => r.Sala) // Incluye los datos de la sala
                .AsQueryable();


            // Filtrar por sala si salaId es mayor que 0
            if (salaId > 0)
            {
                query = query.Where(r => r.Sala.Id == salaId);
            }

            // Filtrar por FechaReserva si está presente
            if (fechaReserva.HasValue)
            {
                query = query.Where(r => r.FechaReserva == fechaReserva.Value);
            }

            // Filtrar por HoraInicio si está presente
            if (horaInicio.HasValue)
            {
                query = query.Where(r => r.HoraInicio == horaInicio.Value);
            }
            // Filtramos por usuarioId si no es nulo
            if (usuarioId.HasValue)
            {
                query = query.Where(r => r.UsuarioId == usuarioId.Value);
            }

            return query.ToList();
        }
        public IEnumerable<Reserva> ObtenerReservasCalendario(DateTime fechaInicio, DateTime fechaFin, int? usuarioId, int? salaId=null)
        {
            var query = _dbSet
                .AsQueryable();


            // Filtramos por el intervalo de fechas

            query = query.Where(r => r.FechaReserva >= fechaInicio && r.FechaReserva <= fechaFin && r.Aprobada!=false);
            // Filtramos por usuarioId si no es nulo
            if (usuarioId.HasValue)
            {
                query = query.Where(r => r.UsuarioId == usuarioId.Value);
            }
            if (salaId.HasValue)
            {
                query = query.Where(r => r.SalaId == salaId.Value);
            }

            return query.ToList();
        }
        
        public void CancelarReserva(int id, int usuarioGestionId)
        {
            // Buscar la reserva con el id proporcionado
            var reserva = _dbSet.FirstOrDefault(r => r.Id == id);

            // Verificar que la reserva exista y que no esté aprobada
            if (reserva != null && reserva.Aprobada == null)
            {
                // Cambiar el estado de la reserva a "Cancelada"
                reserva.FechaBaja = DateTime.Now; // O lo que corresponda según la lógica de tu aplicación
                reserva.Aprobada = false;
                reserva.UsuarioGestionId = usuarioGestionId;

                // Guardar los cambios en la base de datos
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }
        public void ReactivarReserva(int id, int usuarioGestionId)
        {
            // Buscar la reserva con el id proporcionado
            var reserva = _dbSet.FirstOrDefault(r => r.Id == id);

            // Verificar que la reserva exista 
            if (reserva != null )
            {
                // Cambiar el estado de la reserva a "Cancelada"
                reserva.FechaBaja = null; // O lo que corresponda según la lógica de tu aplicación
                reserva.Aprobada = null;
                reserva.UsuarioGestionId = usuarioGestionId;

                // Guardar los cambios en la base de datos
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }
        public void AprobarReserva(int id, int usuarioGestionId)
        {
            // Buscar la reserva con el id proporcionado
            var reserva = _dbSet.FirstOrDefault(r => r.Id == id);

            // Verificar que la reserva exista 
            if (reserva != null)
            {
                // Cambiar el estado de la reserva a "Cancelada"
                reserva.FechaBaja = null; // O lo que corresponda según la lógica de tu aplicación
                reserva.Aprobada = true;
                reserva.UsuarioGestionId = usuarioGestionId;

                // Guardar los cambios en la base de datos
                _db.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }
    }
}
