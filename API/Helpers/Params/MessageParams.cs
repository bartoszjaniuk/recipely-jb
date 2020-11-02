namespace API.Helpers.Params
{
    public class MessageParams : BasePaginationParams
    {
        public string Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}