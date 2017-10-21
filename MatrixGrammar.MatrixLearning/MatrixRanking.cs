using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;

namespace MatrixGrammar.MatrixLearning
{
    public class MatrixRanking : Feature<double>
    {
        public MatrixRanking(Grammar grammar) : base(grammar, "MatrixFeature") {}

        protected override double GetFeatureValueForVariable(VariableNode variable) => 0;

        public static Int32 AbsSumArray(Int32[] arr)
        {
            int result = 0;
            for (int i = 0; i < arr.Length;i++){
                result += Math.Abs(arr[i]);
            }
            return result;
        }

        // Your ranking functions here
        [FeatureCalculator("matPlus")]
		double ScoreRegexPosition(double inScore, double rrScore) => rrScore * inScore;

        [FeatureCalculator("A", Method = CalculationMethod.FromLiteral)]
        double KScore(int[] k) => 1.0 / (AbsSumArray(k));
    }
}
