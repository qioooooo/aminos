using System;

namespace System.Net
{
	// Token: 0x02000682 RID: 1666
	internal class BufferBuilder
	{
		// Token: 0x06003390 RID: 13200 RVA: 0x000D9CF0 File Offset: 0x000D8CF0
		internal BufferBuilder()
			: this(256)
		{
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x000D9CFD File Offset: 0x000D8CFD
		internal BufferBuilder(int initialSize)
		{
			this.buffer = new byte[initialSize];
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x000D9D14 File Offset: 0x000D8D14
		private void EnsureBuffer(int count)
		{
			if (count > this.buffer.Length - this.offset)
			{
				byte[] array = new byte[(this.buffer.Length * 2 > this.buffer.Length + count) ? (this.buffer.Length * 2) : (this.buffer.Length + count)];
				Buffer.BlockCopy(this.buffer, 0, array, 0, this.offset);
				this.buffer = array;
			}
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x000D9D80 File Offset: 0x000D8D80
		internal void Append(byte value)
		{
			this.EnsureBuffer(1);
			this.buffer[this.offset++] = value;
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x000D9DAD File Offset: 0x000D8DAD
		internal void Append(byte[] value)
		{
			this.Append(value, 0, value.Length);
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x000D9DBA File Offset: 0x000D8DBA
		internal void Append(byte[] value, int offset, int count)
		{
			this.EnsureBuffer(count);
			Buffer.BlockCopy(value, offset, this.buffer, this.offset, count);
			this.offset += count;
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x000D9DE5 File Offset: 0x000D8DE5
		internal void Append(string value)
		{
			this.Append(value, 0, value.Length);
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x000D9DF8 File Offset: 0x000D8DF8
		internal void Append(string value, int offset, int count)
		{
			this.EnsureBuffer(count);
			for (int i = 0; i < count; i++)
			{
				char c = value[offset + i];
				if (c > 'ÿ')
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				this.buffer[this.offset + i] = (byte)c;
			}
			this.offset += count;
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06003398 RID: 13208 RVA: 0x000D9E59 File Offset: 0x000D8E59
		internal int Length
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x000D9E61 File Offset: 0x000D8E61
		internal byte[] GetBuffer()
		{
			return this.buffer;
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x000D9E69 File Offset: 0x000D8E69
		internal void Reset()
		{
			this.offset = 0;
		}

		// Token: 0x04002F9F RID: 12191
		private byte[] buffer;

		// Token: 0x04002FA0 RID: 12192
		private int offset;
	}
}
