using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Mobile.Service;
using DXTools.Azure.TeamControl.MobileService.DataObjects;
using DXTools.Azure.TeamControl.MobileService.Models;
using DXTools.Azure.TeamControl.MobileService.Hubs;


namespace DXTools.Azure.TeamControl.MobileService.Controllers
{
    [RoutePrefix("signalr")]
    public class SignalRController : ApiController
    {
        public ApiServices Services { get; set; }

        public string Get(string description)
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<TeamControlHub>();
            IHubContext hubContext = Services.GetRealtime<TeamControlHub>();
            hubContext.Clients.All.alertThis(description);

            return description;
        }
    }
}
