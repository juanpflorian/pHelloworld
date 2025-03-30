using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pHelloworld.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using pHelloworld.Controllers; // Para acceder a _LayoutController
using pHelloworld.Filtros; 



namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarMensajesFiltro))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var layout = new _LayoutController(_context);
            layout.ControllerContext = this.ControllerContext;
            await layout.CargarMensajesEnViewBag();

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var layout = new _LayoutController(_context);
            layout.ControllerContext = this.ControllerContext;
            await layout.CargarMensajesEnViewBag();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
