using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    static class LexerGenerationHelper
    {
        public static string getStringValue(string unprocessedString)
        {
            return unprocessedString.Replace("\"", "");
        }

        public static char getCharValue(string unprocessedString)
        {
            return unprocessedString.Replace("\'", "")[0];
        }

        public static HashSet<char> getSet(string theString)
        {
            HashSet<char> result = new HashSet<char>();

            foreach (char c in theString)
            {
                result.Add(c);
            }

            return result;
        }

        public static HashSet<char> getRangeSet(char initialChar, char finalChar)
        {
            HashSet<char> result = new HashSet<char>();

            for(char c=initialChar; c<=finalChar; c++)
            {
                result.Add(c);
            }

            return result;
        }

        public static string getRegExpExcept(int c)
        {
            string result = getEquivalent('('); // (
            for (int i = 32; i < 126; i++)
            {
                if (i != c)
                    result += ((char)i).ToString() + getEquivalent('|'); // char + |
            }
            if (126 != c)
                result += ((char)126).ToString() + getEquivalent(')'); // char + )
            else
                result += ((char)32).ToString() + getEquivalent(')'); // " " + )
            return result;
        }

        public static string getAny()
        {
            string result = getEquivalent('('); // (
            for (int i = 32; i <= 126; i++)
            {
                    result += ((char)i).ToString() + getEquivalent('|'); // char + |
            }
            result += getEquivalent(')'); // " " + )
            return result;
        }

        public static string getBigOr(List<char> chars)
        {
            string result = getEquivalent('(');

            for (int i = 0; i < chars.Count; i++)
            {
                if (i != chars.Count - 1)
                    result += chars[i].ToString() + getEquivalent('|');
                else
                    result += chars[i].ToString();
            }
            result += getEquivalent(')');
            return result;
        }

        public static string getBigOr(string chars)
        {
            string result = getEquivalent('(');

            for (int i = 0; i < chars.Length; i++)
            {
                if (i != chars.Length - 1)
                    result += chars[i].ToString() + getEquivalent('|');
                else
                    result += chars[i].ToString();
            }
            result += getEquivalent(')');
            return result;
        }

        public static string getEquivalent(char c)
        {
            switch (c)
            {
                case '3': return "ü";
                case '|': return "ä";
                case '.': return "ÿ";
                case '*': return "þ";
                case '(': return "ý";
                case ')': return "û";
            }
            return "";
        }
        public static string getSetRegExp(HashSet<char> set)
        {
            string result="";
            List<char> tmpList = new List<char>();
            foreach (char c in set)
            {
                tmpList.Add(c);
            }
            result = getBigOr(tmpList);
            return result;
        }
    }
}
