module ConfigurationProvider.MemberInfoExtensions

    open System
    open System.Reflection
    
    type MemberInfo with
        member x.SetValue (context: {| Target: Object; Value: Object |}) =
            match x.MemberType with
                | MemberTypes.Field ->
                    (x :?> FieldInfo).SetValue(context.Target, context.Value)
                | MemberTypes.Property ->
                    (x :?> PropertyInfo).SetValue(context.Target, context.Value, null)
                | _ ->
                    invalidArg "self" "MemberInfo must be of type FieldInfo or PropertyInfo"                    
            
        member x.GetMemberType() =
            match x.MemberType with
                | MemberTypes.Field -> (x :?> FieldInfo).FieldType
                | MemberTypes.Property -> (x :?> PropertyInfo).PropertyType
                | _ -> invalidArg "self" "MemberInfo must be of type FieldInfo or PropertyInfo" 