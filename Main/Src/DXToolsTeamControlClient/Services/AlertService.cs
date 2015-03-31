using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXTools.Azure.TeamControl.Client.Services
{
    public class TrayAlertService : IAlertService
    {
        private NotifyIcon notifyIcon;
        private string title;

        public TrayAlertService(NotifyIcon notifyIcon, string title)
        {
            this.notifyIcon = notifyIcon;
            this.title = title;
        }

        public void Error(string message)
        {
            notifyIcon.ShowBalloonTip(0, title, message, ToolTipIcon.Error);
        }

        public void Info(string message)
        {
            notifyIcon.ShowBalloonTip(0, title, message, ToolTipIcon.Info);
        }

        public void Warning(string message)
        {
            notifyIcon.ShowBalloonTip(0, title, message, ToolTipIcon.Warning);
        }

        public void ClientEvents(string message)
        {
            notifyIcon.ShowBalloonTip(0, title, message, ToolTipIcon.None);
        }

    }
}
