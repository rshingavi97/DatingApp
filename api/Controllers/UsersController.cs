using System.Collections;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using System.Collections.Generic;
using System.Linq;
using api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // for [Authorize] attribute
using AutoMapper; // for IMapper
namespace api.Controllers
{
    using api.Interfaces; // for IUserRepository
    using api.DTOs;
    [Authorize]
    /*after adding above attribute here which made it nenecessay to send the
    authorizaton header for all endpoints of this controller*/
    public class UsersController:BaseApiController
    {
         /*Note: IUserRepository is nothing but encapsulation over DataContext hence replace DataContext by IUserRepository*/
       // private readonly DataContext _context;
       private readonly IUserRepository _userRepository;
       private readonly IMapper _autoMapper;
        public UsersController(/*DataContext context*/ IUserRepository userRepository, IMapper mapper)
        {
            /*In order to fetch the data, we need the object of DbContext i.e. DataContext class in our application.
            Hence, initialize the _context data member as an object of DataContext class in the constructor itself.
            */
           // _context = context;
           _userRepository = userRepository;
           _autoMapper =  mapper;
        }

       /* Synchronous mechanism which is not recommended.
       Following two end points are implemented as Blocking i.e. synchronous.
       They will not return until DB has down with the operations.
       By default, End Points support Synchronous mechanism.

        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            var users = _context.Users.ToList(); 
            //for using ToList here, we need to include using System.Linq; namespace
            //since it is commented, this namespace also dont require.
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
        */
        //Following end points are implemented as Asynchronous by using ASYNC keyword
       //Below end point returns the list of users, it would be consumed as  /api/users
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() (commented to use MemberDto)
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
           // var users = await _context.Users.ToListAsync(); commented to use _userRepository
           //var users = await _userRepository.GetUsersAsync();
           // return users; this line wont work because here users is a list of IEnumerable but we need to return the list of ActionResult's IEnumberable hence send it inside Ok response

           //Lets do the mapping
           // saying, please convert the list inside users into the list of type MemberDto and assign it to members
          // var members = _autoMapper.Map<IEnumerable<MemberDto>>(users);
          // return Ok(members);
          //after implementing AutoMapper queryable extension optimizaiton, just one line code
          return Ok(await _userRepository.GetMembersAsync());
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           /* var  appUserObj = await _userRepository.GetUserByUsernameAsync(username);
            var  memberDtoObj = _autoMapper.Map<MemberDto>(appUserObj);
            return memberDtoObj;
            */
            //After implementing AutoMapper queryable extension optimization, instead of above 3 lines, we just need 1 line of code
            return await _userRepository.GetMemberAsync(username);
        }

        //Below end point returns the specific of user of given id, 
        //it would be consumed as  /api/user/3  here 3 is a id
        //[Authorize]
         [HttpGet("id/{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
           // var user = await _context.Users.FindAsync(id);
           var  appUserObj = await _userRepository.GetUserByIdAsync(id);
           var  memberDtoObj = _autoMapper.Map<MemberDto>(appUserObj);
           return memberDtoObj;
        }
        

    }
}