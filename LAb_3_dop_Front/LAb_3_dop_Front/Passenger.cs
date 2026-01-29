using System.Xml.Serialization;

namespace LAb_3_dop_Front
{
    [Serializable]
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

        public Passenger(string brand, string name, int power, int speed, string registrationNumber, string multimediaSystem, int airbagCount)
        : base(brand, name, power, speed, "Легковой", registrationNumber)
        {
            MultimediaSystem = multimediaSystem;
            AirbagCount = airbagCount;
        }

        public Passenger(string registrationNumber, string multimediaSystem, int airbagCount)
            : base("", "", 0, 0, "Легковой", registrationNumber)
        {
            MultimediaSystem = multimediaSystem;
            AirbagCount = airbagCount;
        }
    }
}