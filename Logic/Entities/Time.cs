using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public class Time
    {
        public static DateTime GlobalDate  {get; private set; }

        public Time()
        {
            GlobalDate = new DateTime();
        }


    }
}
