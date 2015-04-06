using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MipSim.Core
{
    public class InstructionParser
    {

        char[] comma = { ',' };
        char[] space = { ' ', '\t' };
        char[] openParen = { '(' };

        public void ParseInstruction(Instruction instruction, List<Instruction> instructionSet)
        {
            var newInstructionString = String.Empty;

            var instructionString = instruction.InstructionString.Trim();
            var instructionStringSplit = instructionString.Split(space, 2);

            if (instructionStringSplit.Count() != 2)
            {
                instructionStringSplit = new string[2];
                instructionStringSplit[0] = instructionString;
                instructionStringSplit[1] = String.Empty;
            }

            var command = instructionStringSplit[0].Trim().ToUpper();

            var parameter = instructionStringSplit[1].Trim();

            if (instruction.Label != null && instruction.Label != String.Empty)
            {
                if (instructionSet.Where(i => i.Label == instruction.Label && i.Address != instruction.Address).Any())
                {
                    throw new Exception("Label already used: " + instruction.Label);
                }
            }
            String[] parameters = null;

            Instruction labeledInstruction = null;
            string[] offsetItems = { };
            var imm = String.Empty;
            var sRS = String.Empty;
            var sRD = String.Empty;
            var sRT = String.Empty;
            var sIMM = String.Empty;
            var opcode = String.Empty;
            var label = String.Empty;
            var FUNC = String.Empty;
            var SHF = String.Empty;
            var RS = String.Empty;
            var RT = String.Empty;
            var RD = String.Empty;
            var IMM = String.Empty;

            switch (command)
            {
                case "J":
                    instruction.Command = "J";
                    labeledInstruction = instructionSet.Where(i => i.Label != String.Empty && i.Label == parameter).FirstOrDefault();
                    if (labeledInstruction == null)
                    {
                        throw new Exception(String.Format("Unknown Label : {0}", parameter));
                    }
                    opcode = Bin(2, 6);
                    label = Bin(labeledInstruction.Address, 26);
                    instruction.Opcode = String.Concat(opcode, label);
                    newInstructionString = String.Format("{0} {1}", command, parameter);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", parameter);
                    break;
                case "DADDU":
                    instruction.Command = "DADDU";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 3)
                    {
                        throw new Exception(String.Format("missing parameter(s), 3 required", parameter));
                    }

                    opcode = Bin(0,6);
                    FUNC = Bin(45,6);
                    SHF = Bin(0,5);
                    RS = Bin(ExtractRegister(parameters[1], out sRS),5);
                    RT = Bin(ExtractRegister(parameters[2], out sRT), 5);
                    RD = Bin(ExtractRegister(parameters[0], out sRD), 5);
                    instruction.Opcode = String.Concat(opcode, RS, RT, RD, SHF, FUNC);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RT, " ", RD, " ", SHF, " ", FUNC);
                    newInstructionString = String.Format("{0} {1}, {2}, {3}", command, sRD, sRS, sRT);
                    break;
                case "DMULT":
                    instruction.Command = "DMULT";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 2)
                    {
                        throw new Exception(String.Format("missing parameter(s), 2 required", parameter));
                    }

                    opcode = Bin(0,6);
                    FUNC = Bin(28,6);
                    SHF = Bin(0,5);
                    RS = Bin(ExtractRegister(parameters[0], out sRS), 5);
                    RT = Bin(ExtractRegister(parameters[1], out sRT), 5);
                    RD = Bin(0,5);
                    instruction.Opcode = String.Concat(opcode, RS, RT, RD, SHF, FUNC);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RT, " ", RD, " ", SHF, " ", FUNC);
                    newInstructionString = String.Format("{0} {1}, {2}", command, sRS, sRT);
                    break;
                case "OR":
                    instruction.Command = "OR";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 3)
                    {
                        throw new Exception(String.Format("missing parameter(s), 3 required", parameter));
                    }

                    opcode = Bin(0, 6);
                    FUNC = Bin(37, 6);
                    SHF = Bin(0, 5);
                    RS = Bin(ExtractRegister(parameters[1], out sRS), 5);
                    RT = Bin(ExtractRegister(parameters[2], out sRT), 5);
                    RD = Bin(ExtractRegister(parameters[0], out sRD), 5);
                    instruction.Opcode = String.Concat(opcode, RS, RT, RD, SHF, FUNC);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RT, " ", RD, " ", SHF, " ", FUNC);
                    newInstructionString = String.Format("{0} {1}, {2}, {3}", command, sRD, sRS, sRT);
                    break;
                case "DSLLV":
                    instruction.Command = "DSLLV";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 3)
                    {
                        throw new Exception(String.Format("missing parameter(s), 3 required", parameter));
                    }

                    opcode = Bin(0, 6);
                    FUNC = Bin(20, 6);
                    SHF = Bin(0, 5);
                    RS = Bin(ExtractRegister(parameters[1], out sRS), 5);
                    RT = Bin(ExtractRegister(parameters[2], out sRT), 5);
                    RD = Bin(ExtractRegister(parameters[0], out sRD), 5);
                    instruction.Opcode = String.Concat(opcode, RS, RT, RD, SHF, FUNC);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RT, " ", RD, " ", SHF, " ", FUNC);
                    newInstructionString = String.Format("{0} {1}, {2}, {3}", command, sRD, sRS, sRT);
                    break;
                case "SLT":
                    instruction.Command = "SLT";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 3)
                    {
                        throw new Exception(String.Format("missing parameter(s), 3 required", parameter));
                    }

                    opcode = Bin(0, 6);
                    FUNC = Bin(42, 6);
                    SHF = Bin(0, 5);
                    RS = Bin(ExtractRegister(parameters[1], out sRS), 5);
                    RT = Bin(ExtractRegister(parameters[2], out sRT), 5);
                    RD = Bin(ExtractRegister(parameters[0], out sRD), 5);
                    instruction.Opcode = String.Concat(opcode, RS, RT, RD, SHF, FUNC);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RT, " ", RD, " ", SHF, " ", FUNC);
                    newInstructionString = String.Format("{0} {1}, {2}, {3}", command, sRD, sRS, sRT);
                    break;
                case "BNE":
                    instruction.Command = "BNE";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 3)
                    {
                        throw new Exception(String.Format("missing parameter(s), 3 required", parameter));
                    }

                    opcode = Bin(5, 6);
                    RS = Bin(ExtractRegister(parameters[0], out sRS), 5);
                    RT = Bin(ExtractRegister(parameters[1], out sRT), 5);
                    labeledInstruction = instructionSet.Where(i => i.Label != String.Empty && i.Label == parameters[2]).FirstOrDefault();

                    var offset = (labeledInstruction.Address / 4 - instruction.Address / 4) - 1;
                    if (labeledInstruction == null)
                    {
                        throw new Exception(String.Format("Label not found: {0}", parameters[2]));
                    }

                    IMM = Bin(offset, 16);

                    instruction.Opcode = String.Concat(opcode, RS, RT, IMM);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RT, " ", IMM);
                    newInstructionString = String.Format("{0} {1}, {2}, {3}", command, sRT, sRS, parameters[2]);

                    break;
                case "LW":
                    instruction.Command = "LW";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 2)
                    {
                        throw new Exception(String.Format("missing parameter(s), 2 required", parameter));
                    }

                    opcode = Bin(35, 6);
                    RD = Bin(ExtractRegister(parameters[0], out sRD), 5);
                    offsetItems = GetOffsetItems(parameters[1]);
                    IMM = Bin(Convert.ToInt32(offsetItems[0], 16), 16);
                    RS = Bin(ExtractRegister(offsetItems[1], out sRS), 5);

                    instruction.Opcode = String.Concat(opcode, RS, RD, IMM);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RD, " ", IMM);
                    newInstructionString = String.Format("{0} {1}, {2}({3})", command, sRD, offsetItems[0], sRS);

                    break;
                case "LWU":
                    instruction.Command = "LWU";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 2)
                    {
                        throw new Exception(String.Format("missing parameter(s), 2 required", parameter));
                    }

                    opcode = Bin(39, 6);
                    RD = Bin(ExtractRegister(parameters[0], out sRD), 5);
                    offsetItems = GetOffsetItems(parameters[1]);
                    IMM = Bin(Convert.ToInt32(offsetItems[0], 16), 16);
                    RS = Bin(ExtractRegister(offsetItems[1], out sRS), 5);      

                    instruction.Opcode = String.Concat(opcode, RS, RD, IMM);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RD, " ", IMM);
                    newInstructionString = String.Format("{0} {1}, {2}({3})", command, sRD, offsetItems[0], sRS);
                    break;
                case "SW":
                    instruction.Command = "SW";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 2)
                    {
                        throw new Exception(String.Format("missing parameter(s), 2 required", parameter));
                    }

                    opcode = Bin(43, 6);
                    RT = Bin(ExtractRegister(parameters[0], out sRT), 5);
                    offsetItems = GetOffsetItems(parameters[1]);
                    IMM = Bin(Convert.ToInt32(offsetItems[0], 16), 16);
                    RS = Bin(ExtractRegister(offsetItems[1], out sRS), 5);   

                    instruction.Opcode = String.Concat(opcode, RS, RT, IMM);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RT, " ", IMM);
                    newInstructionString = String.Format("{0} {1}, {2}({3})", command, sRT, offsetItems[0], sRS);
                    break;
                case "DADDIU":
                    instruction.Command = "DADDIU";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 3)
                    {
                        throw new Exception(String.Format("missing parameter(s), 3 required", parameter));
                    }

                    opcode = Bin(25, 6);
                    RD = Bin(ExtractRegister(parameters[0], out sRD), 5);
                    IMM = Bin(Convert.ToInt32(ExtractImmediate(parameters[2], out imm), 16), 16);
                    RS = Bin(ExtractRegister(parameters[1], out sRS), 5);

                    instruction.Opcode = String.Concat(opcode, RS, RD, IMM);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RD, " ", IMM);
                    newInstructionString = String.Format("{0} {1}, {2}, {3}", command, sRD, sRS, imm);
                    break;
                case "ANDI":
                    instruction.Command = "ANDI";
                    parameters = SplitByComma(parameter);
                    if (parameters.Count() != 3)
                    {
                        throw new Exception(String.Format("missing parameter(s), 3 required", parameter));
                    }

                    opcode = Bin(12, 6);
                    RD = Bin(ExtractRegister(parameters[0], out sRD), 5);
                    IMM = Bin(Convert.ToInt32(ExtractImmediate(parameters[2], out imm), 16), 16);
                    RS = Bin(ExtractRegister(parameters[1], out sRS), 5);   


                    instruction.Opcode = String.Concat(opcode, RS, RD, IMM);
                    instruction.OpcodeFormatted = String.Concat(opcode, " ", RS, " ", RD, " ", IMM);
                    newInstructionString = String.Format("{0} {1}, {2}, {3}", command, sRD, sRS, imm);
                    break;
                default: throw new Exception(String.Format("Unknown Instruction : {0}", command));
            }

            instruction.InstructionString = newInstructionString;
        }

        private String[] SplitByComma(string parameterString)
        {
            return parameterString.Split(comma, StringSplitOptions.RemoveEmptyEntries).Select(s=>s.Trim()).ToArray();
        }

        private String[] GetOffsetItems(string parameterString)
        {
            var offsetItems= new string[2];
            var items = parameterString.Split(openParen).Select(s => s.Trim()).ToArray();
            if (!Regex.IsMatch(items[0], @"^[a-fA-F0-9]+$") || !(items[0].Length == 4))
            {
                throw new Exception("Invalid Offset");
            }

            offsetItems[0] = items[0];

            if (!items[1].EndsWith(")"))
            {
                throw new Exception("Invalid Offset");
            }

            offsetItems[1] = items[1].Remove(items[1].Length - 1);
            return offsetItems;
        }


        private int ExtractRegister(String s, out String id)
        {
            int reg = 0;
            id = s.Trim().ToUpper();
            s = id.Replace("R", String.Empty);
            if (int.TryParse(s, out reg))
            {
                if(reg >= 0 && reg <= 31)
                {

                    return reg;
                }
            }
            throw new Exception("Invalid Register " + s);
        }

        private String ExtractImmediate(String imm, out String imm2)
        {
            imm = imm.Trim();
            imm2 = imm;
            if (imm.StartsWith("#") && imm.Length == 5)
            {
                imm = imm.Replace("#", "");
                if (Regex.IsMatch(imm, @"^[a-fA-F0-9]+$"))
                {
                    return imm;
                }
            }

            throw new Exception("Invalid Immediate " + imm);
        }

        private String Bin(Int64 num, int digits)
        {
            var binString = Convert.ToString(num, 2);
            while (binString.Length < digits)
            {
                binString = String.Format("0{0}", binString);
            }
            return binString;
        }
    }
}
