using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.IDE
{
    public class MemoryTemplate
    {
        public String Address { get; set; }
        public String HexVal { get; set; }

        public MemoryTemplate(int i)
        {
            Address = (i*4).ToString();
            HexVal = "00";
        }
    }
}
