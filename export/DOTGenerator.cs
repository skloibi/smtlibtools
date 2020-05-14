using System;
using System.Collections.Generic;
using System.IO;
using smtsharp.Expressions;
using smtsharp.Expressions.Logic;
using smtsharp.Expressions.Types;
using smtsharp.Util;
using Type = smtsharp.Expressions.Types.Type;

namespace smtsharp.export
{
    public static class DOTGenerator
    {
        public static void FormulaToDOT(FormulaTraversal traversal, string file)
        {
            if (File.Exists(file))
                throw new IOException($"File {file} already exists");

            var path = Path.GetFullPath(file);
            var parent = Path.GetDirectoryName(path);
            // create parent directories
            Directory.CreateDirectory(parent);

            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine("digraph {0} {{", traversal.Formula.Name);

                IList<string> edges = new List<string>();

                void AddEdge(IExpression<Type> expression)
                {
                    // write a format string for this one node
                    writer.WriteLine(GenerateNodeFormat(expression));
                    foreach (var operand in expression.Operands())
                        // accumulate format string and edge from node to operands
                        edges.Add($"\"{expression.Id}\" -> \"{operand.Id}\";");
                }

                traversal.Apply(AddEdge);

                // write lines afterwards to ensure that all formats were defined
                foreach (var edge in edges)
                    writer.WriteLine(edge);

                writer.WriteLine("}");
            }
        }

        private static String GenerateNodeFormat(IExpression<Type> expression)
        {
            return string.Format("{0} [{1}]", expression.Id, expression switch
            {
                Const c => GenerateConstantNode(c, "yellow"),
                Expressions.BitVector.Const c => GenerateConstantNode(c, "aqua"),
                Expressions.FloatingPoint.Const c => GenerateConstantNode(c, "lawngreen"),
                IConstantExpression<Type, object> c => GenerateConstantNode(c, "darkgrey"),
                IVariable<Bool> v => GenerateVariableNode(v, "yellow"),
                IVariable<FixedSizeBitVector> v => GenerateVariableNode(v, "aqua"),
                IVariable<FloatingPoint> v => GenerateVariableNode(v, "lawngreen"),
                IVariable<Type> v => GenerateVariableNode(v, "darkgrey"),
                IExpression<Bool> e => GenerateGenericNode(e, "khaki"),
                IExpression<FixedSizeBitVector> e => GenerateGenericNode(e, "aquamarine"),
                IExpression<FloatingPoint> e => GenerateGenericNode(e, "greenyellow"),
                _ => GenerateGenericNode(expression, "greenyellow")
            });
        }

        private static string GenerateConstantNode<T>(IConstantExpression<Type, T> c, string fillcolor) =>
            $"style=filled shape=oval fillcolor={fillcolor} label=\"{c.Id}:{c.GetType().Name}{{{c.Value}}} [{c.Type}]\"";

        private static string GenerateVariableNode(IVariable<Type> v, string fillcolor) =>
            $"style=filled shape=diamond fillcolor={fillcolor} label=\"{v.Id}:{v.GetType().Name} [{v.Type}] {v.Name}\"";

        private static string GenerateGenericNode(IExpression<Type> e, string fillcolor) =>
            $"style=filled shape=box fillcolor={fillcolor} label=\"{e.Id}:{e.GetType().Name} [{e.Type}]\"";
    }
}