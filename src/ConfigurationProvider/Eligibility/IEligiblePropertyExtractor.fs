namespace ConfigurationProvider.Eligibility

    open ConfigurationProvider.Models
    
    type IEligiblePropertyExtractor =
        abstract member Extract<'a> : unit -> ConfigurationPropertyRequest seq

