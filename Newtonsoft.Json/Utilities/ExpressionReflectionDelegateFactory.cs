using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	internal class ExpressionReflectionDelegateFactory : ReflectionDelegateFactory
	{
		private readonly static ExpressionReflectionDelegateFactory _instance;

		internal static ReflectionDelegateFactory Instance
		{
			get
			{
				return ExpressionReflectionDelegateFactory._instance;
			}
		}

		static ExpressionReflectionDelegateFactory()
		{
			Class6.yDnXvgqzyB5jw();
			ExpressionReflectionDelegateFactory._instance = new ExpressionReflectionDelegateFactory();
		}

		public ExpressionReflectionDelegateFactory()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private Expression BuildMethodCall(MethodBase method, Type type, ParameterExpression targetParameterExpression, ParameterExpression argsParameterExpression)
		{
			Expression[] expressionArray;
			IList<ExpressionReflectionDelegateFactory.ByRefParameter> byRefParameters;
			Expression expression;
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != 0)
			{
				expressionArray = new Expression[(int)parameters.Length];
				byRefParameters = new List<ExpressionReflectionDelegateFactory.ByRefParameter>();
				for (int i = 0; i < (int)parameters.Length; i++)
				{
					ParameterInfo parameterInfo = parameters[i];
					Type parameterType = parameterInfo.ParameterType;
					bool flag = false;
					if (parameterType.IsByRef)
					{
						parameterType = parameterType.GetElementType();
						flag = true;
					}
					Expression expression1 = Expression.Constant(i);
					Expression expression2 = Expression.ArrayIndex(argsParameterExpression, expression1);
					Expression expression3 = this.EnsureCastExpression(expression2, parameterType, !flag);
					if (flag)
					{
						ParameterExpression parameterExpression = Expression.Variable(parameterType);
						byRefParameters.Add(new ExpressionReflectionDelegateFactory.ByRefParameter()
						{
							Value = expression3,
							Variable = parameterExpression,
							IsOut = parameterInfo.IsOut
						});
						expression3 = parameterExpression;
					}
					expressionArray[i] = expression3;
				}
			}
			else
			{
				expressionArray = CollectionUtils.ArrayEmpty<Expression>();
				byRefParameters = CollectionUtils.ArrayEmpty<ExpressionReflectionDelegateFactory.ByRefParameter>();
			}
			if (!method.IsConstructor)
			{
				expression = (!method.IsStatic ? Expression.Call(this.EnsureCastExpression(targetParameterExpression, method.DeclaringType, false), (MethodInfo)method, expressionArray) : Expression.Call((MethodInfo)method, expressionArray));
			}
			else
			{
				expression = Expression.New((ConstructorInfo)method, expressionArray);
			}
			MethodInfo methodInfo = method as MethodInfo;
			MethodInfo methodInfo1 = methodInfo;
			if (methodInfo == null)
			{
				expression = this.EnsureCastExpression(expression, type, false);
			}
			else if (methodInfo1.ReturnType == typeof(void))
			{
				expression = Expression.Block(expression, Expression.Constant(null));
			}
			else
			{
				expression = this.EnsureCastExpression(expression, type, false);
			}
			if (byRefParameters.Count > 0)
			{
				IList<ParameterExpression> parameterExpressions = new List<ParameterExpression>();
				IList<Expression> expressions = new List<Expression>();
				foreach (ExpressionReflectionDelegateFactory.ByRefParameter byRefParameter in byRefParameters)
				{
					if (!byRefParameter.IsOut)
					{
						expressions.Add(Expression.Assign(byRefParameter.Variable, byRefParameter.Value));
					}
					parameterExpressions.Add(byRefParameter.Variable);
				}
				expressions.Add(expression);
				expression = Expression.Block(parameterExpressions, expressions);
			}
			return expression;
		}

		public override Func<T> CreateDefaultConstructor<T>(Type type)
		{
			Func<T> func;
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsAbstract())
			{
				return () => (T)Activator.CreateInstance(type);
			}
			try
			{
				Type type1 = typeof(T);
				Expression expression = Expression.New(type);
				expression = this.EnsureCastExpression(expression, type1, false);
				func = (Func<T>)Expression.Lambda(typeof(Func<T>), expression, new ParameterExpression[0]).Compile();
			}
			catch
			{
				func = () => (T)Activator.CreateInstance(type);
			}
			return func;
		}

		public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
		{
			Expression expression;
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			Type type = typeof(T);
			Type type1 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(type, "instance");
			expression = (!propertyInfo.GetGetMethod(true).IsStatic ? Expression.MakeMemberAccess(this.EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType, false), propertyInfo) : Expression.MakeMemberAccess(null, propertyInfo));
			expression = this.EnsureCastExpression(expression, type1, false);
			return (Func<T, object>)Expression.Lambda(typeof(Func<T, object>), expression, new ParameterExpression[] { parameterExpression }).Compile();
		}

		public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
		{
			Expression expression;
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			expression = (!fieldInfo.IsStatic ? Expression.Field(this.EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType, false), fieldInfo) : Expression.Field(null, fieldInfo));
			expression = this.EnsureCastExpression(expression, typeof(object), false);
			return Expression.Lambda<Func<T, object>>(expression, new ParameterExpression[] { parameterExpression }).Compile();
		}

		public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type type = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(type, "target");
			ParameterExpression parameterExpression1 = Expression.Parameter(typeof(object[]), "args");
			Expression expression = this.BuildMethodCall(method, type, parameterExpression, parameterExpression1);
			return (MethodCall<T, object>)Expression.Lambda(typeof(MethodCall<T, object>), expression, new ParameterExpression[] { parameterExpression, parameterExpression1 }).Compile();
		}

		public override ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type type = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "args");
			Expression expression = this.BuildMethodCall(method, type, null, parameterExpression);
			return (ObjectConstructor<object>)Expression.Lambda(typeof(ObjectConstructor<object>), expression, new ParameterExpression[] { parameterExpression }).Compile();
		}

		public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
		{
			Expression expression;
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			if (fieldInfo.DeclaringType.IsValueType() || fieldInfo.IsInitOnly)
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(fieldInfo);
			}
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			ParameterExpression parameterExpression1 = Expression.Parameter(typeof(object), "value");
			expression = (!fieldInfo.IsStatic ? Expression.Field(this.EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType, false), fieldInfo) : Expression.Field(null, fieldInfo));
			Expression expression1 = this.EnsureCastExpression(parameterExpression1, expression.Type, false);
			BinaryExpression binaryExpression = Expression.Assign(expression, expression1);
			return (Action<T, object>)Expression.Lambda(typeof(Action<T, object>), binaryExpression, new ParameterExpression[] { parameterExpression, parameterExpression1 }).Compile();
		}

		public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
		{
			Expression expression;
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			if (propertyInfo.DeclaringType.IsValueType())
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(propertyInfo);
			}
			Type type = typeof(T);
			Type type1 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(type, "instance");
			ParameterExpression parameterExpression1 = Expression.Parameter(type1, "value");
			Expression expression1 = this.EnsureCastExpression(parameterExpression1, propertyInfo.PropertyType, false);
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			expression = (!setMethod.IsStatic ? Expression.Call(this.EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType, false), setMethod, new Expression[] { expression1 }) : Expression.Call(setMethod, expression1));
			return (Action<T, object>)Expression.Lambda(typeof(Action<T, object>), expression, new ParameterExpression[] { parameterExpression, parameterExpression1 }).Compile();
		}

		private Expression EnsureCastExpression(Expression expression, Type targetType, bool allowWidening = false)
		{
			Type type = expression.Type;
			if (type == targetType || !type.IsValueType() && targetType.IsAssignableFrom(type))
			{
				return expression;
			}
			if (!targetType.IsValueType())
			{
				return Expression.Convert(expression, targetType);
			}
			Expression expression1 = Expression.Unbox(expression, targetType);
			if (allowWidening && targetType.IsPrimitive())
			{
				MethodInfo method = typeof(Convert).GetMethod(string.Concat("To", targetType.Name), new Type[] { typeof(object) });
				if (method != null)
				{
					expression1 = Expression.Condition(Expression.TypeIs(expression, targetType), expression1, Expression.Call(method, expression));
				}
			}
			return Expression.Condition(Expression.Equal(expression, Expression.Constant(null, typeof(object))), Expression.Default(targetType), expression1);
		}

		private class ByRefParameter
		{
			public Expression Value;

			public ParameterExpression Variable;

			public bool IsOut;

			public ByRefParameter()
			{
				Class6.yDnXvgqzyB5jw();
				base();
			}
		}
	}
}