using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class SemaphoreController : Controller
    {
        public async Task<ActionResult> Index()
        {
            string ControladorActual = ControllerContext.RouteData.Values["controller"].ToString();
            Users UserActual = await DAOCommand.InforUserActual(true);
            if (UserActual == null)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este aplicativo."
                });
            }
            bool Acceso = await DAOCommand.VerifyAccessForm(UserActual.Perfiles, ControladorActual);
            if (!Acceso)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este formulario."
                });
            }
            ListasDesplegables Listas = new ListasDesplegables();
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 10, true); //Categorias
            return View(Listas);
        }
        public async Task<ActionResult> PermisosActions()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            List<MenuAndActions> Permisos = await DAOCommand.ListPermisos(InforUser.Perfiles);
            string ControladorActual = ControllerContext.RouteData.Values["controller"].ToString();
            MenuAndActions FormActual = Permisos.Where(Linq => Linq.Permiso == 1 & Linq.Controller == ControladorActual).FirstOrDefault();
            Permisos = Permisos.Where(Linq => Linq.Parent_IdMenu == FormActual.IdMasterMenu & Linq.Level == 0 & Linq.Permiso == 0).ToList();
            return Json(Permisos, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ListSemaphore()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            List<Sites> Sitios = await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 10, true); //Categorias
            return PartialView(await DAOCommand.ListSemaphore(Sitios, null, null));
        }
        public async Task<ActionResult> ListSemaphoreJson(int? IdSemaphore=null, int? IdSubCategory=null)
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            List<Sites> Sitios = await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 10, true); //Categorias
            return Json(await DAOCommand.ListSemaphore(Sitios, IdSemaphore, IdSubCategory), JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SaveSemaphore(Semaphores Semaforo)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.SaveSemaphore(UserActual.IdMasterUsers, Semaforo);
            return new EmptyResult();
        }
        public async Task<ActionResult> DisabledEnabledSemaphore(int IdSemaphore, bool Activar)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.DisabledEnabledSemaphore(UserActual.IdMasterUsers, IdSemaphore, Activar);
            return new EmptyResult();
        }
        public async Task<ActionResult> VerifySiteSemaphore(Sites Sitio)
        {
            return Json(await DAOCommand.VerifySiteSemaphore(Sitio), JsonRequestBehavior.AllowGet);
        }
    }
}