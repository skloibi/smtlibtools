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
        private readonly Dictionary<string, IVariable<Type>> _declarations;

        /// <summary>
        /// Stores all expressions in disjointed buckets that contain the expression IDs.
        /// Whenever an expression is added, it may link two buckets, which may then be merged into one.
        /// </summary>
        // private readonly Dictionary<int, IExpression<Type>>[] _buckets;
        public Formula(string name)
        {
            Name = name;
            _declarations = new Dictionary<string, IVariable<Type>>();
            // _buckets = new Dictionary<int, IExpression<Type>>[] { };
        }

        private int GenerateId() =>
            Interlocked.Increment(ref _idCounter);

        public ICollection<IVariable<Type>> Declarations => _declarations.Values;

        public IVariable<Type>? GetDeclaration(string name)
        {
            return _declarations.TryGetValue(name, out var variable) ? variable : null;
        }

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