using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class ConstProject
    {
        public static string NameProject = "TICKETS UNIFICADO";
        public static string llave = "5A9BC1AB-8233-496F-BC4A-4006C5B89687";
        public string WinuserActual = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4).ToUpper();
        public const string ConexStringELC = @"Data Source=SQLAIB;MultipleActiveResultSets=True;Initial Catalog=ATLANTIC;Persist Security Info=True;User ID=sa;Password=AtlItSys01;Connect Timeout=300";
        //public string WinuserActual = System.Web.HttpContext.Current.User.Identity.Name.ToString().Remove(0, 4).ToUpper();
        //public const string ConexStringELC = @"Data Source=SQLAIB;MultipleActiveResultSets=True;Initial Catalog=ATLANTIC;Persist Security Info=True;User ID=sa;Password=AtlItSys01;Connect Timeout=300";
        //public const string ConexStringTickets = @"Data Source=SQLAIB;MultipleActiveResultSets=True;Initial Catalog=TicketsUnificado;Persist Security Info=True;User ID=sa;Password=AtlItSys01;Connect Timeout=300";
        //public string WinuserActual = "enh9029a"; //duber
        //public string WinuserActual = "EKY7765A";
        //public string WinuserActual = "ECM1432B";
        //public string WinuserActual = "ECM6827B";
        //public string WinuserActual = "ECM5237A";
        //public string WinuserActual = "amrivera";

        //
        public const string LlaveLogApp = "DCFD1632-CF9A-4702-99FE-A8C0D2B63CB8";

        //Excels
        public const string Excel_XLS = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties='Excel 8.0;HDR=Yes;IMEX=2'";

        public const string Excel_XLSX = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1'";
    }
}
