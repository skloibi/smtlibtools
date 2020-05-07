using System;

namespace smtsharp.Expressions
{
    public abstract class Type
    {
        protected Type()
        {
        }

        public static bool operator ==(Type left, Type right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Type left, Type right)
        {
            return !Equals(left, right);
        }
    }

    public abstract class NumericType : Type
    {
        public readonly uint Bits;

        protected NumericType(uint bits)
        {
            Bits = bits;
        }
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

        public override int GetHashCode() =>
            HashCode.Combine(EBits, SBits);

        public override string ToString() =>
            $"FloatingPoint{EBits + SBits}";
    }

    public sealed class Bool : Type
    {
        public static readonly Bool Type = new Bool();
    }
}