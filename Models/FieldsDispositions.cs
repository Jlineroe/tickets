using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class FieldsDispositions
    {
        public long IdFieldsDispositions { get; set; }
        public int IdFieldsUDF { get; set; }
        public string Dispositions { get; set; }
        public long Parent_IdDispositions { get; set; }
        public string DateLog { get; set; }
    }
}