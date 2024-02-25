using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Basket : BaseEntity
    {
        public ICollection<BasketItem> basketitems { get; set; } 

        public Basket() {

            this.basketitems = new List<BasketItem>();
        
        
        }

    }
}
