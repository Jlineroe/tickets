using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using AIBTicketsMVC.Models;

namespace AIBTicketsMVC.Hubs
{
    public class ChatHub:Hub
    {
        public async Task Send(Notifications Notifi)
        {
            Notifi.DateLog = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            await Clients.All.SendChat(Notifi);
        }
        public async Task SendNotification(string user, string message)
        {
            await Clients.All.SendChat(user, message);
        }
    }
}