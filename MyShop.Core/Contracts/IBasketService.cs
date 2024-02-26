using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Core.Contracts
{
    public interface IBasketService
    {

        void AddToBasket(HttpContextBase httpContext, string ProductId);
        void RemoveFromBasket(HttpContextBase httpContext, string itemId);

        List<BasketItemViewModel> GetAllItems(HttpContextBase httpContext);
        BasketViewSummaryModel getBasketSummary(HttpContextBase httpContext);


    }
}
