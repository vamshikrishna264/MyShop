using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {

        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;


        public ProductCategoryRepository()
        {

            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        public void Update(ProductCategory productcategory)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(i => i.Id == productcategory.Id);

            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = productcategory;
            }
            else
            {
                throw new Exception("Product Not found");
            }
        }

        public ProductCategory Find(string Id)
        {
            ProductCategory productcategory = productCategories.Find(i => i.Id == Id);
            if (productcategory != null)
            {
                return productcategory;
            }
            else
            {
                throw new Exception("product Not found");
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory productcategoryToDelete = productCategories.Find(i => i.Id == Id);

            if (productcategoryToDelete != null)
            {
                productCategories.Remove(productcategoryToDelete);
            }
            else
            {
                throw new Exception(" product Not found");
            }
        }
    }
}
