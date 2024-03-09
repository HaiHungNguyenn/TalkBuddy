using AutoMapper;
using TalkBuddy.Common.Interfaces;
using TalkBuddy.Domain.Dtos;
using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.AutoMappings;

public static class AutoMapperConfiguration
{
    public static void RegisterMaps(IMapperConfigurationExpression mapper)
    {
        CreateNotificationMaps(mapper);
    }

    private static void CreateNotificationMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Notification, DtoNotification>();
    }
    private static void CreateClientMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Client, DtoClient>();
    }
}
