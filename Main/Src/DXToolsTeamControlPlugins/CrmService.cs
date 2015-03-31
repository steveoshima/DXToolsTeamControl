using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DXTools.CRM.Solutions.TeamControlPlugins
{
    public class CrmService
    {
        private IOrganizationService orgService;

        public CrmService(IOrganizationService orgService)
        {
            this.orgService = orgService;
        }

        public string GetUserName(Guid userGuid)
        {
            string userName = userGuid.ToString();

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>
	                               <entity name='systemuser'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
	                                  <condition attribute='systemuserid' operator='eq' value='{0}' />
	                                </filter>
                                  </entity>
	                            </fetch>";
            fetchXml = string.Format(fetchXml, userGuid);

            EntityCollection results = orgService.RetrieveMultiple(new FetchExpression(fetchXml));

            if (results.Entities.Count > 0)
            {
                userName = results.Entities[0]["fullname"].ToString();
            }

            return userName;
        }

        public IDictionary<string, string> GetMobileServiceConfiguration()
        {
            var teamConfigurationFields = new Dictionary<string, string>();

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>
	                               <entity name='dxtools_teamcontrolconfiguration'>
                                    <attribute name='dxtools_url' />
                                    <attribute name='dxtools_securitykey' />
                                    <filter type='and'>
	                                  <condition attribute='dxtools_name' operator='eq' value='{0}' />
	                                </filter>
                                  </entity>
	                            </fetch>";
            fetchXml = string.Format(fetchXml, "Azure Mobile Service");

            EntityCollection results = orgService.RetrieveMultiple(new FetchExpression(fetchXml));

            if (results.Entities.Count > 0)
            {
                teamConfigurationFields.Add("mobileserviceurl", results.Entities[0]["dxtools_url"].ToString());
                teamConfigurationFields.Add("mobileservicekey", results.Entities[0]["dxtools_securitykey"].ToString());
            }

            return teamConfigurationFields;
        }
    }
}
