using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class GestionBOE
    {
        public int IdGestionBOE { get; set; }
        public string NoSR { get; set; }
        public int Segmento { get; set; }
        public int Tipologia { get; set; }
        public int Estado { get; set; }
        public int SeguimientoPendiente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEnvioACampo { get; set; }
        public DateTime FechaProximaAccion { get; set; }
        public DateTime FechaBackOffice { get; set; }
        public DateTime FechaActualizacionEstado { get; set; }
        public int DetalleGestion { get; set; }
        public int Canal { get; set; }
        public string Observaciones { get; set; }
        public DateTime RecordCreationDate { get; set; }
        public string RecordCreationUser { get; set; }
    }
}