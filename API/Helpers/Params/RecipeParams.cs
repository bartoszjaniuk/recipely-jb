namespace API.Helpers
{
    public class RecipeParams : BasePaginationParams
    {
        public int? CategoryId { get; set; }
        public int? KitchenOriginId { get; set; }
        public string Ingredient { get; set; }
        public string OrderBy { get; set; } = "name";
        public int MinTime { get; set; } = 2;
        public int MaxTime { get; set; } = 360;


        private string _search;

        public string Search
        {
            get => _search;
            set => _search = value;
        }


    }
}