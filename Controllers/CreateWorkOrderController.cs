using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class CreateWorkOrderController : Controller
    {
        public async Task<ActionResult> Index()
        {
            Tools.SessionSetObject("ListAdjuntos", null);
            Users UserActual = await DAOCommand.InforUserActual(true);
            if (UserActual == null)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este aplicativo."
                });
            }
            var Importar = await DAOCommand.ListPermisos(UserActual.Perfiles, 6); //Crear solicitudes
            if (Importar.Count == 0)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para ingresar a este formulario."
                });
            }
            ListasDesplegables Listas = new ListasDesplegables();
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 6,false); //Crear solicitudes
            return View(Listas);
        }
        public async Task<ActionResult> GetListTemplates(Sites Sitio)
        {
            List<Sites> ListSitios = new List<Sites> { Sitio };
            List<Templates> ListTemplates = await DAOCommand.ListTemplates(null,true, ListSitios);
            return Json(ListTemplates, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetListCategory(Sites Sitio)
        {
            List<Sites> ListSitios = new List<Sites> { Sitio };
            List<Categories> ListCategory = await DAOCommand.ListCategories(ListSitios, null,null,1);
            return Json(ListCategory, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetSubCategory(int? IdCategory)
        {
            Users UserActual = await DAOCommand.InforUserActual(true, true);
            List<Sites> ListSitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 6, false); //Crear solicitudes
            List<Categories> Category = await DAOCommand.ListCategories(ListSitios, IdCategory);
            Category[0].Grupo.UsersXGroups = await DAOCommand.ListUsersXGroup(Category[0].Grupo.IdMasterGroups, true);
            Category[0].SubCategory = await DAOCommand.ListCategories(UserActual.Sitios, null, IdCategory);
            return Json(Category[0], JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> TypeField(FieldsUDF Fields)
        {
            List<FieldsUDF> ListFields= await DAOCommand.ListFieldsUDF(Fields, false, true,true);
            if (ListFields.Where(lq => lq.FieldType.IdFieldsTypesUDF == 8).ToList().Count > 0) {
                ViewBag.ListLocation = await DAOCommand.ListLocation();
            }
            return PartialView(ListFields);
        }
        public async Task<ActionResult> GetListDispositions(long ParentIdDisposi)
        {
            List<FieldsDispositions> ListDispo = await DAOCommand.ListDispositions(null, ParentIdDisposi);
            return Json(ListDispo, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> SaveWorkOrder(WorkOrder Ticket)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            WorkOrder ReturnTicket= await DAOCommand.SaveCrearTicket(UserActual.IdMasterUsers, Ticket);
            /*Mover los archivos*/
            List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
            if (ListAdjuntos != null)
            {
                string path = Server.MapPath($"~/Uploads/WorkOrder/{ReturnTicket.IdWorkOrder}/");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                foreach (var item in ListAdjuntos)
                {
                    string RutaDestino = path + Path.GetFileName(item.NameEncryptedAttachment + item.Extension);
                    System.IO.File.Move(item.FilePath, RutaDestino);
                }
            }
            return new EmptyResult();
        }
        public async Task<ActionResult> AddAttachments(HttpPostedFileWrapper Adjunto)
        {
            List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
            if (ListAdjuntos == null)
            {
                ListAdjuntos = new List<WorkOrder_Attachments>();
            }
            WorkOrder_Attachments ObjAdjunto = new WorkOrder_Attachments();
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
            const decimal wKilo = 1026.865671641791M;
            ObjAdjunto.FileSizeKB = Math.Round(Adjunto.ContentLength / wKilo, 2);
            if (ObjAdjunto.FileSizeKB > 20000)
            {
                ObjAdjunto.msjError = "El peso del archivo no debe superar los 20 MB";
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
        public ActionResult DivAttachment(WorkOrder_Attachments InforAdjunto) => PartialView("~/Views/WorkOrderSolutions/", InforAdjunto);
        public async Task<ActionResult> DeleteAttachment(long NameEncrypted)
        {
            List<WorkOrder_Attachments> ListAdjuntos = await Tools.SessionGetObject<List<WorkOrder_Attachments>>("ListAdjuntos");
            var Objeto = ListAdjuntos.Where(lq => lq.NameEncryptedAttachment == NameEncrypted).ToList();
            if (Objeto.Count > 0)
            {
                if (System.IO.File.Exists(Objeto[0].FilePath)) System.IO.File.Delete(Objeto[0].FilePath);

                ListAdjuntos.RemoveAll(x => x.NameEncryptedAttachment == NameEncrypted);
                Tools.SessionSetObject("ListAdjuntos", ListAdjuntos);
            }
            return new EmptyResult();
        }
        async public Task<ActionResult> ListPlatform(int IdLocation)
        {
            List<WH_Platform> ListData = await DAOCommand.ListPlatform(null, IdLocation);
            return Json(ListData, JsonRequestBehavior.AllowGet);
        }
        async public Task<ActionResult> ListBooth(int IdPlatform)
        {
            List<WH_Booth> ListData = await DAOCommand.ListBooth(IdPlatform);
            return Json(ListData, JsonRequestBehavior.AllowGet);
        }
    }
}