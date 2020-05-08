using System;
using static smtsharp.Generator.Error.Error;

namespace smtsharp.Generator
{
    namespace Error
    {
        public enum Error
        {
            TypeMismatch,
            InvalidBitVectorLiteral
        }

        public static class ErrorMethods
        {
            public static String Format(this Error error, params object[] args)
            {
                return error switch
                {
                    TypeMismatch => String.Format("Invalid type. Got {0} but expected {1}", args),
                    InvalidBitVectorLiteral => String.Format("Invalid bitvector literal: {0}", args),
                    _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
                };
            }
        }
    }
}