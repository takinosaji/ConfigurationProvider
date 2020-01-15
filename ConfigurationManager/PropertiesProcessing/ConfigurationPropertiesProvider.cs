using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigurationManager.FileProcessing;
using ConfigurationManager.Models;
using ConfigurationManager.TextProcessing;

[assembly:InternalsVisibleTo("ConfigurationManager.Tests")]
namespace ConfigurationManager.PropertiesProcessing
{
    public interface IConfigurationPropertiesProvider
    {
        Task<IEnumerable<ConfigurationProperty>> GetAsync<T>();
    }

    public class FileConfigurationPropertiesProvider : IConfigurationPropertiesProvider
    {
        private readonly IFileHierarchyManager _fileManager;
        private readonly ITypeMembersProvider _typeMembersProvider;
        private readonly IFilePropertyParsingFacade _filePropertyParsingFacade;
        private readonly IPropertyValidationFacade _propertyValidationFacade;

        public FileConfigurationPropertiesProvider(
            IFileHierarchyManager fileManager,
            ITypeMembersProvider typeMembersProvider,
            IFilePropertyParsingFacade filePropertyParsingFacade,
            IPropertyValidationFacade propertyValidationFacade)
        {
            _fileManager = fileManager;
            _typeMembersProvider = typeMembersProvider;
            _filePropertyParsingFacade = filePropertyParsingFacade;
            _propertyValidationFacade = propertyValidationFacade;
        }

        public async Task<IEnumerable<ConfigurationProperty>> GetAsync<T>()
        {
            var filesQueue = _fileManager.GetFilesQueue();

            var propertiesDictionary = new Dictionary<string, ConfigurationProperty>();

            while (filesQueue.TryDequeue(out string file))
            {
                await foreach (var property in GetPropertiesIterator<T>(file))
                {
                    propertiesDictionary[property.PropertyName] = property;
                }
            }

            return propertiesDictionary.Select(p => p.Value);
        }

        internal IAsyncEnumerable<ConfigurationProperty> GetPropertiesIterator<T>(string fileName)
        {
            var propertiesFromFile = _filePropertyParsingFacade.GetParsedPropertiesFromFile(fileName);
            var propertiesForType = GetPropertiesForTypeIterator<T>(propertiesFromFile);
            var validatedProperties = _propertyValidationFacade.Validate<T>(propertiesForType);
            return validatedProperties;
        }

        internal async IAsyncEnumerable<ConfigurationTypeProperty> GetPropertiesForTypeIterator<T>(
            IAsyncEnumerable<ConfigurationProperty> properties)
        {
            var eligibleMembers = _typeMembersProvider.GetEligibleMembers<T>();
            await foreach (var property in properties)
            {
                var member = eligibleMembers.FirstOrDefault(m => m.Name == property.FullName
                                                                 || m.Name.EndsWith($".{property.FullName}"));

                if (member == null)
                    continue;

                var configurationTypeProperty = new ConfigurationTypeProperty
                {
                    Namespace = property.Namespace,
                    PropertyName = property.PropertyName,
                    ClassName = property.ClassName,
                    Value = property.Value,
                    Type = member.Type
                };

                yield return configurationTypeProperty;
            }
        }
    }
}