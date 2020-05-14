using System.Numerics;
using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    namespace BitVector
    {
        public class Const : Expression<FixedSizeBitVector>, IConstantExpression<FixedSizeBitVector, BigInteger>
        {
            public BigInteger Value { get; }

            public Const(FixedSizeBitVector type, string name, BigInteger value) : base(type, name) => 
                Value = value;

            public Const(FixedSizeBitVector type, BigInteger value) : base(type, null) => 
                Value = value;
            
            public override IExpression<Type>[] Operands() => new IExpression<Type>[0];

            public override string ToString() => $@"({Value} : {Type})";
        }
    }
}