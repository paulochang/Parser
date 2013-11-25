using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class TokenDefinition
    {
        TokenType type;
        String regExp;
        private RegExpEvaluator matcher;

        public TokenDefinition(TokenType theType, String theRegExp)
        {
            type = theType;
            regExp = theRegExp;
            matcher = new RegExpEvaluator(theRegExp);
        }
        
        public bool evaluate(string theString)
        {
            return matcher.Evaluate(theString);
        }

        public TokenType getTokenType()
        {
            return type;
        }
    }
}
