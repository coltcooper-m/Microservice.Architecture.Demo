﻿using System.Diagnostics.CodeAnalysis;

namespace Exchange.Rates.Ecb.Polling.Api.Options
{
    [ExcludeFromCodeCoverage]
    public class MassTransitOptions
    {
        public string Host { get; set; }

        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string ReceiveEndpointName { get; set; }
        
        public int ReceiveEndpointPrefetchCount { get; set; } = 16;
    }
}
