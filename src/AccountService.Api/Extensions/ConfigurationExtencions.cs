using AccountService.Api.Exceptions;

namespace AccountService.Api.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfigurationSection section, string key)
    {
        var value = section.GetValue<T>(key);

        if (value is null || value is string s && string.IsNullOrWhiteSpace(s))
        {
            throw new EnvironmentConfigurationException(string.Format("'{0}:{1}' не найдено", section.Path, key));
        }

        return value;
    }

    public static T GetRequired<T>(this IConfigurationSection section) { 
        return section.Get<T>() ?? throw new EnvironmentConfigurationException($"Не удалось получить объект {nameof(T)}");
    }
}