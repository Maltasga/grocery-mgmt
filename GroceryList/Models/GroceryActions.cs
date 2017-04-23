using GroceryList.Dao;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace GroceryList.Models
{
    public partial class Grocery
    {
        private GroceryDao dao;

        public void Validfy()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new GroceryException("Name can't be empty");

            if (string.IsNullOrWhiteSpace(Unit))
                throw new GroceryException("Unit can't be empty");

            if (Category.GetCategories().FirstOrDefault(x => x.Id == CategoryId) == null)
                throw new GroceryException("Invalid category");

            if (Price <= 0)
                throw new GroceryException("Price can't be less than 0");
        }

        public void AddOrUpdate()
        {
            dao = new GroceryDao();

            if (dao.GetById(Id) == null)
            {
                dao.Insert(this);
            }
            else
            {
                dao.Update(this);
            }
        }

        public void SetModel()
        {
            dao = new GroceryDao();
            Grocery g = dao.GetById(Id);
            this.Id = g.Id;
            this.Name = g.Name;
            this.Price = g.Price;
            this.CategoryId = g.CategoryId;
            this.Unit = g.Unit;
            this.InBasket = g.InBasket;
            this.Category = Category.GetCategories().First(x => x.Id == this.CategoryId);
        }

        public IEnumerable<Grocery> GetAll()
        {
            dao = new GroceryDao();
            return dao.GetAll();
        }

        public IEnumerable<Grocery> GetOrderedList(string filter)
        {
            if (filter.Contains("Category"))
                filter = filter.Replace("Category", "Category.Name");
            dao = new GroceryDao();
            return dao.GetAll().OrderBy(filter);
        }

        public IEnumerable<Grocery> GetAllInBasket()
        {
            dao = new GroceryDao();
            return dao.GetAll().Where(x => x.InBasket);
        }

        public void Remove()
        {
            dao = new GroceryDao();
            dao.Remove(Id);
        }
    }
}