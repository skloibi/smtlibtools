using System;
using smtsharp.export;
using smtsharp.Expressions;
using smtsharp.Generator;
using smtsharp.Util;
using Type = smtsharp.Expressions.Types.Type;

namespace smtsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var formula = new Formula("SMTLIB");
            var formActions = new FormulaTraversal(formula);
            var parser = new ParserWrapper(formula);
            
            if (args.Length > 0)
            {
                parser.ParseFile(args[0]);
            }

            while (true)
            {
                var line = Console.ReadLine();

                switch (line)
                {
                    case ":exit":
                        return;
                    case ":reset":
                        formula = new Formula("SMTLIB");
                        formActions = new FormulaTraversal(formula);
                        parser = new ParserWrapper(formula);
                        break;
                    case ":show":
                        Console.WriteLine("================");
                        Console.WriteLine("Current formula:");
                        Console.WriteLine($"Nr. of expressions: {formula.Length})");
                        formActions.Apply(Console.WriteLine);
                        Console.WriteLine("================");
                        break;
                    case ":vars":
                        Console.WriteLine("================");
                        Console.WriteLine("Declarations:");
                        Console.WriteLine($"Nr. of variables: {formula.Declarations.Count})");
                        foreach (IVariable<Type> variable in formula.Declarations)
                            Console.WriteLine(variable);
                        Console.WriteLine("================");
                        break;
                    case ":load":
                        var file = Console.ReadLine();
                        Console.WriteLine($"Loading file '{file}'");
                        try
                        {
                            parser.ParseFile(file);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"ERROR: Could not load file: {e.Message}");
                        }

                        break;
                    case ":dot":
                        var dotOutput = Console.ReadLine();
                        Console.WriteLine($"Generating DOT file at '{dotOutput}'");
                        DOTGenerator.FormulaToDOT(formActions, dotOutput);
                        break;
                    default:
                        parser.ParseExpression(line);
                        break;
                }
            }
        }
    }
}