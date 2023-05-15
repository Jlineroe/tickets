using AIBTicketsMVC.App_Code;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class ReclamoDatacredito
    {
        public int Id { get; set; }
        public string Numero_ID { get; set; }
        public string Tipo_Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Entidad { get; set; }
        public string NIT { get; set; }
        public string No_Cuenta { get; set; }
        public string No_Reclamo { get; set; }
        public string Reclamo_Entidad { get; set; }
        public string Estado { get; set; }
        public string Tipo_Reclamo { get; set; }
        public string Subtipo_Reclamo { get; set; }
        public string Leyenda_Reclamo { get; set; }
        public string Tipo_Solucion { get; set; }
        public string Fecha_Colocacion { get; set; }
        public string Fecha_Aplicacion { get; set; }
        public string Canal { get; set; }
        public string Origen { get; set; }
        public string Empresa_Origen { get; set; }
        public string Estado_Robot { get; set; }
        public string EstadoProceso { get; set; }
        public string Observacion { get; set; }
        public DateTime Fecha_Procesamiento { get; set; }
        public string HostName { get; set; }
        public string WinUser { get; set; }
        public string Proceso { get; set; }
        public string Estad { get; set; }
        public string SubEstad { get; set; }


    }
}