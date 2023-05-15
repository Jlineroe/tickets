using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class GestionBOEController : Controller
    {
        public async Task<ActionResult> Index()
        {
            Users UserActual = await DAOCommand.InforUserActual(true);
            ViewBag.UserActual = UserActual.Nombres + ' ' + UserActual.PrimerApellido + ' ' + UserActual.SegundoApellido;
            return View();
        }
        public async Task<ActionResult> Gestion()
        {
            

            ViewBag.Segmento = await DAOCommand.ListDispositions(1004);
            //ViewBag.Tipologia = await DAOCommand.ListDispositions(1005);
            ViewBag.Estado = await DAOCommand.ListDispositions(1006);
            //ViewBag.Seguimiento = await DAOCommand.ListDispositions(1007);
            ViewBag.DetalleGestion = await DAOCommand.ListDispositions(1012);
            ViewBag.Canal = await DAOCommand.ListDispositions(1013);

            
            return PartialView();
        }

        public async Task<ActionResult> GetGestionBOE(GestionBOE gestion)
        {
            List<GestionBOE> Gestion = await DAOCommand.GetGestionBOE(gestion.NoSR);
            return Json(Gestion, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveGestionBOE(GestionBOE gestionOBE)
        {
            await DAOCommand.SaveGestionBOE(gestionOBE); 
            return new EmptyResult();
        }

    }
}