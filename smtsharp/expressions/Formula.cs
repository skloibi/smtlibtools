using System;
using System.Collections.Generic;
using System.Threading;
using Type = smtsharp.Expressions.Types.Type;

namespace smtsharp.Expressions
{
    public class Formula
    {
        private int _idCounter;

        public string Name { get; }

        private readonly Dictionary<string, IVariable<Type>> _declarations;
        private readonly HashSet<IExpression<Type>>[] _buckets;

        public Formula(string name)
        {
            Name = name;
            _declarations = new Dictionary<string, IVariable<Type>>();
            _buckets = new HashSet<IExpression<Type>>[] { };
        }

        private int GenerateId()
        {
            return Interlocked.Increment(ref _idCounter);
        }

        public T Add<T>(T expression) where T : IExpression<Type>
        {
            if (expression.IsDeclared)
                throw new AlreadyInitializedException<T>(this, expression);

            expression.Initialize(this, GenerateId());

            if (expression is IVariable<Type> variable)
            {
                return (T) Declare(variable);
            }

            return expression;
        }

        public IVariable<T> Declare<T>(IVariable<T> variable) where T : Type
        {
            if (_declarations.TryGetValue(variable.Name!, out var existing))
                return (IVariable<T>) existing;
            _declarations[variable.Name] = variable;
            return variable;
        }
    }

    public class AlreadyInitializedException<T> : Exception where T : IExpression<Type>
    {
        public Formula Formula { get; }
        public T Expression { get; }

        public AlreadyInitializedException(Formula formula, T expression) : base(
            $@"Cannot add expression {expression} to formula {formula} as it is already initialized in formula {expression.Formula}")
        {
            Formula = formula;
            Expression = expression;
        }
    }
}