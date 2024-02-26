using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        // GET: Basket

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
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

    }
}