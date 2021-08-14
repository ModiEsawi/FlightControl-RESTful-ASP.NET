using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Helpers
{
    //ID Generator class.
    public class IDGenerator
    {
        //return a random string of size (size)  , with an option to be in lowercase only .
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            { //random char in each iteration...
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /*
         * Given a datetime dd/mm/qwyy, and companyName , 
         * the function return a string of length 9 
         * where first two chars are the first two of letters of companyName
         * then yy , then 3 random letters , then dd, then mm
         */
        public static string GenerateFlightPlanId(DateTime dt, string companyName)
        {
            string id = "";
            if (companyName.Length == 0)
                id += "xx";
            else if (companyName.Length == 1)
                id += companyName + "x";
            else
                id += companyName.Substring(0, 2);
            double year = dt.Year / 100; // from 2019 for example to 19.
            id += Math.Floor(year);
            id += RandomString(3, true);
            id += dt.Day;
            id += dt.Month;
            return id;
        }
    }
}
