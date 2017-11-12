using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Specifications;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace MatrixGrammar.MatrixLearning
{
    public class MatrixLogic : DomainLearningLogic
    {
        int bound = 10;

        public static double[] matSub(double[] A, double[] B)
        {
            if(A.Length!=B.Length || A[0]!=B[0] || A[1]!=B[1]){
                return null;
            }
            double[] result = new double[A.Length];
            result[0] = A[0];
            result[1] = A[1];
            for (int i = 2; i < A.Length; i++)
            {
                result[i] = A[i] - B[i];
            }
            return result;
        }

        public static bool arrayCompLt(double[] M1, double[] M2)
        {
            for (int i = 0; i < M1.Length; i++)
            {
                if (M1[i] > 0 && M1[i] > M2[i])
                {
                    return false;
                }
                if (M1[i] < 0 && M1[i] < M2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static double[] removeDim(double[] A)
        {
            double rows = A[0];
            double cols = A[1];
            double[] result = new double[A.Length - 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = A[i + 2];
            }

            return result;
        }

        public static double[] convertTo1dArray(double[,] A)
        {
            int rows = A.GetLength(0);
            int cols = A.GetLength(1);
            double[] result = new double[rows * cols + 2];
            int index = 2;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[index] = A[i, j];
                    index++;
                }
            }
            result[0] = rows;
            result[1] = cols;
            return result;
        }

        public static double matMulScalarInv(double[] product, double[] factor)
        {
            int m1 = product.Length;
            int m2 = factor.Length;
            if (m1 != m2)
            {
                return double.MaxValue;
            }
            /*if (product == factor)
            {
                return new double[] { 1 };
            }*/
            double[] result = new double[m1];
            result[0] = product[0];
            result[1] = product[1];
            for (int i = 2; i < m1; i++)
            {
                if (factor[i] != 0.0)
                {
                    result[i] = product[i] / factor[i];
                }

            }
            bool same = true;
            for (int i = 3; i < m1; i++)
            {
                if (result[2] != result[i])
                    same = false;
            }
            if (same == true)
                return result[2];
            else
                return double.MaxValue;//new double[] { -1, -1 };
        }

        public static double[] matMulInv(double[] product, double[] factor)
        {
            double[] arr = removeDim(product);
            Matrix<double> prodMatrix = Matrix<double>.Build.Dense((int)product[0], (int)product[1], arr);
            Matrix<double> facMatrix = Matrix<double>.Build.Dense((int)factor[0], (int)factor[1], removeDim(factor));
            if (facMatrix.Determinant() == 0.0)
            {
                return null;
            }
            Matrix<double> facInv = facMatrix.Inverse();
            Matrix<double> factor2 = prodMatrix * facInv;

            double[] result = convertTo1dArray(factor2.Transpose().ToArray());
            /*for (int i = 0; i < result.Length; i++)
            {
                Console.Write(result[i]);
            }*/

            return result;
        }

        public MatrixLogic(Grammar grammar) : base(grammar) {}

        // Your custom learning logic here (for example, witness functions)
        [WitnessFunction("matPlus", 0)]
        DisjunctiveExamplesSpec WitnessPlus1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            var occurrences = new List<double[]>();
            foreach (var example in spec.DisjunctiveExamples)
            {
                State inputState = example.Key;
                double[] M1 = (double[])inputState[rule.Body[1]];
                // the first parameter of Substring is the variable symbol 'x'
                // we extract its current bound value from the given input state
                foreach (double[] M2 in example.Value)
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

        [WitnessFunction("matMultScalar", 0)]
        DisjunctiveExamplesSpec WitnessScalar1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            var occurrences = new List<double>();
            foreach (var example in spec.DisjunctiveExamples)
            {
                State inputState = example.Key;
                // the first parameter of Substring is the variable symbol 'x'
                // we extract its current bound value from the given input state
                double[] M1 = (double[])inputState[rule.Body[1]];
                foreach (double[] M2 in example.Value)
                {
                    double result1 = matMulScalarInv(M2, M1);
                    if(result1 == double.MaxValue){
                        return null;
                    }
                    occurrences.Add(result1);

                }
                result[inputState] = occurrences.Cast<object>();//returning n as [n] instead of int. 
            }
            return new DisjunctiveExamplesSpec(result);
        }

        [WitnessFunction("matMult", 0)]
        DisjunctiveExamplesSpec WitnessMult1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            if (bound == 0)
            {
                return null;
            }
            var result = new Dictionary<State, IEnumerable<object>>();
            var occurrences = new List<double[]>();
            foreach (var example in spec.DisjunctiveExamples)
            {
                State inputState = example.Key;
                // the first parameter of Substring is the variable symbol 'x'
                // we extract its current bound value from the given input state
                double[] M1 = (double[])inputState[rule.Body[1]];
                foreach (double[] M2 in example.Value)
                {
                    occurrences.Add(matMulInv(M2, M1));


                }
                result[inputState] = occurrences;
            }
            bound--;
            //Console.WriteLine(bound);
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
