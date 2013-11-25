using System;
using System.Collections.Generic;

using state = System.Int32;
using input = System.Char;

namespace Parser
{
    class Graph
    {
        public bool isFinalState;
        public int name;
        public Dictionary<char, HashSet<Graph>> trajectories;

        public void Add(char theChar, Graph theGraph)
        {
            HashSet<Graph> newList;
            if (trajectories.TryGetValue(theChar, out newList))
            {
                newList.Add(theGraph);
            }
            else
            {
                newList = new HashSet<Graph>();
                newList.Add(theGraph);
                trajectories.Add(theChar, newList);
            }
        }

        public override string ToString()
        {
            return name.ToString();
        }

        public Graph()
        {
            trajectories = new Dictionary<char, HashSet<Graph>>();
        }
    }
}
