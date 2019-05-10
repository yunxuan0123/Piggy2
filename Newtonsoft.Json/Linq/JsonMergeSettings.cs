using System;

namespace Newtonsoft.Json.Linq
{
	public class JsonMergeSettings
	{
		private Newtonsoft.Json.Linq.MergeArrayHandling _mergeArrayHandling;

		private Newtonsoft.Json.Linq.MergeNullValueHandling _mergeNullValueHandling;

		private StringComparison _propertyNameComparison;

		public Newtonsoft.Json.Linq.MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return this._mergeArrayHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.Linq.MergeArrayHandling.Concat || value > Newtonsoft.Json.Linq.MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeArrayHandling = value;
			}
		}

		public Newtonsoft.Json.Linq.MergeNullValueHandling MergeNullValueHandling
		{
			get
			{
				return this._mergeNullValueHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.Linq.MergeNullValueHandling.Ignore || value > Newtonsoft.Json.Linq.MergeNullValueHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeNullValueHandling = value;
			}
		}

		public StringComparison PropertyNameComparison
		{
			get
			{
				return this._propertyNameComparison;
			}
			set
			{
				if (value < StringComparison.CurrentCulture || value > StringComparison.OrdinalIgnoreCase)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._propertyNameComparison = value;
			}
		}

		public JsonMergeSettings()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._propertyNameComparison = StringComparison.Ordinal;
		}
	}
}