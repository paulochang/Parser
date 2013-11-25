using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class LexerToken
    {
        string identifier;

        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }
        string regexp;

        public string Regexp
        {
            get { return regexp; }
            set { regexp = value; }
        }

        public LexerToken(string id, string reg)
        {
            Identifier = id;
            Regexp = reg;
        }
        public LexerToken()
        {
        }
    }
}
