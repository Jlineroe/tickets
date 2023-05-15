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
    public class DataDisabledController : Controller
    {
        public async Task<ActionResult> Index()
        {
            Users UserActual = await DAOCommand.InforUserActual(true,true,true);
            if (UserActual == null)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este aplicativo."
                });
            }
            var PermisoDesactivar = await DAOCommand.ListPermisos(UserActual.Perfiles, 31); //Desactivar datas
            if (PermisoDesactivar.Count == 0)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este formulario."
                });
            }
            List<WorkOrder_DataImported> ListData = await DAOCommand.ListDataImported(UserActual.Grupos);
            return View(ListData);
        }
        public async Task<ActionResult> DesactivarDatas(int IdDataImported,string DesactivationReason)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.DisabledDataImport(IdDataImported, DesactivationReason, UserActual.IdMasterUsers);
            return new EmptyResult();
        }
    }
}