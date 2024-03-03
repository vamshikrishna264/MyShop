﻿using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class DataContext:DbContext
    {
        public DataContext() : base("DefaultConnection")
        { }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }

        public DbSet<Basket> baskets { get; set; }

        public DbSet<BasketItem> basketitems { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> orders { get; set; }    
        public DbSet<OrderItem> orderitems { get; set; }

    }
}
