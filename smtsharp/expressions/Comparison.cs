using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    namespace Comparison
    {
        public abstract class CompareExpression<T> : Expression<Bool> where T : Type
        {
            public IExpression<T> X { get; }
            public IExpression<T> Y { get; }

            protected CompareExpression(string name, IExpression<T> x, IExpression<T> y) : base(Bool.Type, name)
            {
                X = x;
                Y = y;
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {X, Y};
        }

        public class Eq<T> : CompareExpression<T> where T : Type
        {
            public Eq(string name, IExpression<T> x, IExpression<T> y) :
                base(name, x, y)
            {
            }

            public Eq(IExpression<T> x, IExpression<T> y) :
                base(null, x, y)
            {
            }
        }

        public class Lt<T> : CompareExpression<T> where T : NumericType
        {
            public Lt(string name, IExpression<T> x, IExpression<T> y) :
                base(name, x, y)
            {
            }

            public Lt(IExpression<T> x, IExpression<T> y) :
                base(null, x, y)
            {
            }
        }

        public class Gt<T> : CompareExpression<T> where T : NumericType
        {
            public Gt(string name, IExpression<T> x, IExpression<T> y) :
                base(name, x, y)
            {
            }

            public Gt(IExpression<T> x, IExpression<T> y) :
                base(null, x, y)
            {
            }
        }
    }
}