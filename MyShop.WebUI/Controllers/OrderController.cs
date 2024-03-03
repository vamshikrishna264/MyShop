using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        IOrderservice orderservice;

        public OrderController(IOrderservice orderservice)
        {
            this.orderservice = orderservice;
        }
        public ActionResult Index()
        {
            List<Order> orders = orderservice.GetOrdersList();
            return View(orders);
        }

        public ActionResult GetOrder(string id) {
            
            Order order= orderservice.GetOrder(id);
            return View(order);
        }

        public ActionResult UpdateOrder(string id) {
            ViewBag.StatusList = new List<string>()
            {
                "Order Created",
                "Payment processed",
                "Order Shipped",
                "Order Complete"
            };
            Order order = orderservice.GetOrder(id);
            return View(order);
        
        }
        [HttpPost]
        public ActionResult UpdateOrder(Order Updateorder, string id)
        {

            Order order = orderservice.GetOrder(id);

            order.orderstatus = Updateorder.orderstatus;
            orderservice.UpdateOrder(order);

            return RedirectToAction("Index");

        }


    }
}