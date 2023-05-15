using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class ProfilesController : Controller
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
            List<Sites> ListSites = await DAOCommand.ListSitios();
            ListSites = ListSites.Where(lq => lq.State == true).ToList();
            return View(ListSites);
        }
         
        public async Task<ActionResult> ListProfiles() 
        {
            Users UserActual = await DAOCommand.InforUserActual(true);
            List<Sites> ListSites = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles,6,true); //Menu de perfiles
            return PartialView(await DAOCommand.ListProfile(ListSites)); 
        }
        public async Task<ActionResult> ModalTreeListPermisos() => PartialView(await DAOCommand.MainMenu());
        public async Task<ActionResult> PermisosActions()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            List<MenuAndActions> Permisos = await DAOCommand.ListPermisos(InforUser.Perfiles);
            string ControladorActual = ControllerContext.RouteData.Values["controller"].ToString();
            MenuAndActions FormActual = Permisos.Where(Linq => Linq.Permiso == 1 & Linq.Controller == ControladorActual).FirstOrDefault();
            Permisos = Permisos.Where(Linq => Linq.Parent_IdMenu == FormActual.IdMasterMenu & Linq.Level == 0 & Linq.Permiso == 0).ToList();
            return Json(Permisos, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> VerifyNameProfile(string NameProfile)
        {
            DataTable dt = await DAOCommand.VerifyNameProfile(NameProfile);
            if (dt.Rows.Count > 0)
            {
                return Json($"Perfil {NameProfile} ya existe.");
            }
            return Json(true);
        }
        public async Task<ActionResult> SaveProfile(Profiles Perfil)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.SaveProfile(InforUser.IdMasterUsers, Perfil);
            return new EmptyResult();
        }
        public async Task<ActionResult> UpdateProfile(Profiles Perfil)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.UpdateProfile(InforUser.IdMasterUsers, Perfil);
            return new EmptyResult();
        }
        public async Task<ActionResult> ListProfilesJson(int IdProfile)
        {
            Users InforUser = await DAOCommand.InforUserActual(true, true);
            List<Profiles> ListProfile = await DAOCommand.ListProfile(InforUser.Sitios, IdProfile);
            Profiles Perfil = ListProfile[0];
            Perfil.Menu = await DAOCommand.ListPermisos(ListProfile);
            return Json(Perfil, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DeleteProfile(int IdMasterProfiles)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.DeleteProfile(InforUser.IdMasterUsers, IdMasterProfiles);
            return new EmptyResult();
        }
    }
}