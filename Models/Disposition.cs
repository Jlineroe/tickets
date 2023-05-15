using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Disposition
    {
        public string ID { get; set; }
        public string Nombre { get; set; }
        
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool state { get; set; }
        public bool Estado { get; set; }
        public bool Subestado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime DateLog { get; set; }
        public string Controller { get; set; }
        public string NombreSelect { get; set; }
    }
}