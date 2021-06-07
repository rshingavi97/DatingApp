namespace api.Controllers
{
    using Microsoft.AspNetCore.Mvc; // added for ActionResult class.
    using Microsoft.AspNetCore.Authorization; //added for [Authorize] attribute
    using api.Data;
    public class BuggyController:BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController( DataContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
           return NotFound(); 
        }
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing=_context.Users.Find(-1);  
            //Find() searches for primary key , here -1 is not any primary key hence NULL would be returned.
            var thingToReturn = thing.ToString(); //note here NULL exception generates as calling ToString() on NULL value
            return thingToReturn; //it will never execute as exception generated at above step.
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
           
    }
}