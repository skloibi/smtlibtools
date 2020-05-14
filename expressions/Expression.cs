using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using Microsoft.VisualBasic;
using smtsharp.Expressions.Types;

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

        /// <summary>
        /// Enables indexed (read) access to the operands of the expressions by encapsulating them in an array.
        /// </summary>
        /// <returns>
        /// An array of all operand expressions (if any).
        /// </returns>
        public IExpression<Type>[] Operands();

        public IExpression<Type> this[int i] => Operands()[i];

        /// <summary>
        /// Denotes the targets that receive this expression as an input. This collection should be updated whenever an
        /// expression is created.
        /// </summary>
        /// <returns>
        /// A list of all expressions that use this one or have it as an input / operand.
        /// </returns>
        public IList<IExpression<Type>> Targets();

        public void AddTarget(IExpression<Type> target);

        bool IsDeclared => Id.HasValue;

        void Initialize(Formula formula, int id);
    }

    public abstract class Expression<T> : IExpression<T> where T : Type
    {
        public int? Id { get; private set; }
        public T Type { get; }
        public string? Name { get; }
        public Formula? Formula { get; private set; }

        private readonly IList<IExpression<Type>> _targets;

        protected Expression(T type, string name = null)
        {
            Type = type;
            Name = name;
            _targets = new List<IExpression<Type>>();
        }

        public IList<IExpression<Type>> Targets() => _targets.ToImmutableList();

        public void AddTarget(IExpression<Type> target) => _targets.Add(target);

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
                operand.AddTarget(this);
            }
        }
    }

    public interface IConstantExpression<out T, out V> : IExpression<T> where T : Type
    {
        public V Value { get; }
    }
}