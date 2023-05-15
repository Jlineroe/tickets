using AIBTicketsMVC.Models;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace AIBTicketsMVC.App_Code
{
    public class Tools
    {
        public static string MsjError(string Ruta, Exception ex, object Obj = null)
        {
            string stringObjetos = "";
            if (Obj != null) stringObjetos = "<b>Objetos:</b> " + JsonConvert.SerializeObject(Obj) + "</br>";
            return $@"<br/><b>Ruta:</b>{Ruta}({GetLineErr(ex)}):<br/> 
            {stringObjetos}<br/><b>Detalle de error:</b> </br>{ex.Message}";
        }
        public static void SessionSetObject(string key, object value)
        {
            HttpContext.Current.Session[key] = JsonConvert.SerializeObject(value);
        }
        public async static Task<T> SessionGetObject<T>(string key)
        {
            var value = (string)HttpContext.Current.Session[key];
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        public static void SessionSetObjectPQR(string key, object value)
        {
            HttpContext.Current.Session[key] = JsonConvert.SerializeObject(value);
        }
        public async static Task<T> SessionGetObjectPQR<T>(string key)
        {
            var value = (string)HttpContext.Current.Session[key];
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        public static XLWorkbook ConvertDataTableXExcel(XLWorkbook DocExcel,DataTable dt,string NameHoja)
        {
            List<string> Columnas = dt.Columns.Cast<DataColumn>().Select(lq => lq.ColumnName.ToString()).ToList();
            var HojaExcel = DocExcel.AddWorksheet(NameHoja);
            //codigo ASCII
            int i = 65;
            string LExcel = "", PL = "";
            foreach (string Column in Columnas)
            {
                if (i > 90) {
                    PL += "A";
                    i = 65;
                }
                LExcel = PL + ((char)i).ToString();
                HojaExcel.Column(LExcel).Width = 13;
                HojaExcel.Cell($"{LExcel}1").Value = Column;
                HojaExcel.Cell($"{LExcel}1").Style.Font.FontColor = XLColor.White;
                HojaExcel.Cell($"{LExcel}1").Style.Font.Bold = true;
                HojaExcel.Cell($"{LExcel}1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                HojaExcel.Cell($"{LExcel}1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                HojaExcel.Cell($"{LExcel}1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 79, 129, 189);
                int NCell = 2;
                foreach (DataRow dr in dt.Rows)
                {
                    HojaExcel.Cell($"{LExcel + NCell}").Value = dr[Column].ToString();
                    HojaExcel.Cell($"{LExcel + NCell}").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    HojaExcel.Cell($"{LExcel + NCell}").Style.Border.TopBorderColor = XLColor.FromArgb(0, 79, 129, 189);
                    NCell++;
                }
                i++;
            }
            return DocExcel;
        }
        public static int GetLineErr(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":línea ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber)) { }
            }
            return lineNumber;
        }
        public async static Task LogAplications(string Operacion, string opcion)
        {
            try
            {
                string localIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                WebClient wcLog = new WebClient();
                string winuser = "Error", llave = "Error";
                try
                {
                    ConstProject po = new ConstProject();
                    llave = ConstProject.llave;
                    winuser = po.WinuserActual;
                }
                catch (Exception)
                {
                }
                NameValueCollection myQueryStringCollection = new NameValueCollection()
                {
                    {"op", Operacion},
                    {"wu", winuser},
                    {"ip", localIP},
                    {"lla", llave},
                    {"opc", opcion.Trim()}
                };
                wcLog.QueryString = myQueryStringCollection;
                wcLog.DownloadString("http://aiblogaplicaciones.aib.com.co");
                wcLog.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public async static Task SendMailException(string msjContenido)
        {
            try
            {
                MailMessage mail = new MailMessage
                { From = new MailAddress(MasterMail.SmtpMail, "AQI Notifications No reply") };
                DataTable dt = await DAOCommand.EmailUsersException();
                foreach (DataRow dr in dt.Rows)
                {
                    string Email = ((string.IsNullOrEmpty(dr["EmailCorporativo"].ToString())) ? "" : dr["EmailCorporativo"].ToString());
                    if (Email != "") mail.To.Add(Email);
                }
                mail.Subject = $"EXCEPTION IN {ConstProject.NameProject}";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = msjContenido + "<br/><br/>";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient
                {
                    EnableSsl = true,
                    Port = int.Parse(MasterMail.SmtpPort),
                    Host = MasterMail.SmtpHost,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(MasterMail.SmtpMail, MasterMail.SmtpPwd)
                };
                smtp.Send(mail);
                mail.Dispose();
                smtp.Dispose();
            }
            catch (Exception ex)
            {
                LogAplications("ERROR MAIL", $"Error al enviar Mail de error, {ex.Message}");
            }
        }
        public static void SendMail(string asunto, string msjContenido, string Destinatarios)
        {
            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(MasterMail.SmtpMail, "AQI Notifications No reply")
                };

                foreach (string dest in Destinatarios.Split(new string[] { ";" }, StringSplitOptions.None))
                    mail.To.Add(dest);

                mail.Subject = asunto;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = $"{msjContenido}</br></br>";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient
                {
                    EnableSsl = true,
                    Port = int.Parse(MasterMail.SmtpPort),
                    Host = MasterMail.SmtpHost,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(MasterMail.SmtpMail, MasterMail.SmtpPwd)
                };
                smtp.Send(mail);
                smtp.Dispose();
            }
            catch (Exception)
            {
                //throw new Exception("CommandSQL.SendMail: " + ex.Message);
            }
        }

        public static XLWorkbook ConvertDataTableInExcel(XLWorkbook DocExcel, DataTable dt, string NameHoja)
        {
            List<string> Columnas = dt.Columns.Cast<DataColumn>().Select(lq => lq.ColumnName.ToString()).ToList();
            var HojaExcel = DocExcel.AddWorksheet(NameHoja);
            //codigo ASCII
            int i = 65;
            string LExcel = "", PL = "";
            foreach (string Column in Columnas)
            {
                if (i > 90)
                {
                    PL += "A";
                    i = 65;
                }
                LExcel = PL + ((char)i).ToString();
                HojaExcel.Column(LExcel).Width = 13;
                HojaExcel.Cell(LExcel + "1").Value = Column;
                HojaExcel.Cell(LExcel + "1").Style.Font.FontColor = XLColor.White;
                HojaExcel.Cell(LExcel + "1").Style.Font.Bold = true;
                HojaExcel.Cell(LExcel + "1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                HojaExcel.Cell(LExcel + "1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                HojaExcel.Cell(LExcel + "1").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 79, 129, 189);
                int NCell = 2;
                foreach (DataRow dr in dt.Rows)
                {
                    HojaExcel.Cell(LExcel + NCell).Value = dr[Column].ToString();
                    //HojaExcel.Cell(LExcel + NCell).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    //HojaExcel.Cell(LExcel + NCell).Style.Border.TopBorderColor = XLColor.FromArgb(0, 79, 129, 189);
                    HojaExcel.Cell(LExcel + NCell).Style.Alignment.WrapText = false;
                    NCell++;
                }
                i++;
            }
            return DocExcel;
        }

    }
}
