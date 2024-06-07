using Awake_Data.Db;
using Awake_Data.Repository.IRepository;
using Awake_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awake_Data.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        private readonly AppDbContext dbContext;
        public ProductRepository(AppDbContext db):base(db)
        {
            dbContext = db;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == WC.CategoryName)
            {
                return dbContext.Categories.ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });
            }
            else if (obj == WC.AppTypeName)
                return dbContext.ApplicationTypes.ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });

            return null;
        }

        public void Update(Product obj)
        {
            dbContext.Products.Update(obj);
        }
    }
}
