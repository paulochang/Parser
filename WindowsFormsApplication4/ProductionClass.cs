using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ProductionClass
    {
        String Id;
        int Position;
        List<SymbolClass> Expression;

        public ProductionClass()
        {

        }

        public ProductionClass(String theId, List<SymbolClass> theExpression)
        {
            Id = theId;
            Expression = theExpression;
        }
    }
}
