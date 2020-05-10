using System;
using smtsharp.Expressions;
using smtsharp.Generator;
using smtsharp.Util;
using Type = smtsharp.Expressions.Types.Type;

namespace smtsharp
{
    class Program
    {
        // private static void Print(Func<> a)
        // {
        //     Console.WriteLine("Declarations:");
        //     foreach (IVariable<Type> variable in formula.Declarations)
        //     {
        //         Console.WriteLine(variable);
        //     }
        // }

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
                    case ":check":
                        // TODO:
                        // Console.WriteLine($"; {solver.Check()}");
                        break;
                    case ":show":
                        Console.WriteLine("================\nCurrent formula:");
                        formActions.Apply(Console.WriteLine);
                        Console.WriteLine("================");
                        break;
                    case ":vars":
                        Console.WriteLine("================\nDeclarations:");
                        foreach (IVariable<Type> variable in formula.Declarations)
                            Console.WriteLine(variable);
                        Console.WriteLine("================");
                        break;
                    default:
                        parser.ParseExpression(line);
                        break;
                }
            }
        }
    }
}