using System;

namespace smtsharp.Expressions
{
    namespace Logic
    {
        public abstract class LogicExpression : Expression<Bool>
        {
            protected LogicExpression(Bool type, string name) : base(type, name)
            {
            }
        }

        public class Not : LogicExpression
        {
            public IExpression<Bool> Value { get; }

            public Not(string name, IExpression<Bool> value) : base(Bool.Type, name)
            {
                Value = value;
            }

            public Not(IExpression<Bool> value) : base(Bool.Type, null)
            {
                Value = value;
            }

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
    }
}