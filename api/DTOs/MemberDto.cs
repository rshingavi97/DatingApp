using System.Collections.Generic;
using System;
namespace api.DTOs
{
    public class MemberDto
    {
        /*all following fields are copied from AppUser entity except few changes*/
        public int Id{get;set;}
        public string UserName{get;set;}
        
        /*public byte[] PasswordHash {get;set;}
        public byte[] PasswordSalt {get;set;}
        dont need to send above two fields inside this DTO*/

        /*
        public DateTime DateOfBirth {get;set;}
        Instead of DateOfBirth, send the Age directly */

/*PhotoUrl property is not present inside AppUser entity but using here so that we could pass the url of main photo through this property to the client*/
        public string PhotoUrl {get;set;}
        
        /*how Age is going to be calculated bec this property is not present inside AppUser entity?
        Inside AppUser entity,we have already implemented the GetAge() method.
        AutoMapper use that GetAge() method to initialize Age property ....we dnot need to do anything extra...
        */
        public int Age{get;set;}
        public string  KnownAs {get;set;}
        public DateTime Created {get;set;}
        public DateTime LastActive {get;set;}
        public string   Gender{get;set;}
        public string   Introduction{get;set;}
        public string   LookingFor {get;set;}
        public string   Interest {get;set;}
        public string  Country {get;set;}
       public ICollection<PhotoDto> Photos{get;set;}

        
    }
}