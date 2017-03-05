using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pzlatarov.Yeelight.Exceptions
{
    public class FeatureNotSupportedException : Exception
    {
        public FeatureNotSupportedException(string message) : base(message)
        {
            
        }

        public FeatureNotSupportedException() : base("This feature is not supported by the light bulb.")
        {
        }
    }
}
