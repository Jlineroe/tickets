using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class HomeHistory
    {
        public List<HistoryState> ListHistoryState { get; set; } = new List<HistoryState>();
        public List<HistoryAsigne> ListHistoryAsigne { get; set; } = new List<HistoryAsigne>();
        public List<HistoryCompliance> ListHistoryCompliance { get; set; } = new List<HistoryCompliance>();
        public List<HistoryPercentage> ListHistoryPercentageANS { get; set; } = new List<HistoryPercentage>();
        public List<Goals> ListMeta { get; set; } = new List<Goals>();
        public List<Groups> ListGrupos { get; set; } = new List<Groups>();
        


    }
}