using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Templates
    {
        public int IdTemplates { get; set; }
        public string NameTemplate { get; set; }
        public string DescriptionTemplate { get; set; }
        public Sites Sitio { get; set; } = new Sites();
        public List<FieldsUDF> ListFieldsUDF { get; set; } = new List<FieldsUDF>();
        public List<FieldsUDF> ListFieldsUDFSolutions { get; set; } = new List<FieldsUDF>();
        public string DateLog { get; set; }
        public bool State { get; set; }
    }
}