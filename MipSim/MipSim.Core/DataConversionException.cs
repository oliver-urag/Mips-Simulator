using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipSim.Core
{
    public class DataConversionException:Exception
    {
        public DataConversionException(String source, String target)
            : base(String.Format("Unable to convert from {0} to {1}", source, target))
        {

        }
    }
}
