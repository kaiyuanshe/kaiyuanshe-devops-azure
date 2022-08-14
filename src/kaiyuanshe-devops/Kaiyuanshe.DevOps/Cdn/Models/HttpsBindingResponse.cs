namespace Kaiyuanshe.DevOps.Cdn.Models
{
    public class HttpsBindingResponse
    {
        public bool Succeeded { get; set; }
        public bool IsAsync { get; set; }
        public AsyncInfo AsyncInfo { get; set; }
    }

    public class AsyncInfo
    {
        public string TaskTrackId { get; set; }
        public string TaskStatus { get; set; }
    }
}
