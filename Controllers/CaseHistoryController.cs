using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class CaseHistoryController : Controller
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
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 27, false); //Exportar reportes
            Listas.ListTemplates = await DAOCommand.ListTemplates(null, true, Listas.Sitios);
            Listas.Grupos = await DAOCommand.ListGroups(Listas.Sitios, null, true);
            Listas.Estados = await DAOCommand.ListStatusDefinition(Listas.Sitios, null, null, 1, true);
            return View(Listas);
        }
        public async Task<ActionResult> ListCaseHistory(string ESTADO = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CaseHistorySummary> ListCaseHistory = new List<CaseHistorySummary>();
            if ((ESTADO != "" && ESTADO != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
              ListCaseHistory = await DAOCommand.ListCaseHistory(ESTADO, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        //public async Task<ActionResult> ListTimeGroup()
        //{
        //    Users InforUser = await DAOCommand.InforUserActual(true);
        //    List<Sites> Sitios = await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 10, true); //Categorias
        //    return PartialView(await DAOCommand.ListSemaphore(Sitios, null, null));
        //}

        //public async Task<ActionResult> SelWorkOrderAssigned(WorkOrder_Assigned Params1)
        //{
        //    Users InforUser = await DAOCommand.InforUserActual(true);
        //    List<WorkOrder_Assigned> ListData = await DAOCommand.SelWorkOrderAssigned(Params1, InforUser.IdMasterUsers);
        //    return Json(ListData, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<ActionResult> TimeList()
        //{
        //    Users UserActual = await DAOCommand.InforUserActual(true);
        //    List<TimeList> ListData = await DAOCommand.SelListaTiempos(UserActual);
        //    return PartialView(ListData);
        //}

        //public async Task<ActionResult> SaveWorkOrderAssigned(long IdWorkOrder, int IdStatusDefinition, int IdMasterGroups)
        //{
        //    Users InforUser = await DAOCommand.InforUserActual(true);
        //    await DAOCommand.SaveWorkOrderAssigned(InforUser.IdMasterUsers, IdWorkOrder, IdStatusDefinition, IdMasterGroups);
        //    return new EmptyResult();
        //}



    }
}