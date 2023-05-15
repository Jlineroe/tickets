using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class AttachmentsGeneral
    {

        public int IdParent { get; set; }
        public int IdAttachment { get; set; }
        public string NameAttachment { get; set; }
        public long NameEncryptedAttachment { get; set; } = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssff"));
        public string Extension { get; set; }
        public decimal FileSizeKB { get; set; }
        public string FileSizeKBText { get; set; }
        public bool Active { get; set; }
        public DateTime Record_Creation_Date { get; set; }
        //public Employees EmployeeAttached { get; set; } = new Employees();
        public string FilePath { get; set; }
        public string FilePathDownload { get; set; }
        public string ConexString { get; set; }
        public List<string> ListExcelSheets { get; set; } = new List<string>();
        public string SheetSelected { get; set; }
        public string MsjError { get; set; }

    }
}