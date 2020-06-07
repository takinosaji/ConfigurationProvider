namespace ConfigurationProvider

    type IAsyncConfigurationProvider =
        abstract member GetAsync<'a> : unit -> Async<'a>
