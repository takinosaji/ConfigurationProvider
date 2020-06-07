namespace ConfigurationProvider

    open ConfigurationProvider.Composition
    open ConfigurationProvider.Eligibility
    open ConfigurationProvider.PropertyProvider
    
    type DefaultAsyncConfigProvider(
                                    propertyExtractor: IEligiblePropertyExtractor,
                                    provider: IAsyncConfigurationPropertyProvider,
                                    composer: IConfigurationObjectComposer
                                    ) =
        
        interface IAsyncConfigurationProvider with
            member x.GetAsync<'a>() = async {
                let eligiblePropertyRequests = propertyExtractor.Extract<'a>()
                let! configurationProperties = provider.GetPropertiesAsync eligiblePropertyRequests
                return composer.Compose<'a> configurationProperties
            }
