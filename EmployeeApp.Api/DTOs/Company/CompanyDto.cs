using EmployeeApp.Api.DTOs.Employee;

namespace EmployeeApp.Api.DTOs.Company
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime EstablishedAt { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<EmployeeDto> Employees { get; set; }
    }
}