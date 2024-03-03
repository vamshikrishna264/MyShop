using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Handlers;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        IOrderservice orderService;
        IRepository<Customer> customers;
        // GET: Basket

        public BasketController(IBasketService basketService,IOrderservice orderService,IRepository<Customer> customers)
        {
            this.basketService = basketService;
            this.orderService=orderService;
            this.customers = customers;
        }
       
        public ActionResult Index()
        {
            var model = basketService.GetAllItems(this.HttpContext);

            return View(model);
        }
        
        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");


        }
      
        public ActionResult RemovefromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }
       
        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.getBasketSummary(this.HttpContext);
            return PartialView(basketSummary);
        }
        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer=customers.Collection().FirstOrDefault(m=>m.Email==User.Identity.Name);
            if (customer!=null)
            {
                Order order = new Order()
                {
                   FirstName = customer.FirstName,
                   Surname = customer.LastName,
                   Email = customer.Email,
                   Street = customer.Street,
                   City = customer.City,
                   State = customer.State, 
                   ZipCode = customer.ZipCode,

                };
                return View(order);

            }
            else
            {
                return RedirectToAction("Error");
            }
          
        }
        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order)
        {
            var basketitems = basketService.GetAllItems(this.HttpContext);
            order.orderstatus = "Order Created";
            order.Email = User.Identity.Name;
            //process payment

            order.orderstatus = "payment processed";
            orderService.CreateOrder(order, basketitems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderId = order.Id });
        }

        public ActionResult ThankYou(string orderid)
        {
            ViewBag.orderid = orderid;
            return View();
        }
    }
}