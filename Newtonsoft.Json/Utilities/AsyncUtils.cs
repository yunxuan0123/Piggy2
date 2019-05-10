using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Utilities
{
	internal static class AsyncUtils
	{
		public readonly static Task<bool> False;

		public readonly static Task<bool> True;

		internal readonly static Task CompletedTask;

		static AsyncUtils()
		{
			Class6.yDnXvgqzyB5jw();
			AsyncUtils.False = Task.FromResult<bool>(false);
			AsyncUtils.True = Task.FromResult<bool>(true);
			AsyncUtils.CompletedTask = Task.Delay(0);
		}

		public static Task CancelIfRequestedAsync(this CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			return cancellationToken.FromCanceled();
		}

		public static Task<T> CancelIfRequestedAsync<T>(this CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			return cancellationToken.FromCanceled<T>();
		}

		public static Task FromCanceled(this CancellationToken cancellationToken)
		{
			return new Task(() => {
			}, cancellationToken);
		}

		public static Task<T> FromCanceled<T>(this CancellationToken cancellationToken)
		{
			return new Task<T>(() => default(T), cancellationToken);
		}

		public static bool IsCompletedSucessfully(this Task task)
		{
			return task.Status == TaskStatus.RanToCompletion;
		}

		public static Task<int> ReadAsync(this TextReader reader, char[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled<int>();
			}
			return reader.ReadAsync(buffer, index, count);
		}

		internal static Task<bool> ToAsync(this bool value)
		{
			if (!value)
			{
				return AsyncUtils.False;
			}
			return AsyncUtils.True;
		}

		public static Task WriteAsync(this TextWriter writer, char value, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			return writer.WriteAsync(value);
		}

		public static Task WriteAsync(this TextWriter writer, string value, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			return writer.WriteAsync(value);
		}

		public static Task WriteAsync(this TextWriter writer, char[] value, int start, int count, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			return writer.WriteAsync(value, start, count);
		}
	}
}