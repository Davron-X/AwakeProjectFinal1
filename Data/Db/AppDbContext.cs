using Awake_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Awake_Data.Db
{
    public class AppDbContext:IdentityDbContext
    {
      public  DbSet<ApplicationType> ApplicationTypes { get; set; }
      public  DbSet<Category> Categories { get; set; }
      public  DbSet<Product> Products { get; set; }
      public  DbSet<ApplicationUser> ApplicationUsers  { get; set; }
      public  DbSet<InquiryHeader> InquiryHeaders { get; set; }
      public  DbSet<InquiryDetail> InquiryDetails  { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }
    }
}
