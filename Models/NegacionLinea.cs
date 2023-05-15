using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class NegacionLinea
    {
        public long IdWorkOrder { get; set; }
        public string Base { get; set; }
        public string Imagen { get; set; }
        public string MIN { get; set; }
        public DateTime FechaActivacion { get; set; }
        public string Curcode { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Canal { get; set; }
        public string Ascard { get; set; }
        public DateTime FechaReposicion { get; set; }
        public string Contrato { get; set; }
        public string Grabacion { get; set; }
        public string Reasignacion { get; set; }
        public string Estado { get; set; }
        public string Legalizado { get; set; }
        public string Observaciones { get; set; }
        public string RangoProbable { get; set; }
        public string DireccionInformaCliente { get; set; }
        public string CustomerID { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public string UserLogUltimo { get; set; }
        public string UserLog { get; set; }
        public int Id { get; set; }
        public string EsExportado { get; set; }
        public DateTime FechaRadicacion { get; set; }
        public string Notificacion { get; set; }
        public string FechaDesactivacion { get; set; }
        public int PQR { get; set; }
        public int Cedula { get; set; }
        public string AreaRadica { get; set; }
        public string TipoReclamo { get; set; }
        


    }
}