using AutoMapper;
using EmployeeApp.Domain.Entities;
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

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FullName, opts =>
                {
                    opts.MapFrom(src => $"{src.FirstName} {src.LastName}");
                })
                .ForMember(dest => dest.CompanyName, opts =>
                {
                    opts.MapFrom(src => src.Company.CompanyName);
                });
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();
        }
    }
}
