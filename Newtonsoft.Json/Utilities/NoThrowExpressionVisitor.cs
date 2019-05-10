using System;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	internal class NoThrowExpressionVisitor : ExpressionVisitor
	{
		internal readonly static object ErrorResult;

		static NoThrowExpressionVisitor()
		{
			Class6.yDnXvgqzyB5jw();
			NoThrowExpressionVisitor.ErrorResult = new object();
		}

		public NoThrowExpressionVisitor()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override Expression VisitConditional(ConditionalExpression node)
		{
			if (node.IfFalse.NodeType != ExpressionType.Throw)
			{
				return base.VisitConditional(node);
			}
			return Expression.Condition(node.Test, node.IfTrue, Expression.Constant(NoThrowExpressionVisitor.ErrorResult));
		}
	}
}