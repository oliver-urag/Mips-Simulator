using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.IDE
{
    public class RegisterTemplate
    {
        public String RegisterName { get; set; }
        public String HexVal { get; set; }

        public RegisterTemplate(int i)
        {
            RegisterName = "R" + i.ToString();
            HexVal = "00000000000000000";
        }
    }
}
