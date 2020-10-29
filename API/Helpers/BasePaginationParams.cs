namespace API.Helpers
{
    public class BasePaginationParams
    {
        private const int MaxPageSize = 30;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        private int myVar;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}