using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Goals
    {
        public int IdMeta { get; set; }
        public DateTime FechaActual { get; set; }
        public int Meta { get; set; }
        public int IdMasterGroups { get; set; }
        public int Cerrados { get; set; }
        public int Cumplimiento { get; set; }
    }
}