using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Data.Repository
{
    public interface IDeviceRepository : IRepository<Device>
    {
        //IEnumerable<Device> GetAllByHoDan(int Id);
    }

    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        //public IEnumerable<Device> GetAllByHoDan(int Id)
        //{
        //    var query = from d in DbContext.Devices
        //                join l in DbContext.Lakes
        //                on d.LakeId equals l.Id
        //                join h in DbContext.HoDans
        //                on l.HoDanId equals h.Id
        //                where h.Id.Equals(Id)
        //                select d;
        //    return query;
        //}
    }
}
