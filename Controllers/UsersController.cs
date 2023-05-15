using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class UsersController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ListasDesplegables Listas = new ListasDesplegables();
            try { 
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
                Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 7, true); //Usuarios
                Listas.Grupos = await DAOCommand.ListGroups(Listas.Sitios,null,true);
                Listas.Perfiles = await DAOCommand.ListProfile(Listas.Sitios, null,true);
                Listas.Usuarios = await DAOCommand.ListUsersBaseUnificada();
            }
            catch (Exception ex)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ERROR",
                    DetalleError = @"Vaya algo ha salido mal, intenta de nuevo o contacte con el area de soporte.</br>
                    Error: " + ex.Message
                }) ;
            }
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
        public async Task<ActionResult> ListUsers()
        {
            Users UserActual = await DAOCommand.InforUserActual(true);
            List<Sites> ListSites = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 7,true); //Menu Usuarios
            return PartialView(await DAOCommand.ListUsers(ListSites));
        }
        public async Task<ActionResult> ListUsersJson(long IdUsers)
        {

            Users UserActual = await DAOCommand.InforUserActual(true);
            List<Sites> ListSites = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 7, true); //Menu Usuarios
            List<Users> Users= await DAOCommand.ListUsers(ListSites, IdUsers);
            return Json(Users[0], JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SaveUsers(List<Users> Usuarios, Users UpdUsers)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.SaveUsers(InforUser.IdMasterUsers, Usuarios, UpdUsers);
            return new EmptyResult();
        }
        public async Task<ActionResult> DeleteUsuarios(long IdMasterUser)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.DeleteUsers(InforUser.IdMasterUsers, IdMasterUser);
            return new EmptyResult();
        }
    }
}