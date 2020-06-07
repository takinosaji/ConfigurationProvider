module ConfigurationProvider.Converter

    open System
    open System.ComponentModel
    
    type String with
        member str.Convert (targetType: Type) =
            match str with 
                | null when targetType = typeof<string> ->
                    raise (NotSupportedException "Conversion of null to empty string is overriden")
                | _ ->
                    let converter = TypeDescriptor.GetConverter targetType
                    converter.ConvertFromString str
        member str.Convert<'a>() =
            str.Convert typeof<'a> :?> 'a
            
        member str.TryConvert (targetType: Type) =
            match str with 
                | null when targetType.IsValueType ->
                    (false, Activator.CreateInstance(targetType))
                | _ ->
                    try
                        let converter = TypeDescriptor.GetConverter targetType                    
                        (true, converter.ConvertFromString str)
                    with e ->
                        (false, null)
                        
        member str.TryConvert<'a> (targetType: Type) =
            let conversionResult = str.TryConvert typeof<'a>
            (fst conversionResult, snd conversionResult :?> 'a)
                