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
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

namespace AIBTicketsMVC.Controllers
{
    public class ColocacionPrestamoController : Controller
    {
        async public Task<ActionResult> Index()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;

            //alertas colocacion de prestamo
            List<ColocacionPrestamo> ListAlertasColocacion = new List<ColocacionPrestamo>();
            ListAlertasColocacion = DAOCommand.ListAlertasColocacion();
            List<ColocacionPrestamo> ListAlert = ListAlertasColocacion;
            DateTime fecha = DateTime.Now;
            ViewBag.NumAlertasColocacion = ListAlertasColocacion.Count();
            ViewBag.IdRegistro = ListAlert.ToList();


            return View();
        }
        //public ActionResult ListReclamos()
        public async Task<ActionResult> ListColocacionPrestamos(int? Id)
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;
            List<ColocacionPrestamo> ListReclamosD = new List<ColocacionPrestamo>();
            ListReclamosD = DAOCommand.SelTabla_ColocacionPrestamo(Id); //Plantillas
            DAOCommand objetoDispositionReclamo = new DAOCommand();
            List<ColocacionPrestamo> ListDispTipoCredito = new List<ColocacionPrestamo>();
            ListDispTipoCredito = DAOCommand.SelDispTipoCredito();
            List<ColocacionPrestamo> ListDispTip1 = ListDispTipoCredito;
            List<ColocacionPrestamo> ListDispTip2 = ListDispTipoCredito;
            List<ColocacionPrestamo> ListDispTip3 = ListDispTipoCredito;
            List<ColocacionPrestamo> ListDispTip4 = ListDispTipoCredito;
            ListDispTipoCredito = ListDispTipoCredito.Where(x => x.NombreSelect == "Tipo de credito").ToList();
            ListDispTip1 = ListDispTip1.Where(x => x.NombreSelect == "Tipificacion 1").ToList();
            ListDispTip2 = ListDispTip2.Where(x => x.NombreSelect == "Tipificacion 2").ToList();
            ListDispTip3 = ListDispTip3.Where(x => x.NombreSelect == "Tipificacion 3").ToList();
            ListDispTip4 = ListDispTip4.Where(x => x.NombreSelect == "Tipificacion 4").ToList();
            ViewBag.TipoCredito = ListDispTipoCredito;
            ViewBag.Tip1 = ListDispTip1;
            ViewBag.Tip2 = ListDispTip2;
            ViewBag.Tip3 = ListDispTip3;
            ViewBag.Tip4 = ListDispTip4;

            ViewBag.registro = ListReclamosD;



            return PartialView(ListReclamosD);

        }

        public async Task<ActionResult> ListColocacionPrestamosllamadas(int? Id)
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;
            List<ColocacionPrestamo> ListReclamosD = new List<ColocacionPrestamo>();
            ListReclamosD = DAOCommand.SelTabla_ColocacionPrestamo(Id); //Plantillas
            DAOCommand objetoDispositionReclamo = new DAOCommand();
            List<ColocacionPrestamo> ListDispTipoCredito = new List<ColocacionPrestamo>();
            ListDispTipoCredito = DAOCommand.SelDispTipoCredito();
            List<ColocacionPrestamo> ListDispTip1 = ListDispTipoCredito;
            List<ColocacionPrestamo> ListDispTip2 = ListDispTipoCredito;
            List<ColocacionPrestamo> ListDispTip3 = ListDispTipoCredito;
            List<ColocacionPrestamo> ListDispTip4 = ListDispTipoCredito;
            ListDispTipoCredito = ListDispTipoCredito.Where(x => x.NombreSelect == "Tipo de credito").ToList();
            ListDispTip1 = ListDispTip1.Where(x => x.NombreSelect == "Tipificacion 1").ToList();
            ListDispTip2 = ListDispTip2.Where(x => x.NombreSelect == "Tipificacion 2").ToList();
            ListDispTip3 = ListDispTip3.Where(x => x.NombreSelect == "Tipificacion 3").ToList();
            ListDispTip4 = ListDispTip4.Where(x => x.NombreSelect == "Tipificacion 4").ToList();
            ViewBag.TipoCredito = ListDispTipoCredito;
            ViewBag.Tip1 = ListDispTip1;
            ViewBag.Tip2 = ListDispTip2;
            ViewBag.Tip3 = ListDispTip3;
            ViewBag.Tip4 = ListDispTip4;
            string hoy = Convert.ToString(DateTime.Today);
            hoy = hoy.ToString().Split(' ').ElementAt(0);
            ListReclamosD = ListReclamosD.Where(x => x.Tipificacion2 == "VOLVER A LLAMAR").ToList();
            ListReclamosD = ListReclamosD.Where(x => x.FechaLLamada.Contains(hoy)).ToList();

            ViewBag.registro = ListReclamosD;

            return PartialView(ListReclamosD);

        }


        public async Task<ActionResult> ListColocacionPrestamosId(int Id)
        {
            List<ColocacionPrestamo> ListRegistro = new List<ColocacionPrestamo>();
            ListRegistro = DAOCommand.SelTabla_ColocacionPrestamoId(Id); //Plantillas
            DAOCommand objetoDispositionReclamo = new DAOCommand();

            List<ColocacionPrestamo> ListDispTipoCredito = new List<ColocacionPrestamo>();
            ListDispTipoCredito = DAOCommand.SelDispTipoCredito();
            ViewBag.registro = ListRegistro;
            ViewBag.TipoCredito = ListDispTipoCredito; new SelectList(ListDispTipoCredito, "Id", "Descripcion");

            return Json(ListRegistro[0], JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> FormEdit(int? Id)
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;

            List<ColocacionPrestamo> ListReclamosD = new List<ColocacionPrestamo>();
            ListReclamosD = DAOCommand.SelTabla_ColocacionPrestamo(Id); //Plantillas

            DAOCommand objetoDispositionReclamo = new DAOCommand();
            List<Disposition> lstEstadoReclamo = objetoDispositionReclamo.ObtenerEstado();
            ViewBag.EstadoReclamo = new SelectList(lstEstadoReclamo, "Id", "Descripcion");

            List<Disposition> lstSubEstadoReclamo = objetoDispositionReclamo.ObtenerSubEstado();
            ViewBag.SubEst = new SelectList(lstSubEstadoReclamo, "Id", "Descripcion");

            return PartialView(ListReclamosD);
        }

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

        public async Task<ActionResult> ListDispositions(int? Id)
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;
            DAOCommand objetoDispositionReclamo = new DAOCommand();
            List<ColocacionPrestamo> ListDispTipoCredito = new List<ColocacionPrestamo>();
            ListDispTipoCredito = DAOCommand.SelDispTipoCredito(Id);
            ViewBag.TipoCredito = ListDispTipoCredito;


            return PartialView(ListDispTipoCredito);

        }

        public async Task<ActionResult> ListDispositionsId(ColocacionPrestamo colocacion)
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            string iSuper = DAOCommand.EsAgente(InforUser);
            ViewBag.user = iSuper;
            DAOCommand objetoDispositionReclamo = new DAOCommand();
            List<ColocacionPrestamo> ListDispTipoCredito = new List<ColocacionPrestamo>();
            ListDispTipoCredito = DAOCommand.SelDispTipoCreditoId(colocacion);
            ViewBag.TipoCredito = ListDispTipoCredito;


            return Json(ListDispTipoCredito[0], JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> ActualizarDisposition(ColocacionPrestamo colocacion)
        {
            try
            {
                await DAOCommand.ActualizarDisposition(colocacion);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                //Tools.SendMailExceptionXHilo(Tools.MsjError("TemplatesController.SaveTemplate", ex)).Start();
                throw new Exception();
            }
        }

        public async Task<ActionResult> ActualizarRegistro(ColocacionPrestamo Registro)
        {
            try
            {
                Users InforUser = await DAOCommand.InforUserActual(true);
                await DAOCommand.ActualizarRegistro(Registro);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                //Tools.SendMailExceptionXHilo(Tools.MsjError("TemplatesController.SaveTemplate", ex)).Start();
                throw new Exception();
            }
        }

        public async Task<ActionResult> AddDisposition(ColocacionPrestamo colocacion)
        {

            try
            {
                Users UserActual = await DAOCommand.InforUserActual();
                await DAOCommand.AddDisposition(UserActual.IdMasterUsers, colocacion);
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