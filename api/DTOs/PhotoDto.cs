namespace api.DTOs
{
    public class PhotoDto
    {
        public int Id {get;set;}
        public string Url { get; set;}
        public bool IsMain{ get; set;} /*true denoting such photo is main profile photo*/
        
    }
}