using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class HistoryCompliance
    {
        public string Semana { get; set; }
        public int Cantidad { get; set; }
        public int Meta { get; set; }
        public int Diferencia { get; set; }
        public int Cumplimiento { get; set; }
    }
}