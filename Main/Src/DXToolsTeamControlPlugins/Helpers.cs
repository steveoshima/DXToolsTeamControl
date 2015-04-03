using System;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace DXTools.CRM.Solutions.TeamControlPlugins
{
    public class Helpers
    {
        public string PrefixConst { get; set; }

        /// <summary>
        /// Currently just a GET request but should change to POST request
        /// Try async call for fire and forget
        /// </summary>
        /// <param name="url"></param>
        /// <param name="message"></param>
        /// <param name="applicationUser"></param>
        /// <param name="applicationPassword"></param>
        public void BroadcastMessageUrl(string url, string message, string applicationUser, string applicationPassword)
        {
            string authInfo = applicationUser + ":" + applicationPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            string requestUrl = url + message;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Timeout = 5000;
            request.Headers.Add("Authorization", "Basic " + authInfo);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET"; //could change to POST

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// Gets user from SharedVariables or fetch user entity. 
        /// </summary>
        /// <param name="crmService"></param>
        /// <param name="localContext"></param>
        /// <returns></returns>
        public string GetUser(CrmService crmService, Plugin.LocalPluginContext localContext, bool useParentContextSharedVars)
        {
            string userName;
            Guid userId = localContext.PluginExecutionContext.UserId;            
            string userNameKey = PrefixConst + "UserName_" + userId;
            ParameterCollection sharedVars = (useParentContextSharedVars) ? localContext.PluginExecutionContext.ParentContext.SharedVariables : localContext.PluginExecutionContext.SharedVariables;

            if (!sharedVars.ContainsKey(userNameKey))
            {
                userName = crmService.GetUserName(userId);
                localContext.PluginExecutionContext.SharedVariables.Add(userNameKey, userName);
            }
            else
            {
                userName = sharedVars[userNameKey].ToString();
            }

            return userName;
        }

        /// <summary>
        /// Check the shared variables before fetching crm entity. 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="crmService"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IDictionary<string, string> CheckForSharedVariableFromMobileServiceConfig(Plugin.LocalPluginContext localContext, CrmService crmService, IList<string> keys, bool useParentContextSharedVars)
        {
            var checkedFromPluginSharedVariablesFields = new Dictionary<string, string>();
            bool foundAllKeys = true;
            ParameterCollection sharedVars = (useParentContextSharedVars) ? localContext.PluginExecutionContext.ParentContext.SharedVariables : localContext.PluginExecutionContext.SharedVariables;

            foreach (var key in keys)
            {
                if (!sharedVars.ContainsKey(PrefixConst + key))
                {
                    foundAllKeys = false;
                }
                else
                {
                    checkedFromPluginSharedVariablesFields.Add(key, sharedVars[PrefixConst + key].ToString());
                }
            }

            if (foundAllKeys)
            {
                return checkedFromPluginSharedVariablesFields;
            }

            //else
            IDictionary<string, string> teamConfigurationFields = crmService.GetMobileServiceConfiguration();
            foreach (var key in keys)
            {
                checkedFromPluginSharedVariablesFields.Add(key, teamConfigurationFields[key]);
                localContext.PluginExecutionContext.SharedVariables.Add(PrefixConst + key, teamConfigurationFields[key]);
            }
            return checkedFromPluginSharedVariablesFields;
        }
    }
}
