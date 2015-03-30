using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MipSim.Core;
using System.Data;
using System.Windows.Data;

namespace MipSim.IDE
{
    public class HexValidation:ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value as string;
            if (!Utils.IsHex(str))
            {
                return new ValidationResult(false, "invalid hex");
            }

            return new ValidationResult(true, null);
        }
    }
}
