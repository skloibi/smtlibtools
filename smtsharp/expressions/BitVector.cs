using System.Numerics;
using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    namespace BitVector
    {
        public class Const : ConstantExpression<FixedSizeBitVector, BigInteger>
        {
            public override BigInteger Value { get; }

            public Const(FixedSizeBitVector type, string name, BigInteger value) : base(type, name) => 
                Value = value;

            public Const(FixedSizeBitVector type, BigInteger value) : base(type, null) => 
                Value = value;
        }
    }
}