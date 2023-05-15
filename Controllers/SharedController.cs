using AIBTicketsMVC.App_Code;
using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TicketsUnificado.Controllers
{
    public class SharedController : Controller
    {
        public async Task<ActionResult> Navigation()
        {
            Users InforUser = await DAOCommand.InforUserActual(true);
            NivelesMenu LvlMenus = new NivelesMenu();
            if (InforUser != null) { 
                LvlMenus = await DAOCommand.MainMenu(InforUser.Perfiles, 1);
            }
            ConstProject P = new ConstProject();
            ViewBag.WinuserActual = P.WinuserActual;

            //buscar las alertas de los tickets a vercer
            List<WorkOrder> NumAlerts = await DAOCommand.ListAlertas(InforUser);
            ViewBag.divNumAlerts = NumAlerts.Count();
            List<string> IdWorkOrder = new List<string>();
            foreach(var item in NumAlerts)
            {
                IdWorkOrder.Add(item.IdWorkOrder.ToString());
            }
            ViewData["IdWorkOrder"] = IdWorkOrder;
            return PartialView(LvlMenus);
        }

        
    }
}