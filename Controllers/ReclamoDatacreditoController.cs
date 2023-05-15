using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System.IO;

using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace AIBTicketsMVC.Controllers
{
    public class ReclamoDatacreditoController : Controller
    {
        async public Task<ActionResult> Index()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;

            return View();
        }
        //public ActionResult ListReclamos()
        public async Task<ActionResult> ListReclamos()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;
            if ((iSuper == "DATACREDITO") || (iSuper == "SUPERCENTRALES"))
            {
                List<ReclamoDatacredito> ListReclamosD = new List<ReclamoDatacredito>();
                ListReclamosD = DAOCommand.SelTabla(); //Plantillas
                DAOCommand objetoDispositionReclamo = new DAOCommand();

                List<Disposition> lstEstadoReclamo = objetoDispositionReclamo.ObtenerEstado();
                ViewBag.EstadoReclamo = new SelectList(lstEstadoReclamo, "Id", "Descripcion");

                List<Disposition> lstSubEstadoReclamo = objetoDispositionReclamo.ObtenerSubEstado();
                ViewBag.SubEst = new SelectList(lstSubEstadoReclamo, "Id", "Descripcion");

                return PartialView(ListReclamosD);
            }
            else
            {
                ViewBag.user = iSuper;
                List<ReclamoDatacredito> ListReclamosD = new List<ReclamoDatacredito>();
                ListReclamosD = DAOCommand.SelTablaTemporal(); //Plantillas
                DAOCommand objetoDispositionReclamo = new DAOCommand();

                List<Disposition> lstEstadoReclamo = objetoDispositionReclamo.ObtenerEstado();
                ViewBag.EstadoReclamo = new SelectList(lstEstadoReclamo, "Id", "Descripcion");

                List<Disposition> lstSubEstadoReclamo = objetoDispositionReclamo.ObtenerSubEstado();
                ViewBag.SubEst = new SelectList(lstSubEstadoReclamo, "Id", "Descripcion");

                return PartialView(ListReclamosD);
            }



        }
        public async Task<ActionResult> ListReclamosAgente()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;

            List<ReclamoDatacredito> ListReclamosD = new List<ReclamoDatacredito>();
            ListReclamosD = DAOCommand.SelTabla(); //Plantillas

            DAOCommand objetoDispositionReclamo = new DAOCommand();
            List<Disposition> lstEstadoReclamo = objetoDispositionReclamo.ObtenerEstado();
            ViewBag.EstadoReclamo = new SelectList(lstEstadoReclamo, "Id", "Descripcion");

            List<Disposition> lstSubEstadoReclamo = objetoDispositionReclamo.ObtenerSubEstado();
            ViewBag.SubEst = new SelectList(lstSubEstadoReclamo, "Id", "Descripcion");

            return PartialView(ListReclamosD);
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);


                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }



                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);



                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;



                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();



                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }



                    conString = ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "Tbl_ReclamoDatacredito";

                            //[OPTIONAL]: Map the Excel columns with that of the database table
                            sqlBulkCopy.ColumnMappings.Add("Número ID", "Numero_ID");
                            sqlBulkCopy.ColumnMappings.Add(" Tipo Identificación", "Tipo_Identificacion");
                            sqlBulkCopy.ColumnMappings.Add(" Nombre", "Nombre");
                            sqlBulkCopy.ColumnMappings.Add(" Entidad", "Entidad");
                            sqlBulkCopy.ColumnMappings.Add(" NIT", "NIT");
                            sqlBulkCopy.ColumnMappings.Add(" Número de Cuenta", "No_Cuenta");
                            sqlBulkCopy.ColumnMappings.Add(" Número de Reclamo", "No_Reclamo");
                            sqlBulkCopy.ColumnMappings.Add(" Reclamo Entidad", "Reclamo_Entidad");
                            sqlBulkCopy.ColumnMappings.Add(" Estado", "Estado");
                            sqlBulkCopy.ColumnMappings.Add(" Tipo Reclamo", "Tipo_Reclamo");
                            sqlBulkCopy.ColumnMappings.Add(" Subtipo Reclamo", "Subtipo_Reclamo");
                            sqlBulkCopy.ColumnMappings.Add(" Leyenda Reclamo", "Leyenda_Reclamo");
                            sqlBulkCopy.ColumnMappings.Add(" Tipo de Solución", "Tipo_Solucion");
                            sqlBulkCopy.ColumnMappings.Add("Fecha Colocación", "Fecha_Colocacion");
                            sqlBulkCopy.ColumnMappings.Add(" Fecha Aplicación", "Fecha_Aplicacion");
                            sqlBulkCopy.ColumnMappings.Add(" Canal", "Canal");
                            sqlBulkCopy.ColumnMappings.Add(" Origen", "Origen");
                            sqlBulkCopy.ColumnMappings.Add("EMPRESA ORIGEN", "Empresa_Origen");
                            sqlBulkCopy.ColumnMappings.Add("ESTADO A TRAMITAR", "Estado_Robot");

                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                }

                return View();
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

        public async Task<ActionResult> SaveTablaProduccion()
        {
            bool data = true;

            try
            {
                data = await DAOCommand.SaveTablaProduccion();

            }
            catch (Exception ex)
            {
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> LimpiarTabla(HttpPostedFileBase postedFile)
        {
            bool data = true;
            try
            {

                data = await DAOCommand.TruncateTable();
            }
            catch (Exception ex)
            {
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //UpdateEstado
        public async Task<ActionResult> UpdateEstado(ReclamoDatacredito Reclamo)
        {
            try
            {
                await DAOCommand.UpdateEstado(Reclamo);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                //Tools.SendMailExceptionXHilo(Tools.MsjError("TemplatesController.SaveTemplate", ex)).Start();
                throw new Exception();
            }
        }

        public async Task<ActionResult> FormEditEstados()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;

            List<ReclamoDatacredito> ListReclamosD = new List<ReclamoDatacredito>();
            ListReclamosD = DAOCommand.SelTabla(); //Plantillas

            DAOCommand objetoDispositionReclamo = new DAOCommand();
            List<Disposition> lstEstadoReclamo = objetoDispositionReclamo.ObtenerEstado();
            ViewBag.EstadoReclamo = new SelectList(lstEstadoReclamo, "Id", "Descripcion");

            List<Disposition> lstSubEstadoReclamo = objetoDispositionReclamo.ObtenerSubEstado();
            ViewBag.SubEst = new SelectList(lstSubEstadoReclamo, "Id", "Descripcion");

            return PartialView(ListReclamosD);
        }

        public async Task<ActionResult> ListEstados()
        {
            DAOCommand objetoDispositionReclamo = new DAOCommand();
            Users UserActual = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(UserActual);
            ViewBag.user = iSuper;
            List<Disposition> lstAllEstado = objetoDispositionReclamo.ObtenerAllEstados();
            return PartialView(lstAllEstado);

        }

        public async Task<ActionResult> ResulEstados(int Id)
        {
            DAOCommand objetoDispositionReclamo = new DAOCommand();
            Users UserActual = await DAOCommand.InforUserActual(true);
            List<Disposition> lstAllEstado = await DAOCommand.gestionarEstado(Id);
            ViewBag.estado = lstAllEstado;

            return Json(lstAllEstado[0], JsonRequestBehavior.AllowGet);


        }


        public async Task<ActionResult> AddEstado(string Descripcion, bool Estado)
        {
            try
            {
                await DAOCommand.AddEstado(Descripcion, Estado);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                //Tools.SendMailExceptionXHilo(Tools.MsjError("TemplatesController.SaveTemplate", ex)).Start();
                throw new Exception();
            }
        }

        public async Task<ActionResult> ActualizarEstado(int Id, string Descripcion, bool Estado)
        {
            try
            {
                await DAOCommand.ActualizarEstado(Id, Descripcion, Estado);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                //Tools.SendMailExceptionXHilo(Tools.MsjError("TemplatesController.SaveTemplate", ex)).Start();
                throw new Exception();
            }
        }

        //public async Task<ActionResult> AddListWorkOrder(string reclamo)
        //{
        //    try
        //    {
        //        await DAOCommand.AddListWorkOrder(reclamo);
        //        return new EmptyResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Tools.SendMailExceptionXHilo(Tools.MsjError("TemplatesController.SaveTemplate", ex)).Start();
        //        throw new Exception();
        //    }
        //}

        public async Task<ActionResult> AddListWorkOrder(ReclamoDatacredito Reclamo)
        {

            try
            {
                Users UserActual = await DAOCommand.InforUserActual();
                await DAOCommand.AddListWorkOrder(UserActual.IdMasterUsers, Reclamo);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                //Tools.SendMailExceptionXHilo(Tools.MsjError("TemplatesController.SaveTemplate", ex)).Start();
                throw new Exception();
            }
        }
    }
}