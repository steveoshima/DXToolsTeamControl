using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXTools.Azure.TeamControl.Client.Services
{
    public interface IAlertService
    {
        void Error(string message);
        void Info(string message);        
        void Warning(string message);        
        void ClientEvents(string message);        
    }
}
