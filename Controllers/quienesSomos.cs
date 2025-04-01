using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pHelloworld.Filtros;

namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarNotificacionesFiltro))]
    [ServiceFilter(typeof(CargarMensajesFiltro))]
    public class quienesSomos : Controller
    {
        // GET: quienesSomos
        public ActionResult nuestraMision()
        {
            return View("~/Views/quienesSomos/nuestraMision.cshtml");

        }

        public ActionResult nuestraVision()
        {
            return View("~/Views/quienesSomos/nuestraVision.cshtml");

        }

        public ActionResult nuestraHistoria()
        {
            return View("~/Views/quienesSomos/nuestraHistoria.cshtml");

        }

        public ActionResult contactanos()
        {
            return View("~/Views/quienesSomos/contactanos.cshtml");

        }

    }
}
