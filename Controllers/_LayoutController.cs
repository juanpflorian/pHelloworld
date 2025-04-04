﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Filtros;
using pHelloworld.Models;
using System.Security.Claims;

namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarMensajesFiltro))]
    [ServiceFilter(typeof(CargarNotificacionesFiltro))] // Este es tu nuevo filtro
    public class _LayoutController : Controller
    {
        private readonly AppDbContext _context;

        public _LayoutController(AppDbContext context)
        {
            _context = context;
        }

        // Esto es solo para pruebas
        public async Task<IActionResult> Dummy()
        {
            await CargarMensajesEnViewBag();
            return View(); // puedes crear una Dummy.cshtml si quieres ver esto funcionar
        }

        public async Task CargarMensajesEnViewBag()
        {
            if (!User.Identity.IsAuthenticated) return;

            var idActual = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var mensajes = await _context.Mensaje
                .Where(m => m.IdReceptor == idActual)
                .GroupBy(m => m.IdEmisor)
                .Select(g => g.OrderByDescending(m => m.FechaEnvio).FirstOrDefault())
                .ToListAsync();

            var emisores = await _context.Usuarios
                .Where(u => mensajes.Select(m => m.IdEmisor).Contains(u.id_usuario))
                .ToListAsync();

            var lista = mensajes
                .Select(m => (Emisor: emisores.FirstOrDefault(u => u.id_usuario == m.IdEmisor), UltimoMensaje: m))
                .ToList();

            ViewBag.MensajesRecibidos = lista;
            ViewBag.CantidadMensajesNoLeidos = mensajes.Count(m => !m.Leido);
        }
    }
}
