using System.ComponentModel.DataAnnotations.Schema;
namespace api.Entities
{
    [Table("Photos")]  // from System.ComponentModel.DataAnnotations.Schema;
    public class Photo
    {
        public int Id {get;set;}
        public string Url { get; set;}
        public bool IsMain{ get; set;} /*true denoting such photo is main profile photo*/
        public string PublicId{get;set;}

        //add following two property as Fully Defining relationship for having OnCascadeDelete behavior
        public AppUser AppUser{get;set;} //adding Foregin key 
        public int AppUserId {get;set;}  //Foreign key on (Id column of AppUser table hence its name is AppUserId)
    }
}