using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AIBTicketsMVC.Controllers
{
    public class ImportWorkOrderController : Controller
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
            var Importar = await DAOCommand.ListPermisos(UserActual.Perfiles, 4); //Importar Solicitudes
            if (Importar.Count==0)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este formulario."
                });
            }
            ListasDesplegables Listas = new ListasDesplegables();
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 4, false); //Importar Solicitudes
            Listas.Algorithms = await DAOCommand.ListAlgorithms(true);
            return View(Listas);
        }
        public async Task<ActionResult> GetListTemplates(Sites Sitio)
        {
            List<Sites> ListSitios = new List<Sites> { Sitio };
            List<Templates> ListTemplates = await DAOCommand.ListTemplates(null, true, ListSitios);
            return Json(ListTemplates, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetListGroups(Sites Sitio)
        {
            List<Sites> ListSitios = new List<Sites> { Sitio };
            List<Groups> ListGroups = await DAOCommand.ListGroups(ListSitios,null,true);
            return Json(ListGroups, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ViewAsignarCampos()
        {
            FieldsUDF Campos = new FieldsUDF();
            List<FieldsUDF> ListFields = await DAOCommand.ListFieldsUDF(Campos, false, true, true,true);
            return PartialView(ListFields);
        }
        [HttpPost]
        public async Task<ActionResult> AdjuntarExcel()
        {
            WorkOrder_DataImported Excel = new WorkOrder_DataImported();
            if (Request.Files.Count == 0)
            {
                Excel.returnError = "Seleccione la data a cargar";
                return Json(Excel,JsonRequestBehavior.AllowGet);
            }
            HttpPostedFileBase fileID = Request.Files[0];
            if (Path.GetExtension(fileID.FileName) != ".xls" & Path.GetExtension(fileID.FileName) != ".xlsx")
            {
                Excel.returnError = "El archivo cargado no es de formato excel (xls,xlsx), cargue uno valido para continuar.";
                return Json(Excel, JsonRequestBehavior.AllowGet);
            }
                
            if (fileID.FileName.Length > 100)
            {
                Excel.returnError = "El nombre del archivo no puede superar los 100 caracteres.";
                return Json(Excel, JsonRequestBehavior.AllowGet);
            }
            DataTable dt = await DAOCommand.VerifyNameDataExcel(fileID.FileName);
            if (dt.Rows.Count>0)
            {
                Excel.returnError = "Ya existe un archivo cargado con este nombre, favor verificar.";
                return Json(Excel, JsonRequestBehavior.AllowGet);
            }

            string path = Server.MapPath($"~/Uploads/");
            Excel = await DAOCommand.UploadFileServer(fileID, path);
            return Json(Excel,JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> TypesFields(int IdTemplate)
        {
            Users UserActual = await DAOCommand.InforUserActual(true, true);
            List<Templates> ListTemplates = await DAOCommand.ListTemplates(IdTemplate);
            FieldsUDF ObjField = new FieldsUDF();
            ObjField.Template.IdTemplates = ListTemplates[0].IdTemplates;
            /*Campos de plantilla*/
            ListTemplates[0].ListFieldsUDF = await DAOCommand.ListFieldsUDF(ObjField, false, true, true);
            /*Campos universales*/
            FieldsUDF FieldVacio = new FieldsUDF();
            List<FieldsUDF> ListFieldsNew =await DAOCommand.ListFieldsUDF(FieldVacio, false,true,true,true);
            ListFieldsNew.AddRange(ListTemplates[0].ListFieldsUDF);
            List<string> SQLColumns =new List<string>();
            foreach (var ItemFields in ListFieldsNew)
            {
                SQLColumns.Add(await DAOCommand.ArmarColumnSQL(ItemFields));
            }
            WorkOrder_DataImported InforExcel = await Tools.SessionGetObject<WorkOrder_DataImported>("InforExcel");
            InforExcel.SQLTableTemp = $"CREATE TABLE #WorkOrder_Fields_temp(IdWorkOrderTemp INT IDENTITY(1,1) NOT NULL,{string.Join(",", SQLColumns)});";
            Tools.SessionSetObject("InforExcel", InforExcel);
            return PartialView(ListTemplates[0].ListFieldsUDF);
        }
        public async Task<ActionResult> ColumnasExcel(string HojaExcel)
        {
            WorkOrder_DataImported InforExcel = new WorkOrder_DataImported();
            try
            {
                InforExcel = await Tools.SessionGetObject<WorkOrder_DataImported>("InforExcel");
                InforExcel.HojaSelected = HojaExcel;

                DataTable dt = await DAOCommand.DataTableExcel(InforExcel.ConexString, HojaExcel);
                InforExcel.Columnas = dt.Columns.Cast<DataColumn>().Select(Linq => Linq.ColumnName.ToString()).ToList();
                if (dt.Rows.Count == 0 | dt.Columns.Count == 0) InforExcel.returnError = "Esta hoja no cuenta con ningun registro.";
                else InforExcel.NumRecords = dt.Rows.Count;
                
                Tools.SessionSetObject("InforExcel", InforExcel);
            }
            catch (Exception ex)
            {
                InforExcel.returnError = ex.Message;
                string Error = $"ImportWorkOrder.ColumnasExcel({Tools.GetLineErr(ex)}): {ex.Message}";
                await Tools.LogAplications("ERROR", Error);
                Thread hilo = new Thread(async () => await Tools.SendMailException(Error))
                { IsBackground = true };
                hilo.Start();
            }
            return Json(InforExcel,JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ValidarColumnExcel(ImportWorkOrder Import)
        {
            //VARIABLE SESSION INFOR EXCEL
            WorkOrder_DataImported InforExcel = await Tools.SessionGetObject<WorkOrder_DataImported>("InforExcel");
            try
            {
                await DAOCommand.SqlBulkXColumn(Import, InforExcel);
            }
            catch (Exception ex)
            {
                InforExcel.returnError = ex.Message;
            }
            return Json(InforExcel, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ListUsersXGroup(int IdGroups)
        {
            List<Users> ListUsers = await DAOCommand.ListUsersXGroup(IdGroups,true);
            return Json(ListUsers, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> FinalizarImportacion(ImportWorkOrder Import)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.FinalizeImport(UserActual.IdMasterUsers, Import);
            return new EmptyResult();
        }

        public async static Task GuardarRadicacion(DataTable dtexcel)
        {
            try
            {
                //                MasterUsers CurrentUser = await DAOGeneral.DataCurrentUser();
                SqlCommand cmd = await DAOConfig.SqlCommandGeneralSD();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_WorkOrder_Ins_BasePortalZec";
                if (dtexcel.Rows.Count != 0)
                    foreach (DataRow row in dtexcel.Rows)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@IdPortalZec_ETB", row["IdPortal"].ToString());
                        cmd.Parameters.AddWithValue("@Tipologia", row["Tipologia"].ToString());
                        cmd.Parameters.AddWithValue("@Nombre_Cliente", row["Nombre_Cliente"].ToString());
                        cmd.Parameters.AddWithValue("@Telefono", row["Telefono"].ToString());
                        cmd.Parameters.AddWithValue("@Documento", row["Documento"].ToString());
                        cmd.Parameters.AddWithValue("@Correo", row["Correo"].ToString());
                        cmd.Parameters.AddWithValue("@Fecha_Registro", row["Fecha_Registro"].ToString());
                        cmd.Parameters.AddWithValue("@Telefono_Implicado", row["Telefono_Implicado"].ToString());
                        cmd.Parameters.AddWithValue("@Observacion", row["Observacion"].ToString());
                        cmd.Parameters.AddWithValue("@Usuario_Final", row["Usuario_Final"].ToString());
                        //cmd.Parameters.AddWithValue("@IdUser", CurrentUser.IdMasterUsers);
                        cmd.ExecuteNonQuery();
                    }
            }
            catch (Exception ex)
            {
                //throw new Exception(Tools.MsjError($"{RutaDAO}GuardarGestionPortalZEC", ex));
            }
        }

    }
}