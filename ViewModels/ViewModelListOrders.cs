using AIBTicketsMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIBTicketsMVC.ViewModels
{
    public class ViewModelListOrders
    {
        public List<WorkOrder> ListWorkOrder { get; set; } = new List<WorkOrder>();
        public PaginModel Pagination { get; set; } = new PaginModel();
    }
}