using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Categories
    {
        public int IdCategory { get; set; }
        public int Level { get; set; }
        public int Parent_IdCategory { get; set; }
        public string NameCategory { get; set; }
        public int SLA_HOUR { get; set; }
        public string DescriptionCategory { get; set; }
        public string DateLog { get; set; }
        public bool State { get; set; }
        public List<Categories> SubCategory { get; set; }
        public Groups Grupo { get; set; }
        public Sites Sitio { get; set; }
        public Templates Template { get; set; }
    }
}