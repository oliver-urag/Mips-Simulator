using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class CPU
    {
        private string lastInstructionOpcode;
        private bool stopFetch;

        public List<Instruction> Instructions {get; private set; }
        public List<Register> Registers {get; private set; }
        public List<Memory> Memory {get; private set; }

        public string PC { get; set; }
        public string IF_ID_NPC {get; set; }
        public string IF_ID_IR { get; set; }
        public string ID_EX_NPC { get; set; }
        public string ID_EX_IR { get; set; }
        public string ID_EX_A { get; set; }
        public string ID_EX_B { get; set; }
        public string ID_EX_IMM { get; set; }
        public string EX_MEM_IR { get; set; }
        public string EX_MEM_ALUOutput{ get; set; }
        public int? EX_MEM_COND { get; set; }
        public string MEM_WB_IR{ get; set; }
        public string MEM_WB_LMD { get; set; }
        public string MEM_WB_ALUOutput { get; set; }


        public CPU(List<Instruction> instructions, List<Register> registers, List<Memory> memory)
        {
            Instructions = instructions;
            Registers = registers;
            Memory = memory;
            PC = "0000";
            IF_ID_NPC = "0000";
            lastInstructionOpcode = instructions.Last().OpcodeHex;
        }

        public void Execute()
        {
            //WB
            ExecuteWB();

            //MEM
            ExecuteMEM();
            
            //EX
            ExecuteEX();

            //ID
            ExecuteID();
            
            //IR
            ExecuteIF();
        }

        private void ExecuteIF()
        {
            var instruction = Instructions.Where(i => i.AddressHex == PC).FirstOrDefault();

            if (instruction == null)
            {
                IF_ID_IR = null;
                PC = null;
                IF_ID_NPC = null;
                return;
            }

            IF_ID_IR = instruction.OpcodeHex;

            if (EX_MEM_IR != null)
            {
                var ex_mem_instruction = Instructions.Where(i => i.OpcodeHex == EX_MEM_IR).FirstOrDefault();

                if (ex_mem_instruction.IsBranch() && EX_MEM_COND == 1)
                {
                    PC = EX_MEM_ALUOutput;
                    IF_ID_NPC = PC;
                    return;
                }
            }

            PC = Utils.AddHex(PC, "0004", 4);
            IF_ID_NPC = PC;
        }

        private void ExecuteID()
        {
            ID_EX_IR = IF_ID_IR;

            if (IF_ID_IR != null)
            {
                var instruction = Instructions.Where(i => i.OpcodeHex == ID_EX_IR).FirstOrDefault();
                var aReg = "R" + Utils.ConvertBinToInt(instruction.Opcode.Substring(5, 5)).ToString();
                var bReg = "R" + Utils.ConvertBinToInt(instruction.Opcode.Substring(10, 5)).ToString();
                ID_EX_A = Registers.Where(r => r.Id == aReg).FirstOrDefault().HexValue;
                ID_EX_B = Registers.Where(r => r.Id == bReg).FirstOrDefault().HexValue;
                ID_EX_IMM = Utils.ConvertHexToBin(instruction.Opcode.Substring(16, 16), 16).ToString();
            }
            else
            {
                ID_EX_A = null;
                ID_EX_B = null;
                ID_EX_IMM = null;
            }

        }

        private void ExecuteEX()
        {
            EX_MEM_IR = ID_EX_IR;

            if (EX_MEM_IR != null)
            {
                var instruction = Instructions.Where(i => i.OpcodeHex == EX_MEM_IR).FirstOrDefault();
                var tempALUOut = String.Empty;
                switch(instruction.Command)
                {
                    case "DADDIU":
                        tempALUOut = Convert.ToString(Convert.ToInt64(ID_EX_A, 16) + Convert.ToInt64(ID_EX_IMM, 16), 16);
                        EX_MEM_COND = 0;
                        break;
                    case "DADDU":
                        tempALUOut = Convert.ToString(Convert.ToInt64(ID_EX_A, 16) + Convert.ToInt64(ID_EX_B, 16), 16);
                        EX_MEM_COND = 0;
                        break;
                }

                while (tempALUOut.Length < 16)
                {
                    tempALUOut = "0" + tempALUOut;
                }

                EX_MEM_ALUOutput = tempALUOut;
                            
            }
            else
            {
                EX_MEM_IR = null;
                EX_MEM_ALUOutput = null;
            }
        }

        private void ExecuteMEM()
        {
            MEM_WB_IR = EX_MEM_IR;

            if (MEM_WB_IR != null)
            {
                var instruction = Instructions.Where(i => i.OpcodeHex == MEM_WB_IR).FirstOrDefault();
                MEM_WB_ALUOutput = EX_MEM_ALUOutput;
            }
            else
            {
                EX_MEM_IR = null;
                EX_MEM_ALUOutput = null;
            }
        }

        private void ExecuteWB()
        {
            var WB_IR = MEM_WB_IR;
            if ( WB_IR != null)
            {
                var instruction = Instructions.Where(i => i.OpcodeHex == WB_IR).FirstOrDefault();
                Register outREgister = null;
                switch (instruction.Command)
                {
                    case "DADDIU":
                        outREgister = Registers.Where(r=>r.Id == ("R" + Convert.ToInt64(instruction.Opcode.Substring(11, 5), 2).ToString())).FirstOrDefault();
                        outREgister.HexValue = MEM_WB_ALUOutput;
                        break;
                    case "DADDU":
                        outREgister = Registers.Where(r => r.Id == ("R" + Convert.ToInt64(instruction.Opcode.Substring(16, 5), 2).ToString())).FirstOrDefault();
                        outREgister.HexValue = MEM_WB_ALUOutput;
                        break;
                }
            }
        }

        public void CopyInternalRegisters(ClockCycle clockcycle)
        {
            clockcycle.PC = PC;
            clockcycle.IF_ID_NPC = IF_ID_NPC;
            clockcycle.IF_ID_IR = IF_ID_IR;
            clockcycle.ID_EX_NPC = ID_EX_NPC;
            clockcycle.ID_EX_IR = ID_EX_IR;
            clockcycle.ID_EX_A = ID_EX_A;
            clockcycle.ID_EX_B = ID_EX_B;
            clockcycle.ID_EX_IMM = ID_EX_IMM;
            clockcycle.EX_MEM_IR = EX_MEM_IR;
            clockcycle.EX_MEM_ALUOutput = EX_MEM_ALUOutput;
            clockcycle.EX_MEM_COND = EX_MEM_COND;
            clockcycle.MEM_WB_IR = MEM_WB_IR;
            clockcycle.MEM_WB_LMD = MEM_WB_LMD;
            clockcycle.MEM_WB_ALUOutput = MEM_WB_ALUOutput;
        }
    }
}
