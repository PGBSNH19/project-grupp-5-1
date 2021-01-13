using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Weather
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}