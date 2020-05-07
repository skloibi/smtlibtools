namespace smtsharp.Expressions
{
    namespace Arithmetic
    {
        public abstract class BinaryArithmeticExpression<T> : Expression<T> where T : NumericType
        {
            public IExpression<T> X { get; }
            public IExpression<T> Y { get; }

            protected BinaryArithmeticExpression(T type, string name, IExpression<T> x, IExpression<T> y) :
                base(type, name)
            {
                X = x;
                Y = y;
            }

            public override IExpression<Type>[] Operands() => new IExpression<Type>[] {X, Y};
        }

        public class Add<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Add(T type, string name, IExpression<T> x, IExpression<T> y) : base(type, name, x, y)
            {
            }

            public Add(T type, IExpression<T> x, IExpression<T> y) : base(type, null, x, y)
            {
            }
        }

        public class Sub<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Sub(T type, string name, IExpression<T> x, IExpression<T> y) : base(type, name, x, y)
            {
            }

            public Sub(T type, IExpression<T> x, IExpression<T> y) : base(type, null, x, y)
            {
            }
        }


        public class Mul<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Mul(T type, string name, IExpression<T> x, IExpression<T> y) : base(type, name, x, y)
            {
            }

            public Mul(T type, IExpression<T> x, IExpression<T> y) : base(type, null, x, y)
            {
            }
        }


        public class Div<T> : BinaryArithmeticExpression<T> where T : NumericType
        {
            public Div(T type, string name, IExpression<T> x, IExpression<T> y) : base(type, name, x, y)
            {
            }

            public Div(T type, IExpression<T> x, IExpression<T> y) : base(type, null, x, y)
            {
            }
        }
    }
}