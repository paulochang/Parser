using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Token
    {
        TokenType type;
        String value;

        public Token(TokenType theType, String theValue)
        {
            type = theType;
            value = theValue;
        }

        public TokenType getTokenType()
        {
            return type;
        }

        public string getValue()
        {
            return value;
        }
    }
}
