using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class WH_Booth
    {
        public int IdBooth { get; set; }
        public string Location { get; set; }
        public string Platform { get; set; }
        public string Island { get; set; }
        public string BoothName { get; set; }
        public int BoothNumber { get; set; }
        public string ServiceLine { get; set; }
        public bool WAH { get; set; }
        public bool SwSite { get; set; }
        public string IP { get; set; }
        public string Extension { get; set; }
    }
}