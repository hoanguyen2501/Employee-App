using AutoMapper;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Service.DTOs.AppUser;
using EmployeeApp.Service.DTOs.Company;
using EmployeeApp.Service.DTOs.Employee;

namespace EmployeeApp.Service.Mapper.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();

            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();

            CreateMap<AppUserDto, AuthResponse>();
        }
    }
}
