using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Notifications
    {
        public Users User { get; set; }
        public string DateLog { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
    }
}