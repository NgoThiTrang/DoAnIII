using DoAn.Data.Infrastructure;
using DoAn.Data.Model;
using DoAn.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoAn.Service
{
    public interface IApplicationGroupService
    {
        ApplicationGroup GetDetail(int id);

        IEnumerable<ApplicationGroup> GetAll(int page, int pageSize, out int totalRow, string filter);

        IEnumerable<ApplicationGroup> GetAll();

        ApplicationGroup Add(ApplicationGroup appGroup);

        IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId);
        IEnumerable<ApplicationGroup> GetListGroupByUserId(string userId);

        void Update(ApplicationGroup appGroup);

        ApplicationGroup Delete(int id);

        bool AddUserToGroups(IEnumerable<ApplicationUserGroup> groups, string userId);

        void Save();
    }

    public class ApplicationGroupService : IApplicationGroupService
    {
        private IApplicationGroupRepository _appGroupRepository;
        private IUnitOfWork _unitOfWork;
        private IApplicationUserGroupRepository _appUserGroupRepository;

        public ApplicationGroupService(IUnitOfWork unitOfWork, IApplicationUserGroupRepository appUserGroupRepository,
            IApplicationGroupRepository appGroupRepository)
        {
            this._appGroupRepository = appGroupRepository;
            this._unitOfWork = unitOfWork;
            this._appUserGroupRepository = appUserGroupRepository;
        }

        public ApplicationGroup Add(ApplicationGroup appGroup)
        {
            if (_appGroupRepository.CheckContains(x => x.Name == appGroup.Name))
                throw new Exception("Tên không được trùng");
            return _appGroupRepository.Add(appGroup);
        }

        public bool AddUserToGroups(IEnumerable<ApplicationUserGroup> groups, string userId)
        {
            _appUserGroupRepository.DeleteMulti(x => x.UserId == userId);
            foreach (var userGroup in groups)
            {
                _appUserGroupRepository.Add(userGroup);
            }
            return true;
        }

        public ApplicationGroup Delete(int id)
        {
            var appGroup = this._appGroupRepository.GetSingleById(id);
            return _appGroupRepository.Delete(appGroup);
        }

        public IEnumerable<ApplicationGroup> GetAll()
        {
            return _appGroupRepository.GetAll();
        }

        public IEnumerable<ApplicationGroup> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = _appGroupRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));

            totalRow = query.Count();
            return query.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
        }

        public ApplicationGroup GetDetail(int id)
        {
            return _appGroupRepository.GetSingleById(id);
        }

        public IEnumerable<ApplicationGroup> GetListGroupByUserId(string userId)
        {
            return _appGroupRepository.GetListGroupByUserId(userId);
        }

        public IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId) => _appGroupRepository.GetListUserByGroupId(groupId);

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ApplicationGroup appGroup)
        {
            if (_appGroupRepository.CheckContains(x => x.Name == appGroup.Name && x.Id != appGroup.Id))
                throw new Exception("Tên không được trùng");
            _appGroupRepository.Update(appGroup);
        }
    }
}