using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace DXTools.Azure.TeamControl.MobileService.Hubs
{
    /// <summary>
    /// http://www.asp.net/signalr/overview/guide-to-the-api/hubs-api-guide-server
    /// </summary>
    public class TeamControlHub : Hub
    {
        public ApiServices Services { get; set; }

        public void Alert(string param)
        {
            Clients.All.alertThis(param);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {            
            return base.OnDisconnected(stopCalled);
        }
    }
}