namespace Kaiyuanshe.DevOps.Cdn.Models
{
    public class Endpoint
    {
        public string EndpointID { get; set; }
        public EndpointSettings Settings { get; set; }
        public EndpointStatus Status { get; set; }
    }

    public class EndpointSettings
    {
        public string CustomDomain { get; set; }
        public string Host { get; set; }
        public string ICP { get; set; }
        public EndpointOrigin Origin { get; set; }
        public string ServiceType { get; set; }
    }

    public class EndpointOrigin
    {
        public string[] Addresses { get; set; }
    }

    public class EndpointStatus
    {
        public string Enabled { get; set; }
        public string IcpVerifyStatus { get; set; }
        public string LifetimeStatus { get; set; }
        public string CNameConfigured { get; set; }
        public string FreeTrialExpired { get; set; }
        public string TimeLastUpdated { get; set; }
    }
}
