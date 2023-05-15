using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Semaphores
    {
        public int IdSemaphore { get; set; }
        public Sites Sitio { get; set; } = new Sites();
        public Categories Category { get; set; } = new Categories();
        public Categories SubCategory { get; set; } = new Categories();
        public int SLA_HOUR { get; set; }
        public int GreenTo { get; set; }
        public int OrangeTo { get; set; }
        public string DateLog { get; set; }
        public bool State { get; set; }
    }
}