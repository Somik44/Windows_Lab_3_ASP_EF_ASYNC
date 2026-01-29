using System.Xml.Serialization;

namespace LAb_3_dop_Front
{
    [Serializable]
    [XmlInclude(typeof(Passenger))]
    [XmlInclude(typeof(Truck))]
    public abstract class Cars
    {
        public int Id { get; set; }

        public string Brand { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Power { get; set; }
        public int Speed { get; set; }

        public string Type { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;

        public Cars() { }

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