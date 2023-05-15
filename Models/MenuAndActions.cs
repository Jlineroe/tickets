using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class MenuAndActions
    {
        public int IdMasterMenu { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int? Parent_IdMenu { get; set; }
        public string Controller { get; set; }
        public string Icono { get; set; }
        public int Permiso { get; set; }
        public bool IdIsMenu { get; set; }
    }
}