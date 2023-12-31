namespace EmployeeApp.Service.DTOs.Company
{
    public class UpdateCompanyDto
    {
        public string CompanyName { get; set; }
        public DateTime EstablishedAt { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}