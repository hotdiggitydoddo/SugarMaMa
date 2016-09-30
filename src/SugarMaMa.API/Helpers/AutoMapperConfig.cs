using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.Models.Estheticians;

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
        }
    }
}
