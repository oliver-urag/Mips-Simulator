using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class GenerationErrorSet
    {
        public List<GenerationError> Errors { get; set; }
        public bool HasError 
        {
            get
            {
                return Errors.Any();
            }
        }

        public GenerationErrorSet()
        {
            Errors = new List<GenerationError>();
        }

        public void Add(int codeLine, String message, String codeString)
        {
            Errors.Add(new GenerationError
            {
                CodeLine = codeLine,
                Message = message,
                CodeString = codeString
            });
        }
    }
    public class GenerationError
    {
        public int CodeLine { get; set; }
        public String Message { get; set; }
        public String CodeString { get; set; }
    }
}
