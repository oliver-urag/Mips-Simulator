using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class Memory
    {
        String _binString = "00000000";
        String _hexAddress = "0000";

        public String Address { get; set; }


        public String HexValue 
        {
            get
            {
                var conversionOut = Convert.ToString(Convert.ToInt64(_binString,2), 16).ToUpper();
                while (conversionOut.Length < 2)
                {
                    conversionOut = "0" + conversionOut;
                }
                return conversionOut;
            }
            set
            {
                var hexVal = value;
                if (Regex.IsMatch(hexVal, @"^[a-fA-F0-9]+$") && hexVal.Length == 2)
                {
                    _binString = Convert.ToString(Convert.ToInt64(hexVal, 16), 2);
                    while (_binString.Length < 8)
                    {
                        _binString = "0" + _binString;
                    }
                }
                else
                {
                    throw new DataConversionException("String", "HEX");
                }
            }
        }

        public String BinValue 
        {
            get
            {
                return _binString;
            }
            
            set
            {
                var tempBinString = value;
                if (Regex.IsMatch(tempBinString, @"^[0-1]+$") && tempBinString.Length == 64)
                {
                    _binString = tempBinString;
                }
                else
                {
                    throw new DataConversionException("String", "BIN");
                }
            }
        }

        public Memory(int number)
        {
            var hexString = Convert.ToString(number, 16).ToUpper();
            while (hexString.Length < 4)
            {
                hexString = "0" + hexString;
            }
            Address = hexString;
        }
    }
}
