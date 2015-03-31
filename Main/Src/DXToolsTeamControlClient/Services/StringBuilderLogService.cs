using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXTools.Azure.TeamControl.Client.Services
{
    public class StringBuilderLogService : ILogService
    {
        private StringBuilder sb;

        public StringBuilderLogService(StringBuilder sb)
        {
            this.sb = sb;
        }

        public void Error(string message)
        {
            sb.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": [Error] " + message);
        }

        public void Info(string message)
        {
            sb.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": [Info] " + message);
        }

        public void Warning(string message)
        {
            sb.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": [Warning] " + message);
        }

        public void ClientEvents(string message)
        {
            sb.AppendLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": " + message);
        }
    }
}
