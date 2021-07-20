using AutoMapper;
using TSound.Web.MappingConfiguration;

namespace TSound.Services.Tests.Infrastructure
{
    public class AutoMapperFactory
    {
        public static IMapper Create()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperProfile());
            });

            return mappingConfig.CreateMapper();
        }
    }
}
