using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Client.Configuration;
using Microsoft.Crm.Sdk.Messages;


namespace DXTools.Azure.TeamControl.Client.Services
{
    public class CrmService
    {
        private OrganizationService crmOrganisationService;

        public CrmService()
        {
            CrmConnection crmConnection = new CrmConnection("XrmConnection");
            crmOrganisationService = new OrganizationService(crmConnection);
        }

        public CrmService(string xrmConnectionKey)
        {
            CrmConnection crmConnection = new CrmConnection(xrmConnectionKey);
            crmOrganisationService = new OrganizationService(crmConnection);
        }

        public void PublishAll()
        {
            PublishXmlRequest publishRequest = new PublishXmlRequest();
            publishRequest.ParameterXml = @"<importexportxml>                                                
                                                <entities>                                                   
                                                </entities>                                                
                                                <nodes/>                                                
                                                <securityroles/>                                                
                                                <settings/>                                                
                                                <workflows/>                                            
                                            </importexportxml>";
            try
            {
                PublishXmlResponse publishResponse = (PublishXmlResponse)crmOrganisationService.Execute(publishRequest);
            }
            catch (Exception e)
            {
                throw new Exception("Publish Failure: " + e.Message);
            }
        }

        public void PublishEntities(IList<string> entities)
        {
            string entitiesStr = "";
            string entitiesTemplate = "<entities>{0}</entities>";
            foreach (var entity in entities)
            {
                entitiesStr = entitiesTemplate.FormatWith(entity);
            }            

            PublishXmlRequest publishRequest = new PublishXmlRequest();
            publishRequest.ParameterXml = @"<importexportxml>                                                
                                                <entities>
                                                    {0}                                                  
                                                </entities>                                                
                                                <nodes/>                                                
                                                <securityroles/>                                                
                                                <settings/>                                                
                                                <workflows/>                                            
                                            </importexportxml>".FormatWith(entitiesStr);
            try
            {
                PublishXmlResponse publishResponse = (PublishXmlResponse)crmOrganisationService.Execute(publishRequest);
            }
            catch (Exception e)
            {
                throw new Exception("Publish Failure: " + e.Message);
            }
        }

        public void PublishWebResources(IList<string> webresourceIds)
        {
            string webresourceIdStr = "";
            string webresourceIdTemplate = "<webresource>{{0}}</webresource>";
            foreach (var webresourceId in webresourceIds)
            {
                webresourceIdStr = webresourceIdTemplate.FormatWith(webresourceId);
            }

            PublishXmlRequest publishRequest = new PublishXmlRequest();
            publishRequest.ParameterXml = @"<importexportxml>                                                
                                                <webresources>
                                                    {0}                                                  
                                                </webresources>                           
                                            </importexportxml>".FormatWith(webresourceIdStr);
            try
            {          
                PublishXmlResponse publishResponse = (PublishXmlResponse)crmOrganisationService.Execute(publishRequest);
            }
            catch(Exception e)
            {
                throw new Exception("Publish Failure: "+ e.Message);
            }
        }




    }
}
