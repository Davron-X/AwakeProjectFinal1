using Awake_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awake_Data.Repository.IRepository
{
    public interface IApplicationTypeRepository:IRepository<ApplicationType>
    {
        void Update(ApplicationType obj);
    }
}
