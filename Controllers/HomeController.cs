using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace TicketsUnificado.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
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
            int perfil = 0;
            int perfilScare = 0;
            foreach (var item in UserActual.Perfiles)
            {
                if (item.NameProfile.Contains("Supervisor") || item.NameProfile.Contains("Jefe"))
                {
                    perfil = 1;

                }
                else if (item.NameProfile.Contains("Agente"))
                {
                    perfil = 2;
                    ViewBag.GrupoUsuario = await DAOCommand.ListGroupsInfo(UserActual);
                }

                if (item.NameProfile.Contains("préstamo"))
                {
                    perfilScare = 3;
                    ViewBag.GrupoUsuario = await DAOCommand.ListGroupsInfo(UserActual);
                }

            }
            ViewBag.Perfil = perfil;
            ViewBag.UserActual = UserActual.Nombres + ' ' + UserActual.PrimerApellido + ' ' + UserActual.SegundoApellido;
            ViewBag.Grupos = await DAOCommand.ListGroupsInfo(UserActual);
            ViewBag.scare = perfilScare;

            List<ColocacionPrestamo> NotifColocacion = new List<ColocacionPrestamo>();
            NotifColocacion = DAOCommand.SelTabla_ColocacionPrestamo_not();
            //var not1 = NotifColocacion.Count((x => x.Tipificacion2.Equals("VOLVER A LLAMAR")));//  (x => x. Equals("VOLVER A LLAMAR")));
            string hoy = Convert.ToString(DateTime.Today);
            hoy = hoy.ToString().Split(' ').ElementAt(0);
            //NotifColocacion = NotifColocacion.Where(x => x.Tipificacion2 == "VOLVER A LLAMAR").ToList();
            var not1 = NotifColocacion.Count(x => x.FechaLLamada.Contains(hoy));
            //var not2 = NotifColocacion.Count(x => x.Tipificacion1.Equals(""));
            ViewBag.Notificacion_colocacion = not1;


            return View();
        }

        public async Task<ActionResult> Historico(int IdGroup)
        {
            Users UserActual = await DAOCommand.InforUserActual(true);
            int perfil = 0;
            foreach (var item in UserActual.Perfiles)
            {
                if (item.NameProfile.Contains("Supervisor") || item.NameProfile.Contains("Jefe"))
                {
                    perfil = 1;

                }
                else if (item.NameProfile.Contains("Agente"))
                {
                    perfil = 2;
                }

            }
            HomeHistory Historico = new HomeHistory();
            Historico.ListHistoryState = await DAOCommand.ListHistoricoEstado(UserActual, perfil, IdGroup);
            Historico.ListHistoryPercentageANS = await DAOCommand.ListHistoricoPorcentajeANS(UserActual, perfil, IdGroup);
            Historico.ListHistoryAsigne = await DAOCommand.ListHistoricoAsignado(UserActual, perfil, IdGroup);
            Historico.ListMeta = await DAOCommand.ListMetas(UserActual, perfil, IdGroup);
            Historico.ListHistoryCompliance = await DAOCommand.ListHistoricoCumplimiento(UserActual, perfil, IdGroup);
            ViewBag.Grupos = await DAOCommand.ListGroupsInfo(UserActual);
            ViewBag.fechaHoy = DateTime.Now.ToString("dd-MM-yyyy");
            return PartialView(Historico);
        }

        //public async Task<ActionResult> HistoricoEstado()
        //{
        //    Users UserActual = await DAOCommand.InforUserActual(true);
        //    int Agente = 0;
        //    if (UserActual.Perfiles[0].NameProfile.Contains("Agente"))
        //    {
        //        Agente = 1;
        //    }
        //    List<HistoryState> ListData = await DAOCommand.ListHistoricoEstado(UserActual, Agente);
        //    return PartialView(ListData);
        //}


        //public async Task<ActionResult> HistoricoAsignado()
        //{
        //    Users UserActual = await DAOCommand.InforUserActual(true);
        //    int Agente = 0;
        //    if (UserActual.Perfiles[0].NameProfile.Contains("Agente"))
        //    {
        //        Agente = 1;
        //    }
        //    List<HistoryAsigne> ListData = await DAOCommand.ListHistoricoAsignado(UserActual, Agente);
        //    return PartialView(ListData);
        //}

        //public async Task<ActionResult> HistoricoCumplimiento()
        //{
        //    Users UserActual = await DAOCommand.InforUserActual(true);
        //    int Agente = 0;
        //    if (UserActual.Perfiles[0].NameProfile.Contains("Agente"))
        //    {
        //        Agente = 1;
        //    }
        //    List<HistoryCompliance> ListData = await DAOCommand.ListHistoricoCumplimiento(UserActual, Agente);
        //    return PartialView(ListData);
        //}

        //public async Task<ActionResult> HistoricoPorcentajeANS()
        //{
        //    Users UserActual = await DAOCommand.InforUserActual(true);
        //    int Agente = 0;
        //    if (UserActual.Perfiles[0].NameProfile.Contains("Agente"))
        //    {
        //        Agente = 1;
        //    }
        //    List<HistoryPercentage> ListData = await DAOCommand.ListHistoricoPorcentajeANS(UserActual, Agente);
        //    return PartialView(ListData);
        //}

        public async Task<ActionResult> GetGrupos(int IdMasterGroups, DateTime fecha)
        {
            List<Goals> metas = await DAOCommand.ValMeta(IdMasterGroups, fecha);
            return Json(metas[0], JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveMeta(int IdMasterGroups, int Meta, DateTime fecha)
        {
            Users InforUser = await DAOCommand.InforUserActual();
            await DAOCommand.SaveMeta(InforUser.IdMasterUsers, IdMasterGroups, Meta, fecha);
            return new EmptyResult();
        }
    }
}