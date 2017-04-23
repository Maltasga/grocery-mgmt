using System.Collections.Generic;

namespace GroceryList.Models
{
    public class Category
    {
        public Category() { }
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category(1, "Beverages"),
                new Category(2, "Frozen Foods"),
                new Category(3, "Bakery"),
                new Category(4, "Personal Care"),
                new Category(5, "Cleaners"),
                new Category(6, "Canned")
            };
        }
    }
}