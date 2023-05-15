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
    public class GroupsController : Controller
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
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles,8,true); //Grupos
            Listas.Grupos = await DAOCommand.ListGroups(Listas.Sitios);
            Listas.LineasNegocios = await DAOCommand.ListLOB();
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
        public async Task<ActionResult> ListGroups()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            List<Sites> ListSitio = await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 8,true); //Grupos
            return PartialView(await DAOCommand.ListGroups(ListSitio));
        }
        public async Task<ActionResult> SaveGroup(Groups Groups)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.SaveGroup(InforUser.IdMasterUsers, Groups);
            return new EmptyResult();
        }
        public async Task<ActionResult> DeleteGroups(int IdGroup)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.DeleteGroups(InforUser.IdMasterUsers, IdGroup);
            return new EmptyResult();
        }
        public async Task<ActionResult> GroupsJson(int IdGroup)
        {
            Users InforUser = await DAOCommand.InforUserActual(true, true);
            List<Groups> ListGrupos = await DAOCommand.ListGroups(InforUser.Sitios ,IdGroup);
            Groups NewGroup = ListGrupos[0];
            //NewGroup.UsersXGroups = DAOCommand.UsersGroups(IdGroup);
            NewGroup.GruposAEscalar = await DAOCommand.GruposAEscalar(IdGroup);
            NewGroup.TypesScaled = await DAOCommand.TypesScaledXGroups(IdGroup);
            return Json(NewGroup, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> VerifyNameGroup(string NameGroup)
        {
            DataTable dt = await DAOCommand.VerifyNameGroup(NameGroup);
            if (dt.Rows.Count > 0)
            {
                return Json($"Grupo {NameGroup} ya existe.");
            }
            return Json(true);
        }
        public async Task<ActionResult> VerifyNameTipoEscalamiento(int IdGroups, string TipoEscalamiento)
        {
            DataTable dt = await DAOCommand.VerifyNameTipoEscalamiento(IdGroups, TipoEscalamiento);
            if (dt.Rows.Count > 0)
            {
                return Json(false);
            }
            return Json(true);
        }
        public async Task<ActionResult> DeleteTipoEscala(int IdGroups, string TipoEscalamiento)
        {
            await DAOCommand.DeleteTipoEscala(IdGroups, TipoEscalamiento);
            return new EmptyResult();
        }

    }
}