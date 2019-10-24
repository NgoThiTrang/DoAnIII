using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Web.Models
{
    public class BindingParam
    {
        private static BindingParam _instance;
        #region Khởi tạo Singleton đảm bảo tại một thời điểm chỉ có duy nhất một thể hiện class được tạo ra
        protected BindingParam()
        {
        }
        public static BindingParam Instance()
        {
            // nếu chưa có thì tạo thể hiện duy nhất
            if (_instance == null)
                _instance = new BindingParam();

            return _instance;
        }
        #endregion
        public  string ConnectionString = ConfigurationManager.ConnectionStrings["BkresContext"].ConnectionString;

        public  ParamModel FillParamModel(IDataReader idr, bool close)
        {
            ParamModel model = new ParamModel();
            model.time = Convert.ToDateTime(idr["time"]);
            model.value = SetNullDouble(idr["value"]);

            if (close)
                idr.Close();
            return model;
        }

        public  List<ParamModel> FillAllParamModel(IDataReader idr)
        {
            List<ParamModel> models = new List<ParamModel>();
            try
            {
                while (idr.Read())
                {
                    ParamModel model = new ParamModel();
                    model = FillParamModel(idr, false);
                    models.Add(model);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (idr != null)
                {
                    idr.Close();
                }
            }

            return models;
        }

        public  List<ParamModel> List(int DeviceId, DateTime Time_Package, int paramId)
        {
            IDataReader idr = SqlHelper.ExecuteReader(ConnectionString, "Usp_ReportDataPackage", Time_Package, paramId, DeviceId);
            return FillAllParamModel(idr);
        }

        public  double NullDouble
        {
            get { return 0; }
        }

        public  double SetNullDouble(object objValue)
        {
            double retValue = NullDouble;
            try
            {
                if (!(objValue == System.DBNull.Value))
                {
                    retValue = Convert.ToDouble(objValue);
                }
            }
            catch { }
            return retValue;
        }
    }
}