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
    public class CategoriesController : Controller
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
            Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 10,true); //Categorias
            Listas.Grupos = await DAOCommand.ListGroups(Listas.Sitios);
            return View(Listas);
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
        public async Task<ActionResult> ListCategories()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            List<Sites> Sitios = await DAOCommand.ListSitiosConPermisos(InforUser.Perfiles, 10,true); //Categorias
            return PartialView(await DAOCommand.ListCategories(Sitios, null,null,1));
        }
        public async Task<ActionResult> VerifyNameCategory(string NameCategory)
        {
            DataTable dt = await DAOCommand.VerifyNameCategory(NameCategory);
            if (dt.Rows.Count > 0)
            {
                return Json($"Categoria {NameCategory} ya existe en este u otro sitio.");
            }
            return Json(true);
        }
        public async Task<ActionResult> UpdateSLASubCategory(Categories Category)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.UpdateSLASubCategory(InforUser.IdMasterUsers, Category);
            return new EmptyResult();
        }
        public async Task<ActionResult> SaveCategories(Categories Categoria)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.SaveCategoria(InforUser.IdMasterUsers, Categoria);
            return new EmptyResult();
        }
        public async Task<ActionResult> DeleteCategory(int IdCategory)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.DeleteCategory(InforUser.IdMasterUsers, IdCategory);
            return new EmptyResult();
        }
        public async Task<ActionResult> CategoriesJson(int IdCategory)
        {
            Users InforUser = await DAOCommand.InforUserActual(true, true);
            List<Categories> Categoria = await DAOCommand.ListCategories(InforUser.Sitios,IdCategory);
            Categoria[0].SubCategory = await DAOCommand.ListCategories(InforUser.Sitios, null, IdCategory);
            Categoria[0].SubCategory = Categoria[0].SubCategory.Where(lq => lq.State == true).ToList();
            return Json(Categoria[0], JsonRequestBehavior.AllowGet);
        }
    }
}