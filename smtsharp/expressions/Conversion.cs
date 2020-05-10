using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    namespace Conversion
    {
        public class Convert<TIn, TOut> : Expression<TOut>
            where TOut : Type
            where TIn : Type
        {
            public IExpression<TIn> Value { get; }
            public TOut ToType { get; }

            protected Convert(TOut type, string name, IExpression<TIn> value) : base(type, name)
            {
                ToType = type;
                Value = value;
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {Value};
        }
        
        public class Extract : Convert<FixedSizeBitVector, FixedSizeBitVector>
        {
            public uint StartIdx { get; }

            public Extract(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> value, uint startIdx) : 
                base(type, name, value)
            {
                StartIdx = startIdx;
            }
            
            public Extract(FixedSizeBitVector type, IExpression<FixedSizeBitVector> value, uint startIdx) : 
                this(type, null, value, startIdx)
            {
            }
        }

        public class SignExtend : Convert<FixedSizeBitVector, FixedSizeBitVector>
        {
            public SignExtend(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> value) : 
                base(type, name, value) {}

            public SignExtend(FixedSizeBitVector type, IExpression<FixedSizeBitVector> value) : 
                base(type, null, value)
            {
                
            } 
        }
        
        public class ZeroExtend : Convert<FixedSizeBitVector, FixedSizeBitVector>
        {
            public ZeroExtend(FixedSizeBitVector type, string name, IExpression<FixedSizeBitVector> value) : 
                base(type, name, value) {}

            public ZeroExtend(FixedSizeBitVector type, IExpression<FixedSizeBitVector> value) : 
                base(type, null, value)
            {
                
            }
        }
    }
}