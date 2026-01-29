using System.Xml.Serialization;

namespace LAb_3_dop_Front
{
    [Serializable]
    public class Truck : Cars
    {
        public int WheelCount { get; set; }
        public int BodyVolume { get; set; }

        public Truck() : base()
        {
            Type = "Грузовой";
        }

        public Truck(string brand, string name, int power, int speed, string registrationNumber)
            : base(brand, name, power, speed, "Грузовой", registrationNumber)
        {
        }

        public Truck(string brand, string name, int power, int speed,
                    string registrationNumber, int wheelCount, int bodyVolume)
            : base(brand, name, power, speed, "Грузовой", registrationNumber)
        {
            WheelCount = wheelCount;
            BodyVolume = bodyVolume;
        }

        public Truck(string registrationNumber, int wheelCount, int bodyVolume)
            : base("", "", 0, 0, "Грузовой", registrationNumber)
        {
            WheelCount = wheelCount;
            BodyVolume = bodyVolume;
        }
    }
}