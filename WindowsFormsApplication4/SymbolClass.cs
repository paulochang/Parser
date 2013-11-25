using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public enum SymbolType
    {
        Token,
        Production
    }
    public class SymbolClass
    {
        String Id;
        SymbolType type;
    }
}
