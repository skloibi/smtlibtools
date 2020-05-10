using smtsharp.Expressions.Types;

namespace smtsharp.Expressions
{
    public interface IVariable<out T> : IExpression<T> where T : Type
    {
    }

    public class Variable<T> : Expression<T>, IVariable<T> where T : Type
    {
        public Variable(T type, string name) : base(type, name)
        {
        }

        public override IExpression<Type>[] Operands() => new IExpression<Type>[0];

        public override string ToString()
        {
            return $"{Name} : {Type}";
        }
    }
}