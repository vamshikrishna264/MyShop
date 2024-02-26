using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public  class BasketViewSummaryModel
    {
        public int BasketCount { get; set; }
        public decimal BasketTotal { get; set; }

        public BasketViewSummaryModel()
        {

        }
        public BasketViewSummaryModel(int basketCount, decimal basketTotal)
        {
            this.BasketCount = basketCount;
            this.BasketTotal = basketTotal;
        }
    }
}
