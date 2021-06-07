using System; // for Exception class here.
using System.Net; // HttpStatusCode
using Microsoft.AspNetCore.Http;  // used for RequestDelegate
using Microsoft.Extensions.Logging; // used for ILogger
using Microsoft.Extensions.Hosting; // used for IHostEnvironment 
using System.Threading.Tasks; // used for InvokeAsync
using System.Text.Json; // for JsonSerializerOptions
namespace api.Middleware
{
    using api.Errors; // for ApiException class.
   public class ExceptionMiddleware 
    {
        /* This class is going to be used inside Pipleline i.e. Configure() method of Startup class.
        As being ExceptionMiddleware, it has to consider 3 things:
        1. Keep on delegating the coming requests in the middleware pipeline so it needs RequestDelegate class object
        2. Logging out the given exceptions into Terminal so it needs ILogger class object
        3. Above 2 steps/operations could be different on diff environments i.e. Production, Development, Testing etc.
           so it needs to use the IHostEnvironment class object for doing so.
        */
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware( RequestDelegate next,ILogger<ExceptionMiddleware> logger,                      IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) //as being middleware, it is getting the access to the HTTP request sent by client
        {
            try{
                await _next(context); 
                /*simply pass the HttpContext i.e. HTTP request to the next step as it is working in the pipeline
                if any exception generates in the pipeline then it would be looked like it generated in this try block so it would be handled in the CATCH block.*/
            }
            catch(Exception ex)
            {
                /*
                a. Generate the log on the Server Terminal else it would be hidden or silent.
                b. Create the response of given context (i.e. create the response for HTTP request sent by client)  
                This response can have ContextType, StatusCode.
                c. Add the additional information into response based on current environment i..e Dev,Prod,Test.
                d. Then compile above details into ApiException class object and store into RESPONSE variable
                e. As JSON data are always in lowercase hence we need to serialize ApiException class object
                f. for serialization, define the policy as JSON having CamelCase.
                g. send the response asynchronously by using AWAIT keyword.
                */
                _logger.LogError(ex, ex.Message);
                // above statement displays the Exception's message on the server terminal
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //i.e. 500 error code
                var response =  _env.IsDevelopment()?/*development env*/
                                                    new ApiException(context.Response.StatusCode, 
                                                    ex.Message,
                                                    ex.StackTrace?.ToString()
                                                    )
                                                    :/* production evn*/
                                                    new ApiException(context.Response.StatusCode, 
                                                    "Internal Server Error");
                                                     // ex.StackTrace?.ToString() dont send internal error to productionl hence not using it.
                                                
                
                var options = new JsonSerializerOptions{
                                                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                                                        // prepare the response into normal json format using camelcase hence using this policy.
                                                        };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }

        }
    }
}