using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.VersionSpace;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Logging;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using MatrixGrammar.MatrixLearning;
using System.Numerics;

namespace MatrixGrammar
{
    class Program
    {
        public static void Main(string[] args)
        {

            var parseResult = DSLCompiler.ParseGrammarFromFile("MatrixGrammar.grammar");
            parseResult.TraceDiagnostics();
            var grammar = parseResult.Value;
            Console.WriteLine(grammar.Name);
            //var ast = ProgramNode.Parse("matPlus(matPlus(M, M), M))", grammar, ASTSerializationFormat.HumanReadable);
            //double[] inp = new double[] { 2, 2, 1, 2, 3, 4 };
            System.Random random = new System.Random();
            double[] inp = new double[10002];
            double[] opt = new double[10002];
            inp[0] = 100;
            inp[1] = 100;
            opt[0] = 100;
            opt[1] = 100;
            for (int i = 2; i < 10002; i++)
            {
                double val = random.NextDouble();
                inp[i] = val; // NextDouble already returns double from [0,1)
                opt[i] = 2 * val;
            }
            //double[] opt = new double[] { 2, 2, 2, 4, 6, 8 };
            var input = State.Create(grammar.InputSymbol, inp);
            var spec = new ExampleSpec(new Dictionary<State, object> { [input] = opt });
            //var engine = new SynthesisEngine(grammar);
            var engine = new SynthesisEngine(grammar, new SynthesisEngine.Config
            {
                Strategies = new ISynthesisStrategy[]
    {
                    new DeductiveSynthesis(new MatrixLogic(grammar)),
    }
            });
            ProgramSet learned = engine.LearnGrammar(spec);
            Console.Write(learned.Size);
        }
    }
}
