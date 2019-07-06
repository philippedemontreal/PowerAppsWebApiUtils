using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace PowerAppsWebApiUtils.Linq
{
    internal static class Evaluator 
    {
        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        ///<chref="https://blogs.msdn.microsoft.com/mattwar/2007/08/01/linq-building-an-iqueryable-provider-part-iii/"/>
        public static Expression PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated) 
        {
            return new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
        }
    
        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression) {
            return PartialEval(expression, Evaluator.CanBeEvaluatedLocally);
        }
    
        private static bool CanBeEvaluatedLocally(Expression expression) 
        {
            return expression.NodeType != ExpressionType.Parameter;
        }
    
    }

    /// <summary>
    /// Evaluates & replaces sub-trees when first candidate is reached (top-down)
    /// </summary>
    internal class SubtreeEvaluator: ExpressionVisitor
    {
        HashSet<Expression> _candidates;

        internal SubtreeEvaluator(HashSet<Expression> candidates) 
        {
            _candidates = candidates;
        }

        internal Expression Eval(Expression exp) 
            => Visit(exp);

        public override Expression Visit(Expression exp) 
        {
            if (exp == null) 
                return null;

            if (_candidates.Contains(exp)) 
                return Evaluate(exp);

            return base.Visit(exp);
        }

        protected override Expression VisitMemberInit(MemberInitExpression exp)
            => base.VisitMemberInit(exp);
            
        private Expression Evaluate(Expression e) 
        {
            if (e.NodeType == ExpressionType.Constant) 
                return e;

            LambdaExpression lambda = Expression.Lambda(e);
            Delegate fn = lambda.Compile();
            return Expression.Constant(fn.DynamicInvoke(null), e.Type);
        }
    }

    /// <summary>
    /// Performs bottom-up analysis to determine which nodes can possibly
    /// be part of an evaluated sub-tree.
    /// </summary>
    internal class Nominator : ExpressionVisitor 
    {
        private Func<Expression, bool> _fnCanBeEvaluated;
        private HashSet<Expression> _candidates;
        private Type _returnType;
        bool cannotBeEvaluated;
 
        internal Nominator(Func<Expression, bool> fnCanBeEvaluated) 
        {
            _fnCanBeEvaluated = fnCanBeEvaluated;
        }
 
        internal HashSet<Expression> Nominate(Expression expression) 
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null && methodCallExpression.Arguments.Count > 0)
                _returnType = TypeSystem.GetElementType(methodCallExpression.Arguments[0].Type);
            else    
                _returnType = TypeSystem.GetElementType(expression.Type);


            _candidates = new HashSet<Expression>();

            Visit(expression);

            return _candidates;
        }
 
        public override Expression Visit(Expression expression) 
        {
            if (expression != null) 
            {
                bool saveCannotBeEvaluated = this.cannotBeEvaluated;
                cannotBeEvaluated = false;
                
                base.Visit(expression);
                
                if (!cannotBeEvaluated) 
                {
                    if (_fnCanBeEvaluated(expression))  
                    {
                        if ((expression.NodeType != ExpressionType.New || expression.Type != _returnType))
                            _candidates.Add(expression);
                    }
                    else 
                    {
                        cannotBeEvaluated = true;
                    }
                }
                cannotBeEvaluated |= saveCannotBeEvaluated;
            }
            return expression;
        }
    }
    
}