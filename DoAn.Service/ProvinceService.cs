using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Data.Repository;
using System.Collections.Generic;

namespace DoAn.Service
{
    public interface IProvinceService
    {
        void Insert(Province entity);

        IEnumerable<Province> GetAll();

        void Update(Province entity);

        Province GetById(int Id);
        Province Delete(int Id);

        void Save();
    }

    public class ProvinceService : IProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;
        private IUnitOfWork _unitOfWork;

        public ProvinceService(IProvinceRepository provinceRepository, IUnitOfWork unitOfWork)
        {
            _provinceRepository = provinceRepository;
            _unitOfWork = unitOfWork;
        }

        public void Insert(Province entity) => _provinceRepository.Add(entity);

        public IEnumerable<Province> GetAll() => _provinceRepository.GetAll();

        public void Update(Province entity) => _provinceRepository.Update(entity);

        public void Save() => _unitOfWork.Commit();

        public Province GetById(int Id) => _provinceRepository.GetSingleById(Id);

        public Province Delete(int Id)
        {
            return _provinceRepository.Delete(Id);
        }
    }
}