using System.Collections.Generic;

namespace API.Entities
{
    public class KitchenOrigin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}