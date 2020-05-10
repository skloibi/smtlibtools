using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    namespace Conditional
    {
        public class Ite<T> : Expression<T> where T : Type
        {
            public IExpression<Bool> Condition { get; }
            public IExpression<T> TrueExpr { get; }
            public IExpression<T> FalseExpr { get; }

            public Ite(string name, IExpression<Bool> condition, IExpression<T> trueExpr, IExpression<T> falseExpr) : 
                base(trueExpr.Type, name)
            {
                TrueExpr = trueExpr;
                FalseExpr = falseExpr;
                Condition = condition;
            }
            
            public Ite(IExpression<Bool> condition, IExpression<T> trueExpr, IExpression<T> falseExpr) : 
                this(null, condition, trueExpr, falseExpr)
            {
            }

            public override IExpression<Type>[] Operands() => 
                new IExpression<Type>[] { Condition, TrueExpr, FalseExpr };
            
            public override string ToString() => $@"({Condition} ? {TrueExpr} : {FalseExpr})";
        }
        
    }
}