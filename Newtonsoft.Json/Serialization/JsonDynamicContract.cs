using Newtonsoft.Json.Utilities;
using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	public class JsonDynamicContract : JsonContainerContract
	{
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>> _callSiteGetters;

		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>> _callSiteSetters;

		public JsonPropertyCollection Properties
		{
			get;
		}

		public Func<string, string> PropertyNameResolver
		{
			get;
			set;
		}

		public JsonDynamicContract(Type underlyingType)
		{
			Class6.yDnXvgqzyB5jw();
			this._callSiteGetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>>(new Func<string, CallSite<Func<CallSite, object, object>>>(JsonDynamicContract.CreateCallSiteGetter));
			this._callSiteSetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>>(new Func<string, CallSite<Func<CallSite, object, object, object>>>(JsonDynamicContract.CreateCallSiteSetter));
			base(underlyingType);
			this.ContractType = JsonContractType.Dynamic;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		private static CallSite<Func<CallSite, object, object>> CreateCallSiteGetter(string name)
		{
			return CallSite<Func<CallSite, object, object>>.Create(new NoThrowGetBinderMember((GetMemberBinder)DynamicUtils.BinderWrapper.GetMember(name, typeof(DynamicUtils))));
		}

		private static CallSite<Func<CallSite, object, object, object>> CreateCallSiteSetter(string name)
		{
			return CallSite<Func<CallSite, object, object, object>>.Create(new NoThrowSetBinderMember((SetMemberBinder)DynamicUtils.BinderWrapper.SetMember(name, typeof(DynamicUtils))));
		}

		internal bool TryGetMember(IDynamicMetaObjectProvider dynamicProvider, string name, out object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object>> callSite = this._callSiteGetters.Get(name);
			object target = callSite.Target(callSite, dynamicProvider);
			if (target != NoThrowExpressionVisitor.ErrorResult)
			{
				value = target;
				return true;
			}
			value = null;
			return false;
		}

		internal bool TrySetMember(IDynamicMetaObjectProvider dynamicProvider, string name, object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object, object>> callSite = this._callSiteSetters.Get(name);
			return callSite.Target(callSite, dynamicProvider, value) != NoThrowExpressionVisitor.ErrorResult;
		}
	}
}