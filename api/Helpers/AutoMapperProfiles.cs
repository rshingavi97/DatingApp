using AutoMapper; // for Profile
using System.Linq; // for FirstOrDefault
namespace api.Helpers
{
    using api.Entities;
    using api.DTOs;
    using api.Extensions; //for CalculateAge()
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
        /* here, we need to specify what we need to map to whom...e.g. we need to map AppUser to MemberDto hence specify it properly*/
            CreateMap<AppUser, MemberDto>()
            .ForMember( dest=>dest.PhotoUrl, 
                        opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
            .ForMember(dest=>dest.Age, opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
            /*since modifying an individual property hence above syntax.
            Here, saying modify PhotoUrl property*/
            /*Through opt saying, where we need to map the property
              Through src syaing, soruce is Photos table where you would get the PhotoUrl property
              FirstOrDefault saying, if isMain is true then use given URL as photoUrl
            */
            /* above dest means destination i.e. MemberDto and src means source i.e. AppUser */
            
            CreateMap<Photo, PhotoDto>();
        }
        
    }
}