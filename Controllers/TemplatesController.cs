using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class TemplatesController : Controller
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
            return View();
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
        public async Task<ActionResult> ListTemplates()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            List<Sites> ListSitio = await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 11, true); //Plantillas
            return PartialView(await DAOCommand.ListTemplates(null,null,ListSitio));
        }
        public async Task<ActionResult> DeleteTemplate(int IdTemplate)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.EnabledDisabledTemplate(InforUser.IdMasterUsers, IdTemplate,false);
            return new EmptyResult();
        }
        public async Task<ActionResult> ActivateTemplate(int IdTemplate)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.EnabledDisabledTemplate(InforUser.IdMasterUsers, IdTemplate,true);
            return new EmptyResult();
        }
        public async Task<ActionResult> SaveTemplate(Templates Template)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.SaveTemplateFull(InforUser.IdMasterUsers, Template);
            return new EmptyResult();
        }
        public async Task<ActionResult> FormEditTemplates(int? id=null)
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            ListasDesplegables Listas = new ListasDesplegables();
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 11, true); //Plantillas

            var PermisoCrear = await DAOCommand.ListPermisos(InforUser.Perfiles, 19); //Crear plantillas
            if (PermisoCrear.Count == 0 & id == null)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para crear plantillas."
                });
            }
            var PermisoModificar = await DAOCommand.ListPermisos(InforUser.Perfiles, 20); //Modificar plantillas
            if (PermisoModificar.Count ==0 & id != null)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para editar plantillas."
                });
            }
            List<Templates> Templates = new List<Templates>();
            if (id != null)
            {
                Templates = await DAOCommand.ListTemplates(id);
            }
            Templates Template = new Templates();
            if (Templates.Count > 0)
            {
                Template = Templates[0];
                FieldsUDF ObjField = new FieldsUDF();
                ObjField.Template.IdTemplates = Template.IdTemplates;
                Template.ListFieldsUDF = await DAOCommand.ListFieldsUDF(ObjField, false,true,true);
                Template.ListFieldsUDFSolutions = await DAOCommand.ListFieldsUDF(ObjField, true, true, true);
            }
            else if(Templates.Count==0 & id != null)
            {
                return View("~/Views/Home/ErrorPartial.cshtml", new ErrorViewModel
                {
                    TituloError = "ACCESO DENEGADO",
                    DetalleError = "Usted no cuenta con permisos para editar esta plantilla."
                });
            }
            Listas.FieldsTypes = await DAOCommand.ListFieldsTypesUDF(null,true);
            Listas.ListTypesRequired = await DAOCommand.ListTypesRequired();
            ViewBag.Listas = Listas;
            return View(Template);
        }
        public async Task<ActionResult> ModalEditInput()
        {
            ListasDesplegables Listas = new ListasDesplegables();
            Listas.FieldsTypes = await DAOCommand.ListFieldsTypesUDF(null, true);
            Listas.ListTypesRequired = await DAOCommand.ListTypesRequired();
            return PartialView(Listas);
        }
        public async Task<ActionResult> VerifyNameTemplate(string NameTemplate)
        {
            DataTable dt = await DAOCommand.VerifyNameTemplate(NameTemplate);
            if (dt.Rows.Count > 0)
            {
                return Json($"Plantilla {NameTemplate} ya existe en este sitio.");
            }
            return Json(true);
        }
        public async Task<ActionResult> FieldsUDFJson(FieldsUDF Fields)
        {
            List<FieldsUDF> dataFieldsUDF = await DAOCommand.ListFieldsUDF(Fields);
            return Json(dataFieldsUDF[0], JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DisabledFields(int IdFields)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.EnabledDisabledFieldsUDF(InforUser.IdMasterUsers, IdFields, false);
            return new EmptyResult();
        }
        public async Task<ActionResult> EnabledFields(int IdFields)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.EnabledDisabledFieldsUDF(InforUser.IdMasterUsers, IdFields, true);
            return new EmptyResult();
        }
        public async Task<ActionResult> UpdateFieldsUDF(FieldsUDF Fields)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.UpdateFieldsUDF(InforUser.IdMasterUsers, Fields);
            return new EmptyResult();
        }
        public async Task<ActionResult> UpdateTemplate(Templates Template)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.UpdateTemplates(InforUser.IdMasterUsers, Template);
            return new EmptyResult();
        }
        public async Task<ActionResult> SaveFieldsUDF(FieldsUDF Fields)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            FieldsUDF FieldsUDF = await DAOCommand.SaveNewFieldsUDF(InforUser.IdMasterUsers, Fields);
            return PartialView("ViewTypeField", FieldsUDF);
        }
        public ActionResult ViewTypeFieldLocal(FieldsUDF Fields)=> PartialView(Fields);
        public async Task<ActionResult> FieldsTypeList(FieldsUDF Fields)
        {
            List<FieldsUDF> dataFieldsUDF = await DAOCommand.ListFieldsUDF(Fields,Fields.SolutionField,true);
            return Json(dataFieldsUDF, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ListDispositions(int? IdFieldsUDF=null,long? Parent_IdDispositions=null)
        {
            List<FieldsDispositions> ListDispositions = await DAOCommand.ListDispositions(IdFieldsUDF, Parent_IdDispositions);
            return Json(ListDispositions, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DeleteItemDispositions(int IdDispositions)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.DisabledDispositions(InforUser.IdMasterUsers, IdDispositions);
            return new EmptyResult();
        }

        public async Task<ActionResult> DiasHabiles(int DiasHabiles)
        {
            List<DiasHabiles> ListDiasHabiles = await DAOCommand.DiasHabiles(DiasHabiles);
            return Json(ListDiasHabiles, JsonRequestBehavior.AllowGet);
        }
    }
}