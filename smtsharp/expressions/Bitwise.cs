using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    public class Bitwise
    {
        public class BitwiseExpression : Expression<FixedSizeBitVector>
        {
            public IExpression<FixedSizeBitVector> X { get; }
            public IExpression<FixedSizeBitVector> Y { get; }

            public BitwiseExpression(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : 
                base(x.Type, name)
            {
                X = x;
                Y = x;
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {X, Y};
        }

        public class Not : Expression<FixedSizeBitVector>
        {
            public IExpression<FixedSizeBitVector> Value { get; }

            public Not(string name, IExpression<FixedSizeBitVector> value) : base(value.Type, name) => 
                Value = value;

            public Not(IExpression<FixedSizeBitVector> value) : this(null, value)
            {
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {Value};
            
            public override string ToString() => $@"~{Value}";
        }

        public class And : BitwiseExpression
        {
            public And(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : 
                base(name, x, y)
            {
            }

            public And(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} & {Y})";
        }

        public class Or : BitwiseExpression
        {
            public Or(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : 
                base(name, x, y)
            {
            }

            public Or(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} | {Y})";
        }
        
        public class XOr : BitwiseExpression
        {
            public XOr(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(name, x, y)
            {
            }

            public XOr(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} ^ {Y})";
        }

        public class Shl : BitwiseExpression
        {
            public Shl(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) :
                base(name, x, y)
            {
            }

            public Shl(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} << {Y})";
        }

        public class AShr : BitwiseExpression
        {
            public AShr(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : 
                base(name, x, y)
            {
            }

            public AShr(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} >> {Y})";
        }

        public class LShr : BitwiseExpression
        {
            public LShr(string name, IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : 
                base(name, x, y)
            {
            }

            public LShr(IExpression<FixedSizeBitVector> x, IExpression<FixedSizeBitVector> y) : base(null, x, y)
            {
            }
            
            public override string ToString() => $@"({X} >>> {Y})";
        }
    }
}