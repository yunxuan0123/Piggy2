using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Utilities
{
	internal class Base64Encoder
	{
		private readonly char[] _charsLine;

		private readonly TextWriter _writer;

		private byte[] _leftOverBytes;

		private int _leftOverBytesCount;

		public Base64Encoder(TextWriter writer)
		{
			Class6.yDnXvgqzyB5jw();
			this._charsLine = new char[76];
			base();
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = writer;
		}

		public void Encode(byte[] buffer, int index, int count)
		{
			this.ValidateEncode(buffer, index, count);
			if (this._leftOverBytesCount > 0)
			{
				if (this.FulfillFromLeftover(buffer, index, ref count))
				{
					return;
				}
				int base64CharArray = Convert.ToBase64CharArray(this._leftOverBytes, 0, 3, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, base64CharArray);
			}
			this.StoreLeftOverBytes(buffer, index, ref count);
			int num = index + count;
			int num1 = 57;
			while (index < num)
			{
				if (index + num1 > num)
				{
					num1 = num - index;
				}
				int base64CharArray1 = Convert.ToBase64CharArray(buffer, index, num1, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, base64CharArray1);
				index += num1;
			}
		}

		public async Task EncodeAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable;
			this.ValidateEncode(buffer, index, count);
			if (this._leftOverBytesCount > 0)
			{
				if (!this.FulfillFromLeftover(buffer, index, ref count))
				{
					int base64CharArray = Convert.ToBase64CharArray(this._leftOverBytes, 0, 3, this._charsLine, 0);
					configuredTaskAwaitable = this.WriteCharsAsync(this._charsLine, 0, base64CharArray, cancellationToken).ConfigureAwait(false);
					await configuredTaskAwaitable;
				}
				else
				{
					return;
				}
			}
			this.StoreLeftOverBytes(buffer, index, ref count);
			int num = index + count;
			int num1 = 57;
			while (index < num)
			{
				if (index + num1 > num)
				{
					num1 = num - index;
				}
				int base64CharArray1 = Convert.ToBase64CharArray(buffer, index, num1, this._charsLine, 0);
				configuredTaskAwaitable = this.WriteCharsAsync(this._charsLine, 0, base64CharArray1, cancellationToken).ConfigureAwait(false);
				await configuredTaskAwaitable;
				index += num1;
			}
		}

		public void Flush()
		{
			if (this._leftOverBytesCount > 0)
			{
				int base64CharArray = Convert.ToBase64CharArray(this._leftOverBytes, 0, this._leftOverBytesCount, this._charsLine, 0);
				this.WriteChars(this._charsLine, 0, base64CharArray);
				this._leftOverBytesCount = 0;
			}
		}

		public Task FlushAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			if (this._leftOverBytesCount <= 0)
			{
				return AsyncUtils.CompletedTask;
			}
			int base64CharArray = Convert.ToBase64CharArray(this._leftOverBytes, 0, this._leftOverBytesCount, this._charsLine, 0);
			this._leftOverBytesCount = 0;
			return this.WriteCharsAsync(this._charsLine, 0, base64CharArray, cancellationToken);
		}

		private bool FulfillFromLeftover(byte[] buffer, int index, ref int count)
		{
			int num = this._leftOverBytesCount;
			while (num < 3 && count > 0)
			{
				int num1 = num;
				num = num1 + 1;
				int num2 = index;
				index = num2 + 1;
				this._leftOverBytes[num1] = buffer[num2];
				count--;
			}
			if (count != 0 || num >= 3)
			{
				return false;
			}
			this._leftOverBytesCount = num;
			return true;
		}

		private void StoreLeftOverBytes(byte[] buffer, int index, ref int count)
		{
			int num = count % 3;
			if (num > 0)
			{
				count -= num;
				if (this._leftOverBytes == null)
				{
					this._leftOverBytes = new byte[3];
				}
				for (int i = 0; i < num; i++)
				{
					this._leftOverBytes[i] = buffer[index + count + i];
				}
			}
			this._leftOverBytesCount = num;
		}

		private void ValidateEncode(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > (int)buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
		}

		private void WriteChars(char[] chars, int index, int count)
		{
			this._writer.Write(chars, index, count);
		}

		private Task WriteCharsAsync(char[] chars, int index, int count, CancellationToken cancellationToken)
		{
			return this._writer.WriteAsync(chars, index, count, cancellationToken);
		}
	}
}