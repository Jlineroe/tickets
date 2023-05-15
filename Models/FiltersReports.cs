using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class FiltersReports
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string[] ArrayStatus { get; set; }
        public string[] ArrayGroups { get; set; }
        public int IdTemplates { get; set; }
    }
}