using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIBTicketsMVC.Models
{
    public static class FormatsExcel
    {
        public static string XLSX = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1'";
        public static string XLS = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties='Excel 8.0;HDR=Yes;IMEX=2'";
    }
}
