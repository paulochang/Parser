using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class ThompsonHelper
    {
        private Stack<char> expressionStack;

        int nodeName = 0;

        private int getName()
        {
            return nodeName++;
        }

        private Graph doAutomaton(Graph start, Graph end)
        {
            char tempchar;
            tempchar = expressionStack.Pop();
            switch (tempchar)
            {
                case 'ä': // suppose s|t
                    {
                        Graph NsStart = new Graph();
                        Graph NsEnd = new Graph();
                        Graph NtStart = new Graph();
                        Graph NtEnd = new Graph();

                        NsStart.name = getName(); //name N(s) automaton
                        NsEnd.name = getName();
                        NtStart.name = getName(); //name N(t) automaton
                        NtEnd.name = getName();

                        doAutomaton(NtStart, NtEnd); //generate N(t) automaton
                        doAutomaton(NsStart, NsEnd); //generate N(s) automaton

                        start.Add('ü', NsStart); //add N(s) as destiny from start
                        start.Add('ü', NtStart); //add N(t) as destiny from start

                        NsEnd.Add('ü', end); //add ending node to N(s)
                        NtEnd.Add('ü', end); //add ending node to N(t)
                    }
                    break;

                case 'ÿ':
                    {
                        Graph middleNode = new Graph();

                        middleNode.name = getName();

                        doAutomaton(middleNode, end); //join N(t) with ending node
                        doAutomaton(start, middleNode); //join N(t) with starting node
                    }
                    break;

                case 'þ':
                    {
                        Graph NsStart = new Graph();
                        Graph NsEnd = new Graph();

                        NsStart.name = getName();
                        NsEnd.name = getName();

                        doAutomaton(NsStart, NsEnd);

                        start.Add('ü', NsStart);
                        NsEnd.Add('ü', NsStart);
                        NsEnd.Add('ü', end);
                        start.Add('ü', end);
                    }
                    break;

                default:
                    {
                        start.Add(tempchar, end);
                    }
                    break;
            }

            return start;
        }

        public Graph getAutomaton(string expression, out Graph finalGraph)
        {
            Graph Start = new Graph();
            Graph End = new Graph();

            Start.name = getName();
            End.isFinalState = true;

            expressionStack = new Stack<char>();

            foreach (char c in expression)
            {
                expressionStack.Push(c);
            }

            Start = doAutomaton(Start, End);

            End.name = getName();

            finalGraph = End;

            return Start;
        }
    }
}
