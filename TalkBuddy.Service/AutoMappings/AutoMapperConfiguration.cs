using AutoMapper;
using TalkBuddy.Common.Interfaces;

namespace TalkBuddy.Service.AutoMappings;

public static class AutoMapperConfiguration
{
    public static void RegisterMaps(IMapperConfigurationExpression mapper)
    {
		var configurers = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i == typeof(IAutomapperConfigurer)));

		foreach (var configurer in configurers)
		{
			var instance = Activator.CreateInstance(configurer) as IAutomapperConfigurer;
			instance?.Configure(mapper);
		}
    }

    private static void CreateClientMaps(IMapperConfigurationExpression mapper)
    {
    }
}
