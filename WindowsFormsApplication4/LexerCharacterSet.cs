using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class LexerCharacterSet
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

        private HashSet<char> pointedSet;

        public HashSet<char> PointedSet
        {
            get { return pointedSet; }
            set { pointedSet = value; }
        }

        public LexerCharacterSet(string id, string reg, HashSet<char> set)
        {
            Identifier = id;
            Regexp = reg;
            PointedSet = set;
        }
        public LexerCharacterSet()
        {
        }
    }
}
