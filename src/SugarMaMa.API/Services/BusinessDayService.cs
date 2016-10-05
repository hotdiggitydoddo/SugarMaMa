using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;
using SugarMaMa.API.Models.Estheticians;
using SugarMaMa.API.Models.SpaServices;
using SugarMaMa.API.Models.Shifts;
using SugarMaMa.API.Models.BusinessDays;

namespace SugarMaMa.API.Services
{
    public class BusinessDayService : IBusinessDayService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BusinessDay> _businessDays;


        public BusinessDayService(IRepository<BusinessDay> businessDays, IMapper mapper)
        {
            _businessDays = businessDays;
            _mapper = mapper;
        }

        public async Task<List<BusinessDayViewModel>> GetBusinessDaysAsync()
        {
            var businessDays = await _businessDays.GetAsync(x => x.Location);
            return _mapper.Map<List<BusinessDayViewModel>>(businessDays);
        }

    }

    public interface IBusinessDayService
    {
        Task<List<BusinessDayViewModel>> GetBusinessDaysAsync();
    }
}
