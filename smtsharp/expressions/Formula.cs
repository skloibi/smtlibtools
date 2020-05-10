#nullable enable
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

        public IEnumerable<IVariable<Type>> Declarations => _declarations.Values;

        public IVariable<Type>? GetDeclaration(string name)
        {
            if (_declarations.TryGetValue(name, out var variable))
            {
                Console.WriteLine(variable.GetType().FullName);
                return variable;
            }

            return null;
        }

        public T Add<T>(T expression) where T : IExpression<Type>
        {
            if (expression.IsDeclared)
                throw new AlreadyInitializedException<T>(this, expression);

            expression.Initialize(this, GenerateId());

            return expression;
        }

        public IVariable<T> Declare<T>(IVariable<T> variable) where T : Type
        {
            if (_declarations.TryGetValue(variable.Name!, out var existing))
                return (IVariable<T>) existing;
            _declarations[variable.Name!] = variable;
            return variable;
        }

        public IVariable<T> Declare<T>(T type, string name) where T : Type
        {
            if (_declarations.TryGetValue(name, out var existing))
                return (IVariable<T>) existing;
            var variable = new Variable<T>(type, name);
            _declarations[name] = variable;
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