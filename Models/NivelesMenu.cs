using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class NivelesMenu
    {
        public List<MenuAndActions> MenuLvl1 { get; set; } = new List<MenuAndActions>();
        public List<MenuAndActions> MenuLvl2 { get; set; } = new List<MenuAndActions>();
        public List<MenuAndActions> MenuLvl3 { get; set; } = new List<MenuAndActions>();
        public List<MenuAndActions> Actions { get; set; } = new List<MenuAndActions>();
    }
}