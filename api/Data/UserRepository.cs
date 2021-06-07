using System.Threading.Tasks; //for Task
using Microsoft.EntityFrameworkCore; //for SingleOrDefaultAsync as its EF supported method.
using System.Collections.Generic; // for IEnumberable
using System.Linq;
using AutoMapper; // for IMapper
using AutoMapper.QueryableExtensions; // for Project
namespace api.Data
{
    using api.Interfaces; // for IUserRepository
    using api.Entities; // for AppUser
    using api.DTOs;
    public class UserRepository:IUserRepository
    {
        //we need to use the DbContext hence use the consturctor to inject it
        private readonly DataContext _context;
        private readonly IMapper  _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _context =  context;
            _mapper =   mapper;
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id); //search the user into Users table by using given id
        }
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
           // return await _context.Users.SingleOrDefaultAsync(x=>x.UserName == username);
           return await _context.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x=>x.UserName==username);
                                      
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.Include(p=>p.Photos).ToListAsync();
        }
        public async Task<bool> SaveAllAsync() //return true if saving done else false
        {
            var result = await _context.SaveChangesAsync(); //SaveChangesAsync()returns more than 0 if successful else 0
            return (result > 0?true:false);
        }
        //Note in all above operations we need to use ASYNC as doing operations synchronously on DB
        //In below method, we dont need to do any operaiton on DB directly, instead,just updating the state of AppUser entity inside memory
        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
           return await _context.Users
                                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                .ToListAsync();
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {

            return await _context.Users
                                 .Where(x=>x.UserName==username)
                                 .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                 .SingleOrDefaultAsync();
        }

        
    }
}