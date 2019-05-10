using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	internal class NoThrowGetBinderMember : GetMemberBinder
	{
		private readonly GetMemberBinder _innerBinder;

		public NoThrowGetBinderMember(GetMemberBinder innerBinder)
		{
			Class6.yDnXvgqzyB5jw();
			base(innerBinder.Name, innerBinder.IgnoreCase);
			this._innerBinder = innerBinder;
		}

		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, CollectionUtils.ArrayEmpty<DynamicMetaObject>());
			return new DynamicMetaObject((new NoThrowExpressionVisitor()).Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
		}
	}
}