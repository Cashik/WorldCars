using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCars
{
    public class CarInfoClass
    {
        public int id;
        public string body_type;
        public string car_make;
        public int promoter_id;
        public string description;
        public int max_speed;
        public int cost;
        public DateTime datetime;
        public float rating;
        public int transmission;
        public int drive_type;
        public string name;
        public byte[] image;

        public CarInfoClass()
        {
            id = -1;
            body_type = car_make = description = name = "";
        }
    }
}
