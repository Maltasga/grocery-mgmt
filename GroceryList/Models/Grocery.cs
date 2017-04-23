using System;
using System.Linq;

namespace GroceryList.Models
{
    public partial class Grocery
    {
        public Grocery()
        {
            Id = Guid.NewGuid();
            Category = new Category();
        }

        public Grocery(Guid id)
        {
            Id = id;
            Category = new Category();
        }

        public Grocery(Guid id, string name, double price, string unit, int categoryId, bool inBasket)
        {
            Id = id;
            Name = name;
            Price = price;
            Unit = unit;
            CategoryId = categoryId;
            InBasket = inBasket;
            this.Category = Category.GetCategories().FirstOrDefault(x => x.Id == categoryId);
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public bool InBasket { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}