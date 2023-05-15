using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Sites
    {
        public int IdMasterSites { get; set; }
        public string NameSite { get; set; }
        public bool RequiredSubStatus { get; set; }
        public string DescriptionSite { get; set; }
        public string DateLog { get; set; }
        public bool State { get; set; }
    }
}