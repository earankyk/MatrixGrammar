using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;

namespace MatrixGrammar.MatrixLearning
{
    public class MatrixRanking : Feature<double>
    {
        public MatrixRanking(Grammar grammar) : base(grammar, "MatrixFeature") {}

        protected override double GetFeatureValueForVariable(VariableNode variable) => 0;

		// Your ranking functions here
        [FeatureCalculator("matPlus")]
		double ScoreRegexPosition(double inScore, double rrScore) => rrScore * inScore;

    }
}
