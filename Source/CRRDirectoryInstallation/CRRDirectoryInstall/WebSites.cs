using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRRDirectoryInstall
{
    public class WebSites
    {
        public bool AdminEnabled { get; set; }
        public string AvailabilityState { get; set; }
        public string ComputeMode { get; set; }
        public string ContentAvailabilityState { get; set; }
        public string DeploymentId { get; set; }
        public bool SiteEnabled { get; set; }
        public string WebSpace { get; set; }
        public string GeoRegion { get; set; }
        public DateTime LastModifiedTimeUtc { get; set; }
        public string RepositorySiteName { get; set; }
        public string WebSiteName { get; set; }
        public string RuntimeAvailabilityState { get; set; }
        public string SubscriptionId { get; set; }
        public string Status { get; set; }
        public string SiteMode { get; set; }
        public string StorageRecoveryDefaultState { get; set; }
        public string UsageState { get; set; }
    }
}
