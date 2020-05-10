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
            public Eq(string name, IExpression<T> x, IExpression<T> y) : base(name, x, y)
            {
            }

            public Eq(IExpression<T> x, IExpression<T> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} == {Y})";
        }

        public class Lt<T> : CompareExpression<T> where T : NumericType
        {
            public Lt(string name, IExpression<T> x, IExpression<T> y) : base(name, x, y)
            {
            }

            public Lt(IExpression<T> x, IExpression<T> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} < {Y})";
        }
        
        public class ULt : CompareExpression<FixedSizeBitVector>
        {
            public ULt(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(name, x, y)
            {
            }

            public ULt(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} |<| {Y})";
        }
    }
}