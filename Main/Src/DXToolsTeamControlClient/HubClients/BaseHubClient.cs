using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.WindowsAzure.MobileServices;
using DXTools.Azure.TeamControl.Client.Services;


namespace DXTools.Azure.TeamControl.Client
{
    public abstract class BaseHubClient
    {
        protected HubConnection hubConnection;
        protected IHubProxy hubProxy;

        public string HubConnectionUrl { get; set; }
        public string HubConnectionKey { get; set; }
        public MobileServiceUser HubConnectionUser { get; set; }
        public string HubProxyName { get; set; }
        public TraceLevels HubTraceLevel { get; set; }
        public System.IO.TextWriter HubTraceWriter { get; set; }
        public IAlertService HubAlertService { get; set; }
        public ILogService HubLogService { get; set; }
        public bool ForceClose { get; set; }
        public string HubClientUser { get; set; }

        public ConnectionState State
        {
            get { return hubConnection.State; }
        }

        protected void Init()
        {
            hubConnection = new HubConnection(HubConnectionUrl)
            {
                TraceLevel = HubTraceLevel,
                TraceWriter = HubTraceWriter
            };

            if (HubConnectionUser != null)
            {
                hubConnection.Headers["x-zumo-auth"] = HubConnectionUser.MobileServiceAuthenticationToken;
            }
            else
            {
                hubConnection.Headers["x-zumo-application"] = HubConnectionKey;
            }

            ForceClose = false;

            //IWebProxy systemProxy = System.Net.WebRequest.GetSystemWebProxy();
            //hubConnection.Proxy = systemProxy;

            hubProxy = hubConnection.CreateHubProxy(HubProxyName);

            hubConnection.Received += _hubConnection_Received;
            hubConnection.Reconnected += _hubConnection_Reconnected;
            hubConnection.Reconnecting += _hubConnection_Reconnecting;
            hubConnection.StateChanged += _hubConnection_StateChanged;
            hubConnection.Error += _hubConnection_Error;
            hubConnection.ConnectionSlow += _hubConnection_ConnectionSlow;
            hubConnection.Closed += _hubConnection_Closed;
        }

        public void CloseHub()
        {
            hubConnection.Stop(new Exception("Manual"));
            hubConnection.Dispose();
        }

        protected void StartHubInternal()
        {
            bool connection = false;
            if (hubConnection.State == ConnectionState.Disconnected)
            {
                hubConnection.Start().ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            connection = false;
                            HubLogService.Error("Error Opening Faulted Connection");
                        }
                        else if (task.IsCanceled)
                        {
                            connection = false;
                            HubLogService.Error("Error Opening Cancelled Connection");
                        }
                        else if (task.IsCompleted)
                        {
                            connection = true;
                            HubLogService.ClientEvents(HubClientUser + " Connected");
                            HubAlertService.ClientEvents(HubClientUser + " Connected");
                        }
                    }).Wait(10000);

                if (!connection)
                {
                    HubLogService.ClientEvents("Connecting...");
                    StartHubInternal();
                }
            }
        }

        public abstract void StartHub();

        void _hubConnection_Closed()
        {
            HubLogService.ClientEvents("Closed - State:" + hubConnection.State);
            HubAlertService.ClientEvents("Connection Closed");

            if (!ForceClose)
            {
                StartHubInternal();
            }
        }

        void _hubConnection_ConnectionSlow()
        {
            HubLogService.ClientEvents("ConnectionSlow - State:" + hubConnection.State);
            HubAlertService.ClientEvents("Connection Slow");
        }

        void _hubConnection_Error(Exception obj)
        {
            HubLogService.ClientEvents("Error State:" + hubConnection.State);

            if (obj.Message == "Manual")
            {
                ForceClose = true;
            }
        }

        void _hubConnection_StateChanged(StateChange obj)
        {
            HubLogService.ClientEvents("StateChanged - State:" + hubConnection.State);
        }

        void _hubConnection_Reconnecting()
        {
            HubLogService.ClientEvents("Reconnecting - State:" + hubConnection.State);
        }

        void _hubConnection_Reconnected()
        {
            HubLogService.ClientEvents("Reconnected - State:" + hubConnection.State);
            HubAlertService.ClientEvents("Reconnected");
        }

        void _hubConnection_Received(string obj)
        {
            HubLogService.ClientEvents("Received - State:" + hubConnection.State);
        }
    }
}
