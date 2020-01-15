﻿using System.Threading.Tasks;
using ConfigurationManager.ObjectCreation;
using ConfigurationManager.PropertiesProcessing;

namespace ConfigurationManager
{
    public interface IConfigurationProvider
    {
        Task<T> GetAsync<T>();
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfigurationPropertiesProvider _configurationPropertiesProvider;
        private readonly IObjectCreator _objectCreator;

        public ConfigurationProvider(
            IConfigurationPropertiesProvider configurationPropertiesProvider,
            IObjectCreator objectCreator)
        {
            _configurationPropertiesProvider = configurationPropertiesProvider;
            _objectCreator = objectCreator;
        }

        public async Task<T> GetAsync<T>()
        {
            var properties = await _configurationPropertiesProvider.GetAsync<T>();
            return _objectCreator.Create<T>(properties);
        }
    }
}