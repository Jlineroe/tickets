using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using AIBTicketsMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AIBTicketsMVC.Controllers
{
    public class ListWorkOrdersController : Controller
    {
        public async Task<ActionResult> Index()
        {
            await Tools.LogAplications("Ingreso","Solicitudes");
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
            //PERFIL USUARIO ACTUAL
            int perfil = 0;
            foreach (var item in UserActual.Perfiles)
            {
                if (item.NameProfile.Contains("Supervisor"))
                {
                    perfil = 1;

                }
                else if (item.NameProfile.Contains("Agente"))
                {
                    perfil = 2;
                }

            }
            //PAGINACION
            try
            {
                await Tools.LogAplications("Ingreso", "Solicitudes");
                FiltersWorkOrder Filters = new FiltersWorkOrder { 
                    pag = 1,
                    top = 10
                };
                ViewModelListOrders ViewModel = new ViewModelListOrders();
                ViewModel.ListWorkOrder = await DAOCommand.ListWorkOrderNew(UserActual, Filters);
                ViewModel.Pagination.TotalRegis = await DAOCommand.CountWorkOrder(UserActual, Filters);
                ViewModel.Pagination.PaginaActual = Filters.pag;
                ViewModel.Pagination.RegisXPagina = Filters.top;
                ViewBag.Perfil = perfil;
                if (ViewModel.Pagination.TopDefault.Max() < ViewModel.Pagination.TotalRegis) {
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
        public async Task<ActionResult> ListOrders(FiltersWorkOrder Filters)
        {
            Users UserActual = await DAOCommand.InforUserActual(true);
            List<WorkOrder> ListData= await DAOCommand.ListWorkOrderNew(UserActual, Filters);
            return PartialView(ListData);
        }
        public async Task<ActionResult> Pagination(FiltersWorkOrder Filters)
        {
            Users UserActual = await DAOCommand.InforUserActual(true);
            PaginModel Pagin = new PaginModel();
            Pagin.TotalRegis = await DAOCommand.CountWorkOrder(UserActual, Filters);
            Pagin.PaginaActual = Filters.pag;
            Pagin.RegisXPagina = Filters.top;
            if (Pagin.TopDefault.Max() < Pagin.TotalRegis)
            {
                Pagin.TopDefault.Add(Pagin.TotalRegis);
            }
            return PartialView(Pagin);
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
        public async Task<ActionResult> ViewFilters()
        {
            Users UserActual = await DAOCommand.InforUserActual(true,true,true);
            ListasDesplegables List = new ListasDesplegables();
            List.Grupos = await DAOCommand.ListGroupsXUsers(UserActual.IdMasterUsers);
            List.ListTemplates = await DAOCommand.ListTemplates(null,true, UserActual.Sitios);
            List.Estados = await DAOCommand.ListStatusDefinition(UserActual.Sitios, null,null,1,true);
            foreach (var item in UserActual.Grupos)
            {
                List.Usuarios.AddRange(await DAOCommand.ListUsersXGroup(item.IdMasterGroups));
            }
            List<Users> ListResult = (from U in List.Usuarios
                             group U by new { U.IdMasterUsers, U.Identificacion, U.Nombres, U.PrimerApellido, U.SegundoApellido } into User
                             where User.Count() > 0
                             select new Users()
                             {
                                 IdMasterUsers = User.Key.IdMasterUsers,
                                 Identificacion = User.Key.Identificacion,
                                 Nombres = User.Key.Nombres,
                                 PrimerApellido = User.Key.PrimerApellido,
                                 SegundoApellido = User.Key.SegundoApellido
                             }).ToList();
            List.Usuarios = ListResult;
            return PartialView(List);
        }
        public async Task<ActionResult> ListUsersXGroup(int IdGroups)
        {
            List<Users> ListUsers = await DAOCommand.ListUsersXGroup(IdGroups, true);
            return Json(ListUsers, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ViewReassign()
        {
            Users UserActual = await DAOCommand.InforUserActual();
            ListasDesplegables Listas = new ListasDesplegables();
            Listas.Grupos = await DAOCommand.ListGroupsXUsers(UserActual.IdMasterUsers);
            Listas.Algorithms = await DAOCommand.ListAlgorithms(false);
            return PartialView(Listas);
        }
        public async Task<ActionResult> WorkOrderReassign(ImportWorkOrder WorkOrder)
        {
            Users UserActual = await DAOCommand.InforUserActual();
            await DAOCommand.UpdateReassignWorkOrder(UserActual.IdMasterUsers, WorkOrder);
            return new EmptyResult();
        }
    }
}