using System;
using System.Collections.Generic;
using System.Threading;

namespace smtsharp.Expressions
{
    public class Formula
    {
        private int _idCounter;

        public string Name { get; }

        private readonly Dictionary<string, IExpression<Type>> _declarations;
        private readonly HashSet<IExpression<Type>>[] _buckets;

        public Formula(string name)
        {
            Name = name;

            _declarations = new Dictionary<string, IExpression<Type>>();
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
            return expression;
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