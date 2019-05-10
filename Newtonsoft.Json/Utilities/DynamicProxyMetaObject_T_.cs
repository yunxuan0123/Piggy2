using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	internal sealed class DynamicProxyMetaObject<T> : DynamicMetaObject
	{
		private readonly DynamicProxy<T> _proxy;

		private static System.Linq.Expressions.Expression[] NoArgs
		{
			get
			{
				return CollectionUtils.ArrayEmpty<System.Linq.Expressions.Expression>();
			}
		}

		internal DynamicProxyMetaObject(System.Linq.Expressions.Expression expression, T value, DynamicProxy<T> proxy)
		{
			Class6.yDnXvgqzyB5jw();
			base(expression, BindingRestrictions.Empty, value);
			this._proxy = proxy;
		}

		public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
		{
			if (!this.IsOverridden("TryBinaryOperation"))
			{
				return base.BindBinaryOperation(binder, arg);
			}
			return this.CallMethodWithResult("TryBinaryOperation", binder, DynamicProxyMetaObject<T>.GetArgs(new DynamicMetaObject[] { arg }), (DynamicMetaObject e) => binder.FallbackBinaryOperation(this, arg, e), null);
		}

		public override DynamicMetaObject BindConvert(ConvertBinder binder)
		{
			if (!this.IsOverridden("TryConvert"))
			{
				return base.BindConvert(binder);
			}
			return this.CallMethodWithResult("TryConvert", binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicMetaObject e) => binder.FallbackConvert(this, e), null);
		}

		public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryCreateInstance"))
			{
				return base.BindCreateInstance(binder, args);
			}
			return this.CallMethodWithResult("TryCreateInstance", binder, DynamicProxyMetaObject<T>.GetArgArray(args), (DynamicMetaObject e) => binder.FallbackCreateInstance(this, args, e), null);
		}

		public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!this.IsOverridden("TryDeleteIndex"))
			{
				return base.BindDeleteIndex(binder, indexes);
			}
			return this.CallMethodNoResult("TryDeleteIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), (DynamicMetaObject e) => binder.FallbackDeleteIndex(this, indexes, e));
		}

		public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
		{
			if (!this.IsOverridden("TryDeleteMember"))
			{
				return base.BindDeleteMember(binder);
			}
			return this.CallMethodNoResult("TryDeleteMember", binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicMetaObject e) => binder.FallbackDeleteMember(this, e));
		}

		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!this.IsOverridden("TryGetIndex"))
			{
				return base.BindGetIndex(binder, indexes);
			}
			return this.CallMethodWithResult("TryGetIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), (DynamicMetaObject e) => binder.FallbackGetIndex(this, indexes, e), null);
		}

		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			if (!this.IsOverridden("TryGetMember"))
			{
				return base.BindGetMember(binder);
			}
			return this.CallMethodWithResult("TryGetMember", binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicMetaObject e) => binder.FallbackGetMember(this, e), null);
		}

		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryInvoke"))
			{
				return base.BindInvoke(binder, args);
			}
			return this.CallMethodWithResult("TryInvoke", binder, DynamicProxyMetaObject<T>.GetArgArray(args), (DynamicMetaObject e) => binder.FallbackInvoke(this, args, e), null);
		}

		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryInvokeMember"))
			{
				return base.BindInvokeMember(binder, args);
			}
			DynamicProxyMetaObject<T>.Fallback fallback = (DynamicMetaObject e) => binder.FallbackInvokeMember(this, args, e);
			return this.BuildCallMethodWithResult("TryInvokeMember", binder, DynamicProxyMetaObject<T>.GetArgArray(args), this.BuildCallMethodWithResult("TryGetMember", new DynamicProxyMetaObject<T>.GetBinderAdapter(binder), DynamicProxyMetaObject<T>.NoArgs, fallback(null), (DynamicMetaObject e) => binder.FallbackInvoke(e, args, null)), null);
		}

		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			if (!this.IsOverridden("TrySetIndex"))
			{
				return base.BindSetIndex(binder, indexes, value);
			}
			return this.CallMethodReturnLast("TrySetIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes, value), (DynamicMetaObject e) => binder.FallbackSetIndex(this, indexes, value, e));
		}

		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			if (!this.IsOverridden("TrySetMember"))
			{
				return base.BindSetMember(binder, value);
			}
			return this.CallMethodReturnLast("TrySetMember", binder, DynamicProxyMetaObject<T>.GetArgs(new DynamicMetaObject[] { value }), (DynamicMetaObject e) => binder.FallbackSetMember(this, value, e));
		}

		public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
		{
			if (!this.IsOverridden("TryUnaryOperation"))
			{
				return base.BindUnaryOperation(binder);
			}
			return this.CallMethodWithResult("TryUnaryOperation", binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicMetaObject e) => binder.FallbackUnaryOperation(this, e), null);
		}

		private DynamicMetaObject BuildCallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, IEnumerable<System.Linq.Expressions.Expression> args, DynamicMetaObject fallbackResult, DynamicProxyMetaObject<T>.Fallback fallbackInvoke)
		{
			ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(typeof(object), null);
			IList<System.Linq.Expressions.Expression> expressions = new List<System.Linq.Expressions.Expression>()
			{
				System.Linq.Expressions.Expression.Convert(base.Expression, typeof(T)),
				DynamicProxyMetaObject<T>.Constant(binder)
			};
			expressions.AddRange<System.Linq.Expressions.Expression>(args);
			expressions.Add(parameterExpression);
			DynamicMetaObject dynamicMetaObject = new DynamicMetaObject(parameterExpression, BindingRestrictions.Empty);
			if (binder.ReturnType != typeof(object))
			{
				dynamicMetaObject = new DynamicMetaObject(System.Linq.Expressions.Expression.Convert(dynamicMetaObject.Expression, binder.ReturnType), dynamicMetaObject.Restrictions);
			}
			if (fallbackInvoke != null)
			{
				dynamicMetaObject = fallbackInvoke(dynamicMetaObject);
			}
			ParameterExpression[] parameterExpressionArray = new ParameterExpression[] { parameterExpression };
			System.Linq.Expressions.Expression[] expressionArray = new System.Linq.Expressions.Expression[] { System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), expressions), dynamicMetaObject.Expression, fallbackResult.Expression, binder.ReturnType) };
			return new DynamicMetaObject(System.Linq.Expressions.Expression.Block((IEnumerable<ParameterExpression>)parameterExpressionArray, expressionArray), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions).Merge(fallbackResult.Restrictions));
		}

		private DynamicMetaObject CallMethodNoResult(string methodName, DynamicMetaObjectBinder binder, System.Linq.Expressions.Expression[] args, DynamicProxyMetaObject<T>.Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			IList<System.Linq.Expressions.Expression> expressions = new List<System.Linq.Expressions.Expression>()
			{
				System.Linq.Expressions.Expression.Convert(base.Expression, typeof(T)),
				DynamicProxyMetaObject<T>.Constant(binder)
			};
			expressions.AddRange<System.Linq.Expressions.Expression>(args);
			return new DynamicMetaObject(System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), expressions), System.Linq.Expressions.Expression.Empty(), dynamicMetaObject.Expression, typeof(void)), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
		}

		private DynamicMetaObject CallMethodReturnLast(string methodName, DynamicMetaObjectBinder binder, IEnumerable<System.Linq.Expressions.Expression> args, DynamicProxyMetaObject<T>.Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(typeof(object), null);
			IList<System.Linq.Expressions.Expression> expressions = new List<System.Linq.Expressions.Expression>()
			{
				System.Linq.Expressions.Expression.Convert(base.Expression, typeof(T)),
				DynamicProxyMetaObject<T>.Constant(binder)
			};
			expressions.AddRange<System.Linq.Expressions.Expression>(args);
			expressions[expressions.Count - 1] = System.Linq.Expressions.Expression.Assign(parameterExpression, expressions[expressions.Count - 1]);
			ParameterExpression[] parameterExpressionArray = new ParameterExpression[] { parameterExpression };
			System.Linq.Expressions.Expression[] expressionArray = new System.Linq.Expressions.Expression[] { System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), expressions), parameterExpression, dynamicMetaObject.Expression, typeof(object)) };
			return new DynamicMetaObject(System.Linq.Expressions.Expression.Block((IEnumerable<ParameterExpression>)parameterExpressionArray, expressionArray), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
		}

		private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, IEnumerable<System.Linq.Expressions.Expression> args, DynamicProxyMetaObject<T>.Fallback fallback, DynamicProxyMetaObject<T>.Fallback fallbackInvoke = null)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			return this.BuildCallMethodWithResult(methodName, binder, args, dynamicMetaObject, fallbackInvoke);
		}

		private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
		{
			Type type = binder.GetType();
			while (!type.IsVisible())
			{
				type = type.BaseType();
			}
			return System.Linq.Expressions.Expression.Constant(binder, type);
		}

		private static System.Linq.Expressions.Expression[] GetArgArray(DynamicMetaObject[] args)
		{
			return new NewArrayExpression[] { System.Linq.Expressions.Expression.NewArrayInit(typeof(object), DynamicProxyMetaObject<T>.GetArgs(args)) };
		}

		private static System.Linq.Expressions.Expression[] GetArgArray(DynamicMetaObject[] args, DynamicMetaObject value)
		{
			System.Linq.Expressions.Expression expression;
			System.Linq.Expressions.Expression expression1 = value.Expression;
			System.Linq.Expressions.Expression[] expressionArray = new System.Linq.Expressions.Expression[] { System.Linq.Expressions.Expression.NewArrayInit(typeof(object), DynamicProxyMetaObject<T>.GetArgs(args)), null };
			if (expression1.Type.IsValueType())
			{
				expression = System.Linq.Expressions.Expression.Convert(expression1, typeof(object));
			}
			else
			{
				expression = expression1;
			}
			expressionArray[1] = expression;
			return expressionArray;
		}

		private static IEnumerable<System.Linq.Expressions.Expression> GetArgs(params DynamicMetaObject[] args)
		{
			return ((IEnumerable<DynamicMetaObject>)args).Select<DynamicMetaObject, System.Linq.Expressions.Expression>((DynamicMetaObject arg) => {
				System.Linq.Expressions.Expression expression = arg.Expression;
				if (!expression.Type.IsValueType())
				{
					return expression;
				}
				return System.Linq.Expressions.Expression.Convert(expression, typeof(object));
			});
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._proxy.GetDynamicMemberNames((T)base.Value);
		}

		private BindingRestrictions GetRestrictions()
		{
			if (base.Value == null && base.HasValue)
			{
				return BindingRestrictions.GetInstanceRestriction(base.Expression, null);
			}
			return BindingRestrictions.GetTypeRestriction(base.Expression, base.LimitType);
		}

		private bool IsOverridden(string method)
		{
			return ReflectionUtils.IsMethodOverridden(this._proxy.GetType(), typeof(DynamicProxy<T>), method);
		}

		private delegate DynamicMetaObject Fallback(DynamicMetaObject errorSuggestion);

		private sealed class GetBinderAdapter : GetMemberBinder
		{
			internal GetBinderAdapter(InvokeMemberBinder binder)
			{
				Class6.yDnXvgqzyB5jw();
				base(binder.Name, binder.IgnoreCase);
			}

			public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
			{
				throw new NotSupportedException();
			}
		}
	}
}