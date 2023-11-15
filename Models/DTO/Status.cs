using Newtonsoft.Json;   
using System.ComponentModel.DataAnnotations;

namespace MovieStore.Models.DTO
{
    public class Status
    {
        
        [Required]
        public int StatusCode { get; set; }
        [Required]
        public String Message { get; set; }
    }
}
