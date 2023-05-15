using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class FiltersWorkOrder
    {
        public int pag { get; set; } = 1;
        public int top { get; set; } = 10;
        public string OrderBy { get; set; }
        public string OrderType { get; set; }
        public List<long> ListIdWorkOrder { get; set; } = new List<long>();
        public string Title { get; set; }
        public string Description { get; set; }
        public int IdTemplate { get; set; }
        public List<int> IdStatus { get; set; } = new List<int>();
        public List<int> IdGroupsAssign { get; set; } = new List<int>();
        public List<int> IdUsersCrea { get; set; } = new List<int>();
        public List<int> IdUsersAssign { get; set; } = new List<int>();
        public List<int> IdUsersScaled { get; set; } = new List<int>();
        public long PQR { get; set; }
        public long Cuenta { get; set; } 
        public string X_COORDINATE{ get; set; } 
        public long Numero { get; set; }
    }
}