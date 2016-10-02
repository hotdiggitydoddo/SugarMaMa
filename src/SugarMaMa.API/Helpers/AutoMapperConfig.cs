using AutoMapper;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.Models.Estheticians;
using SugarMaMa.API.Models.BusinessDays;
using SugarMaMa.API.Models.Shifts;
using SugarMaMa.API.Models.SpaServices;

namespace SugarMaMa.API.Helpers
{
    public class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MyProfile());
            });

            return config.CreateMapper();
        }
    }

    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<AddEstheticianViewModel, ApplicationUser>()
                .ForMember(d => d.UserName, m => m.MapFrom(s => s.Email));

            CreateMap<BusinessDay, BusinessDayViewModel>();
            CreateMap<Location, LocationViewModel>();
            CreateMap<Shift, ShiftViewModel>();
            CreateMap<SpaService, SpaServiceViewModel>();
        }
    }
}
