using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using Microsoft.Xrm.Client;
using DXTools.Azure.TeamControl.Client.Services;

namespace DXTools.Azure.TeamControl.Client
{
    public partial class SettingsForm : Form
    {
        private const string CONNECTIONSTRING_NAME = "XrmConnection";
        private ConfigService configService;

        public SettingsForm()
        {
            InitializeComponent();

            configService = new ConfigService();

            //load defaults
            textBoxServerName.Text = configService.GetConnectionConfigValue(CONNECTIONSTRING_NAME, Application.ExecutablePath);

        }
   
        #region button event handlers

        private void buttonUpdateSettings_Click(object sender, EventArgs e)
        {
            string connectionTemplate = "Url=https://{0}.crm4.dynamics.com/XRMServices/2011/Organization.svc; Username={1}; Password={2}";
            string connectionString = connectionTemplate.FormatWith(textBoxServerName.Text, textBoxLogin.Text, textBoxPassword.Text);

            configService.UpdateConnectionConfig(CONNECTIONSTRING_NAME, connectionString, Application.ExecutablePath);

            this.Close();
        }
        
        #endregion
        
    }
}
