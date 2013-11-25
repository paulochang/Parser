using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class LexerKeyword
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

        public LexerKeyword(string id, string reg)
        {
            Identifier = id;
            Regexp = reg;
        }
        public LexerKeyword()
        {
        }
    }
}
