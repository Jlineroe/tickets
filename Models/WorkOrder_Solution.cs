using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.Models
{
    public class WorkOrder_Solution
    {
        public long IdWorkOrderSolutions { get; set; }
        public WorkOrder_Escalations WorkOrderEscalations { get; set; } = new WorkOrder_Escalations();
        public long IdWorkOrder { get; set; }
        public StatusDefinition Status { get; set; } = new StatusDefinition();
        public StatusDefinition SubStatus { get; set; } = new StatusDefinition();
        public string Resolutions { get; set; }
        public string DateLog { get; set; }
        public List<FieldsUDF> ListFielsUDFSolution { get; set; } = new List<FieldsUDF>();
        public BPB_Servicio BPBServicio { get; set; } = new BPB_Servicio();

        public Cta_Contable CtaContable { get; set; } = new Cta_Contable();

        //public ServicioDefinition Servicio { get; set; } = new ServicioDefinition();

    }
}