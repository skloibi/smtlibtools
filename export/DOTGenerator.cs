using System.Diagnostics;
using System.IO;
using smtsharp.Expressions;
using smtsharp.Expressions.Types;
using smtsharp.Util;

namespace smtsharp.export
{
    public static class DOTGenerator
    {
        public static void FormulaToDOT(FormulaTraversal traversal, string file)
        {
            if (File.Exists(file))
                throw new IOException($"File {file} already exists");

            var parent = Path.GetDirectoryName(file);
            // create parent directories
            Directory.CreateDirectory(parent);

            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine("digraph {0} {{", traversal.Formula.Name);

                void AddEdge(IExpression<Type> expression)
                {
                    foreach (var operand in expression.Operands())
                        writer.WriteLine("\"{0}\" -> \"{1}\";", expression.Id, operand.Id);
                }

                traversal.Apply(AddEdge);
                writer.WriteLine("}");
            }
        }
    }
}