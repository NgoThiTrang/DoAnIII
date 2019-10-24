using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Data.Repository;
using System.Collections.Generic;

namespace DoAn.Service
{
    public interface IExceptionLogService
    {
        void Insert(ExceptionLog entity);

        IEnumerable<ExceptionLog> GetAll();

        void Update(ExceptionLog entity);

        ExceptionLog GetById(int Id);

        void Save();
    }

    public class ExceptionLogService : IExceptionLogService
    {
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private IUnitOfWork _unitOfWork;

        public ExceptionLogService(IExceptionLogRepository exceptionLogRepository, IUnitOfWork unitOfWork)
        {
            this._exceptionLogRepository = exceptionLogRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Insert(ExceptionLog entity) => _exceptionLogRepository.Add(entity);

        public IEnumerable<ExceptionLog> GetAll() => _exceptionLogRepository.GetAll();

        public void Update(ExceptionLog entity) => _exceptionLogRepository.Update(entity);

        public void Save() => _unitOfWork.Commit();

        public ExceptionLog GetById(int Id) => _exceptionLogRepository.GetSingleById(Id);
    }
}