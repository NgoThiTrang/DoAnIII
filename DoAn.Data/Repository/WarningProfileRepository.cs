using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using System;
using System.Data.SqlClient;

namespace DoAn.Data.Repository
{
    public interface IWaringProfileRepository : IRepository<WarningProfile>
    {
        bool InsertCauHinhCanhBaoByUserId(string UserId);
    }

    public class WarningProfileRepository : RepositoryBase<WarningProfile>, IWaringProfileRepository
    {
        public WarningProfileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool InsertCauHinhCanhBaoByUserId(string UserId)
        {
            bool result = false;
            try
            {
                var userId = new SqlParameter { ParameterName = "@UserId", Value = (object)UserId ?? DBNull.Value };
                DbContext.Database.ExecuteSqlCommand("exec Usp_InsertCauHinhCanhBao @UserId", UserId);
                result = true;
            }
            catch (Exception ex)
            {
                throw new System.NotImplementedException(ex.Message);
            }
            return result;
        }
    }
}