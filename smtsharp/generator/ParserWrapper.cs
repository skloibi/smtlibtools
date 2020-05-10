using System;
using System.IO;
using System.Text;
using smtsharp.Expressions;
using smtsharp.Expressions.Types;

namespace smtsharp.Generator
{
    /// <summary>
    /// Simple wrapper for generated parser instances which allows multiple parsing steps for a
    /// single formula. This is currently performed by initializing a new scanner and parser instance
    /// for each new request and either takes direct input or reads a target file. 
    /// </summary>
    public class ParserWrapper
    {
        private readonly Formula _formula;

        public ParserWrapper(Formula formula)
        {
            _formula = formula;
        }

        private static Parser.Declaration GetDeclarationAction(Formula formula)
        {
            return (type, name) =>
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
        }

        private void InvokeParser(Scanner scanner)
        {
            // TODO: stream seems to be capped when mapped to the scanner's buffer - 
            // TODO: find a way to actually reuse the same parser instance
            Parser p = new Parser(scanner);
            p.GetDeclaredVariable = _formula.GetDeclaration;
            p.Declare = GetDeclarationAction(_formula);
            p.Assert = assertion => _formula.Add(assertion);
            p.Parse();
            if (p.errors.count > 0)
                Console.WriteLine($"Errors: {p.errors.count}");
        }

        public void ParseExpression(string expression)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(expression);
            using (var stream = new MemoryStream(buffer))
            {
                InvokeParser(new Scanner(stream));
            }
        }

        public void ParseFile(string file)
        {
            InvokeParser(new Scanner(file));
        }
    }
}