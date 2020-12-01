using Microsoft.AspNetCore.Mvc;
using api.Data;
using System.Collections.Generic;
using System.Linq;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            /*In order to fetch the data, we need the object of DbContext i.e. DataContext class in our application.
            Hence, initialize the _context data member as an object of DataContext class in the constructor itself.
            */
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }

        //Below end point returns the specific of user of given id, 
        //it would be consumed as  /api/user/3  here 3 is a id
        [HttpGet("{id}")]
        public ActionResult<AppUser> GetUser(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }
        
       //Below end point returns the list of users, it would be consumed as  /api/users
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        //Below end point returns the specific of user of given id, 
        //it would be consumed as  /api/user/3  here 3 is a id
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }*/
        

    }
}