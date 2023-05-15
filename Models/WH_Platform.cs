using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class WH_Platform
    {
        public int IdPlatform { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public string Record_Creation_Date { get; set; }
        public string Record_Update_Date { get; set; }
        public bool? Active { get; set; }
    }
}