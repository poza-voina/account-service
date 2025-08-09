using AccountService.Api.Exceptions;

namespace AccountService.Api.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequired<T>(this IConfigurationSection section) { 
        return section.Get<T>() ?? throw new EnvironmentConfigurationException($"Не удалось получить объект {nameof(T)}");
    }
}