#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Type = smtsharp.Expressions.Types.Type;

namespace smtsharp.Expressions
{
    public class Formula
    {
        private int _idCounter = 1;
        public int Length => _idCounter;

        public string Name { get; }

        /// <summary>
        /// Contains the "leaves" of the formula (i.e. the named variables).
        /// </summary>
        private readonly IDictionary<string, IVariable<Type>> _declarations;

        public Formula(string name)
        {
            Name = name;
            _declarations = new SortedDictionary<string, IVariable<Type>>();
        }

        private int GenerateId() =>
            Interlocked.Increment(ref _idCounter);

        public ICollection<IVariable<Type>> Declarations => _declarations.Values;

        public IVariable<Type>? GetDeclaration(string name) =>
            _declarations.TryGetValue(name, out var variable) ? variable : null;

        public T Add<T>(T expression) where T : IExpression<Type>
        {
            if (expression.IsDeclared)
                throw new AlreadyInitializedException<T>(this, expression);
            expression.Initialize(this, GenerateId());
            return expression;
        }

        public bool Declare<T>(T type, string name) where T : Type
        {
            if (_declarations.ContainsKey(name))
                return false;
            var variable = new Variable<T>(type, name);
            _declarations[name] = variable;
            return true;
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