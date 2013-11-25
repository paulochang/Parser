using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Infix2PostfixHelper
    {
        Stack<char> tempStack;
        string output;
        string alphabet;
        int tempPos;

        private bool isOperator(char tempo)
        {
            return ("äþ".IndexOf(tempo) != -1);
        }

        private bool isSymbol(char tempo)
        {
            return ("ýû[]+?äþÿ".IndexOf(tempo) != -1);
        }

        private string addConcats(string s)
        {
            string processedString = "";

            for (int i = 0; i < s.Length; i++)
            {
                char c1 = s[i];

                if (i + 1 < s.Length)
                {
                    char c2 = s[i + 1];

                    processedString += c1;

                    if (!(c1.Equals('ý') || c2.Equals('û') || isOperator(c2) || c1.Equals('ä')))
                    {
                        processedString += 'ÿ';
                    }
                }
            }

            processedString += s[s.Length - 1];

            return processedString;
        }

        private string getAlphabet(string tempo)
        {
            string result = "";
            foreach (char c in tempo)
            {
                if (!isSymbol(c) && result.IndexOf(c) == -1)
                    result += c;
            }
            return result;
        }

        private int jer(char tempo)
        {
            return "ýäÿþ".IndexOf(tempo);
        }

        private void operador(char tempo)
        {
            while ((tempStack.Count() != 0) && (jer(tempStack.Peek()) >= jer(tempo)))
            {
                output += tempStack.Pop().ToString();
            }
            tempStack.Push(tempo);
        }

        public string converter(string s)
        {
            tempStack = new Stack<char>();
            output = "";
            tempPos = 0;

            s = addConcats(s);

            alphabet = getAlphabet(s);

            while (tempPos < s.Length)
            {
                switch (s[tempPos])
                {
                    case 'ä':
                    case 'ÿ':
                    case 'þ':
                        operador(s[tempPos]);
                        tempPos++;
                        break;
                    case 'ý':
                        tempStack.Push(s[tempPos]);
                        tempPos++;
                        break;
                    case 'û':
                        while ((tempStack.Count != 0) && (tempStack.Peek() != 'ý'))
                        {
                            output += tempStack.Pop();
                        }
                        tempStack.Pop();
                        tempPos++;
                        break;
                    default:
                        output += s[tempPos];
                        tempPos++;
                        break;
                }
            }

            while (tempStack.Count != 0)
            {
                output += tempStack.Pop();
            }

            return output;
        }
    }
}
