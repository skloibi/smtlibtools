using System;
using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    namespace Types
    {
        public abstract class Type
        {
            protected bool Equals(Type other) => throw new NotImplementedException();

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Type) obj);
            }

            public override int GetHashCode() => throw new NotImplementedException();

            public static bool operator ==(Type left, Type right) => Equals(left, right);

            public static bool operator !=(Type left, Type right) => !Equals(left, right);

            public override string ToString() => GetType().Name;
        }

        public abstract class NumericType : Type
        {
            public readonly uint Bits;

            protected NumericType(uint bits) => Bits = bits;
        }

        public sealed class FixedSizeBitVector : NumericType
        {
            private FixedSizeBitVector(uint bits) : base(bits)
            {
            }

            public static FixedSizeBitVector Bv(uint width) =>
                new FixedSizeBitVector(width);

            private bool Equals(FixedSizeBitVector other)
                => Bits == other.Bits;

            public override bool Equals(object obj) =>
                ReferenceEquals(this, obj) || obj is FixedSizeBitVector other && Equals(other);

            public override int GetHashCode() => (int) Bits;

            public static bool operator ==(FixedSizeBitVector left, FixedSizeBitVector right) => Equals(left, right);

            public static bool operator !=(FixedSizeBitVector left, FixedSizeBitVector right) => !Equals(left, right);

            public override string ToString() => $"BitVec{Bits}";
        }

        public sealed class FloatingPoint : NumericType
        {
            public readonly uint EBits;
            public readonly uint SBits;

            public FloatingPoint(uint ebits, uint sbits) : base(ebits + sbits)
            {
                EBits = ebits;
                SBits = sbits;
            }


            private bool Equals(FloatingPoint other) =>
                EBits == other.EBits && SBits == other.SBits;

            public override bool Equals(object obj) =>
                ReferenceEquals(this, obj) || obj is FloatingPoint other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(EBits, SBits);

            public static bool operator ==(FloatingPoint left, FloatingPoint right) => Equals(left, right);

            public static bool operator !=(FloatingPoint left, FloatingPoint right) => !Equals(left, right);

            public override string ToString() =>
                $"FloatingPoint{EBits + SBits}";
        }

        public sealed class Bool : Type
        {
            public static readonly Bool Type = new Bool();
        }
    }
}