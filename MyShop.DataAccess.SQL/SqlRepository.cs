using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SqlRepository<T> : IRepository<T> where T : BaseEntity
    {

        internal DataContext context;
        internal DbSet<T> dbset;
        public SqlRepository(DataContext context)
        {
            this.context = context;
            this.dbset = context.Set<T>();
        }
        public IQueryable<T> Collection()
        {
            return dbset;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);
            if(context.Entry(t).State==EntityState.Detached)
            {
                dbset.Attach(t);

            }
            dbset.Remove(t);
        }

        public T Find(string Id)
        {
            return  dbset.Find(Id);
        }

        public void Insert(T t)
        {
            dbset.Add(t);
        }

        public void Update(T t)
        {
            dbset.Attach(t);
            context.Entry(t).State= EntityState.Modified;
        }
    }
}
