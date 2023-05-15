using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class WorkOrder
    {
        public long IdWorkOrder { get; set; }
        public long IdWorkOrderReference { get; set; }
        public string Semaphore { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateSAP { get; set; }
        public string DateModification { get; set; }
        public string DateLog { get; set; }
        public List<WorkOrder_Attachments> ListAttachments { get; set; } = new List<WorkOrder_Attachments>();
        public WorkOrder_DataImported WorkOrderDataImported { get; set; } = new WorkOrder_DataImported();
        public WorkOrder_Solution WorkOrderSolution { get; set; } = new WorkOrder_Solution();
        public Users UsersCreate { get; set; } = new Users();
        public Users UsersAssigned { get; set; } = new Users();
        public Users UsersScaled { get; set; } = new Users();
        public Templates Template { get; set; } = new Templates();
        public StatusDefinition Status { get; set; } = new StatusDefinition();
        public StatusDefinition SubStatus { get; set; } = new StatusDefinition();
        public Categories Category { get; set; } = new Categories();
        public Categories SubCategory { get; set; } = new Categories();
        public Groups GrupoAsignado { get; set; } = new Groups();
        public AlgorithmsAssignment Algorithm { get; set; } = new AlgorithmsAssignment();
        public List<FieldsUDF> ListFieldsUDF { get; set; } = new List<FieldsUDF>();
        public List<FieldsUDF> ListFielsUDFSolution { get; set; } = new List<FieldsUDF>();
        /*QUEMON PARA PQR BOG*/
        public long PQR { get; set; }
        public long Cuenta { get; set; }
        public string X_COORDINATE { get; set; }
        public string NUMERO { get; set; }
        public int DiasPQR { get; set; }

        /*CAMPOS PQR AIRE*/
        public string FechaCorreo { get; set; }
        public string FechaCorreoVIP { get; set; }

        /* negacion de linea 08.2021    */

        public List<NegacionLinea> NegacionLinea { get; set; } = new List<NegacionLinea>();
    }
}