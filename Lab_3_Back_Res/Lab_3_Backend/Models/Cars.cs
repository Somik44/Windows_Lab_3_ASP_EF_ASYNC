using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_3_Backend.Models
{
    public class Cars
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Power { get; set; }

        [Required]
        public int Speed { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;


        public Cars()
        {
        }

        protected Cars(string brand, string name, int power, int speed, string type, string registrationNumber)
        {
            Brand = brand;
            Name = name;
            Power = power;
            Speed = speed;
            Type = type;
            RegistrationNumber = registrationNumber;
        }
    }
}