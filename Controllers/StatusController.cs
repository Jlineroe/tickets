using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class StatusController : Controller
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
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 9,true); //Estados
            Listas.TypesActions =await DAOCommand.ListStatus_TypeActions();
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
        public async Task<ActionResult> ListStatus()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            List<Sites> ListSites= await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 9,true); //Estados
            return PartialView(await DAOCommand.ListStatusDefinition(ListSites, null, null, 1));
        }
        public async Task<ActionResult> VerifyNameStatus(string NameStatus, int? pIdSitio = null)
        {
            DataTable dt =await DAOCommand.VerifyNameStatus(NameStatus,pIdSitio);
            if (dt.Rows.Count > 0)
            {
                return Json($"Estado {NameStatus} ya existe.", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DeleteStatus(int IdStatus)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.DeleteStatus(InforUser.IdMasterUsers, IdStatus);
            return new EmptyResult();
        }
        public async Task<ActionResult> SaveStatus(StatusDefinition Status)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            string mensaje = await DAOCommand.SaveStatusDefinition(InforUser.IdMasterUsers, Status);
            
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SearchInforStatus(int IdStatus)
        {
            Users InforUser = await DAOCommand.InforUserActual(true, true);
            List<StatusDefinition> Status = await DAOCommand.ListStatusDefinition(InforUser.Sitios, IdStatus,null,1);
            Status[0].SubStatus = await DAOCommand.ListStatusDefinition(InforUser.Sitios, null, IdStatus,2);
            Status[0].SubStatus= Status[0].SubStatus.Where(lq => lq.State == true).ToList();
            return Json(Status[0], JsonRequestBehavior.AllowGet);
        }

      

    }
}