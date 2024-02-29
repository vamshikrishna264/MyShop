using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Controllers.Mocks;
using System;
using System.Linq;
using System.Web.Mvc;


namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //setup
            IRepository<Product> products = new Mockcontext<Product>();

            IRepository<Basket> baskets = new Mockcontext<Basket>();
            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(products,baskets);
            var controller = new BasketController(basketService);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);


            //Act
            /// basketService.AddToBasket(httpContext,"1");
            controller.AddToBasket("1");

            Basket basket = baskets.Collection().FirstOrDefault();

            //Assert

            Assert.IsNotNull(basket);
            Assert.AreEqual(1,basket.basketitems.Count);
            Assert.AreEqual("1", basket.basketitems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel() {
            IRepository<Product> products = new Mockcontext<Product>();

            IRepository<Basket> baskets = new Mockcontext<Basket>();

            products.Insert(new Product { Id = "1", Price = 10.00m});
            products.Insert(new Product { Id = "2", Price = 5.00m });

            Basket basket = new Basket();
            basket.basketitems.Add(new BasketItem { ProductId = "1", Quantity = 2 });
            basket.basketitems.Add(new BasketItem { ProductId = "2", Quantity = 1 });
            baskets.Insert(basket);

        

            IBasketService basketService = new BasketService(products, baskets);
            var controller=new BasketController(basketService);
            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            var result = controller.BasketSummary() as PartialViewResult;
            var BasketSummary = (BasketViewSummaryModel)result.ViewData.Model;

            Assert.AreEqual(3, BasketSummary.BasketCount);
            Assert.AreEqual(25.00m,BasketSummary.BasketTotal);
        }
    }
}
