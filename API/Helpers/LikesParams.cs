namespace API.Helpers
{
    public class LikesParams : BasePaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}