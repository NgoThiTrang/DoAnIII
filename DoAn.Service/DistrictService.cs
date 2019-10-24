using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Data.Repository;
using System.Collections.Generic;

namespace DoAn.Service
{
    public interface IDistrictService
    {
        void Insert(District entity);

        IEnumerable<District> GetAll();

        void Update(District entity);

        District GetById(int Id);

        IEnumerable<District> GetListByProvinceId(int Id);
        District Delete(int Id);

        void Save();
    }

    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;
        private IUnitOfWork _unitOfWork;

        public DistrictService(IDistrictRepository districtRepository, IUnitOfWork unitOfWork)
        {
            this._districtRepository = districtRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Insert(District entity) => _districtRepository.Add(entity);

        public IEnumerable<District> GetAll() => _districtRepository.GetAll();

        public void Update(District entity) => _districtRepository.Update(entity);

        public void Save() => _unitOfWork.Commit();

        public District GetById(int Id) => _districtRepository.GetSingleById(Id);

        public IEnumerable<District> GetListByProvinceId(int Id) => _districtRepository.GetMulti(x => x.ProvinceId.Equals(Id));

        public District Delete(int Id)
        {
            return _districtRepository.Delete(Id);
        }
    }
}