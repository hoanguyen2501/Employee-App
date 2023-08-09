using AutoMapper;
using EmployeeApp.Api.DTOs.Company;
using EmployeeApp.Api.DTOs.Employee;
using EmployeeApp.Api.Entities;

namespace EmployeeApp.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();

            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();
        }
    }
}