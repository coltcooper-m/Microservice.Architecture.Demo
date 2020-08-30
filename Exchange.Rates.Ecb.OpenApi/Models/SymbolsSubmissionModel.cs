using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Exchange.Rates.Ecb.OpenApi.Models
{
    public sealed class SymbolsSubmissionModel
    {
        /// <summary>
        /// Currency symbols list. Ex: EUR,CHF,CZK
        /// </summary>
        [Required]
        [FromQuery(Name = "Symbols")]
        public string Symbols { get; set; }
    }
}
