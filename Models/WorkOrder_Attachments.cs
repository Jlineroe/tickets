using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class WorkOrder_Attachments
    {
        public long IdAttachment { get; set; }
        public long IdWorkOrder { get; set; }
        public long IdWorkOrderSolutions { get; set; }
        public string NameAttachment { get; set; }
        public long NameEncryptedAttachment { get; set; }
        public string Extension { get; set; }
        public decimal FileSizeKB { get; set; }
        public string FilePath { get; set; }
        public string FilePathDownload { get; set; }
        public string DateLog { get; set; }
        public Users UsersGestiona { get; set; } = new Users();
        public string msjError { get; set; }
    }
}