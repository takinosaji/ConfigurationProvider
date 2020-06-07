namespace ConfigurationProvider.Eligibility

    open System
    open System.Reflection
    open System.Linq
    
    open ConfigurationProvider.Models
    
    type AliasAttribute(alias) =
        inherit Attribute()
        member x.Alias = alias
    
    type IReflectionBasedExtractor =
        abstract member GetUsedBindingFlags: unit -> BindingFlags
    
    type CustomAttributeMemberInfoExtractor() =
        let _flags = BindingFlags.Public ||| BindingFlags.Instance
        
        interface IEligiblePropertyExtractor with 
            member x.Extract<'a>() =        
                let targetType = typeof<'a>
                targetType
                    .GetProperties(_flags).Cast<MemberInfo>()
                    .Concat(targetType.GetFields(_flags).Cast<MemberInfo>())
                    .Where(fun p -> p.GetCustomAttribute<AliasAttribute>() <> Unchecked.defaultof<AliasAttribute>)
                    .Select(fun m -> ConfigurationPropertyRequest(
                        m, m.GetCustomAttribute<AliasAttribute>().Alias))
                    
        interface IReflectionBasedExtractor with
            member x.GetUsedBindingFlags() = _flags


