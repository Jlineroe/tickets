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
    public class AgentBinnacleController : Controller
    {
        // GET: AgentBinnacle
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
            Listas.Bases = await DAOCommand.ListBases();
            List<SINO> Sino = await DAOCommand.ListSino();
            Listas.Imagenes = Sino;
            Listas.Ascard = Sino;
            Listas.Contrato = Sino;
            Listas.Grabacion = Sino;
            Listas.Reasignacion = Sino;
            Listas.Canal = await DAOCommand.ListCanal();
            Listas.Legalizado = Sino;
            Listas.TipoReclamo = await DAOCommand.ListTipoReclamo();
            Listas.EstadoBitacora = await DAOCommand.ListEstadoBitacora();

            //Listas.Sitios = await DAOCommand.ListSitiosConPermisos(UserActual.Perfiles, 6, false); //Crear solicitudes
            return View(Listas);
        }
    }
}