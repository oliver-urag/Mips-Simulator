using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class ClockCycle
    {
        public int Number { get; private set; }
        public string PC { get; set; }
        public string IF_ID_NPC { get; set; }
        public string IF_ID_IR { get; set; }
        public string ID_EX_NPC { get; set; }
        public string ID_EX_IR { get; set; }
        public string ID_EX_A { get; set; }
        public string ID_EX_B { get; set; }
        public string ID_EX_IMM { get; set; }
        public string EX_MEM_IR { get; set; }
        public string EX_MEM_B { get; set; }
        public string EX_MEM_ALUOutput { get; set; }
        public int? EX_MEM_COND { get; set; }
        public string MEM_WB_IR { get; set; }
        public string MEM_WB_LMD { get; set; }
        public string MEM_WB_ALUOutput { get; set; }

        public ClockCycle(int number)
        {
            Number = number;
        }

    }
}
