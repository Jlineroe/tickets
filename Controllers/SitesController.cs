using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIBTicketsMVC.Models;
using AIBTicketsMVC.App_Code;
using System.Threading.Tasks;
using System.Data;

namespace AIBTicketsMVC.Controllers
{
    public class SitesController : Controller
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
            return View();
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
        public async Task<ActionResult> ListSitios() => PartialView(await DAOCommand.ListSitios());
        public async Task<ActionResult> SitiosJson(int IdSite)
        {
            List<Sites> Sitios= await DAOCommand.ListSitios(IdSite);
            return Json(Sitios[0], JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GuardarSitio(Sites Sitios)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            DAOCommand.SaveSitio(UserActual.IdMasterUsers, Sitios);
            return new EmptyResult();
        }
        public async Task<ActionResult> UpdateSitios(Sites Sitios)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            DAOCommand.UpdateSitio(UserActual.IdMasterUsers, Sitios);
            return new EmptyResult();
        }
        public async Task<ActionResult> DesactivarSitios(int IdSitio)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            DAOCommand.DeleteSitio(UserActual.IdMasterUsers, IdSitio);
            return new EmptyResult();
        }
        public async Task<ActionResult> VerifyNameSitio(string NameSitio)
        {
            DataTable dt = await DAOCommand.VerifyNameSitio(NameSitio);
            if (dt.Rows.Count > 0)
            {
                return Json($"El sitio {NameSitio} ya existe.");
            }
            return Json(true);
        }
    }
}