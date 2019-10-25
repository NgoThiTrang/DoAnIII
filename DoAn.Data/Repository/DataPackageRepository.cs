using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Data.Repository
{
    public interface IDataPackageRepository : IRepository<DataPackage>
    {
        IEnumerable<object> GetTop(int deviceId, int count, string param);

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

        public IEnumerable<object> GetTop(int deviceId, int count, string param)
        {
            try
            {
                //var DeviceId = new SqlParameter { ParameterName = "@deviceId", Value = (object)deviceId ?? DBNull.Value };
                //var Count = new SqlParameter { ParameterName = "@count", Value = (object)count ?? DBNull.Value };
                //var Param = new SqlParameter { ParameterName = "@param", Value = (object)param ?? DBNull.Value };
                //var result = DbContext.Database.SqlQuery<dynamic>("Usp_GetDataPackageByParam @param, @deviceId, @count", Param, DeviceId, Count).ToList();
                var results = DbContext.DynamicListFromSql("Usp_GetDataPackageByParam @param, @deviceId, @count", new Dictionary<string, object> { { "@param", param }, { "@deviceId", deviceId }, { "@count", count } }).ToList();

                return results;
            }
            catch (Exception ex)
            {
                throw new System.NotImplementedException(ex.Message);
            }
            //var query = (from data in DbContext.DataPackages
            //             orderby data.TimePackage descending
            //             select data).Where(x => x.DeviceId.Equals(DeviceId)).Take(count);
            
            //return query;
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
