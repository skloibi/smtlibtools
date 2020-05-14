using System;
using System.Collections;
using smtsharp.Expressions.Types;
using FP = smtsharp.Expressions.Types.FloatingPoint;
using Type = smtsharp.Expressions.Types.Type;

namespace smtsharp.Expressions
{
    namespace FloatingPoint
    {
        public class Const : Expression<FP>, IConstantExpression<FP, (bool, BitArray, BitArray)>
        {
            public bool Signed { get; }
            private readonly BitArray _exponent;
            private readonly BitArray _significand;

            /// <summary>
            /// Returns the wrapped exponent to ensure immutability.
            /// </summary>
            public BitArray Exponent => new BitArray(_exponent);

            /// <summary>
            /// Returns the wrapped significand to ensure immutability.
            /// </summary>
            public BitArray Significand => new BitArray(_significand);

            public (bool, BitArray, BitArray) Value => (Signed, Significand, Exponent);

            public Const(FP type, string name, bool signed, BitArray exponent, BitArray significand) :
                base(type, name)
            {
                Signed = signed;
                _exponent = exponent;
                _significand = significand;
            }

            public Const(FP type, bool signed, BitArray exponent, BitArray significand) :
                this(type, null, signed, exponent, significand)
            {
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[0];

            public override string ToString() => $@"({(Convert.ToByte(Signed))} {_exponent} {_significand})";
        }

        public class IsZero : Expression<Bool>
        {
            public IExpression<FP> Operand { get; }

            public IsZero(string name, IExpression<FP> operand) : base(Bool.Type, name) =>
                Operand = operand;

            public IsZero(IExpression<FP> operand) : base(Bool.Type, null) =>
                Operand = operand;

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {Operand};

            public override string ToString() => $"isZero?({Operand})";
        }
    }
}