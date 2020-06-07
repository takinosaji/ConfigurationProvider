namespace ConfigurationProvider.Composition
 
    open System
    
    open ConfigurationProvider.Converter
    open ConfigurationProvider.Composition

    type ActivatorObjectComposer =
        interface IConfigurationObjectComposer with
            member x.Compose configurationProperties =    
                let instance = Activator.CreateInstance<'a>()
                for configProperty in configurationProperties do
                    if configProperty.PropertyValue = null then
                        raise (NotSupportedException
                                   (sprintf "Composition object properties from nulls is not allowed.\t %s is null"
                                        configProperty.MemberInfo.Name))
                    let memberType = configProperty.MemberInfo.GetMemberType()
                    let convertedValue = configProperty.PropertyValue.Convert memberType
                    configProperty.MemberInfo.SetValue(instance, convertedValue)
                instance

