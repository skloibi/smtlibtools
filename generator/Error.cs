using System;
using static smtsharp.Generator.Error.ErrorType;

namespace smtsharp.Generator
{
    namespace Error
    {
        public enum ErrorType
        {
            TypeMismatch,
            InvalidBitVectorLiteral,
            InvalidNumericLiteral,
            MissingVariableDeclaration,
            VariableAlreadyDeclared,
            InvalidOperand
        }

        public static class ErrorMethods
        {
            public static String Format(this ErrorType error, params object[] args)
            {
                return error switch
                {
                    TypeMismatch => String.Format("Invalid type. Got {0} but expected {1}", args),
                    InvalidBitVectorLiteral => String.Format("Invalid bitvector literal: {0}", args),
                    InvalidNumericLiteral => String.Format("Invalid numeric literal: {0}", args),
                    MissingVariableDeclaration => String.Format("Missing declaration for variable: {0}", args),
                    VariableAlreadyDeclared => String.Format("Variable {0} is already declared", args),
                    InvalidOperand => String.Format("Invalid operand {0} for operation {1}", args),
                    _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
                };
            }
        }
    }
}