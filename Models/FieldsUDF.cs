using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class FieldsUDF
    {
        public int IdFieldsUDF { get; set; }
        public Templates Template { get; set; } = new Templates();
        public FieldsTypesUDF FieldType { get; set; } = new FieldsTypesUDF();
        public TypesRequired TypeRequired { get; set; } = new TypesRequired();
        public string NameFieldsUDF { get; set; }
        public int Longitud { get; set; }
        public string NameField { get; set; }
        public List<FieldsDispositions> Dispositions { get; set; } = new List<FieldsDispositions>();
        public bool SolutionField { get; set; }
        public bool Universal { get; set; }
        public int Position { get; set; }
        public string DateLog { get; set; }
        public bool State { get; set; }
        public FieldsDispositions ParentDispositions { get; set; } = new FieldsDispositions();
        public string Value { get; set; } = "";
    }
}