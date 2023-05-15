using System;

namespace AIBTicketsMVC.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Winuser = Environment.UserName.ToUpper().Replace("AIB/", "");
        public string TituloError { get; set; }
        public string DetalleError { get; set; }
    }
}