using System;
using System.Globalization;

namespace System.Text
{
	// Token: 0x020003E6 RID: 998
	public abstract class DecoderFallbackBuffer
	{
		// Token: 0x060029AA RID: 10666
		public abstract bool Fallback(byte[] bytesUnknown, int index);

		// Token: 0x060029AB RID: 10667
		public abstract char GetNextChar();

		// Token: 0x060029AC RID: 10668
		public abstract bool MovePrevious();

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060029AD RID: 10669
		public abstract int Remaining { get; }

		// Token: 0x060029AE RID: 10670 RVA: 0x00082D4A File Offset: 0x00081D4A
		public virtual void Reset()
		{
			while (this.GetNextChar() != '\0')
			{
			}
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x00082D54 File Offset: 0x00081D54
		internal void InternalReset()
		{
			this.byteStart = null;
			this.Reset();
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x00082D64 File Offset: 0x00081D64
		internal unsafe void InternalInitialize(byte* byteStart, char* charEnd)
		{
			this.byteStart = byteStart;
			this.charEnd = charEnd;
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x00082D74 File Offset: 0x00081D74
		internal unsafe virtual bool InternalFallback(byte[] bytes, byte* pBytes, ref char* chars)
		{
			if (this.Fallback(bytes, (int)((long)(pBytes - this.byteStart) - (long)bytes.Length)))
			{
				char* ptr = chars;
				bool flag = false;
				char nextChar;
				while ((nextChar = this.GetNextChar()) != '\0')
				{
					if (char.IsSurrogate(nextChar))
					{
						if (char.IsHighSurrogate(nextChar))
						{
							if (flag)
							{
								throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
							}
							flag = true;
						}
						else
						{
							if (!flag)
							{
								throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
							}
							flag = false;
						}
					}
					if (ptr >= this.charEnd)
					{
						return false;
					}
					*(ptr++) = nextChar;
				}
				if (flag)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
				}
				chars = ptr;
			}
			return true;
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x00082E14 File Offset: 0x00081E14
		internal unsafe virtual int InternalFallback(byte[] bytes, byte* pBytes)
		{
			if (!this.Fallback(bytes, (int)((long)(pBytes - this.byteStart) - (long)bytes.Length)))
			{
				return 0;
			}
			int num = 0;
			bool flag = false;
			char nextChar;
			while ((nextChar = this.GetNextChar()) != '\0')
			{
				if (char.IsSurrogate(nextChar))
				{
					if (char.IsHighSurrogate(nextChar))
					{
						if (flag)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
						}
						flag = true;
					}
					else
					{
						if (!flag)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
						}
						flag = false;
					}
				}
				num++;
			}
			if (flag)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"));
			}
			return num;
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x00082EA4 File Offset: 0x00081EA4
		internal void ThrowLastBytesRecursive(byte[] bytesUnknown)
		{
			StringBuilder stringBuilder = new StringBuilder(bytesUnknown.Length * 3);
			int num = 0;
			while (num < bytesUnknown.Length && num < 20)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\\x{0:X2}", new object[] { bytesUnknown[num] }));
				num++;
			}
			if (num == 20)
			{
				stringBuilder.Append(" ...");
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_RecursiveFallbackBytes", new object[] { stringBuilder.ToString() }), "bytesUnknown");
		}

		// Token: 0x04001431 RID: 5169
		internal unsafe byte* byteStart = null;

		// Token: 0x04001432 RID: 5170
		internal unsafe char* charEnd = null;
	}
}
