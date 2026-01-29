namespace Lab_3_Backend.Models
{
    public class Passenger : Cars
    {
        public string MultimediaSystem { get; set; } = string.Empty;
        public int AirbagCount { get; set; }

        public Passenger() : base()
        {
            Type = "Легковой";
        }

        public Passenger(string brand, string name, int power, int speed, string registrationNumber)
        : base(brand, name, power, speed, "Легковой", registrationNumber)
        {
        }

        public Passenger(string brand, string name, int power, int speed,
        string registrationNumber, string multimediaSystem, int airbagCount)
        : base(brand, name, power, speed, "Легковой", registrationNumber)
        {
            MultimediaSystem = multimediaSystem;
            AirbagCount = airbagCount;
        }
    }
}