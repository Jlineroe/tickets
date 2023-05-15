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
    public class CasosPqrPlntMovilEscritaController : Controller
    {
        // GET: CasosPqrPlntMovilEscrita
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
        public async Task<ActionResult> ListCasePrepago(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCasePrepago(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        public async Task<ActionResult> ListCasePrepago13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCasePrepago13(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        public async Task<ActionResult> ListCasePospago(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCasePospago(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        public async Task<ActionResult> ListCasePospago13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCasePospago13(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        public async Task<ActionResult> ListCaseAscard(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCaseAscard(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        public async Task<ActionResult> ListCaseAscard13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCaseAscard13(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        public async Task<ActionResult> ListCaseCuotasAscard(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCaseCuotasAscard(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        public async Task<ActionResult> ListCaseCuotasAscard13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCaseCuotasAscard13(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }

        public async Task<ActionResult> ListCaseEliminacionCentrales(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCaseEliminacionCentrales(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
        public async Task<ActionResult> ListCaseEliminacionCentrales13(string Idsolutions = null, string Cuscode = null, string Fechainicio = null, string Fechafinal = null)
        {
            List<CasosPqrMovilEscrita> ListCaseHistory = new List<CasosPqrMovilEscrita>();
            if ((Idsolutions != "" && Idsolutions != null) || (Idsolutions != "" && Cuscode != null) || (Fechainicio != "" && Fechainicio != null) || (Fechafinal != null && Fechafinal != ""))
                ListCaseHistory = await DAOCommand.ListCaseEliminacionCentrales13(Idsolutions, Cuscode, Fechainicio, Fechafinal);
            return PartialView(ListCaseHistory);
        }
    }
}