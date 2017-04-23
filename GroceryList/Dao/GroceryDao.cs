using GroceryList.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml;

namespace GroceryList.Dao
{

    public class GroceryDao
    {
        private string path = HostingEnvironment.MapPath("~/App_Data/XMLGrocery.xml");
        private FileStream fileStream;
        private XmlDocument db;

        public GroceryDao()
        {
            db = new XmlDocument();
            db.Load(path);
            fileStream = new FileStream(path, FileMode.Open);
        }

        public Grocery GetById(Guid id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Grocery> GetAll()
        {
            ICollection<Grocery> groceries = new List<Grocery>();
            XmlNodeList xmlList = db.GetElementsByTagName("Grocery");

            for (int i = 0; i < xmlList.Count; i++)
            {
                XmlElement element = (XmlElement)db.GetElementsByTagName("Grocery")[i];
                Guid id = Guid.Parse(element.GetAttribute("Id"));
                string name = element.GetAttribute("Name");
                double price = Convert.ToDouble(element.GetAttribute("Price"));
                string unit = element.GetAttribute("Unit");
                int categoryId = Convert.ToInt32(element.GetAttribute("CategoryId"));
                bool inBasket = Convert.ToBoolean(element.GetAttribute("InBasket"));
                groceries.Add(new Grocery(id, name, price, unit, categoryId, inBasket));
            }
            Dispose();
            return groceries;
        }

        public void Insert(Grocery grocery)
        {
            XmlElement root = db.CreateElement("Grocery");
            root.SetAttribute("Id", grocery.Id.ToString());
            root.SetAttribute("Name", grocery.Name);
            root.SetAttribute("Price", grocery.Price.ToString());
            root.SetAttribute("Unit", grocery.Unit);
            root.SetAttribute("CategoryId", grocery.CategoryId.ToString());
            root.SetAttribute("InBasket", grocery.InBasket.ToString());
            db.DocumentElement.AppendChild(root);
            Dispose();
        }

        public void Update(Grocery grocery)
        {
            XmlNodeList xmlList = db.GetElementsByTagName("Grocery");
            for (int i = 0; i < xmlList.Count; i++)
            {
                XmlElement element = (XmlElement)db.GetElementsByTagName("Grocery")[i];
                Guid id = Guid.Parse(element.GetAttribute("Id"));
                if (id == grocery.Id)
                {
                    element.SetAttribute("Name", grocery.Name);
                    element.SetAttribute("Price", grocery.Price.ToString());
                    element.SetAttribute("Unit", grocery.Unit);
                    element.SetAttribute("CategoryId", grocery.CategoryId.ToString());
                    element.SetAttribute("InBasket", grocery.InBasket.ToString());
                    break;
                }
            }
            Dispose();
        }

        public void Remove(Guid id)
        {
            XmlNodeList xmlList = db.GetElementsByTagName("Grocery");
            for (int i = 0; i < xmlList.Count; i++)
            {
                XmlElement element = (XmlElement)db.GetElementsByTagName("Grocery")[i];
                Guid groceryId = Guid.Parse(element.GetAttribute("Id"));
                if (groceryId == id)
                {
                    db.DocumentElement.RemoveChild(element);
                    break;
                }
            }
            Dispose();
        }

        public void Dispose()
        {
            fileStream.Close();
            db.Save(path);
        }
    }
}