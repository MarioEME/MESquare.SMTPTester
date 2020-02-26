using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace MESquare.SMTPTester
{
    public partial class frmMain : Form
    {
        private Core _core = new Core(
            Properties.Settings.Default.SMTPConfigsURL
            , Properties.Settings.Default.SMTPConfigsPath
        );

        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            FillCommonSTMPServers();
        }

        private void lstCommonSMTPServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSMTPConfigs();
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            TestSMTP();
        }

        private void TestSMTP()
        {
            try
            {
                txtResult.Text = "";

                var smtp = new SmtpClient
                {
                    Host = txtSMTPHost.Text
                };
                if (int.TryParse(txtPort.Text, out int port))
                    smtp.Port = port;

                smtp.UseDefaultCredentials = cbUseDefaultCredentials.Checked;
                smtp.EnableSsl = cbUseSecuredConnection.Checked;

                if (cbUseAuthentication.Checked)
                    smtp.Credentials = new NetworkCredential(txtLogin.Text, txtPassword.Text);

                using (var message = new MailMessage(txtFrom.Text, txtTo.Text))
                {
                    message.Subject = "SMTP Tester";
                    message.Body = "If you read this, the test was successful.";
                    message.IsBodyHtml = false;
                    smtp.Send(message);
                };

                txtResult.Text = $"Message sent without errors. Please check the email account {txtTo.Text}.";

            }
            catch (Exception ex)
            {
                txtResult.Text =
                    $@"Error sending the email: {ex.Message}
== Error Details ==
{ex.ToString()}
";

            }
        }

        private void FillCommonSTMPServers()
        {
            lstCommonSMTPServers.ValueMember = "Name";
            lstCommonSMTPServers.DisplayMember = "Name";
            lstCommonSMTPServers.DataSource = _core.CommonSMTPConfigs;

        }

        private void UpdateSMTPConfigs()
        {
            var configs = lstCommonSMTPServers.SelectedItem as SMTPConfig;
            if (configs == null)
                configs = new SMTPConfig();

            txtSMTPHost.Text = configs.SMTPHost;
            txtPort.Text = configs.Port.ToString();
            cbUseAuthentication.Checked = configs.UseAuthentication;
            cbUseSecuredConnection.Checked = configs.UseSecuredConnection;
            txtInformation.Text = String.IsNullOrWhiteSpace(configs.Information)
                ? "No information available for SMTP server" 
                : configs.Information;
        }


    }
}
