namespace api.Errors
{
    public class ApiException
    {
        public int StatusCode {get;set;}
        public string Message {get;set;}
        public string Details {get;set;} //holding the stack trace in case of exception.

        public ApiException(int statusCode, string message=null, string details=null)
        {
            StatusCode =  statusCode;
            Message = message;
            Details = details;
        }
    }
}