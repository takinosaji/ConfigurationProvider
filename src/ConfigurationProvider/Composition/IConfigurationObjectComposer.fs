namespace ConfigurationProvider.Composition

    open ConfigurationProvider.Models
        
    type IConfigurationObjectComposer =
        abstract member Compose<'T> : ConfigurationProperty seq -> 'T