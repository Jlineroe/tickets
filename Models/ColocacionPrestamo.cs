using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class ColocacionPrestamo
    {
        public int Id { get; set; }
        public string Identificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Especialidades { get; set; }
        public string CiudadResidencia { get; set; }
        public string TipoCredito { get; set; }
        public string Tipificacion1 { get; set; }
        public string Tipificacion2 { get; set; }
        public string Tipificacion3 { get; set; }
        public string Tipificacion4 { get; set; }
        public string NombreArchivo { get; set; }
        public string DataActiva { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaCarga { get; set; }
        public string FechaModificacion { get; set; }
        public string HostName { get; set; }
        public string winuser { get; set; }
        public string ingresos { get; set; }
        public string Valor_interesado { get; set; }
        public string Valor_cuota { get; set; }
        public string Plazo { get; set; }
        public List<Disposition> Disposition { get; set; } = new List<Disposition>();
        public string Descripcion { get; set; }
        public bool state { get; set; }
        public bool Estado { get; set; }
        public bool Subestado { get; set; }
        public DateTime DateLog { get; set; }
        public string FechaLLamada { get; set; }
        public string Controller { get; set; }
        public string NombreSelect { get; set; }
        public int pendientes { get; set; }
        
    }
}