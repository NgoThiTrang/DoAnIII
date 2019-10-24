using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Data.Repository;
using System.Collections.Generic;

namespace DoAn.Service
{
    public interface IDeviceService
    {
        void Insert(Device entity);

        IEnumerable<Device> GetAll();

        IEnumerable<Device> GetListByDistrictId(int Id);

        //IEnumerable<Device> GetAllByHoDan(int Id);

        void Update(Device entity);

        Device GetById(int Id);
        Device Delete(int Id);

        void Save();
    }

    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private IUnitOfWork _unitOfWork;

        public DeviceService(IDeviceRepository deviceRepository, IUnitOfWork unitOfWork)
        {
            this._deviceRepository = deviceRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Insert(Device entity) => _deviceRepository.Add(entity);

        public IEnumerable<Device> GetAll() => _deviceRepository.GetAll();

        public void Update(Device entity) => _deviceRepository.Update(entity);

        public void Save() => _unitOfWork.Commit();

        public Device GetById(int Id) => _deviceRepository.GetSingleById(Id);

       

        public IEnumerable<Device> GetListByDistrictId(int Id)
        {
            return _deviceRepository.GetMulti(x => x.DistrictId.Equals(Id));
        }

        public Device Delete(int Id)
        {
            return _deviceRepository.Delete(Id);
        }

        //public IEnumerable<Device> GetAllByHoDan(int Id) => _deviceRepository.GetAllByHoDan(Id);
    }
}