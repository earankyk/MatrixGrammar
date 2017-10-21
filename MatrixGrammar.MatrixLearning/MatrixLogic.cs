using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace MatrixGrammar.MatrixLearning
{
    public class MatrixLogic : DomainLearningLogic
    {
        public static Int32[] matSub(Int32[] A, Int32[] B)
        {
            int m1 = A.Length;
            int m2 = B.Length;
            if (m1 != m2)
            {
                return null;
            }
            int[] result = new int[m1];
            for (int i = 0; i < m1; i++)
            {
                result[i] = A[i] - B[i];
            }
            return result;
        }

        public static bool arrayCompLt(Int32[] M1, Int32[] M2)
        {
            for (int i = 0; i < M1.Length; i++)
            {
                if(M1[i] > M2[i]){
                    return false;
                }
            }
            return true;
        }

        public MatrixLogic(Grammar grammar) : base(grammar) {}

        // Your custom learning logic here (for example, witness functions)
        [WitnessFunction("matPlus", 0)]
        DisjunctiveExamplesSpec WitnessPlus1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            var occurrences = new List<Int32[]>();
            foreach (var example in spec.DisjunctiveExamples)
            {
                State inputState = example.Key;
                Int32[] M1 = (Int32[])inputState[rule.Body[1]];
                // the first parameter of Substring is the variable symbol 'x'
                // we extract its current bound value from the given input state
                foreach (Int32[] M2 in example.Value)
                {
                    if(Enumerable.SequenceEqual(M1, M2) || !arrayCompLt(M1, M2)){
                        return null;
                    }
                    occurrences.Add(matSub(M2, M1));
                }
                result[inputState] = occurrences;
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
