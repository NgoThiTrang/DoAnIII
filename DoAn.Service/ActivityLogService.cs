using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Data.Repository;
using System.Collections.Generic;
using System.Linq;

namespace DoAn.Service
{
    public interface IActivityLogService
    {
        IEnumerable<ActivityLog> GetAll(string keyword);
        IEnumerable<ActivityLog> GetAll();

        IEnumerable<ActivityLog> GetAllPaging(string keyword, int page, int pageSize);

        void Save();
    }

    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private IUnitOfWork _unitOfWork;

        public IEnumerable<ActivityLog> GetAll()
        {
            return _activityLogRepository.GetAll();
        }

        public IEnumerable<ActivityLog> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _activityLogRepository.GetMulti(x => x.Content.Contains(keyword) );
            else
                return _activityLogRepository.GetAll();
        }

        public IEnumerable<ActivityLog> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _activityLogRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Content.Contains(keyword));
            int totalRow = query.Count();

            return query = query.OrderByDescending(x => x.CreatedDate)
                  .Skip((page - 1) * pageSize).Take(pageSize);

            //  var data = query.ProjectTo<ActivityLogViewModel>().ToList();

            //var paginationSet = new PagedResult<ActivityLogViewModel>()
            //{
            //    Results = data,
            //    CurrentPage = page,
            //    RowCount = totalRow,
            //    PageSize = pageSize
            //};
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}