using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Data.Repository
{
    public interface IActivityLogRepository : IRepository<ActivityLog>
    {
    }

    public class ActivityLogRepository : RepositoryBase<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
