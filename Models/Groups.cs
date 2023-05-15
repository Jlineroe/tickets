
using System.Collections.Generic;

namespace AIBTicketsMVC.Models
{
    public class Groups
    {
        public int IdMasterGroups { get; set; }
        public LineaNegocio LOB { get; set; } = new LineaNegocio();
        public string NameGroup { get; set; }
        public string DescriptionGroup { get; set; }
        public bool ReturnUser { get; set; }
        public string DateLog { get; set; }
        public bool State { get; set; }
        public Sites Sitio { get; set; } = new Sites();
        public List<Groups> GruposAEscalar { get; set; } = new List<Groups>();
        public List<Users> UsersXGroups { get; set; } = new List<Users>();
        public List<string> TypesScaled { get; set; }
    }
}