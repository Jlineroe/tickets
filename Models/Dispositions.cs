using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class Dispositions
    {
        public int IdDisposition { get; set; }
        public int IdParent { get; set; }
        public string Descripcion { get; set; }

    }
}