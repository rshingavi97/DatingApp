using System.Threading.Tasks;  //Task
using System.Collections.Generic; //IEnumerable
namespace api.Interfaces
{
    using api.Entities; // for AppUser
    using api.DTOs;
    public interface IUserRepository
    {
        void Update(AppUser user); //Allowing update the user
        Task<bool> SaveAllAsync(); //to save all db operations asynchronously.
        Task<IEnumerable<AppUser>> GetUsersAsync(); //returning all users 
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync(); 
        //Unlike GetUsersAsync(), it is returning list of MemberDto..Optimization
        Task<MemberDto> GetMemberAsync(string username);
        //Unlike GetUserByUsernameAsync(), it is returning MemberDto..Optimization

        
    }
}