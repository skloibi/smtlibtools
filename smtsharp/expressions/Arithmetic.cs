using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    namespace Arithmetic
    {
        public abstract class BinaryArithmeticExpression<T> : Expression<T> where T : NumericType
        {
            public IExpression<T> X { get; }
            public IExpression<T> Y { get; }

            protected BinaryArithmeticExpression(string name, IExpression<T> x, IExpression<T> y) :
                base(x.Type, name)
            {
                X = x;
                Y = y;
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {X, Y};
        }

        public class Neg<T> : Expression<T> where T : NumericType
        {
            public IExpression<T> Value { get; }

            public Neg(string name, IExpression<T> value) : base(value.Type, name) => 
                Value = value;

            public Neg(IExpression<T> value) : base(value.Type, null) =>
                Value = value;

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {Value};
        }

        public class Add<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Add(string name, IExpression<T> x, IExpression<T> y) : base(name, x, y)
            {
            }

            public Add(IExpression<T> x, IExpression<T> y) : base(null, x, y)
            {
            }
        }

        public class Sub<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Sub(string name, IExpression<T> x, IExpression<T> y) : base(name, x, y)
            {
            }

            public Sub(IExpression<T> x, IExpression<T> y) : base(null, x, y)
            {
            }
        }


        public class Mul<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Mul(string name, IExpression<T> x, IExpression<T> y) : base(name, x, y)
            {
            }

            public Mul(IExpression<T> x, IExpression<T> y) : base(null, x, y)
            {
            }
        }

        public class Div<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Div(string name, IExpression<T> x, IExpression<T> y) : base(name, x, y)
            {
            }

            public Div(IExpression<T> x, IExpression<T> y) : base(null, x, y)
            {
            }
        }
        
        public class UDiv : BinaryArithmeticExpression<FixedSizeBitVector>
        {
            public UDiv(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(name, x, y)
            {
            }

            public UDiv(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
        }

        public class Rem<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Rem(string name, IExpression<T> x, IExpression<T> y) : base(name, x, y)
            {
            }

            public Rem(IExpression<T> x, IExpression<T> y) : base(null, x, y)
            {
            }
        }
        
        public class URem : BinaryArithmeticExpression<FixedSizeBitVector>
        {
            public URem(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(name, x, y)
            {
            }

            public URem(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
        }
    }
}