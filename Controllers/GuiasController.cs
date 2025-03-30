using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class GuiasController : Controller
{
    private readonly AppDbContext _context;

    public GuiasController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> guias()
    {
        var guias = await _context.Usuarios
            .Where(u => u.Tipo_Usuario == "Guía")  
            .ToListAsync();

        return View(guias);
    }
}
