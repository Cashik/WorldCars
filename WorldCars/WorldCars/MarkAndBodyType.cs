using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCars
{
    class MarkAndBodyType
    {
        public int id;
        public string name;
        public MarkAndBodyType(){
            
        }
        public MarkAndBodyType(int _id,string _name)
        {
            id = _id;
            name = _name;
        }

    }
}
