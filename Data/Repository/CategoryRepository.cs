using Awake_Data.Db;
using Awake_Data.Repository.IRepository;
using Awake_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awake_Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext dbContext;
        public CategoryRepository(AppDbContext db):base(db)
        {
            dbContext = db;
        }
        public void Update(Category obj)
        {
            var objFromDb = dbContext.Categories.FirstOrDefault(x=>x.Id==obj.Id);
            if (objFromDb!=null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.DisplayOrder = obj.DisplayOrder;
            }
        }
    }
}
