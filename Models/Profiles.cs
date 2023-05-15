using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Profiles
    {
        public int IdMasterProfiles { get; set; } = 0;
        public string NameProfile { get; set; }
        public string DescriptionProfile { get; set; }
        public string DateLog { get; set; }
        public bool State { get; set; }
        public List<MenuAndActions> Menu { get; set; } = new List<MenuAndActions>();
        public List<Sites> Sitios { get; set; } = new List<Sites>();
    }
}