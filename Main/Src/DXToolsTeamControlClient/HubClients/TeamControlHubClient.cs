using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.WindowsAzure.MobileServices;
using DXTools.Azure.TeamControl.Client.Services;

namespace DXTools.Azure.TeamControl.Client.HubClients
{
    public class TeamControlHubClient : BaseHubClient
    {
        public string TeamControlHubConnectionUrl { get; set; }
        public string TeamControlHubConnectionKey { get; set; }
        public System.IO.TextWriter TeamControlHubTraceWriter { get; set; }
        public IAlertService TeamControlAlertService { get; set; }
        public ILogService TeamControlLogService { get; set; }
        public string TeamControlClientUser { get; set; }

        public TeamControlHubClient()
        {
        }

        public new void Init()
        {
            HubConnectionUrl = TeamControlHubConnectionUrl;
            HubConnectionKey = TeamControlHubConnectionKey;
            HubProxyName = "TeamControlHub";
            HubTraceLevel = TraceLevels.All;
            HubTraceWriter = Console.Out; // TeamControlHubTraceWriter;
            HubLogService = TeamControlLogService;
            HubAlertService = TeamControlAlertService;
            HubClientUser = TeamControlClientUser;

            base.Init();
            hubProxy.On<string>("alertThis", RecieveAlert);
            StartHubInternal();
        }

        public override void StartHub()
        {
            if (hubConnection != null)
            {
                hubConnection.Dispose();
            }

            Init();
        }

        public void RecieveAlert(string message)
        {
            TeamControlLogService.Info(message);
            TeamControlAlertService.Info(message);
        }

        public void AddAlert(string message)
        {
            if (hubConnection.State == ConnectionState.Connected)
            {
                hubProxy.Invoke("Alert", message).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        TeamControlLogService.Error("Error with connection:" + task.Exception.GetBaseException());
                    }
                }).Wait();

                TeamControlLogService.Info("Sending Alert " + message);
            }
        }

    }
}
