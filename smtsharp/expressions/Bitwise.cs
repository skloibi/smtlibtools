using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    public class Bitwise
    {
        public class BitwiseExpression : Expression<FixedSizeBitVector>
        {
            public IExpression<FixedSizeBitVector> X { get; }
            public IExpression<FixedSizeBitVector> Y { get; }

            public BitwiseExpression(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> x,
                IExpression<FixedSizeBitVector> y) : base(type, name)
            {
                X = x;
                Y = x;
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {X, Y};
        }

        public class Not : Expression<FixedSizeBitVector>
        {
            public IExpression<FixedSizeBitVector> Value { get; }

            public Not(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> value) : base(type, name)
            {
                Value = value;
            }

            public Not(FixedSizeBitVector type, IExpression<FixedSizeBitVector> value) : base(type, null)
            {
                Value = value;
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {Value};
        }

        public class And : BitwiseExpression
        {
            public And(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> x,
                IExpression<FixedSizeBitVector> y) : base(type, name, x, y)
            {
            }

            public And(FixedSizeBitVector type, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) :
                base(type, null, x, y)
            {
            }
        }

        public class Or : BitwiseExpression
        {
            public Or(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> x,
                IExpression<FixedSizeBitVector> y) : base(type, name, x, y)
            {
            }

            public Or(FixedSizeBitVector type, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) :
                base(type, null, x, y)
            {
            }
        }

        public class Shl : BitwiseExpression
        {
            public Shl(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> x,
                IExpression<FixedSizeBitVector> y) : base(type, name, x, y)
            {
            }

            public Shl(FixedSizeBitVector type, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) :
                base(type, null, x, y)
            {
            }
        }

        public class AShr : BitwiseExpression
        {
            public AShr(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> x,
                IExpression<FixedSizeBitVector> y) : base(type, name, x, y)
            {
            }

            public AShr(FixedSizeBitVector type, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) :
                base(type, null, x, y)
            {
            }
        }

        public class LShr : BitwiseExpression
        {
            public LShr(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> x,
                IExpression<FixedSizeBitVector> y) : base(type, name, x, y)
            {
            }

            public LShr(FixedSizeBitVector type, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) :
                base(type, null, x, y)
            {
            }
        }
    }
}