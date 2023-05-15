using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class PaginModel
    {
        public int PaginaActual { get; set; }
        public int TotalRegis { get; set; }
        public int RegisXPagina { get; set; }
        public string OthesFiltres { get; set; } = "";
        public List<int> TopDefault { get; set; } = new List<int>{ 10, 25, 50, 100, 500 };
    }
}