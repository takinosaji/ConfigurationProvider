namespace ConfigurationProvider.PropertyProvider

    open System
    
    open ConfigurationProvider.Models
    open ConfigurationProvider.PropertyProvider

    exception ConfigurationPropertyNotFoundException of string
    
    type EnvironmentVariablePropertyProvider =
        interface IAsyncConfigurationPropertyProvider with
            member x.GetPropertiesAsync propertyRequests = async {
                return seq {
                    for request in propertyRequests do
                        match Environment.GetEnvironmentVariable request.Alias with
                            | null ->
                                raise (ConfigurationPropertyNotFoundException
                                           (sprintf "Environment variable %s was not found" request.Alias))
                            | value ->                              
                                yield ConfigurationProperty (request.MemberInfo, value)
                    }
                }
       
                    

