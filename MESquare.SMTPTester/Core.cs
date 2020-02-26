using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MESquare.SMTPTester
{
    public class Core
    {
        public List<SMTPConfig> CommonSMTPConfigs { get; } = new List<SMTPConfig>();
        private String _smtpConfigsPath;
        private String _smtpConfigsUrl;

        public Core(String smtpConfigsUrl, String smtpConfigsPath)
        {

            _smtpConfigsUrl = smtpConfigsUrl;
            _smtpConfigsPath = smtpConfigsPath;

            LoadCommonSMTPConfigsFromUrl();
            LoadCommonSMTPConfigsFromFile();

        }

        private void LoadCommonSMTPConfigsFromUrl()
        {
            try
            {
                using (var webClient = new WebClient())
                    CommonSMTPConfigs.AddRange(
                        JSON.From<List<SMTPConfig>>(
                            webClient.DownloadString(_smtpConfigsUrl)));
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        private void LoadCommonSMTPConfigsFromFile()
        {
            try
            {
                if (File.Exists(_smtpConfigsPath))
                    CommonSMTPConfigs.AddRange(
                       JSON.From<List<SMTPConfig>>(
                           File.ReadAllText(_smtpConfigsPath)));
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        private void Log(String message)
        {
            File.AppendAllText(
                "MESquare.SMTPTester.log"
                , $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} :: {message}");
        }

    }
}
