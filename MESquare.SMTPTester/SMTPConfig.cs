using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MESquare.SMTPTester
{
    public class SMTPConfig
    {
        public String Name { get; set; }
        public String SMTPHost { get; set; }
        public int Port { get; set; }
        public Boolean UseSecuredConnection { get; set; }
        public Boolean UseAuthentication { get; set; }
        public String Information { get; set; }
    }
}
