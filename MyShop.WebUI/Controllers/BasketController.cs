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
        // GET: Basket

        public BasketController(IBasketService basketService,IOrderservice orderService)
        {
            this.basketService = basketService;
            this.orderService=orderService;
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
        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(Order order)
        {
            var basketitems = basketService.GetAllItems(this.HttpContext);
            order.orderstatus = "Order Created";
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