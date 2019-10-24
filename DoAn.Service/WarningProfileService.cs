using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Data.Repository;
using System.Collections.Generic;

namespace DoAn.Service
{
    public interface IWarningProfileService
    {
        IEnumerable<WarningProfile> GetAll();
        void Insert(WarningProfile warningProfile);
        IEnumerable<WarningProfile> GetByUserId(string userId);

        WarningProfile GetById(int Id);
        WarningProfile Delete(int Id);
        void Update(WarningProfile warningprofile);

        void Save();

        bool InsertCauHinhCanhBaoByUser(string userId);
    }

    public class WarningProfileService : IWarningProfileService
    {
        private readonly IWaringProfileRepository _waringProfileRepository;
        private IUnitOfWork _unitOfWork;

        public WarningProfileService(IWaringProfileRepository warningProfileRepository, IUnitOfWork unitOfWork)
        {
            _waringProfileRepository = warningProfileRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<WarningProfile> GetAll() => _waringProfileRepository.GetAll();

        public IEnumerable<WarningProfile> GetByUserId(string userId) => _waringProfileRepository.GetMulti(x => x.User.Equals(userId));

        public WarningProfile GetById(int Id) => _waringProfileRepository.GetSingleById(Id);

        public bool InsertCauHinhCanhBaoByUser(string userId) => _waringProfileRepository.InsertCauHinhCanhBaoByUserId(userId);

        public void Save() => _unitOfWork.Commit();

        public void Update(WarningProfile warningprofile) => _waringProfileRepository.Update(warningprofile);

        public WarningProfile Delete(int Id)
        {
            return _waringProfileRepository.Delete(Id);
        }

        public void Insert(WarningProfile warningProfile)
        {
             _waringProfileRepository.Add(warningProfile);
        }
    }
}