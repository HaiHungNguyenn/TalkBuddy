using AutoMapper;
using TalkBuddy.DAL.Implementations;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Service.AutoMappings;
using TalkBuddy.Service.Implementations;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
        //var config = new MapperConfiguration(AutoMapperConfiguration.RegisterMaps);
        //var mapper = config.CreateMapper();
        //services.AddSingleton(mapper);
        return services;
    }
}