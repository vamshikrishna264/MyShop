﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        // GET: ProductManager
        IRepository<Product> context;
        IRepository<ProductCategory> productcategories;

        public ProductManagerController(IRepository<Product> productcontext,IRepository<ProductCategory> _productcategories)
        {
            productcategories = _productcategories;
            context = productcontext;
        }
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.product = new Product();
            viewModel.productcategories = productcategories.Collection();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product,HttpPostedFileBase file)
        {
           

            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    product.Image=product.Id+ Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//Productimages//") + product.Image);
                }
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
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.product = product;
                viewModel.productcategories = productcategories.Collection();
                return View(viewModel);
           
            }
            else
            {
                throw new Exception("product not found");
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product,string Id, HttpPostedFileBase file)
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
                if(file!=null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//Productimages//") + productToEdit.Image);
                }
                productToEdit.Name = product.Name;
                productToEdit.Description = product.Description;
                productToEdit.Category = product.Category;
                productToEdit.Price= product.Price;
              

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