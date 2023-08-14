using AutoMapper;
using EmployeeApp.Service.Mapper.Profiles;

namespace EmployeeApp.Service.Mapper
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration ConfigureMappings()
        {
            return new MapperConfiguration(config =>
            {
                config.AddProfile<MapperProfile>();
            });
        }
    }
}
