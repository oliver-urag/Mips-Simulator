using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class OpcodeGenerationException:Exception
    {
        public GenerationErrorSet ErrorSet { get; private set; }

        public OpcodeGenerationException(GenerationErrorSet errorSet)
            :base("Opcode Generation encountred errors")
        {
            ErrorSet = errorSet;
        }
    }
}
