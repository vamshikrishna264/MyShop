using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        // GET: ProductCategoryManager
        IRepository<ProductCategory> context;

        public ProductCategoryManagerController(IRepository<ProductCategory> productcontext)
        {
            context = productcontext;
        }
        public ActionResult Index()
        {
            List<ProductCategory> productCategories = context.Collection().ToList();
            return View(productCategories);
        }

        public ActionResult Create()
        {
            ProductCategory productcategory = new ProductCategory();
            return View(productcategory);
        }
        [HttpPost]
        public ActionResult Create(ProductCategory productcategory)
        {


            if (!ModelState.IsValid)
            {
                return View(productcategory);
            }
            else
            {
                context.Insert(productcategory);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            ProductCategory productcategory = context.Find(Id);

            if (productcategory != null)
            {
                return View(productcategory);
            }
            else
            {
                throw new Exception("product not found");
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productcategory, string Id)
        {
            ProductCategory productcategoryToEdit = context.Find(Id);

            if (productcategoryToEdit == null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productcategory);
                }
                productcategoryToEdit.Category = productcategory.Category;
                
                

                context.Commit();

                return RedirectToAction("Index");

            }
        }

        public ActionResult Delete(string Id)
        {
            ProductCategory productcategoryToDelete = context.Find(Id);

            if (productcategoryToDelete != null)
            {
                return View(productcategoryToDelete);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productcategoryToDelete = context.Find(Id);

            if (productcategoryToDelete == null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productcategoryToDelete);
                }

                context.Delete(Id);

                context.Commit();

                return RedirectToAction("Index");

            }
        }
    }
}