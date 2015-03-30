using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class Utils
    {
        public static bool IsHex(String hex)        
        {
            return Regex.IsMatch(hex, @"^[a-fA-F0-9]+$");
        }

        public static string AddHex(string hex1, string hex2, int digits)
        {
            int sum = Convert.ToInt32(hex1, 16) + Convert.ToInt32(hex2, 16);
            string hex = Convert.ToString(sum, 16);

            if (hex.Length > digits)
            {
                throw new Exception("Value Overflow");
            }

            while(hex.Length < digits)
            {
                hex = "0" + hex;
            }

            return hex;
        }

        public static long ConvertBinToInt(string bin)
        {
            return Convert.ToInt32(bin, 2);
        }

        public static string ConvertHexToBin(string bin, int digits)
        {
            var conversionOut = Convert.ToString(Convert.ToInt64(bin, 2), 16).ToUpper();
            while (conversionOut.Length < 16)
            {
                conversionOut = "0" + conversionOut;
            }
            return conversionOut;
        }
    }
}
