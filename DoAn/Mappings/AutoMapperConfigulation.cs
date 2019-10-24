using AutoMapper;
using DoAn.Data.Model;
using Web.Models;

namespace DoAn.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();
                cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
                cfg.CreateMap<District, DistrictModel>();

                cfg.CreateMap<Param, ParamModel>();
            });
        }
    }
}