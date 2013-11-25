using System;
using System.Collections.Generic;
using System.Linq;

using state = System.Int32;
using input = System.Char;

namespace Parser
{
    class subsetConstructor
    {
        private int num = 1;

        private state getNewState()
        {
            return num++;
        }

        private HashSet<Graph> eClosure(HashSet<Graph> T)
        {
            HashSet<Graph> result = new HashSet<Graph>();
            Stack<Graph> theStack = new Stack<Graph>();

            foreach (Graph g in T)
            {
                result.Add(g);
                theStack.Push(g);
            }

            while (theStack.Count != 0)
            {
                Graph t = theStack.Pop();
                HashSet<Graph> theList;
                if (t.trajectories.TryGetValue('ü', out theList))
                {
                    foreach (Graph u in theList)
                    {
                        if (!result.Contains(u))
                        {
                            result.Add(u);
                            theStack.Push(u);
                        }

                    }
                }
            }

            return result;
        }

        private HashSet<Graph> eClosure(Graph T)
        {
            HashSet<Graph> result = new HashSet<Graph>();
            Stack<Graph> theStack = new Stack<Graph>();

            result.Add(T);

            theStack.Push(T);

            while (theStack.Count != 0)
            {
                Graph t = theStack.Pop();
                HashSet<Graph> theList;
                if (t.trajectories.TryGetValue('ü', out theList))
                {
                    foreach (Graph u in theList)
                    {
                        if (!result.Contains(u))
                        {
                            result.Add(u);
                            theStack.Push(u);
                        }

                    }
                }
            }

            return result;
        }

        private HashSet<Graph> Move(HashSet<Graph> nodes, char transitionChar)
        {
            HashSet<Graph> result = new HashSet<Graph>();
            foreach (Graph t in nodes)
            {
                HashSet<Graph> tempList = new HashSet<Graph>();
                if (t.trajectories.TryGetValue(transitionChar, out tempList))
                {
                    foreach (Graph g in tempList)
                        result.Add(g);
                }
            }
            return result;
        }

        private HashSet<Graph> Move(Graph node, char transitionChar)
        {
            HashSet<Graph> result = new HashSet<Graph>();

            HashSet<Graph> tempList = new HashSet<Graph>();
            if (node.trajectories.TryGetValue(transitionChar, out tempList))
            {
                foreach (Graph g in tempList)
                    result.Add(g);
            }
            return result;
        }

        private string getNames(HashSet<Graph> graphs)
        {
            List<state> result = new List<state>();
            foreach (Graph g in graphs)
            {
                result.Add(g.name);
            }
            result.Sort();

            string returnValue = "";
            foreach (state s in result)
            {
                returnValue += ("," + s.ToString());
            }
            return returnValue;
        }

        HashSet<char> getAlphabet(HashSet<Graph> GraphSet)
        {
            HashSet<char> alphabet = new HashSet<char>();
            foreach (Graph g in GraphSet)
            {
                Dictionary<char, HashSet<Graph>>.KeyCollection keyColl = g.trajectories.Keys;
                foreach (char c in keyColl)
                {
                    if ((c != 'ü') && (!alphabet.Contains(c)))
                        alphabet.Add(c);
                }
            }
            return alphabet;
        }

        public class SetComparer : IEqualityComparer<HashSet<Graph>>
        {

            public bool Equals(HashSet<Graph> x, HashSet<Graph> y)
            {
                return x.SetEquals(y);
            }

            public int GetHashCode(HashSet<Graph> obj)
            {
                return obj.GetHashCode();
            }
        }

        public DFA BuildDfa(Graph nfa, Graph final)
        {
            DFA dfa = new DFA();
            HashSet<HashSet<Graph>> markedStates = new HashSet<HashSet<Graph>>();
            HashSet<HashSet<Graph>> unMarkedStates = new HashSet<HashSet<Graph>>();

            Dictionary<string, state> dfaStateNum = new Dictionary<string, state>();

            HashSet<state> nfaInitial = new HashSet<state>();
            nfaInitial.Add(nfa.name);

            HashSet<Graph> first = eClosure(nfa);
            unMarkedStates.Add(first);

            state dfaInitial = getNewState();
            dfaStateNum.Add(getNames(first), dfaInitial);
            dfa.start = dfaInitial;

            SetComparer mySetComparer = new SetComparer();

            while (unMarkedStates.Count != 0)
            {
                HashSet<Graph> aState = unMarkedStates.ElementAt(0);
                unMarkedStates.Remove(aState);
                markedStates.Add(aState);

                if (aState.Contains(final))
                {
                    dfa.final.Add(dfaStateNum[getNames(aState)]);
                }
                IEnumerator<input> iE = getAlphabet(aState).GetEnumerator();

                while (iE.MoveNext())
                {
                    HashSet<Graph> next = eClosure(Move(aState, iE.Current));

                    if ((!unMarkedStates.Contains(next, mySetComparer)) && (!markedStates.Contains(next, mySetComparer)))
                    {
                        unMarkedStates.Add(next);
                        dfaStateNum.Add(getNames(next), getNewState());
                    }

                    string theStateSet = getNames(aState);
                    KeyValuePair<state, input> transition = new KeyValuePair<state, input>(dfaStateNum[theStateSet], iE.Current);

                    dfa.transitionTable[transition] = dfaStateNum[getNames(next)];

                }
            }
            return dfa;
        }
    }
}
