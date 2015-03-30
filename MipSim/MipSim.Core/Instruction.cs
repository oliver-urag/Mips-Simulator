using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class Instruction
    {
        public int Address { get; private set; }
        public String InstructionString { get; set; }
        public String Label { get;  set; }
        public String Command { get; set; }
        public String Opcode { get; set; }
        
        public String OpcodeHex
        {
            get
            {
                var conversionOut = Convert.ToString(Convert.ToInt64(Opcode, 2), 16).ToUpper();
                while (conversionOut.Length < 8)
                {
                    conversionOut = "0" + conversionOut;
                }
                return conversionOut;
            }
        }

        public String OpcodeFormatted { get; set; }

        public String AddressHex
        {
            get
            {
                var hex = Convert.ToString(Address, 16);
                while (hex.Length < 4)
                {
                    hex = "0" + hex;
                }

                return hex;
            }
        }

        public Instruction(int address, string instructionString, string label)
        {
            Address = address;
            InstructionString = instructionString;
            Label = label;
            Opcode = "00000000000000000000000000000000";
        }

        public bool IsBranch()
        {
            return Command == "BNE" || Command == "J";
        }
    }
}
