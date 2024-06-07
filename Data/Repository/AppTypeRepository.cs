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
    public class AppTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly AppDbContext dbContext;
        public AppTypeRepository(AppDbContext db):base(db)
        {
            dbContext = db;
        }
        public void Update(ApplicationType obj)
        {
            var objFromDb = dbContext.ApplicationTypes.FirstOrDefault(x=>x.Id==obj.Id);
            if (objFromDb!=null)
            {
                objFromDb.Name = obj.Name;
            }
        }
    }
}
