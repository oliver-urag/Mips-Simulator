using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class CPU
    {
        private bool _branch;
        private bool _dataHazard;
        private bool _controlHazard;

        private List<String> _lockedRegisters;
        private List<String> _registersToUnlock;
        public List<Instruction> Instructions {get; private set; }
        public List<Register> Registers {get; private set; }
        public List<Memory> Memory {get; private set; }

        public Dictionary<String, String> Pipeline { get; private set; }

        private Instruction IF_ID_Inst;
        private Instruction ID_EX_Inst;
        private Instruction EX_MEM_Inst;
        private Instruction MEM_WB_Inst;

        private Instruction old_EX_MEM_Inst { get; set; }
        private string old_EX_MEM_B { get; set; }
        private string old_EX_MEM_ALUOutput { get; set; }
        private int? old_EX_MEM_COND { get; set; }

        public string PC { get; set; }
        public string IF_ID_NPC {get; set; }
        public string IF_ID_IR { get; set; }
        public string ID_EX_NPC { get; set; }
        public string ID_EX_IR { get; set; }
        public string ID_EX_A { get; set; }
        public string ID_EX_B { get; set; }
        public string ID_EX_IMM { get; set; }
        public string EX_MEM_IR { get; set; }
        public string EX_MEM_B { get; set; }
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
        }

        public void Execute()
        {
            Pipeline = new Dictionary<String, String>();

            WB(); 
            MEM(); 
            EX(); 
            ID(); 
            IF();

            ClearRegisters();

            if (_branch)
            {
                _controlHazard = true;
            }
            else
            {
                _controlHazard = false;
            }
        }

        void WB()
        {
            if(MEM_WB_Inst == null)
            {
                return;
            }

            var instruction = MEM_WB_Inst;
                
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
                    var hiReg = Registers.Where(r=>r.Id == "HI").FirstOrDefault();
                    var loReg = Registers.Where(r=>r.Id == "LO").FirstOrDefault();
                    hiReg.HexValue = MEM_WB_ALUOutput.Substring(0, 16);
                    loReg.HexValue = MEM_WB_ALUOutput.Substring(16, 16);
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

            Pipeline.Add(instruction.AddressHex, "WB");
        }

        void MEM()
        {
            MEM_WB_Inst = null;
            MEM_WB_IR = null;
            MEM_WB_LMD = null;
            MEM_WB_ALUOutput = null;

            if(EX_MEM_Inst == null)
            {
                return;
            }

            MEM_WB_Inst = EX_MEM_Inst;
            MEM_WB_IR = MEM_WB_Inst.OpcodeHex;
            switch (MEM_WB_Inst.Command)
            {
                case "LW":
                    MEM_WB_LMD = Load(EX_MEM_ALUOutput, 8, true);
                    break;
                case "LWU":
                    MEM_WB_LMD = Load(EX_MEM_ALUOutput, 8, false);
                    break;
                case "SW":
                    Store(EX_MEM_ALUOutput, EX_MEM_B, 8);
                    break;
                case "BNE":
                case "J":
                    if (EX_MEM_Inst != null)
                    {
                        var ex_mem_instruction = EX_MEM_Inst;

                        if (ex_mem_instruction.IsBranch() && EX_MEM_COND == 1)
                        {
                            PC = EX_MEM_ALUOutput.Substring(EX_MEM_ALUOutput.Length - 4, 4);
                            IF_ID_NPC = PC;
                            IF_ID_Inst = null;
                            IF_ID_IR = null;
                        }
                    }
                    _branch = false;
                    break;
                default:
                    MEM_WB_ALUOutput = EX_MEM_ALUOutput;
                    break;
            }

            Pipeline.Add(MEM_WB_Inst.AddressHex, "MEM");   
        }

        void EX()
        {
            old_EX_MEM_Inst = EX_MEM_Inst;
            old_EX_MEM_ALUOutput = EX_MEM_ALUOutput;
            old_EX_MEM_B = EX_MEM_B;
            old_EX_MEM_COND = EX_MEM_COND;

            EX_MEM_Inst = null;
            EX_MEM_IR = null;
            EX_MEM_ALUOutput = null;
            EX_MEM_B = null;
            EX_MEM_COND = null;

            if (ID_EX_Inst == null)
            {
                return;
            }

            String aluOutput = String.Empty;
            int? cond = null;

            EX_MEM_Inst = ID_EX_Inst;
            EX_MEM_IR = ID_EX_Inst.OpcodeHex;
            ALU(EX_MEM_Inst.Command, ID_EX_A, ID_EX_B, ID_EX_IMM, out aluOutput, out cond);
            EX_MEM_ALUOutput = aluOutput;
            EX_MEM_COND = cond;
            EX_MEM_B = ID_EX_B;
            Pipeline.Add(EX_MEM_IR, "EX");       

            Pipeline.Add(EX_MEM_Inst.AddressHex, "EX");   
        }

        void ID()
        {
            ID_EX_Inst = null;
            ID_EX_IR = null;
            ID_EX_A = null;
            ID_EX_B = null;
            ID_EX_IMM = null;
            ID_EX_NPC = null;

            if (IF_ID_Inst == null)
            {
                return;
            }
            // if previous instruction is branch don't allow 
            if (_controlHazard)
            {
                return;
            }

            // Check Branch
            if (IF_ID_Inst.IsBranch())
            {
                _branch = true;
            }

            var aReg = "R" + Utils.ConvertBinToInt(IF_ID_Inst.Opcode.Substring(6, 5)).ToString();
            var bReg = "R" + Utils.ConvertBinToInt(IF_ID_Inst.Opcode.Substring(11, 5)).ToString();

            if (IsLocked(aReg) || IsLocked(bReg))
            {
                _dataHazard = true;
                return;
            }

            _dataHazard = false;
            
            ID_EX_Inst = IF_ID_Inst;
            ID_EX_IR = ID_EX_Inst.OpcodeHex;
            ID_EX_A = Registers.Where(r => r.Id == aReg).FirstOrDefault().HexValue;
            ID_EX_B = Registers.Where(r => r.Id == bReg).FirstOrDefault().HexValue;
            ID_EX_IMM = Utils.ConvertBinToHex(ID_EX_Inst.Opcode.Substring(16, 16), 16).ToString();
            ID_EX_NPC = IF_ID_NPC;

            switch (ID_EX_Inst.Command)
            {
                case "ORI":
                case "ANDI":
                case "DADDIU":
                    LockRegister("R" + Utils.ConvertBinToInt(ID_EX_Inst.Opcode.Substring(11, 5)).ToString());
                    break;
                case "DADDU":
                case "DSLLV":
                case "SLT":
                    LockRegister("R" + Utils.ConvertBinToInt(ID_EX_Inst.Opcode.Substring(16, 5)).ToString());
                    break;
            }

            Pipeline.Add(ID_EX_Inst.AddressHex, "ID");
        }

        void IF()
        {
            if (_dataHazard || _controlHazard)
            {
                return;
            }

            var instruction = Instructions.Where(i => i.AddressHex == PC).FirstOrDefault();

            if (instruction == null)
            {
                PC = null;
                IF_ID_NPC = null;
                IF_ID_Inst = null;
                IF_ID_IR = null;
                return;
            }



            PC = Utils.AddHex(PC, "0004", 4);
            IF_ID_NPC = PC;
            IF_ID_Inst = instruction;
            IF_ID_IR = IF_ID_Inst.OpcodeHex;
            Pipeline.Add(IF_ID_Inst.AddressHex, "IF");

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
                    tempALUOut = Convert.ToString(Convert.ToInt64(a, 16) * Convert.ToInt64(b, 16), 16);
                    while (tempALUOut.Length < 32)
                    {
                        tempALUOut = "0" + tempALUOut;
                    }
                    aluOutput = tempALUOut.ToUpper();
                    return;
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
                    tempALUOut = Convert.ToString(Convert.ToInt64(ID_EX_NPC, 16) + (Convert.ToInt64(imm, 16) << 2), 16);
                    cond = a != b ? 1 : 0;
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
        }

        #endregion Register Management

        #region Memory Access

        private void Store(String address, String hexValue, int hexDigitCount)
        {
            address = address.Substring(12, 4);
            hexValue = hexValue.Substring((hexValue.Length - hexDigitCount), hexDigitCount);
            var numericAddress = Convert.ToInt32(address, 16);
            var bytes = Enumerable.Range(0, hexValue.Length / 2).Select(i => hexValue.Substring(i * 2, 2));
            for(int i = 0; i <bytes.Count(); i++)
            {
                var addressToWrite = Utils.ConvertIntToHex(numericAddress + i, 4);
                var mem = Memory.Where(m => m.Address == addressToWrite).FirstOrDefault();
                if (mem == null)
                {
                    throw new Exception("Memory out of bounds");
                }
                mem.HexValue = bytes.ElementAt((bytes.Count() - 1) - i);
            }
        }

        private String Load(String address, int hexDigitCount, bool signExtend)
        {
            var retValBin = String.Empty;
            var extension = "0";
            var byteCount = hexDigitCount / 2;
            address = address.Substring(12, 4);
            var numericAddress = Convert.ToInt32(address, 16);

            for (int i = 0; i < byteCount; i++)
            {
                var addressToRead  = Utils.ConvertIntToHex(numericAddress + i, 4);
                var mem = Memory.Where(m => m.Address == addressToRead).FirstOrDefault();
                if (mem == null)
                {
                    throw new Exception("Memory out of bounds");
                }
                
                retValBin = mem.BinValue + retValBin;
            }

            if (signExtend)
            {
                extension = retValBin.ElementAt(0).ToString();
            }

            while (retValBin.Length < 64)
            {
                retValBin = extension + retValBin;
            }

            return Utils.ConvertBinToHex(retValBin, 16);

        }

        #endregion

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
            clockcycle.EX_MEM_B = EX_MEM_B;
            clockcycle.EX_MEM_ALUOutput = EX_MEM_ALUOutput;
            clockcycle.EX_MEM_COND = EX_MEM_COND;
            clockcycle.MEM_WB_IR = MEM_WB_IR;
            clockcycle.MEM_WB_LMD = MEM_WB_LMD;
            clockcycle.MEM_WB_ALUOutput = MEM_WB_ALUOutput;
        }

        #endregion Clock Cycles


    }
}
