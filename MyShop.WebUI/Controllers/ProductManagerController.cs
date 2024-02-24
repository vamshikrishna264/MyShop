using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        // GET: ProductManager
        ProductRepository context;

        public ProductManagerController()
        {
            context = new ProductRepository();
        }
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
           

            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);

            if (product!= null)
            {
                return View(product);
            }
            else
            {
                throw new Exception("product not found");
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product,string Id)
        {
           Product productToEdit = context.Find(Id);

            if (productToEdit == null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                productToEdit.Name = product.Name;
                productToEdit.Description = product.Description;
                productToEdit.Category = product.Category;
                productToEdit.Price= product.Price;
                productToEdit.Image=product.Image;

                context.Commit();

                return RedirectToAction("Index");
                    
            }
        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);

            if (productToDelete!= null)
            {
                return View(productToDelete);
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
            Product productToDelete = context.Find(Id);

            if (productToDelete== null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productToDelete);
                }

                context.Delete(Id);

                context.Commit();

                return RedirectToAction("Index");

            }
        }


    }
}