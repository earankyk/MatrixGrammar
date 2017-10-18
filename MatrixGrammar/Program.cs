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
            var ast = ProgramNode.Parse("matPlus(matPlus(M, M), M))", grammar, ASTSerializationFormat.HumanReadable);
            Int32[][] inp = new Int32[2][];
            inp[0] = new Int32[2] {1, 2};
            inp[1] = new Int32[2] {1, 2};
            var input = State.Create(grammar.InputSymbol, inp);
            Int32[][] opt = new Int32[2][];
            opt[0] = new Int32[2] { 2, 4 };
            opt[1] = new Int32[2] { 2, 4 };
            var spec = new ExampleSpec(new Dictionary<State, object> { [input] = opt });
            var engine = new SynthesisEngine(grammar);
            ProgramSet learned = engine.LearnGrammar(spec);
            Console.Write(learned.IsEmpty);
            Int32[][] output = (Int32[][]) ast.Invoke(input);
            Console.Write(output);
        }
    }
}
