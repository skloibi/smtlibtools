#nullable enable
namespace smtsharp.Expressions
{
    public interface IExpression<out T> where T : Type
    {
        /// <summary>
        /// Unique ID that is set when the expression is added to a formula.
        /// This ID is used for both identification and for serialization.
        /// </summary>
        int? Id { get; }

        /// <summary>
        /// The type of this expression.
        /// </summary>
        T Type { get; }

        /// <summary>
        /// Optional name that can be provided for expressions.
        /// This should be unique per formula.
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// The formula in which this expression is declared. This is null if the expression was not added to a formula
        /// yet.
        /// </summary>
        Formula? Formula { get; }

        bool IsDeclared => Id.HasValue;

        void Initialize(Formula formula, int id);
    }

    public abstract class Expression<T> : IExpression<T> where T : Type
    {
        public int? Id { get; private set; }
        public T Type { get; }
        public string? Name { get; }
        public Formula? Formula { get; private set; }

        protected Expression(T type, string name)
        {
            Type = type;
            Name = name;
        }

        public abstract IExpression<Type>[] Operands();

        public void Initialize(Formula formula, int id)
        {
            Formula = formula;
            Id = id;

            var operands = Operands();
            for (var i = 0; i < operands.Length; i++)
            {
                var operand = operands[i];
                if (!operand.IsDeclared)
                    formula.Add(operand);
            }
        }

        public IExpression<Type> this[int i] => Operands()[i];
    }
}