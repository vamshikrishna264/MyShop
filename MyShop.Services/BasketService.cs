﻿using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public  class BasketService:IBasketService
    {

        IRepository<Product> productcontext;
        IRepository<Basket> basketcontext;

        public const string BasketSessionName="eCommerceBasket";

        public BasketService(IRepository<Product> productcontext, IRepository<Basket> basketcontext)
        {
            this.productcontext = productcontext;
            this.basketcontext = basketcontext;
        }
        private Basket GetBasket(HttpContextBase contextBase,bool CreateIfNull)
        {
            HttpCookie cookie = contextBase.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
               string  basketId = cookie.Value; 
                if (string.IsNullOrEmpty(basketId))
                {
                    basket=basketcontext.Find(basketId);
                }
                else
                {
                    if(CreateIfNull)
                    {
                        basket = CreateNewBasket(contextBase);
                    }
                }


            }
            else
            {
                if(CreateIfNull)
                {
                    basket = CreateNewBasket(contextBase);
                }
            }
            return basket;
        }
        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket= new Basket();
            basketcontext.Insert(basket);
            basketcontext.Commit();

            HttpCookie cookie=new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;

        }

        public void AddToBasket(HttpContextBase httpContext,string ProductId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.basketitems.Where(p=>p.Id== ProductId).FirstOrDefault();
            if (item==null) {
                item = new BasketItem ()
               {
                    BasketId = basket.Id,
                    ProductId = ProductId,
                    Quantity = 1
                };
                basket.basketitems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }
            basketcontext.Commit();
        }
        public void RemoveFromBasket(HttpContextBase httpContext,string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.basketitems.Where(p => p.Id == itemId).FirstOrDefault();
            if (item!=null)
            {
                basket.basketitems.Remove(item);
                basketcontext.Commit();
            }
        }

        public List<BasketItemViewModel> GetAllItems(HttpContextBase httpContext)
        {
            Basket Basket = GetBasket(httpContext, false);
            if (Basket != null)
            {
                var result = (from b in Basket.basketitems
                              join p in productcontext.Collection() on b.Id equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Price = p.Price,
                                  Image = p.Image,

                              }).ToList();
                return result;
                            
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketViewSummaryModel getBasketSummary(HttpContextBase httpContext)
        {

            Basket Basket = GetBasket(httpContext, false);
            BasketViewSummaryModel model= new BasketViewSummaryModel(0,0);
            if (Basket != null)
            {
                int? BasketCount = (from item in Basket.basketitems
                                    select item.Quantity).Sum();
                decimal? baskettotal = (from item in Basket.basketitems
                                        join p in productcontext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                model.BasketCount = BasketCount ?? 0;
                model.BasketTotal = baskettotal ?? decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }
        }
    }
}
