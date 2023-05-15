using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class WorkOrder_Escalations
    {
        public long IdWorkOrder { get; set; }
        public long IdWorkOrderEscalations { get; set; }
        public Users UserGestiona { get; set; } = new Users();
        public Users UserScaled { get; set; } = new Users();
        public Groups GroupsScaled { get; set; } = new Groups();
        public string TypeScaling { get; set; }
        public string Comments { get; set; }
        public string DateLog { get; set; }
    }
}