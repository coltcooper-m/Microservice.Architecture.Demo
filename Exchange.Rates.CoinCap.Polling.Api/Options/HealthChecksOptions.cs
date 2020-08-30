using System.Diagnostics.CodeAnalysis;

namespace Exchange.Rates.CoinCap.Polling.Api.Options
{
    [ExcludeFromCodeCoverage]
    public class HealthChecksOptions
    {
        public string PrivateMemoryName { get; set; }
      
        public int PrivateMemoryMax { get; set; } = 1073741824;

        public string ProcessAllocatedMemoryName { get; set; }

        public int ProcessAllocatedMemoryMax { get; set; } = 1073741824;
    }
}
