using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OpenXmlPowerTools;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Threading;
using System.Data.OleDb;


namespace AIBTicketsMVC.Controllers

{

    public class WorkOrderSolutionsController : Controller


    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["TicketsUnificado"].ToString();
            con = new SqlConnection(constring);
        }
        public async Task<ActionResult> Index(long IdWorkOrder)
        {
            List<WorkOrder> DataWorkOrder = new List<WorkOrder>();
            try
            {
                //Se borran los datos de la session
                Tools.SessionSetObject("ListAdjuntos", null);
                Users UserActual = await DAOCommand.InforUserActual(true, true);
                DataWorkOrder = await DAOCommand.ListWorkOrder(UserActual, IdWorkOrder);
                if (DataWorkOrder.Count == 0)
                {
                    return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                    {
                        TituloError = "ACCESO DENEGADO",
                        DetalleError = "Usted no cuenta con permisos para ingresar a esta solicitud."
                    });
                }
                if (DataWorkOrder[0].WorkOrderSolution.IdWorkOrderSolutions != 0)
                {
                    BPB_Servicio temserv = DataWorkOrder[0].WorkOrderSolution.BPBServicio;
                    Cta_Contable tempcont = DataWorkOrder[0].WorkOrderSolution.CtaContable;
                    List<WorkOrder_Solution> Result = await DAOCommand.ListWorkOrder_Solution(null, DataWorkOrder[0].WorkOrderSolution.IdWorkOrderSolutions);
                    DataWorkOrder[0].WorkOrderSolution = Result[0];
                    DataWorkOrder[0].WorkOrderSolution.BPBServicio = temserv;
                    DataWorkOrder[0].WorkOrderSolution.CtaContable = tempcont;
                }
                FieldsUDF ObjField = new FieldsUDF();
                ObjField.Template.IdTemplates = DataWorkOrder[0].Template.IdTemplates;
                /*CAMPOS DEL CLIENTE*/
                DataWorkOrder[0].ListFieldsUDF = await DAOCommand.WorkOrder_FieldsGeneral(false, IdWorkOrder, ObjField);

                /* verificación realcionados*/
                bool HabilRelac = false;
                Users InforUser = await DAOCommand.InforUserActual(true);
                List<WorkOrder> Relacionados = await DAOCommand.ListRelacionados(InforUser, IdWorkOrder);
                if (Relacionados.Count > 0)
                {
                    HabilRelac = true;
                }
                ViewBag.TicketsRelacionados = HabilRelac;
                /* END verificación realcionados*/

               

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
            if(DataWorkOrder[0].Template.IdTemplates == 12) 
                return View("~/Views/WorkOrderSolutions/index12.cshtml", DataWorkOrder[0]);


            else if (DataWorkOrder[0].Template.IdTemplates == 13)
                return View("~/Views/WorkOrderSolutions/index13.cshtml", DataWorkOrder[0]);
            else
                return View(DataWorkOrder[0]);
        }
        public async Task<ActionResult> FieldsSolutions(WorkOrder ObjWorkOrder)
        {
               Users UserActual = await DAOCommand.InforUserActual(true, true);
            FieldsUDF ObjField = new FieldsUDF();
            List<Templates> ListTemplate = await DAOCommand.ListTemplates(ObjWorkOrder.Template.IdTemplates);
            ObjField.Template.IdTemplates = ObjWorkOrder.Template.IdTemplates;
            List<Sites> ListSitesTempl = new List<Sites>();
            if (ListTemplate.Count > 0)
            {
                ListSitesTempl.Add(ListTemplate[0].Sitio);
                List<StatusDefinition> Estados = await DAOCommand.ListStatusDefinition(ListSitesTempl, ObjWorkOrder.Status.IdStatusDefinition);
                if (Estados.Count > 0)
                {
                    ObjWorkOrder.Status = Estados[0];

                }

                /*CAMPOS DEL GESTION*/
                ObjWorkOrder.ListFielsUDFSolution = await DAOCommand.WorkOrder_FieldsGeneral(true, ObjWorkOrder.WorkOrderSolution.IdWorkOrderSolutions, ObjField);
                /*LISTAS DESPLEGABLES*/
                List<StatusDefinition> ListStatus = await DAOCommand.ListStatusDefinition(ListSitesTempl, null, null, 1, true);
                if (ObjWorkOrder.Status.TypeAction.IdTypeActions == 3)//ESCALAMIENTO
                {
                    ViewBag.ListEstados = ListStatus.Where(lq => lq.TypeAction.IdTypeActions != 1);
                }
                else
                {
                    ViewBag.ListEstados = ListStatus.Where(lq => lq.TypeAction.IdTypeActions != 1 & lq.TypeAction.IdTypeActions != 5);
                }
                ViewBag.ListSubEstados = await DAOCommand.ListStatusDefinition(ListSitesTempl, null, ObjWorkOrder.Status.IdStatusDefinition, 2, true);
                ViewBag.ListGroupsScaled = await DAOCommand.GruposAEscalar(ObjWorkOrder.GrupoAsignado.IdMasterGroups);
                ViewBag.ListTypesScaled = await DAOCommand.TypesScaledXGroups(ObjWorkOrder.WorkOrderSolution.WorkOrderEscalations.GroupsScaled.IdMasterGroups);
            }
            /*END LISTAS DESPLEGABLES*/
            /*PERMISOS EN FORMULARIO*/
            bool EditarTicket = false;
            if (ObjWorkOrder.UsersScaled.IdMasterUsers != 0 & ObjWorkOrder.UsersScaled.IdMasterUsers == UserActual.IdMasterUsers)
            {
                EditarTicket = true;
            }
            else if (ObjWorkOrder.UsersScaled.IdMasterUsers == 0 & ObjWorkOrder.UsersAssigned.IdMasterUsers == UserActual.IdMasterUsers)
            {
                EditarTicket = true;
            }
            ViewBag.PermisosFull = await DAOCommand.ListPermisos(UserActual.Perfiles, 5); //Mostrar todas las solicitudes
            if (ViewBag.PermisosFull.Count > 0)
            {
                EditarTicket = true;
            }
            ViewBag.PermisosEditar = EditarTicket;
            /*END PERMISOS EN FORMULARIO*/

            /// area que genero ajuste
            if (ObjWorkOrder.Template.IdTemplates == 12 || ObjWorkOrder.Template.IdTemplates == 13)
            {
                DAOCommand objetoArea = new DAOCommand();
                List<Gerencia> lstArea = objetoArea.ObtenerArea();
                ViewBag.area_genero_ajuste = new SelectList(lstArea, "area_genero_ajuste", "area_genero_ajuste");

                DAOCommand objetoCuenta = new DAOCommand();
                List<Cta_Contable> lstCuenta = objetoCuenta.ObtenerCuenta();
                
                ViewBag.Cuenta = new SelectList(lstCuenta, "Servicio", "Servicio_cuenta");
            }
            return PartialView(ObjWorkOrder);
        }
        public async Task<ActionResult> GetSubStatus(int IdStatus)
        {
            Users InforUser = await DAOCommand.InforUserActual(true, true);
            List<StatusDefinition> Status = await DAOCommand.ListStatusDefinition(InforUser.Sitios, IdStatus, null, 1, true);
            Status[0].SubStatus = await DAOCommand.ListStatusDefinition(InforUser.Sitios, null, IdStatus, 2, true);
            return Json(Status[0], JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetTypesScaled(int IdGroups)
        {
            Groups Grupo = new Groups();
            Grupo.IdMasterGroups = IdGroups;
            Grupo.UsersXGroups = await DAOCommand.ListUsersXGroup(IdGroups, true);
            Grupo.TypesScaled = await DAOCommand.TypesScaledXGroups(IdGroups);
            return Json(Grupo, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetDispositionsByServicio(string servicio)
        {
            List<Disposition> dispositions = await DAOCommand.WorkOrder_GetDispositions(servicio);
            return Json(dispositions, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetCtaContable(string ID)
        {
            List<Disposition> dispositions = await DAOCommand.WorkOrder_GetCtaContable(ID);
            return Json(dispositions, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SaveWorkOrderSolutions(WorkOrder_Solution Solution)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.SaveWorkOrderSolutions(UserActual.IdMasterUsers, Solution);
            /*Mover los archivos*/
            List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
            if (ListAdjuntos != null)
            {
                string path = Server.MapPath($"~/Uploads/WorkOrder/{Solution.IdWorkOrder}/");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                foreach (var item in ListAdjuntos)
                {
                    string RutaDestino = path + Path.GetFileName(item.NameEncryptedAttachment + item.Extension);
                    System.IO.File.Move(item.FilePath, RutaDestino);
                }
            }
            return new EmptyResult();
        }
        //EXPORTAR EXCEL PLANILLA AJUSTE MOVIL
        public async Task<ActionResult> PlanillaAjustesMovil_Excel(string Fechainicio = null, string Fechafinal = null)
        {
            try
            {
                //List<CCicloConsultaResumenCorregidos> DatosCasosDevueltos = await CCicloConsultaDAO.Cciclo_ListCasosDevueltosExcel();
                DataTable Lista = new DataTable();
                string CurrentColumnName = "";
                DataTable DatosPlanillaAjustesMovil = new DataTable();
                //List<string> subcuentasList = subcuentas.Split(',').ToList<string>();

                DatosPlanillaAjustesMovil = await DAOCommand.PlanillaAjustesMovil_Excel(Fechainicio, Fechafinal);
                //Funcion para crear las columnas que no coincidan con las columnas que se encuentran actualmente en atlas
                foreach (DataColumn Column in DatosPlanillaAjustesMovil.Columns)
                {
                    CurrentColumnName = Column.ColumnName.ToUpper();
                    if (!Lista.Columns.Contains(CurrentColumnName))
                    {
                        Lista.Columns.Add(CurrentColumnName);
                    }
                }
                ///////////////////////
                foreach (DataRow Row in DatosPlanillaAjustesMovil.Rows)
                {
                    DataRow newRow = Lista.NewRow();
                    for (int i = 0; i < Row.ItemArray.Length; i++)
                    {

                        newRow[DatosPlanillaAjustesMovil.Columns[i].ColumnName.ToUpper()] = Row[i].ToString();
                    }
                    Lista.Rows.Add(newRow);
                }




                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Casos");
                if (Lista.Rows.Count > 0)
                {
                    ws.Cells["A1"].LoadFromDataTable(Lista, true);



                    //List<WorkOrderDispositions> Grupos = CreateWorkOrdersDAO.ListDispositions("EstadoCOA").Result;
                    //ExcelWorksheet GruposHoja = pck.Workbook.Worksheets.Add("Estados");

                    //GruposHoja.Cells["A1"].Value = "Descripcion";
                    //int cont = 2;
                    //foreach (var item in Grupos)
                    //{
                    //    GruposHoja.Cells["A" + cont].Value = item.Descripcion;
                    //    cont++;
                    //}



                    ws.Cells["A:AZ"].AutoFitColumns();

                }

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();

                return View(DatosPlanillaAjustesMovil);
            }
            catch (Exception ex)
            {
                // Tools.SendMailExceptionXHilo(Tools.MsjError("CasosxCuenta.ListCasosxCuentaExcel", ex)).Start();
                throw new Exception();
            }
        }
        public async Task<ActionResult> SaveWorkOrderSolutions_2(WorkOrder_Solution Solution)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.SaveWorkOrderSolutions_2(UserActual.IdMasterUsers, Solution);
            /*Mover los archivos*/
            List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
            if (ListAdjuntos != null)
            {
                string path = Server.MapPath($"~/Uploads/WorkOrder/{Solution.IdWorkOrder}/");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                foreach (var item in ListAdjuntos)
                {
                    string RutaDestino = path + Path.GetFileName(item.NameEncryptedAttachment + item.Extension);
                    System.IO.File.Move(item.FilePath, RutaDestino);
                }
            }
            return new EmptyResult();
        }
        public async Task<ActionResult> ViewAttachments(long IdWorkOrder)
        {
            return PartialView(await DAOCommand.ListWorkOrder_Attachments(IdWorkOrder));
        }
        [HttpPost]
        public async Task<ActionResult> AddAttachments(HttpPostedFileWrapper Adjunto)
        {
            List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
            if (ListAdjuntos == null)
            {
                ListAdjuntos = new List<WorkOrder_Attachments>();
            }
            WorkOrder_Attachments ObjAdjunto = new WorkOrder_Attachments();
            const decimal wKilo = 1026.865671641791M;
            ObjAdjunto.FileSizeKB = Math.Round(Adjunto.ContentLength / wKilo, 2);
            if (ObjAdjunto.FileSizeKB > 20000)
            {
                ObjAdjunto.msjError = "El peso del archivo no debe superar los 20 MB";
                return Json(ObjAdjunto, JsonRequestBehavior.AllowGet);
            }
            if (Adjunto.FileName.Length > 100)
            {
                ObjAdjunto.msjError = "El nombre del archivo no puede superar los 100 caracteres.";
                return Json(ObjAdjunto, JsonRequestBehavior.AllowGet);
            }
            ObjAdjunto.NameAttachment = Adjunto.FileName;
            var Objeto = ListAdjuntos.Where(lq => lq.NameAttachment == Adjunto.FileName).ToList();
            if (Objeto.Count > 0)
            {
                ObjAdjunto.msjError = "Ya existe un archivo con este nombre.";
                return Json(ObjAdjunto, JsonRequestBehavior.AllowGet);
            }
            ObjAdjunto.NameEncryptedAttachment = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssff"));
            ObjAdjunto.Extension = Path.GetExtension(Adjunto.FileName);
            string path = Server.MapPath($"~/Uploads/temp/");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string filePath = path + Path.GetFileName(ObjAdjunto.NameEncryptedAttachment + ObjAdjunto.Extension);
            Adjunto.SaveAs(filePath);
            ObjAdjunto.FilePath = filePath;
            ObjAdjunto.FilePathDownload = $@"/Uploads/temp/{ObjAdjunto.NameEncryptedAttachment + ObjAdjunto.Extension}";
            ListAdjuntos.Add(ObjAdjunto);
            Tools.SessionSetObject("ListAdjuntos", ListAdjuntos);
            return Json(ObjAdjunto, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DivAttachment(WorkOrder_Attachments InforAdjunto) => PartialView(InforAdjunto);
        public async Task<ActionResult> FieldsTypeBooth(int IdBooth)
        {
            if (IdBooth == 0)
            {
                return PartialView(new WH_Booth());
            }
            List<WH_Booth> ListData = await DAOCommand.ListBooth(null, IdBooth);
            if (ListData.Count > 0)
            {
                return PartialView(ListData.Single());
            }
            else
            {
                return PartialView(new WH_Booth());
            }
        }
        public async Task<ActionResult> DeleteAttachment(long IdWorkOrder, long NameEncrypted)
        {
            List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
            if (ListAdjuntos == null)
            {
                Users UserActual = await DAOCommand.InforUserActual();
                await DAOCommand.DeleteAttachment(UserActual.IdMasterUsers, IdWorkOrder, NameEncrypted);
            }
            else
            {
                var Objeto = ListAdjuntos.Where(lq => lq.NameEncryptedAttachment == NameEncrypted).ToList();
                if (Objeto.Count > 0)
                {
                    if (System.IO.File.Exists(Objeto[0].FilePath)) System.IO.File.Delete(Objeto[0].FilePath);

                    ListAdjuntos.RemoveAll(x => x.NameEncryptedAttachment == NameEncrypted);
                    Tools.SessionSetObject("ListAdjuntos", ListAdjuntos);
                }
            }
            return new EmptyResult();
        }

        public async Task<ActionResult> ListRelacionados(long IdWorkOrder)
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            //Users UserActual = await DAOCommand.InforUserActual(true, true);
            //List<Sites> Sitios = await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 10, true); //Categorias
            return PartialView(await DAOCommand.ListRelacionados(InforUser, IdWorkOrder));
        }

        /* NegacionLinea 08.2021 */
        public async Task<ActionResult> NegacionLinea_Alistamiento(WorkOrder ObjWorkOrder)
        {
            Users UserActual = await DAOCommand.InforUserActual(true, true);
            FieldsUDF ObjField = new FieldsUDF();
            List<Templates> ListTemplate = await DAOCommand.ListTemplates(ObjWorkOrder.Template.IdTemplates);

            ObjField.Template.IdTemplates = ObjWorkOrder.Template.IdTemplates;

            List<Sites> ListSitesTempl = new List<Sites>();

            if (ListTemplate.Count > 0)
            {
                ListSitesTempl.Add(ListTemplate[0].Sitio);
                List<StatusDefinition> Estados = await DAOCommand.ListStatusDefinition(ListSitesTempl, ObjWorkOrder.Status.IdStatusDefinition);
                ObjWorkOrder.Status = Estados[0];
                /*CAMPOS DEL GESTION*/
                ObjWorkOrder.ListFielsUDFSolution = await DAOCommand.WorkOrder_FieldsGeneral(true, ObjWorkOrder.WorkOrderSolution.IdWorkOrderSolutions, ObjField);





                /*LISTAS DESPLEGABLES*/
                List<StatusDefinition> ListStatus = await DAOCommand.ListStatusDefinition(ListSitesTempl, null, null, 1, true);
                if (ObjWorkOrder.Status.TypeAction.IdTypeActions == 3)//ESCALAMIENTO
                {
                    ViewBag.ListEstados = ListStatus.Where(lq => lq.TypeAction.IdTypeActions != 1);
                }
                else
                {
                    ViewBag.ListEstados = ListStatus.Where(lq => lq.TypeAction.IdTypeActions != 1 & lq.TypeAction.IdTypeActions != 5);
                }
                ViewBag.ListSubEstados = await DAOCommand.ListStatusDefinition(ListSitesTempl, null, ObjWorkOrder.Status.IdStatusDefinition, 2, true);
                ViewBag.ListGroupsScaled = await DAOCommand.GruposAEscalar(ObjWorkOrder.GrupoAsignado.IdMasterGroups);
                ViewBag.ListTypesScaled = await DAOCommand.TypesScaledXGroups(ObjWorkOrder.WorkOrderSolution.WorkOrderEscalations.GroupsScaled.IdMasterGroups);
            }
            /*END LISTAS DESPLEGABLES*/
            /*PERMISOS EN FORMULARIO*/
            bool EditarTicket = false;
            if (ObjWorkOrder.UsersScaled.IdMasterUsers != 0 & ObjWorkOrder.UsersScaled.IdMasterUsers == UserActual.IdMasterUsers)
            {
                EditarTicket = true;
            }
            else if (ObjWorkOrder.UsersScaled.IdMasterUsers == 0 & ObjWorkOrder.UsersAssigned.IdMasterUsers == UserActual.IdMasterUsers)
            {
                EditarTicket = true;
            }
            ViewBag.PermisosFull = await DAOCommand.ListPermisos(UserActual.Perfiles, 5); //Mostrar todas las solicitudes
            if (ViewBag.PermisosFull.Count > 0)
            {
                EditarTicket = true;
            }
            ViewBag.PermisosEditar = EditarTicket;
            /*END PERMISOS EN FORMULARIO*/


            /* NegacionLinea_Alistamiento */

            return PartialView(await DAOCommand.NegacionLinea_Alistamiento(ObjWorkOrder.IdWorkOrder));
        }


        public async Task<ActionResult> GuardarNegacionLinea_Alistamiento(NegacionLinea Datos)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.GuardarNegacionLinea_Alistamiento(UserActual.Winuser, Datos);

            return new EmptyResult();
        }

        public async Task<ActionResult> NegacionLinea_Alistamiento_ExcelTodo(WorkOrder ObjWorkOrder)
        {
            try
            {

                List<NegacionLinea> Datos = await DAOCommand.NegacionLinea_AlistamientoExcel(ObjWorkOrder.IdWorkOrder);

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("NegacionLineaTodos");


                //Ordenes
                ws.Cells["A1"].Value = "IdWorkOrder";
                ws.Cells["B1"].Value = "Base";
                ws.Cells["C1"].Value = "Imagen";
                ws.Cells["D1"].Value = "FechaActivacion";
                ws.Cells["E1"].Value = "Curcode";
                ws.Cells["F1"].Value = "Nombre";
                ws.Cells["G1"].Value = "Apellido";
                ws.Cells["H1"].Value = "Canal";
                ws.Cells["I1"].Value = "Ascard";
                ws.Cells["J1"].Value = "FechaReposicion";
                ws.Cells["K1"].Value = "Contrato";
                ws.Cells["L1"].Value = "Grabacion";
                ws.Cells["M1"].Value = "Reasignacion";
                ws.Cells["N1"].Value = "Estado";
                ws.Cells["O1"].Value = "Legalizado";
                ws.Cells["P1"].Value = "Observaciones";
                ws.Cells["Q1"].Value = "RangoProbable";
                ws.Cells["R1"].Value = "DireccionInformaCliente";
                ws.Cells["S1"].Value = "CustomerID";
                ws.Cells["T1"].Value = "Ciudad";
                ws.Cells["U1"].Value = "Departamento";
                ws.Cells["V1"].Value = "MIN";
                ws.Cells["V1"].Value = "MIN";
                ws.Cells["W1"].Value = "FechaRadicacion";
                ws.Cells["X1"].Value = "Notificacion";
                ws.Cells["Y1"].Value = "FechaDesactivacion";
                ws.Cells["Z1"].Value = "PQR";
                ws.Cells["AA1"].Value = "Cedula";
                ws.Cells["AB1"].Value = "AreaRadica";
                ws.Cells["AC1"].Value = "TipoReclamo";




                //Ordenes
                int rowStart = 2;
                foreach (var item in Datos)
                {

                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.IdWorkOrder;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.Base;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.Imagen;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.FechaActivacion;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.Curcode;
                    ws.Cells[string.Format("F{0}", rowStart)].Value = item.Nombre;
                    ws.Cells[string.Format("G{0}", rowStart)].Value = item.Apellido;
                    ws.Cells[string.Format("H{0}", rowStart)].Value = item.Canal;
                    ws.Cells[string.Format("I{0}", rowStart)].Value = item.Ascard;
                    ws.Cells[string.Format("J{0}", rowStart)].Value = item.FechaReposicion;
                    ws.Cells[string.Format("K{0}", rowStart)].Value = item.Contrato;
                    ws.Cells[string.Format("L{0}", rowStart)].Value = item.Grabacion;
                    ws.Cells[string.Format("M{0}", rowStart)].Value = item.Reasignacion;
                    ws.Cells[string.Format("N{0}", rowStart)].Value = item.Estado;
                    ws.Cells[string.Format("O{0}", rowStart)].Value = item.Legalizado;
                    ws.Cells[string.Format("P{0}", rowStart)].Value = item.Observaciones;
                    ws.Cells[string.Format("Q{0}", rowStart)].Value = item.RangoProbable;
                    ws.Cells[string.Format("R{0}", rowStart)].Value = item.DireccionInformaCliente;
                    ws.Cells[string.Format("S{0}", rowStart)].Value = item.CustomerID;
                    ws.Cells[string.Format("T{0}", rowStart)].Value = item.Ciudad;
                    ws.Cells[string.Format("U{0}", rowStart)].Value = item.Departamento;
                    ws.Cells[string.Format("V{0}", rowStart)].Value = item.MIN;
                    ws.Cells[string.Format("W{0}", rowStart)].Value = item.FechaRadicacion;
                    ws.Cells[string.Format("X{0}", rowStart)].Value = item.Notificacion;
                    ws.Cells[string.Format("Y{0}", rowStart)].Value = item.FechaDesactivacion;
                    ws.Cells[string.Format("Z{0}", rowStart)].Value = item.PQR;
                    ws.Cells[string.Format("AA{0}", rowStart)].Value = item.Cedula;
                    ws.Cells[string.Format("AB{0}", rowStart)].Value = item.AreaRadica;
                    ws.Cells[string.Format("AC{0}", rowStart)].Value = item.TipoReclamo;


                    rowStart++;
                }
                rowStart = 2;


                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReportNegacionLineaTodos.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
                return View(Datos);




            }
            catch (Exception ex)
            {
                //Tools.SendMailExceptionXHilo(Tools.MsjError("CierreCiclo.Cciclo_ListCasosDevueltosExcel", ex)).Start();
                throw new Exception();
            }
        }

        public async Task<ActionResult> NegacionLinea_AlistamientoExcelPendiente(WorkOrder ObjWorkOrder)
        {
            try
            {

                List<NegacionLinea> Datos = await DAOCommand.NegacionLinea_AlistamientoExcelPendiente(ObjWorkOrder.IdWorkOrder);

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("NegacionLineaPendientes");


                //Ordenes
                ws.Cells["A1"].Value = "IdWorkOrder";
                ws.Cells["B1"].Value = "Base";
                ws.Cells["C1"].Value = "Imagen";
                ws.Cells["D1"].Value = "FechaActivacion";
                ws.Cells["E1"].Value = "Curcode";
                ws.Cells["F1"].Value = "Nombre";
                ws.Cells["G1"].Value = "Apellido";
                ws.Cells["H1"].Value = "Canal";
                ws.Cells["I1"].Value = "Ascard";
                ws.Cells["J1"].Value = "FechaReposicion";
                ws.Cells["K1"].Value = "Contrato";
                ws.Cells["L1"].Value = "Grabacion";
                ws.Cells["M1"].Value = "Reasignacion";
                ws.Cells["N1"].Value = "Estado";
                ws.Cells["O1"].Value = "Legalizado";
                ws.Cells["P1"].Value = "Observaciones";
                ws.Cells["Q1"].Value = "RangoProbable";
                ws.Cells["R1"].Value = "DireccionInformaCliente";
                ws.Cells["S1"].Value = "CustomerID";
                ws.Cells["T1"].Value = "Ciudad";
                ws.Cells["U1"].Value = "Departamento";
                ws.Cells["V1"].Value = "MIN";
                ws.Cells["W1"].Value = "FechaRadicacion";
                ws.Cells["X1"].Value = "Notificacion";
                ws.Cells["Y1"].Value = "FechaDesactivacion";
                ws.Cells["Z1"].Value = "PQR";
                ws.Cells["AA1"].Value = "Cedula";
                ws.Cells["AB1"].Value = "AreaRadica";
                ws.Cells["AC1"].Value = "TipoReclamo";

                //Ordenes
                int rowStart = 2;
                foreach (var item in Datos)
                {

                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.IdWorkOrder;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.Base;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.Imagen;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.FechaActivacion;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.Curcode;
                    ws.Cells[string.Format("F{0}", rowStart)].Value = item.Nombre;
                    ws.Cells[string.Format("G{0}", rowStart)].Value = item.Apellido;
                    ws.Cells[string.Format("H{0}", rowStart)].Value = item.Canal;
                    ws.Cells[string.Format("I{0}", rowStart)].Value = item.Ascard;
                    ws.Cells[string.Format("J{0}", rowStart)].Value = item.FechaReposicion;
                    ws.Cells[string.Format("K{0}", rowStart)].Value = item.Contrato;
                    ws.Cells[string.Format("L{0}", rowStart)].Value = item.Grabacion;
                    ws.Cells[string.Format("M{0}", rowStart)].Value = item.Reasignacion;
                    ws.Cells[string.Format("N{0}", rowStart)].Value = item.Estado;
                    ws.Cells[string.Format("O{0}", rowStart)].Value = item.Legalizado;
                    ws.Cells[string.Format("P{0}", rowStart)].Value = item.Observaciones;
                    ws.Cells[string.Format("Q{0}", rowStart)].Value = item.RangoProbable;
                    ws.Cells[string.Format("R{0}", rowStart)].Value = item.DireccionInformaCliente;
                    ws.Cells[string.Format("S{0}", rowStart)].Value = item.CustomerID;
                    ws.Cells[string.Format("T{0}", rowStart)].Value = item.Ciudad;
                    ws.Cells[string.Format("U{0}", rowStart)].Value = item.Departamento;
                    ws.Cells[string.Format("V{0}", rowStart)].Value = item.MIN;
                    ws.Cells[string.Format("W{0}", rowStart)].Value = item.FechaRadicacion;
                    ws.Cells[string.Format("X{0}", rowStart)].Value = item.Notificacion;
                    ws.Cells[string.Format("Y{0}", rowStart)].Value = item.FechaDesactivacion;
                    ws.Cells[string.Format("Z{0}", rowStart)].Value = item.PQR;
                    ws.Cells[string.Format("AA{0}", rowStart)].Value = item.Cedula;
                    ws.Cells[string.Format("AB{0}", rowStart)].Value = item.AreaRadica;
                    ws.Cells[string.Format("AC{0}", rowStart)].Value = item.TipoReclamo;


                    rowStart++;
                }
                rowStart = 2;


                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReportNegacionLineaPendientes.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
                return View(Datos);




            }
            catch (Exception ex)
            {
                //Tools.SendMailExceptionXHilo(Tools.MsjError("CierreCiclo.Cciclo_ListCasosDevueltosExcel", ex)).Start();
                throw new Exception();
            }
        }

        public async Task<ActionResult> EliminarNegacionLinea_Alistamiento(int Id)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.EliminarNegacionLinea_Alistamiento(UserActual.Winuser, Id);

            return new EmptyResult();
        }



        //[HttpPost]
        public async Task<ActionResult> CargarExcelNegacionLinea(HttpPostedFileBase ArchivoExcel, long IdWorkOrder)
        {
            try
            {
                Users UserActual = await DAOCommand.InforUserActual();
                await DAOCommand.CargarExcelNegacionLinea(ArchivoExcel, Server.MapPath("~"), IdWorkOrder);
                Response.Redirect("?IdWorkOrder=" + IdWorkOrder);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        ///codigo nuevo    

        /// datos cuenta contable
        //public async static Task<List<Cta_Contable>> WorkOrder_GetContable(string cod_cuenta)
        [HttpPost]
        public JsonResult WorkOrder_GetContable(string servicio)
        {
            Cta_Contable result = new Cta_Contable();
            try
            {
                connection();
                string query = "SELECT * FROM [tbl_contable] where [servicio]='" + servicio + "'";

                SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandText = query,
                    CommandType = CommandType.Text,
                    CommandTimeout = 3600
                };

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Cta_Contable item = new Cta_Contable
                    {
                        cod_cuenta = dr["cod_cuenta"].ToString(),
                        servicio = dr["servicio"].ToString(),
                        iva = dr["iva"].ToString(),
                    };
                    result = item;
                }
            }
            catch (Exception ex)
            {
                //Item item = new Item { Codigo = "", Descripcion = "(CumplimientoDAO.ConsultarCampanas) " + ex.Message };
                //result.Add(item);
            }
            return Json(result);

        }

        [HttpPost]
        public JsonResult WorkOrder_GetGerencia(string area_genero_ajuste)
        {
            Gerencia result = new Gerencia();
            try
            {
                connection();
                string query = "SELECT * FROM tbl_gerencia where area_genero_ajuste = '" + area_genero_ajuste + "'";

                SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandText = query,
                    CommandType = CommandType.Text,
                    CommandTimeout = 3600
                };

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Gerencia item = new Gerencia
                    {
                        area_genero_ajuste = dr["area_genero_ajuste"].ToString(),
                        gerencia = dr["gerencia"].ToString(),
                    };
                    result = item;
                }
            }
            catch (Exception ex)
            {
                //Item item = new Item { Codigo = "", Descripcion = "(CumplimientoDAO.ConsultarCampanas) " + ex.Message };
                //result.Add(item);
            }
            return Json(result);

        }

        // exportar postpago
        public ActionResult ExportarPostpago(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_Postpago", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);

                DateTime fechaactual = DateTime.Now;
                string nombre = fechaactual.Year.ToString() + "" + fechaactual.Month.ToString() + "" + fechaactual.Day.ToString() + "" + fechaactual.Hour.ToString() + "" + fechaactual.Minute.ToString() + "" + fechaactual.Second.ToString();
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "ExportarPostpago";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_Postpago.xlsx");
                        using (MyMemoryStream)
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();

                        }
                    }

                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Postpago.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public ActionResult ExportarPrepago(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_Prepago", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);

                DateTime fechaactual = DateTime.Now;
                string nombre = fechaactual.Year.ToString() + "" + fechaactual.Month.ToString() + "" + fechaactual.Day.ToString() + "" + fechaactual.Hour.ToString() + "" + fechaactual.Minute.ToString() + "" + fechaactual.Second.ToString();
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "PQR_Prepago";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_Prepago.xlsx");
                        using (MyMemoryStream)
                        {
                            //string FilePath = @"c:\temp\Prepago" + nombre + ".xlsx";
                            //string basePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                            //wb.SaveAs(MyMemoryStream);
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();

                            
                        }

                        
                    }
                   
                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                   "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Prepago.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult ExportarAscard(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_Ascard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "PQR_Ascard";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_Ascard.xlsx");
                        using (MyMemoryStream)
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }


                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Ascard.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult ExportarCuotasAscard(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_CuotasAscard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "PQR_CuotasAscard";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_CuotasAscard.xlsx");
                        using (MyMemoryStream)
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();


                        }
                    }

                    
                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "CuotasAscard.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult ExportarEliminarCentrales(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_EliminarCentrales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "PQR_EliminarCentrales";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_EliminarCentrales.xlsx");
                        using (MyMemoryStream)
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();


                        }
                    }


                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "EliminarCentrales.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// / importar excel
        /// </summary>
        /// <param name="pathDAMerc"></param>
        ///// <param name="fechaHoySt"></param>
        //public static void ProcesarExcel(string pathDAMerc, string fechaHoySt)
        //{

        //    try
        //    {
        //        // CARGO DATA
        //        string fileDataName = pathDAMerc; //"";
        //        FileInfo[] fileInfo = null;
        //        //string pathLogDir = Path.Combine(Application.StartupPath, "log");
        //        try
        //        {
        //            //string rutaLocal = Application.StartupPath + "\\Debug\\" + fechaAyerSt + "\\";
        //            string startupPath = "../";
        //            string rutaLocal = startupPath;
        //            DirectoryInfo directoryInfo = new DirectoryInfo(rutaLocal);
        //            fileInfo = directoryInfo.GetFiles("*.xlsx", SearchOption.TopDirectoryOnly);
        //            if (fileInfo.Count() > 0)
        //            {
        //                fileDataName = fileInfo[0].FullName;
        //            }
        //            else
        //            {
        //                fileInfo = directoryInfo.GetFiles("*.xls", SearchOption.TopDirectoryOnly);
        //                if (fileInfo.Count() > 0)
        //                {
        //                    fileDataName = fileInfo[0].FullName;
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception(String.Format("Problemas al cargar el archivo de Excel...\nERROR: {0}", e.Message));
        //        }
        //        if (fileDataName != "")
        //        {
        //            string conStringExcel = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
        //            conStringExcel = string.Format(conStringExcel, fileDataName);
        //            SqlConnection conDB = new SqlConnection();
        //            //DataTable dtExcelServicio = new DataTable();
        //            DataTable dtExcel = new DataTable(); using (OleDbConnection connExcel = new OleDbConnection(conStringExcel))
        //            {
        //                using (OleDbCommand cmdExcel = new OleDbCommand())
        //                {
        //                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
        //                    {
        //                        cmdExcel.Connection = connExcel; //Get the name of First Sheet.
        //                        connExcel.Open();
        //                        DataTable dtExcelSchema;
        //                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //                        connExcel.Close(); // Hoja Correctivas
        //                                           //cmdExcel.CommandText = "SELECT * FROM [Correctivas$]";
        //                        cmdExcel.CommandText = $"SELECT * FROM [{sheetName}]";
        //                        odaExcel.SelectCommand = cmdExcel;
        //                        connExcel.Open();
        //                        odaExcel.Fill(dtExcel);
        //                        connExcel.Close();
        //                    }
        //                }
        //            }
        //            if (dtExcel.Rows.Count == 0)
        //            {
        //                throw new Exception("Ningún registro cargado desde el EXCEL. Verifique.");
        //            }
        //            foreach (DataRow row in dtExcel.Rows)
        //            {
        //                //responseMessage = "OK"; string strFiltro = (row["NOMBRE_PASO"].ToString()); //if (int.TryParse(strNum, out int num))
        //                {
        //                    WorkOrderPostpago cargaData = new WorkOrderPostpago();
        //                    cargaData.IdWorkOrderSolutions = row.IsNull("IdWorkOrderSolutions") ? "" : (row["IdWorkOrderSolutions"].ToString());
        //                    cargaData.UDF_VARCHAR371 = ((String.IsNullOrEmpty(row["ANALISTA"].ToString()) ? "" : row["ANALISTA"].ToString())).Trim();
        //                    cargaData.UDF_BIGINT97 = ((String.IsNullOrEmpty(row["UDF_BIGINT97"].ToString()) ? "" : row["UDF_BIGINT97"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR372 = ((String.IsNullOrEmpty(row["UDF_VARCHAR372"].ToString()) ? "" : row["UDF_VARCHAR372"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR373 = ((String.IsNullOrEmpty(row["UDF_VARCHAR373"].ToString()) ? "" : row["UDF_VARCHAR373"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR374 = (((String.IsNullOrEmpty(row["UDF_VARCHAR374"].ToString()) ? "" : row["UDF_VARCHAR374"].ToString())).Trim());
        //                    cargaData.UDF_VARCHAR375 = ((String.IsNullOrEmpty(row["UDF_VARCHAR375"].ToString()) ? "" : row["UDF_VARCHAR375"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR389 = ((String.IsNullOrEmpty(row["UDF_VARCHAR389"].ToString()) ? "" : row["UDF_VARCHAR389"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR390 = ((String.IsNullOrEmpty(row["UDF_VARCHAR390"].ToString()) ? "" : row["UDF_VARCHAR390"].ToString())).Trim();
        //                    cargaData.UDF_VARLIST155 = ((String.IsNullOrEmpty(row["UDF_VARLIST155"].ToString()) ? "" : row["UDF_VARLIST155"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR348 = ((String.IsNullOrEmpty(row["UDF_VARCHAR348"].ToString()) ? "" : row["UDF_VARCHAR348"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR349 = ((String.IsNullOrEmpty(row["UDF_VARCHAR349"].ToString()) ? "" : row["UDF_VARCHAR349"].ToString())).Trim();
        //                    cargaData.UDF_DATE74 = ((String.IsNullOrEmpty(row["UDF_DATE74"].ToString()) ? "" : row["UDF_DATE74"].ToString())).Trim();
        //                    cargaData.UDF_DATE75 = ((String.IsNullOrEmpty(row["UDF_DATE75"].ToString()) ? "" : row["UDF_DATE75"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR391 = ((String.IsNullOrEmpty(row["UDF_VARCHAR391"].ToString()) ? "" : row["UDF_VARCHAR391"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR392 = ((String.IsNullOrEmpty(row["UDF_VARCHAR392"].ToString()) ? "" : row["UDF_VARCHAR392"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR393 = ((String.IsNullOrEmpty(row["UDF_VARCHAR393"].ToString()) ? "" : row["UDF_VARCHAR393"].ToString())).Trim();
        //                    cargaData.UDF_VARLIST156 = ((String.IsNullOrEmpty(row["UDF_VARLIST156"].ToString()) ? "" : row["UDF_VARLIST156"].ToString())).Trim();
        //                    cargaData.UDF_VARLIST157 = ((String.IsNullOrEmpty(row["UDF_VARLIST157"].ToString()) ? "" : row["UDF_VARLIST157"].ToString())).Trim();
        //                    cargaData.UDF_VARLIST158 = ((String.IsNullOrEmpty(row["UDF_VARLIST158"].ToString()) ? "" : row["UDF_VARLIST158"].ToString())).Trim();
        //                    cargaData.UDF_VARCHAR395 = ((String.IsNullOrEmpty(row["UDF_VARCHAR395"].ToString()) ? "" : row["UDF_VARCHAR395"].ToString())).Trim();
        //                    cargaData.UDF_DATE80 = ((String.IsNullOrEmpty(row["UDF_DATE80"].ToString()) ? "" : row["UDF_DATE80"].ToString())).Trim();
        //                    cargaData.UDF_DATE81 = ((String.IsNullOrEmpty(row["UDF_DATE81"].ToString()) ? "" : row["UDF_DATE81"].ToString())).Trim();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error generado por cargado de la información: " + ex.Message);
        //    }
        //}

        //public void InsertData(WorkOrderPostpago DataLP)
        //{
        //    connection();
        //    try
        //    {

        //        string sql = string.Empty;
        //        sql += "INSERT INTO Camp_Data.dbo.tbl_GestionLineaPrueba_Mercurio\n";
        //        sql += " ([IdWorkOrderSolutions] as WorKOrder ,UDF_VARCHAR401 as PRE_Radicado,UDF_VARLIST162 as PRE_Tipo_Reclamo,UDF_VARCHAR402 as PRE_Nombre_Titular,UDF_VARCHAR403 as PRE_Min,UDF_VARCHAR404 as PRE_CUSTCODE,UDF_VARCHAR405 as PRE_Valor,UDF_VARCHAR406 as PRE_Concepto,UDF_VARCHAR407 as PRE_Analista,UDF_DATE86 as PRE_Periodo_Ajustado_Desde,UDF_DATE87 as PRE_Periodo_Ajustado_Hasta,UDF_VARLIST163 as PRE_Aliado,UDF_VARCHAR408 as PRE_Estado,UDF_DATE88 as PRE_Actualizacion_Anterior,UDF_DATE89 as PRE_Ultima_Actualizacion

        //,[UDF_VARCHAR412]
        //,[UDF_VARLIST165]
        //,[UDF_VARCHAR413]
        //,[UDF_VARCHAR414]
        //,[UDF_VARCHAR415]
        //,[UDF_DATE90]
        //,[UDF_DATE91]) \n";
        //          sql += " values()";
        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}

        /// plantilla 13 exportar e importar

        public ActionResult ExportarPostpago13(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_Postpago13", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);

                DateTime fechaactual = DateTime.Now;
                string nombre = fechaactual.Year.ToString() + "" + fechaactual.Month.ToString() + "" + fechaactual.Day.ToString() + "" + fechaactual.Hour.ToString() + "" + fechaactual.Minute.ToString() + "" + fechaactual.Second.ToString();
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "ExportarPostpago";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_Postpago_verbales.xlsx");
                        using (MyMemoryStream)
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();

                        }
                    }

                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Postpago.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult ExportarPrepago13(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_Prepago13", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);

                DateTime fechaactual = DateTime.Now;
                string nombre = fechaactual.Year.ToString() + "" + fechaactual.Month.ToString() + "" + fechaactual.Day.ToString() + "" + fechaactual.Hour.ToString() + "" + fechaactual.Minute.ToString() + "" + fechaactual.Second.ToString();
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "PQR_Prepago";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_Prepago_verbales.xlsx");
                        using (MyMemoryStream)
                        {
                            //string FilePath = @"c:\temp\Prepago" + nombre + ".xlsx";
                            //string basePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                            //wb.SaveAs(MyMemoryStream);
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();


                        }


                    }

                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                   "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Prepago.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult ExportarAscard13(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_Ascard13", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "PQR_Ascard";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_Ascard_Verbales.xlsx");
                        using (MyMemoryStream)
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }


                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Ascard.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult ExportarCuotasAscard13(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_CuotasAscard13", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "PQR_CuotasAscard";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_CuotasAscard_Verbales.xlsx");
                        using (MyMemoryStream)
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();


                        }
                    }


                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "CuotasAscard.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult ExportarEliminarCentrales13(FormCollection formCollection)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TicketsUnificado"].ConnectionString);

                cmd = new SqlCommand("Sp_WorkOrder_PQR_EliminarCentrales13", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                con.Open();
                cmd.ExecuteNonQuery();

                da.SelectCommand = cmd;
                da.Fill(dt);
                MemoryStream MyMemoryStream = new MemoryStream();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "PQR_EliminarCentrales";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Data");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + "WorkOrder_PQR_EliminarCentrales_Verbales.xlsx");
                        using (MyMemoryStream)
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();


                        }
                    }


                }

                return new FileContentResult(MyMemoryStream.ToArray(),
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "EliminarCentrales.xlsx"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       

    }


}