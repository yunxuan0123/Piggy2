using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Linq
{
	public class JRaw : JValue
	{
		public JRaw(JRaw other)
		{
			Class6.yDnXvgqzyB5jw();
			base(other);
		}

		public JRaw(object rawJson)
		{
			Class6.yDnXvgqzyB5jw();
			base(rawJson, JTokenType.Raw);
		}

		internal override JToken CloneToken()
		{
			return new JRaw(this);
		}

		public static JRaw Create(JsonReader reader)
		{
			JRaw jRaw;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					jRaw = new JRaw(stringWriter.ToString());
				}
			}
			return jRaw;
		}

		public static async Task<JRaw> CreateAsync(JsonReader reader, CancellationToken cancellationToken = null)
		{
			JRaw.<CreateAsync>d__0 variable = new JRaw.<CreateAsync>d__0();
			variable.reader = reader;
			variable.cancellationToken = cancellationToken;
			variable.<>t__builder = AsyncTaskMethodBuilder<JRaw>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<JRaw.<CreateAsync>d__0>(ref variable);
			return variable.<>t__builder.Task;
		}
	}
}