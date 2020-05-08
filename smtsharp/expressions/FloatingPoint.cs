using System.Collections;
using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    namespace FloatingPoint
    {
        public class Const : ConstantExpression<FixedSizeBitVector, (bool, BitArray, BitArray)>
        {
            public bool Signed { get; }

            private readonly BitArray _significand;
            private readonly BitArray _exponent;

            /// <summary>
            /// Returns the wrapped significand to ensure immutability.
            /// </summary>
            public BitArray Significand => new BitArray(_significand);

            /// <summary>
            /// Returns the wrapped exponent to ensure immutability.
            /// </summary>
            public BitArray Exponent => new BitArray(_exponent);

            public override (bool, BitArray, BitArray) Value => (Signed, Significand, Exponent);

            public Const(FixedSizeBitVector type, string name, bool signed, BitArray significand, BitArray exponent) :
                base(type, name)
            {
                Signed = signed;
                _significand = significand;
                _exponent = exponent;
            }

            public Const(FixedSizeBitVector type, bool signed, BitArray significand, BitArray exponent) :
                base(type, null)
            {
            }
        }
    }
}