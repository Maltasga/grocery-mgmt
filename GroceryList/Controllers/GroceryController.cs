using GroceryList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GroceryList.Controllers
{
    public class GroceryController : Controller
    {
        private Grocery model;

        public ActionResult Index()
        {
            model = new Grocery();
            IEnumerable<Grocery> groceryList = model.GetAll();
            ViewBag.InBasketCount = groceryList.Count(x => x.InBasket);
            return View(groceryList);
        }

        public ActionResult New()
        {
            model = new Grocery();
            VerifyCatchedError(ref model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Grocery model)
        {
            try
            {
                model.Validfy();
                model.AddOrUpdate();
                return RedirectToAction("Index");
            }
            catch (GroceryException ex)
            {
                TempData["catchedError"] = new KeyValuePair<string, Grocery>(ex.Message, model);
                return RedirectToAction("New");
            }
        }

        public ActionResult Detail(Guid id)
        {
            model = new Grocery(id);
            model.SetModel();
            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            model = new Grocery(id);
            VerifyCatchedError(ref model);
            model.SetModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Grocery model)
        {
            try
            {
                model.Validfy();
                model.AddOrUpdate();
                return RedirectToAction("Index");
            }
            catch (GroceryException ex)
            {
                TempData["catchedError"] = new KeyValuePair<string, Grocery>(ex.Message, model);
                return RedirectToAction("New");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public int ToBasketShop(Guid id, bool add)
        {
            Grocery model = new Grocery(id);
            model.SetModel();
            model.InBasket = add;
            model.AddOrUpdate();
            int i = model.GetAllInBasket().Count();
            return i;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(Guid id)
        {
            model = new Grocery(id);
            try
            {
                model.Remove();
                return Json(new
                {
                    status = HttpStatusCode.OK,
                    inBasketShop = model.GetAllInBasket().Count()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message
                });
            }
        }

        public ActionResult OrderBy(string order)
        {
            try
            {
                model = new Grocery();

                // Query an anonymous object to get the formatted list
                return Json((from i in model.GetOrderedList(order)
                             select new
                             {
                                 Id = i.Id,
                                 Name = i.Name,
                                 Category = i.Category.ToString(),
                                 Price = i.Price.ToString("c"),
                                 InBasket = i.InBasket
                             }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private void VerifyCatchedError(ref Grocery model)
        {
            if (TempData.ContainsKey("catchedError"))
            {
                KeyValuePair<string, Grocery> catchError = (KeyValuePair<string, Grocery>)TempData["catchedError"];
                ViewBag.MessageError = catchError.Key;
                model = catchError.Value;
            }
        }
    }
}