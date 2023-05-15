using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Cta_Contable
    {

        public string ID { get; set; }
        public string Nombre { get; set; }

        ///
        public string cod_cuenta { get; set; }
        public string servicio { get; set; }
        public string iva { get; set; }
        public string servicio_cuenta { get { return string.Format("{0} - {1}", this.servicio, this.cod_cuenta); } }
    }
}