using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class ImportWorkOrder
    {
        public List<Users> ListUsers { get; set; } = new List<Users>();
        public Groups Grupo { get; set; } = new Groups();
        public AlgorithmsAssignment Algorithms { get; set; } = new AlgorithmsAssignment();
        public List<string> ColumnsSQL { get; set; }
        public List<string> ColumnsExcel { get; set; }
        public FieldsUDF FieldUDF { get; set; } = new FieldsUDF();
        public Templates Plantilla { get; set; } = new Templates();
        public List<long> ListIdWorkOrder { get; set; } = new List<long>();
    }
}