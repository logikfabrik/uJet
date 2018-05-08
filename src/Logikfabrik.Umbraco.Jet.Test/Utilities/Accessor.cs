using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    [Flags]
    public enum Accessor
    {
        Get = 1,

        Set = 2,

        GetSet = Get | Set
    }
}
