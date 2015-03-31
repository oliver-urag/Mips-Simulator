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
        private bool _freezeFetch;
        private List<String> _lockedRegisters;
        private List<String> _registersToUnlock;
        public List<Instruction> Instructions {get; private set; }
        public List<Register> Registers {get; private set; }
        public List<Memory> Memory {get; private set; }

        public Dictionary<String, String> Pipeline { get; private set; }

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
            _lockedRegisters = new List<String>();
            _registersToUnlock = new List<String>();
            Pipeline = new Dictionary<String, String>();
            Instructions = instructions;
            Registers = registers;
            Memory = memory;
            PC = "0000";
            IF_ID_NPC = "0000";
            lastInstructionOpcode = instructions.Last().OpcodeHex;
        }

        public void Execute()
        {
            Pipeline = new Dictionary<String, String>();

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

            ClearRegisters();
        }

        private void ExecuteIF()
        {
            if (_freezeFetch)
            {
                return;
            }

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
            Pipeline.Add(IF_ID_IR, "IF");
        }

        private void ExecuteID()
        {
            ID_EX_IR = IF_ID_IR;

            if (IF_ID_IR != null)
            {
                var instruction = Instructions.Where(i => i.OpcodeHex == ID_EX_IR).FirstOrDefault();

                var aReg = "R" + Utils.ConvertBinToInt(instruction.Opcode.Substring(6, 5)).ToString();
                var bReg = "R" + Utils.ConvertBinToInt(instruction.Opcode.Substring(11, 5)).ToString();

                if(IsLocked(aReg) || IsLocked(bReg))
                {
                    ID_EX_IR = null;
                    _freezeFetch = true;
                    return;
                }

                _freezeFetch = false;

                ID_EX_A = Registers.Where(r => r.Id == aReg).FirstOrDefault().HexValue;
                ID_EX_B = Registers.Where(r => r.Id == bReg).FirstOrDefault().HexValue;
                ID_EX_IMM = Utils.ConvertBinToHex(instruction.Opcode.Substring(16, 16), 16).ToString();

                switch (instruction.Command)
                {
                    case "ORI":
                    case "ANDI":
                    case "DADDIU":
                        LockRegister("R" + Utils.ConvertBinToInt(instruction.Opcode.Substring(11, 5)).ToString());
                        break;
                }

                Pipeline.Add(IF_ID_IR, "ID");              
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
                String aluOutput = String.Empty;
                int? cond = null;
                ALU(instruction.Command, ID_EX_A, ID_EX_B, ID_EX_IMM, out aluOutput, out cond);
                EX_MEM_ALUOutput = aluOutput;
                EX_MEM_COND = cond;
                Pipeline.Add(EX_MEM_IR, "EX");                            
            }
            else
            {
                EX_MEM_IR = null;
                EX_MEM_ALUOutput = null;
                EX_MEM_COND = null;
            }
        }

        private void ExecuteMEM()
        {
            MEM_WB_IR = EX_MEM_IR;

            if (MEM_WB_IR != null)
            {
                var instruction = Instructions.Where(i => i.OpcodeHex == MEM_WB_IR).FirstOrDefault();
                MEM_WB_ALUOutput = EX_MEM_ALUOutput;
                Pipeline.Add(MEM_WB_IR, "MEM");
            }
            else
            {
                MEM_WB_IR = null;
                MEM_WB_ALUOutput = null;
                MEM_WB_LMD = null;
            }
        }

        private void ExecuteWB()
        {
            var WB_IR = MEM_WB_IR;

            if (WB_IR != null)
            {
                var instruction = Instructions.Where(i => i.OpcodeHex == WB_IR).FirstOrDefault();
                var outRegisterId = String.Empty;
                var outRegisterValue = String.Empty;
                switch (instruction.Command)
                {
                    case "DADDIU":
                    case "ANDI"  :
                        outRegisterId = "R" + Convert.ToInt64(instruction.Opcode.Substring(11, 5), 2).ToString();
                        outRegisterValue = MEM_WB_ALUOutput;
                        break;
                    case "LW" :
                    case "LWU":
                        outRegisterId = "R" + Convert.ToInt64(instruction.Opcode.Substring(11, 5), 2).ToString();
                        outRegisterValue = MEM_WB_LMD;
                        break;
                    case "DADDU":
                    case "OR"   :
                    case "DSLLV":
                    case "SLT"  :
                        outRegisterId = "R" + Convert.ToInt64(instruction.Opcode.Substring(16, 5), 2).ToString();
                        outRegisterValue = MEM_WB_ALUOutput;
                        break;
                    case "DMULT":
                        break;
                    case "J"  :
                    case "BNE":
                    case "SW" :
                        break;
                }

                if (outRegisterId != "R0")
                {
                    var outRegister = Registers.Where(r => r.Id == outRegisterId).FirstOrDefault();
                    if (outRegister != null)
                    {
                        outRegister.HexValue = outRegisterValue;
                        UnlockRegister(outRegisterId);
                    }
                }

                Pipeline.Add(WB_IR, "WB");
            }      
        }

        private void ALU(String command, String a, String b, String imm, out String aluOutput, out int? cond)
        {
            String tempALUOut = String.Empty;
            cond = 0;
            switch (command)
            {
                case "DADDU":
                    tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) + Convert.ToInt64(b, 16), 16);
                    break;
                case "DMULT":
                    tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) * Convert.ToInt64(b, 16), 32);
                    break;
                case "OR":
                    tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) | Convert.ToInt64(b, 16), 16);
                    break;
                case "DSLLV":
                    var shift = Utils.ConvertHexToBin(b,32);
                    shift = shift.Substring(shift.Length - 6, 6);
                    tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) << Convert.ToInt32(shift, 2), 16);
                    break;
                case "SLT":
                    tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) < Convert.ToInt32(b, 16) ? 1 : 0, 16);
                    break;
                case "DADDIU":
                    tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) + Convert.ToInt64(imm, 16), 16);
                    break;
                case "ANDI":
                    tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) & Convert.ToInt64(imm, 16), 16);
                    break;
                case "BNE":
                    tempALUOut = Convert.ToString(Convert.ToInt64(ID_EX_NPC,16) + (Convert.ToInt64(imm, 16) << 2), 16);
                    break;
                case "J":
                    tempALUOut = Convert.ToString(Convert.ToInt64(imm, 16) << 2, 16);
                    cond = 1;
                    break;
                case "LW":
                case "LWU":
                case "SW": tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) + Convert.ToInt64(imm, 16), 16); 
                           break;
               
            }

            while (tempALUOut.Length < 16)
            {
                tempALUOut = "0" + tempALUOut;
            }

            aluOutput = tempALUOut.ToUpper();
        }

        #region Register Management

        public void LockRegister(String id)
        {
            if (id != "R0")
            {
                _lockedRegisters.Add(id);
            }
        }

        public void UnlockRegister(String id)
        {
            _registersToUnlock.Add(id) ;
        }

        public bool IsLocked(String id)
        {
            return _lockedRegisters.Contains(id);
        }

        public void ClearRegisters()
        {
            foreach (var reg in _registersToUnlock)
            {
                _lockedRegisters.Remove(reg);
            }

            _registersToUnlock.Clear();

            if (!_lockedRegisters.Any())
            {
                _freezeFetch = false;
            }
        }

        #endregion Register Management

        #region Clock Cycles

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

        #endregion Clock Cycles

    }
}
