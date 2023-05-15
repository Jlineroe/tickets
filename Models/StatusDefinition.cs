using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class StatusDefinition
    {
        public int IdStatusDefinition { get; set; }
        public int Level { get; set; }
        public int? Parent_IdStatus { get; set; }
        public string NameStatus{ get; set; }
        public string DescriptionStatus { get; set; }
        public Status_TypesActions TypeAction { get; set; } = new Status_TypesActions();
        public List<StatusDefinition> SubStatus { get; set; } = new List<StatusDefinition>();
        public string DateLog { get; set; }
        public bool State { get; set; }
        public bool Universal { get; set; }
        public Sites Sitio { get; set; } = new Sites();
    }
}