using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class TimeList
    {
        public string NombreAgente { get; set; }
        public long IdWorkOrder { get; set; }
        public string Grupo { get; set; }
        public string FechaApertura { get; set; }
        public string FechaDia { get; set; }
        public string HoraApertura { get; set; }
        public string FechaCierre { get; set; }
        public string HoraCierre { get; set; }
        public int DuracionGestion { get; set; }
        
        public int TiempoReal { get; set; }
        public string Estado { get; set; }
    }
}