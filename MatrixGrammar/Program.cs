using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Logging;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using MatrixGrammar.MatrixLearning;

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
            Int32[,] inp = new Int32[2, 2];
            var input = State.Create(grammar.InputSymbol, inp);
            var output = (string)ast.Invoke(input);
        }
    }
}
