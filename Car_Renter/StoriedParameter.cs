using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renter
{
   public class StoriedParameter
    {

        public enum Validtion { Error, Ok, Edit }
        public static bool Isint(string input)
        {
            int test = 0;
            return int.TryParse(input, out test);
        }

        public static bool Isdouble(string input)
        {
            double test = 0;
            return double.TryParse(input, out test);
        }
        public static bool IsDate(string input)
        {
            DateTime test ;
            return DateTime.TryParse(input, out test);
        }
    }
}
