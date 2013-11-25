using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using state = System.Int32;
using input = System.Char;

namespace Parser
{
    class RegExpEvaluator
    {
        Infix2PostfixHelper infix2postfix;
        Graph automaton;
        DFA DfaAutomaton;

        ThompsonHelper automatonConstructor;
        subsetConstructor DfaConstructor;

        public RegExpEvaluator(string regexp)
        {
            infix2postfix = new Infix2PostfixHelper();
            automatonConstructor = new ThompsonHelper();
            DfaConstructor = new subsetConstructor();
            String expression = infix2postfix.converter(regexp);
            Graph final;
            automaton = automatonConstructor.getAutomaton(expression, out final);
            DfaConstructor = new subsetConstructor();
            DfaAutomaton = DfaConstructor.BuildDfa(automaton, final);
        }

        public bool Evaluate(string evaluatedString)
        {   
            return DfaAutomaton.simulate(evaluatedString);
        }
    }
}
