using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIBTicketsMVC.Models
{
    public class WorkOrder_DataImported
    {
        public int IdDataImported { get; set; }
        public Users UserImport { get; set; }
        public Users UserDesactivated { get; set; }
        public int NumRecords { get; set; }
        public string NameData { get; set; }
        public long NameDataEncrypted { get; set; }
        public string Extension { get; set; }
        public string DesactivationReason { get; set; }
        public string DateDesactivation { get; set; }
        public string DateLog { get; set; }
        public bool State { get; set; }
        public string ConexString { get; set; }
        public string returnError { get; set; }
        public string HojaSelected { get; set; }
        public string SQLTableTemp { get; set; }
        public List<string> NameHojas { get; set; }
        public List<string> Columnas { get; set; }
    }
}
