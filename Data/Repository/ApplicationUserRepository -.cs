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
    public class ApplicationUserRepository  : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly AppDbContext dbContext;
        public ApplicationUserRepository(AppDbContext db):base(db)
        {
            dbContext = db;
        }
      
    }
}
