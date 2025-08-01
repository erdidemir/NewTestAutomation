using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomationProject.Models.Users;

namespace TestAutomationProject.Models.Configuration
{
    public class ConfigurationModel
    {
        public string Browser { get; set; }
        public bool Headless { get; set; }
        public string BaseUrl { get; set; }
        public string ReferenceOrganization { get; set; }
        public string Workspace { get; set; }
        public int ElementDelayMilliSeconds { get; set; }
        public Dictionary<string, UserCredentials> UITestUsers { get; set; }
    }
}
