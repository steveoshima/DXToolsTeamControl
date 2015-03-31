using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.AspNet.SignalR.Client;
using System.Security.Principal;
using DXTools.Azure.TeamControl.Client.Services;

namespace DXTools.Azure.TeamControl.Client
{
    static class Program
    {
        private static ApplicationContext applicationContext = null;
        private static NotifyIcon trayIcon = null;
        private static ContextMenu menu = null;
        private static string executingPath = Path.GetDirectoryName(Application.ExecutablePath).Replace("bin\\Debug", string.Empty);
        private const string CONNECTIONSTRING_NAME = "XrmConnection";
        private const string MOBILE_SERVICE_KEY = "MobileServiceKey";
        private const string MOBILE_SERVICE_URI = "MobileServiceUri";
        private static HubClients.TeamControlHubClient teamControlHub;
        private static StringBuilder sbLog = new StringBuilder();

        /// <summary>
        /// The main entry point for the application.
        /// A quick tray signalr client application. Notifications get set to the tray ballon tip
        /// TDOO: Add a queue for publish requests.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            applicationContext = new ApplicationContext();            
            InitializeNotifyArea();
            InitializeSignalR();
            Application.Run(applicationContext);            
        }

        private static void InitializeNotifyArea()
        {
            menu = new ContextMenu(new MenuItem[] {                
                new MenuItem("Warn of imminent publish", OnClickWarn), 
                new MenuItem("Log", OnClickHistory),
                new MenuItem("Disconnect", OnClickDisconnect),
                new MenuItem("Reconnect", OnClickReconnect),
                new MenuItem("Quit", OnClickStop)
            });

            trayIcon = new NotifyIcon();
            trayIcon.Icon = new Icon("icon.ico");
            trayIcon.Visible = true;
            trayIcon.ContextMenu = menu;
            trayIcon.MouseDown += OnTrayIconMouseDown;
        }

        private static void InitializeSignalR()
        {
            var configService = new ConfigService();
            string mobileServiceKey = configService.GetConfigValue(MOBILE_SERVICE_KEY, Application.ExecutablePath);
            string mobileServiceUri = configService.GetConfigValue(MOBILE_SERVICE_URI, Application.ExecutablePath);

            teamControlHub = new HubClients.TeamControlHubClient
            {
                TeamControlHubConnectionUrl = mobileServiceUri,
                TeamControlHubConnectionKey = mobileServiceKey,
                TeamControlClientUser = GetUser(),
                TeamControlLogService = new StringBuilderLogService(sbLog),
                TeamControlAlertService = new TrayAlertService(trayIcon, "Team Control"),

            };
            teamControlHub.StartHub();
        }

        private static void Reconnect()
        {
            teamControlHub.StartHub();
        }

        private static void Disconnect()
        {
            teamControlHub.CloseHub();
        }

        private static void ShowHistoryForm()
        {
            var historyForm = new HistoryForm();
            historyForm.Show();
            historyForm.LoadLog(sbLog.ToString());      
        }

        private static void Warn()
        {
            Task.Factory.StartNew(() =>
            {
                teamControlHub.AddAlert(GetUser() + ": is warning about a imminent publish");

            }).ContinueWith(task =>
            {
                //finally
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void RemotePublish()
        {
            var configService = new ConfigService();
            string crmOrganisation = configService.GetConnectionConfigValue(CONNECTIONSTRING_NAME, Application.ExecutablePath);
            var crmService = new CrmService(crmOrganisation);

            Task.Factory.StartNew(() =>
            {
                teamControlHub.AddAlert(GetUser() + ": is warning about a imminent publish");

            }).ContinueWith(task =>
            {
                //finally
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void ShowSettingsForm()
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show();
        }

        private static string GetUser() 
        {
            var currentUser = WindowsIdentity.GetCurrent().Name;
            string[] currentUserSplit = currentUser.Split('\\');
            var currentUserAsEmail = currentUserSplit[1] + "@" + currentUserSplit[0];
            return currentUserAsEmail;
        }

        #region ui events

        private static void OnClickWarn(object sender, EventArgs e)
        {
            Warn();
        }

        private static void OnClickHistory(object sender, EventArgs e)
        {
            ShowHistoryForm();
        }

        private static void OnClickSettings(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private static void OnClickRemotePublish(object sender, EventArgs e)
        {
            RemotePublish();
        }

        private static void OnClickReconnect(object sender, EventArgs e)
        {
            Reconnect();
        }

        private static void OnClickDisconnect(object sender, EventArgs e)
        {
            Disconnect();
        }

        private static void OnClickStop(object sender, EventArgs e)
        {
            // kill the message pump and the application thread
            teamControlHub.CloseHub();
            applicationContext.ExitThread();
        }

        private static void OnTrayIconMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowHistoryForm();
            }
        }

        #endregion
    }
}
