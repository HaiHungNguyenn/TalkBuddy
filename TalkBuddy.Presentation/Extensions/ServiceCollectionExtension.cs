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
        services.AddScoped<IClientChatBoxService, ClientChatBoxService>();
        services.AddScoped<IChatBoxService, ChatBoxService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IFriendShipRepository, FriendShipRepository>();
        services.AddScoped<IChatBoxRepository, ChatBoxRepository>();
        services.AddScoped<IFriendShipService, FriendShipService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
        services.AddScoped<IFacebookOAuthService, FacebookOAuthService>();
        services.AddScoped<INotificationRepository,NotificationRepository>();
        services.AddScoped<INotificationService,NotificationService>();
        var config = new MapperConfiguration(AutoMapperConfiguration.RegisterMaps);
        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
        services.AddScoped<IBlobService,BlobService>();
        //var config = new MapperConfiguration(AutoMapperConfiguration.RegisterMaps);
        //var mapper = config.CreateMapper();
        //services.AddSingleton(mapper);
        return services;
    }
}
