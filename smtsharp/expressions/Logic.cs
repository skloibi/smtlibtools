using System;
using smtsharp.Expressions.Types;
using Type = smtsharp.Expressions.Types.Type;

namespace smtsharp.Expressions
{
    namespace Logic
    {
        public class Const : ConstantExpression<Bool, bool>
        {
            public static readonly Const True = new Const(true);
            public static readonly Const False = new Const(false);

            public override bool Value { get; }

            private Const(bool value) : base(Bool.Type, null) => 
                Value = value;

            public static implicit operator bool(Const c) => c.Value;
            public static implicit operator Const(bool b) => b ? True : False;
        }

        public abstract class LogicExpression : Expression<Bool>
        {
            protected LogicExpression(Bool type, string name) : base(type, name)
            {
            }
        }

        public class Not : LogicExpression
        {
            public IExpression<Bool> Value { get; }

            public Not(string name, IExpression<Bool> value) : base(Bool.Type, name) => 
                Value = value;

            public Not(IExpression<Bool> value) : base(Bool.Type, null) => 
                Value = value;

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {Value};
        }

        public abstract class BinaryLogicExpression : LogicExpression
        {
            public IExpression<Bool> X { get; }
            public IExpression<Bool> Y { get; }

            protected BinaryLogicExpression(string name, IExpression<Bool> x, IExpression<Bool> y) :
                base(Bool.Type, name)
            {
                X = x;
                Y = y;
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {X, Y};
        }

        public class Implies : BinaryLogicExpression
        {
            public Implies(string name, IExpression<Bool> x, IExpression<Bool> y) : base(name, x, y)
            {
            }

            public Implies(IExpression<Bool> x, IExpression<Bool> y) : base(null, x, y)
            {
            }
        }

        public class And : BinaryLogicExpression
        {
            public And(string name, IExpression<Bool> x, IExpression<Bool> y) : base(name, x, y)
            {
            }

            public And(IExpression<Bool> x, IExpression<Bool> y) : base(null, x, y)
            {
            }
        }

        public class Or : BinaryLogicExpression
        {
            public Or(string name, IExpression<Bool> x, IExpression<Bool> y) : base(name, x, y)
            {
            }

            public Or(IExpression<Bool> x, IExpression<Bool> y) : base(null, x, y)
            {
            }
        }
        
        public class XOr : BinaryLogicExpression
        {
            public XOr(string name, IExpression<Bool> x, IExpression<Bool> y) : base(name, x, y)
            {
            }

            public XOr(IExpression<Bool> x, IExpression<Bool> y) : base(null, x, y)
            {
            }
        }
    }
}