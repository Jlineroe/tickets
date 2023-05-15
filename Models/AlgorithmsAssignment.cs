using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class AlgorithmsAssignment
    {
        public int IdAlgorithmsAssignment { get; set; }
        public string NameAlgorithm { get; set; }
        public string DescriptionAlgorithm { get; set; }
        public string SQLStringAlgorithm { get; set; }
        public bool ImportData { get; set; }
        public DateTime DateLog { get; set; }
        public bool State { get; set; }
    }
}