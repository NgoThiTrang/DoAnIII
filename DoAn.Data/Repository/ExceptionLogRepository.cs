using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Data.Repository
{
    public interface IExceptionLogRepository : IRepository<ExceptionLog>
    {
    }

    public class ExceptionLogRepository : RepositoryBase<ExceptionLog>, IExceptionLogRepository
    {
        public ExceptionLogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
