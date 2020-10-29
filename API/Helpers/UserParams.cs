namespace API.Helpers
{
    public class UserParams : BasePaginationParams
    {
        public string CurrentUsername { get; set; }
        public string OrderBy { get; set; } = "lastActive";
        
    }
}