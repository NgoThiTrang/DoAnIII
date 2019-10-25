using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Data.Repository;
using System;
using System.Collections.Generic;

namespace DoAn.Service
{
    public interface IDataPackageService
    {
        IEnumerable<DataPackage> GetAll();

        IEnumerable<object> GetTop(int deviceId, int count, string paras);

        IEnumerable<Data.Model.Param> ReportDataPackage(int deviceId, DateTime date, int paramId);

        IEnumerable<DataPackage> GetByDeviceId(int DeviceId);

        DataPackage GetParamnewest(int deviceId);
    }

    public class DataPackageService : IDataPackageService
    {
        private readonly IDataPackageRepository _dataPackageRepository;
        private IUnitOfWork _unitOfWork;

        public DataPackageService(IDataPackageRepository dataPackageRepository, IUnitOfWork unitOfWork)
        {
            this._dataPackageRepository = dataPackageRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<DataPackage> GetAll() => _dataPackageRepository.GetAll();

        public IEnumerable<DataPackage> GetByDeviceId(int DeviceId) => _dataPackageRepository.GetMulti(x => x.DeviceId.Equals(DeviceId));

        public DataPackage GetParamnewest(int deviceId) => _dataPackageRepository.GetParamnewest(deviceId);

        public IEnumerable<object> GetTop(int deviceId, int count, string paras) => _dataPackageRepository.GetTop(deviceId, count, paras);

        public IEnumerable<Data.Model.Param> ReportDataPackage(int deviceId, DateTime date, int paramId) => _dataPackageRepository.ReportDatPackage(deviceId, date, paramId);
    }
}