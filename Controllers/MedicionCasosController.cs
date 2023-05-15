using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using AIBTicketsMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace AIBTicketsMVC.Controllers
{
    public class MedicionCasosController : Controller
    {
        // GET: MedicionCasos
        public async Task<ActionResult> Index()
        {
            await Tools.LogAplications("Ingreso", "Bitacora Gestion");
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
            //PAGINACION
            try
            {
                await Tools.LogAplications("Ingreso", "Bitacora Gestion");
                FiltersWorkOrder Filters = new FiltersWorkOrder
                {
                    pag = 1,
                    top = 10
                };
                ViewModelListOrders ViewModel = new ViewModelListOrders();
                ViewModel.ListWorkOrder = await DAOCommand.ListWorkOrderNew(UserActual, Filters);
                ViewModel.Pagination.TotalRegis = await DAOCommand.CountWorkOrder(UserActual, Filters);
                ViewModel.Pagination.PaginaActual = Filters.pag;
                ViewModel.Pagination.RegisXPagina = Filters.top;
                if (ViewModel.Pagination.TopDefault.Max() < ViewModel.Pagination.TotalRegis)
                {
                    ViewModel.Pagination.TopDefault.Add(ViewModel.Pagination.TotalRegis);
                }
                return View(ViewModel);
            }
            catch (Exception ex)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ERROR",
                    DetalleError = @"Vaya algo ha salido mal, intenta de nuevo o contacte con el area de soporte.</br>
                    Error: " + ex.Message
                });
            }
        }

        public ActionResult Gestion()
        {
            return View();
        }

        public ActionResult Historico()
        {
            return View();
        }
    }
}