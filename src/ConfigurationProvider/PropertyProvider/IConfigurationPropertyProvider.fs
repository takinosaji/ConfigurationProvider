namespace ConfigurationProvider.PropertyProvider

    open ConfigurationProvider.Models
    
    type IAsyncConfigurationPropertyProvider =
        abstract member GetPropertiesAsync: ConfigurationPropertyRequest seq -> Async<ConfigurationProperty seq>