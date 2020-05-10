using System;
using System.IO;
using System.Text;
using smtsharp.Expressions;
using smtsharp.Expressions.Logic;
using smtsharp.Expressions.Types;
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
                    default:
                        byte[] buffer = Encoding.UTF8.GetBytes(line);
                        using (var stream = new MemoryStream(buffer))
                        {
                            Parser p = new Parser(new Scanner(stream));
                            p.GetDeclaredVariable = formula.GetDeclaration;
                            p.Declare = (type, name) =>
                            {
                                switch (type)
                                {
                                    case Bool b:
                                        return formula.Declare(b, name);
                                    case FixedSizeBitVector bv:
                                        return formula.Declare(bv, name);
                                    case FloatingPoint fp:
                                        return formula.Declare(fp, name);
                                    default:
                                        return formula.Declare(type, name);
                                }
                            };
                            p.Assert = assertion => formula.Add(assertion);

                            // try
                            // {
                                p.Parse();
                            // }
                            // catch (Exception e)
                            // {
                                
                            // }

                            Console.WriteLine($"Errors: {p.errors.count}");
                            Console.WriteLine("Declarations:");
                            foreach (IVariable<Type> variable in formula.Declarations)
                            {
                                Console.WriteLine(variable);
                            }
                        }

                        break;
                }
            }
        }
    }
}