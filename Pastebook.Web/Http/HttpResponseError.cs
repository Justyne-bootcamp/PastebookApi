namespace Pastebook.Web.Http
{
    public class HttpResponseError
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class HttpResponseResult
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
