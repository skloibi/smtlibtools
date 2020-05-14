using System.Collections.Generic;
using smtsharp.Expressions;
using smtsharp.Expressions.Types;

namespace smtsharp.Util
{
    public class FormulaTraversal
    {
        public delegate void Action(IExpression<Type> expression);

        public Formula Formula { get; }


        public FormulaTraversal(Formula formula) =>
            Formula = formula;

        public void Apply(Action action)
        {
            ISet<IExpression<Type>> visited = new HashSet<IExpression<Type>>();

            foreach (var expr in Formula.Declarations) 
                Apply(action, expr, visited);
        }

        private static void Apply(Action action, IExpression<Type> expression, ISet<IExpression<Type>> visited)
        {
            if (!visited.Contains(expression))
            {
                action(expression);
                visited.Add(expression);
                foreach (var target in expression.Targets())
                    Apply(action, target, visited);
                foreach (var operand in expression.Operands())
                    Apply(action, operand, visited);
            }
        }
    }
}