using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class WorkOrder_Assigned
    {
        public long IdWorkOrder { get; set; }
        public int IdStatusDefinition { get; set; }

    }
}