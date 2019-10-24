using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Data.Repository
{
    public interface IDataPackageRepository : IRepository<DataPackage>
    {
        IEnumerable<DataPackage> GetTop1000(int DeviceId);

        IEnumerable<Param> ReportDatPackage(int deviceId, DateTime date, int paramId);

        DataPackage GetParamnewest(int deviceId);
    }

    public class DataPackageRepository : RepositoryBase<DataPackage>, IDataPackageRepository
    {
        public DataPackageRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public DataPackage GetParamnewest(int deviceId)
        {
            try
            {
                var DeviceId = new SqlParameter { ParameterName = "@DeviceId", Value = (object)deviceId ?? DBNull.Value };
                var result = DbContext.Database.SqlQuery<DataPackage>("exec Usp_GetParamnewest  @DeviceId", DeviceId).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw new System.NotImplementedException(ex.Message);
            }
        }

        public IEnumerable<DataPackage> GetTop1000(int DeviceId)
        {
            var query = (from data in DbContext.DataPackages
                         orderby data.TimePackage descending
                         select data).Where(x => x.DeviceId.Equals(DeviceId)).Take(1000);
            return query;
        }

        public IEnumerable<Param> ReportDatPackage(int deviceId, DateTime date, int paramId)
        {
            try
            {
                var DeviceId = new SqlParameter { ParameterName = "@DeviceId", Value = (object)deviceId ?? DBNull.Value };
                var Date = new SqlParameter { ParameterName = "@DateTimeNow", Value = (object)date ?? DBNull.Value };
                var ParamId = new SqlParameter { ParameterName = "@paramId", Value = (object)paramId ?? DBNull.Value };
                var result = DbContext.Database.SqlQuery<Param>("Usp_ReportDataPackage @DateTimeNow, @paramId, @DeviceId", Date, ParamId, DeviceId).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new System.NotImplementedException(ex.Message);
            }
        }

    }
}
