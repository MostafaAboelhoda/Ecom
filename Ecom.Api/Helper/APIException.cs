namespace Ecom.Api.Helper
{
    public class APIException : ResponseAPI
    {
        public APIException(int statusCode, string? message = null, string details =null) : base(statusCode, message)
        {
            this.Details = details;
        }
        public string Details { get; set; }
    }
}
