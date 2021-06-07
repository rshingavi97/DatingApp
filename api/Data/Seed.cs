using System.Security.Cryptography;//FOR HMACSHA512
using System.Text; //Encoding
using System.Threading.Tasks; //for Task return type
using Microsoft.EntityFrameworkCore; //for AnyAsync() method 
using System.Text.Json; // for serializaiton and deserialization of JSON data
using System.Collections.Generic; //for List
namespace api.Data
{
    using api.Entities;
    public class Seed
    {
        /*
        If given ASYNC method is returning any class then we need to mention return type as Task<'name of class whose object returning'>
        If given ASYNC method is not returning anything then we need to just mention return type as Task.
        SeedUsers() method is not going to return anything hence return type just as Task which providing Asynchronous functionality.
        */
        public static async Task SeedUsers(DataContext context)
        {
            //First check asynchronously whether Users table having any records, If it has then dont insert anything just return
            if( await context.Users.AnyAsync())
            return;
            //It means, Users table is empty, lets proceed further
            //Load the records from UserSeedData.json file 
            var records = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            //Data is loaded into string format inside records variable hence we need to deserialize into AppUser class object format
            //so that we could insert it into Users table so lets do the deserializaiton
            var userList = JsonSerializer.Deserialize<List<AppUser>>(records);
            //Lets iterate userList in order to insert the records into Users table.
            foreach( var userObj in userList)
            {
            /* Every user into Users table must have its username and password.
            Every userObj is itself a user of given portal hence we are going to use the UserName property of given record as username.
            Note: username must be in lowercase hence converting given UserName into lowercase before inserting.
            Time being, Even though, there are 10 different records, but all of them is having same password since inserting data manually.
            Even though password is same, but it must be encrypted.
            */
            using var hmac = new HMACSHA512();
            userObj.UserName = userObj.UserName.ToLower();
            userObj.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
            /* password is password for time being as inserting fake data... */
            userObj.PasswordSalt = hmac.Key;
            context.Users.Add(userObj);
            /* note: we dont need to use AWAIT for above method as we are just updating EntityFramework above*/
            }
            /* after adding all above data into EF, now, we need to update the DB, so we could AWAIT for this operation*/
            await context.SaveChangesAsync();
        }
        
    }
}