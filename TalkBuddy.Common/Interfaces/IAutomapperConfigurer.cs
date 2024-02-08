using AutoMapper;

namespace TalkBuddy.Common.Interfaces;

public interface IAutomapperConfigurer
{
	void Configure(IMapperConfigurationExpression mapper);
}
