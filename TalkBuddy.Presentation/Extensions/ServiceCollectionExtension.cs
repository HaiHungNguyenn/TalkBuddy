using AutoMapper;
using TalkBuddy.Service.AutoMappings;

namespace TalkBuddy.Presentation.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        var config = new MapperConfiguration(AutoMapperConfiguration.RegisterMaps);
        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }
}