module ConfigurationProvider.Models

    open System.Reflection
    
    type ConfigurationPropertyRequest(
                                         memberInfo: MemberInfo,
                                         alias: string
                                     ) =
        member x.MemberInfo = memberInfo
        member x.Alias = alias


    
    type ConfigurationProperty(
                                memberInfo: MemberInfo,
                                propertyValue: string
                              ) =
        member x.MemberInfo = memberInfo
        member x.PropertyValue = propertyValue