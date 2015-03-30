using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class Opcode
    {
        static string[] newLine = { "\n" };
        static char[] colon = { ':' };

        private InstructionParser parser { get; set; }

        public static List<Instruction> Generate(string code)
        {
            var instructionSet = new List<Instruction>();
            var errors = new GenerationErrorSet();
            var initialAddress = 0;

            var parser = new InstructionParser();
            var codeStrings = code.Replace("\r","").Split(newLine, StringSplitOptions.RemoveEmptyEntries);
            var ctr = 0;

            foreach (var codeString in codeStrings)
            {
                var labelSplit = codeString.Split(colon);
                
                if (labelSplit.Count() > 2)
                {
                    errors.Add(ctr + 1, "Too many labels", codeString);
                    ctr++;
                    continue;
                }

                var label = String.Empty;
                var instructionString = labelSplit[0];

                if(labelSplit.Count() == 2)
                {
                    label = labelSplit[0].Trim();
                    instructionString = labelSplit[1].Trim();
                    if (!IsAlphaNumeric(label) || IsReserved(label))
                    {
                        errors.Add(ctr + 1, "Invalid label", codeString);
                        ctr++;
                        continue;
                    }
                }
               
                var instruction = new Instruction(initialAddress + (ctr * 4), instructionString, label);
                instructionSet.Add(instruction);
                ctr++;
            }

            if (errors.HasError)
            {
                throw new OpcodeGenerationException(errors);
            }

            ctr = 0;
            foreach (var instruction in instructionSet)
            {
                try
                {
                    parser.ParseInstruction(instruction, instructionSet);
                }
                catch (Exception e)
                {
                    if (instruction.Label.Length > 0)
                    {
                        errors.Add(ctr, e.Message, String.Format("{0}:{1}", instruction.Label, instruction.InstructionString));
                    }
                    else
                    {
                        errors.Add(ctr, e.Message, String.Format("{0}", instruction.InstructionString));
                    }
                }
                ctr++;
            }

            if (errors.HasError)
            {
                throw new OpcodeGenerationException(errors);
            }

            return instructionSet;
        }

        static private bool IsAlphaNumeric(String s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z0-9]+$");
        }

        static private bool IsReserved(String s)
        {
            String[] commands = { "DADDU", "DMULT", "OR", "DSLLV", "SLT", "BNE", "LW", "LWU", "SW", "DADDIU", "ANDI", "J" };
            String[] regIds = {"R0","R1","R2","R3","R4", "R5","R6","R7","R8","R9","R10","R11","R12","R13","R14", "R15","R16","R17","R18","R9",
                               "R20","R21","R22","R23","R24", "R25","R26","R27","R28","R29","R30","R31"};

            if(commands.Any(c=> c == s.ToUpper()) || regIds.Any(r=>r == s.ToUpper()))
            {
                return true;
            }

            return false;
        }


    }
}
