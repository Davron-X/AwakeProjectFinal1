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
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly AppDbContext dbContext;
        public InquiryDetailRepository(AppDbContext db):base(db)
        {
            dbContext = db;
        }

      

        public void Update(InquiryDetail obj)
        {
            dbContext.InquiryDetails.Update(obj);
        }
    }
}
