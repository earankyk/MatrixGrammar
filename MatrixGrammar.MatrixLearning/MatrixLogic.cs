using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Specifications;
using System.Collections.Generic;

namespace MatrixGrammar.MatrixLearning
{
    public class MatrixLogic : DomainLearningLogic
    {
        public static Int32[][] matSub(Int32[][] A, Int32[][] B)
        {
            int m1 = A.GetLength(0);
            int n1 = A[0].GetLength(0);
            int m2 = B.GetLength(0);
            int n2 = B[0].GetLength(0);
            if (m1 != m2 || n1 != n2)
            {
                return null;
            }
            int[][] result = new int[m1][];
            for (int i = 0; i < m1; i++)
            {
                result[i] = new int[n1];
                for (int j = 0; j < n1; j++)
                {
                    result[i][j] = A[i][j] - B[i][j];
                }
            }
            return result;
        }

        public MatrixLogic(Grammar grammar) : base(grammar) {}

        // Your custom learning logic here (for example, witness functions)
        [WitnessFunction("matPlus", 0)]
        DisjunctiveExamplesSpec WitnessPlus1(GrammarRule rule, ExampleSpec spec)
        {
            Console.Write("Here\n");
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.Examples)
            {
                State inputState = example.Key;
                // the first parameter of Substring is the variable symbol 'x'
                // we extract its current bound value from the given input state
                Int32[][] M1 = (Int32[][])inputState[rule.Body[1]];
                result[inputState] = matSub((Int32[][])example.Value, M1);
            }
            return new DisjunctiveExamplesSpec(result);
        }

        /*[WitnessFunction("matPlus", 1)]
        DisjunctiveExamplesSpec WitnessPlus2(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.Examples)
            {
                State inputState = example.Key;
                // the first parameter of Substring is the variable symbol 'x'
                // we extract its current bound value from the given input state
                Int32[][] M1 = (Int32[][])inputState[rule.Body[0]];
                result[inputState] = matSub((Int32[][])example.Value, M1);
            }
            return new DisjunctiveExamplesSpec(result);
        }*/
    }
}
