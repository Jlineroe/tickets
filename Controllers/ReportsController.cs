using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class ReportsController : Controller
    {
        public async Task<ActionResult> Index()
        {
            Users UserActual = await DAOCommand.InforUserActual(true);
            if (UserActual == null)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este aplicativo."
                });
            }
            var PermisoExportar = await DAOCommand.ListPermisos(UserActual.Perfiles, 27); //Exportar reportes
            if (PermisoExportar.Count == 0)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este formulario."
                });
            }
            ListasDesplegables Listas = new ListasDesplegables();
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 27, false); //Exportar reportes
            Listas.ListTemplates = await DAOCommand.ListTemplates(null,true, Listas.Sitios);
            Listas.Grupos = await DAOCommand.ListGroups(Listas.Sitios, null,true);
            Listas.Estados = await DAOCommand.ListStatusDefinition(Listas.Sitios, null,null, 1, true);
            return View(Listas);
        }

        public async Task<ActionResult> ExportarExcel(FormCollection Params)
        {
            try
            {
                string Status =(Params["DdlStatus"]==null ? "" : Params["DdlStatus"]);
                string Groups = (Params["DdlGroups"] == null ? "" : Params["DdlGroups"]); 
                FiltersReports Filters = new FiltersReports { 
                    FechaInicio= Params["TxtInicio"],
                    FechaFin= Params["TxtFin"],
                    ArrayStatus = Status.Split(','),
                    ArrayGroups= Groups.Split(','),
                    IdTemplates= int.Parse(Params["DdlPlantilla"] == null ? "0" : Params["DdlPlantilla"])
                };
                Users UserActual = await DAOCommand.InforUserActual(true,true);
                List<DataTable> ListDT = await DAOCommand.ListDTFullWorkOrder(UserActual, Filters);
                XLWorkbook DocExcel = new XLWorkbook();
                foreach(var dt in ListDT)
                {
                    DocExcel = Tools.ConvertDataTableXExcel(DocExcel, dt, dt.TableName);
                }
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ReporteTickets.xlsx");

                MemoryStream MyMemoryStream = new MemoryStream();
                DocExcel.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index");
        }
    }
}