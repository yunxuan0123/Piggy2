using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	internal class NoThrowSetBinderMember : SetMemberBinder
	{
		private readonly SetMemberBinder _innerBinder;

		public NoThrowSetBinderMember(SetMemberBinder innerBinder)
		{
			Class6.yDnXvgqzyB5jw();
			base(innerBinder.Name, innerBinder.IgnoreCase);
			this._innerBinder = innerBinder;
		}

		public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, new DynamicMetaObject[] { value });
			return new DynamicMetaObject((new NoThrowExpressionVisitor()).Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
		}
	}
}