using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class FieldsTypesUDF
    {
        public int IdFieldsTypesUDF { get; set; }
        public string TypeDataSQL { get; set; }
        public string NameUDF { get; set; }
        public string NameTypeFieldsShort{ get; set; }
        public string NameTypeFields{ get; set; } 
        public string IconFields{ get; set; }
        public bool State { get; set; }
    }
}