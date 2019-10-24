using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Data.Repository
{
    public interface IDistrictRepository : IRepository<District>
    {
    }

    public class DistrictRepository : RepositoryBase<District>, IDistrictRepository
    {
        public DistrictRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
