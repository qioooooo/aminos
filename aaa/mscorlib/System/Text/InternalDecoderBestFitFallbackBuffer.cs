using System;
using System.Threading;

namespace System.Text
{
	// Token: 0x020003E7 RID: 999
	internal sealed class InternalDecoderBestFitFallbackBuffer : DecoderFallbackBuffer
	{
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x00082F5C File Offset: 0x00081F5C
		private static object InternalSyncObject
		{
			get
			{
				if (InternalDecoderBestFitFallbackBuffer.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref InternalDecoderBestFitFallbackBuffer.s_InternalSyncObject, obj, null);
				}
				return InternalDecoderBestFitFallbackBuffer.s_InternalSyncObject;
			}
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x00082F88 File Offset: 0x00081F88
		public InternalDecoderBestFitFallbackBuffer(InternalDecoderBestFitFallback fallback)
		{
			this.oFallback = fallback;
			if (this.oFallback.arrayBestFit == null)
			{
				lock (InternalDecoderBestFitFallbackBuffer.InternalSyncObject)
				{
					if (this.oFallback.arrayBestFit == null)
					{
						this.oFallback.arrayBestFit = fallback.encoding.GetBestFitBytesToUnicodeData();
					}
				}
			}
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x00083000 File Offset: 0x00082000
		public override bool Fallback(byte[] bytesUnknown, int index)
		{
			this.cBestFit = this.TryBestFit(bytesUnknown);
			if (this.cBestFit == '\0')
			{
				this.cBestFit = this.oFallback.cReplacement;
			}
			this.iCount = (this.iSize = 1);
			return true;
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x00083044 File Offset: 0x00082044
		public override char GetNextChar()
		{
			this.iCount--;
			if (this.iCount < 0)
			{
				return '\0';
			}
			if (this.iCount == 2147483647)
			{
				this.iCount = -1;
				return '\0';
			}
			return this.cBestFit;
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x0008307B File Offset: 0x0008207B
		public override bool MovePrevious()
		{
			if (this.iCount >= 0)
			{
				this.iCount++;
			}
			return this.iCount >= 0 && this.iCount <= this.iSize;
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x000830B0 File Offset: 0x000820B0
		public override int Remaining
		{
			get
			{
				if (this.iCount <= 0)
				{
					return 0;
				}
				return this.iCount;
			}
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000830C3 File Offset: 0x000820C3
		public override void Reset()
		{
			this.iCount = -1;
			this.byteStart = null;
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000830D4 File Offset: 0x000820D4
		internal unsafe override int InternalFallback(byte[] bytes, byte* pBytes)
		{
			return 1;
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x000830D8 File Offset: 0x000820D8
		private char TryBestFit(byte[] bytesCheck)
		{
			int num = 0;
			int num2 = this.oFallback.arrayBestFit.Length;
			if (num2 == 0)
			{
				return '\0';
			}
			if (bytesCheck.Length == 0 || bytesCheck.Length > 2)
			{
				return '\0';
			}
			char c;
			if (bytesCheck.Length == 1)
			{
				c = (char)bytesCheck[0];
			}
			else
			{
				c = (char)(((int)bytesCheck[0] << 8) + (int)bytesCheck[1]);
			}
			if (c < this.oFallback.arrayBestFit[0] || c > this.oFallback.arrayBestFit[num2 - 2])
			{
				return '\0';
			}
			int num3;
			while ((num3 = num2 - num) > 6)
			{
				int i = (num3 / 2 + num) & 65534;
				char c2 = this.oFallback.arrayBestFit[i];
				if (c2 == c)
				{
					return this.oFallback.arrayBestFit[i + 1];
				}
				if (c2 < c)
				{
					num = i;
				}
				else
				{
					num2 = i;
				}
			}
			for (int i = num; i < num2; i += 2)
			{
				if (this.oFallback.arrayBestFit[i] == c)
				{
					return this.oFallback.arrayBestFit[i + 1];
				}
			}
			return '\0';
		}

		// Token: 0x04001433 RID: 5171
		internal char cBestFit;

		// Token: 0x04001434 RID: 5172
		internal int iCount = -1;

		// Token: 0x04001435 RID: 5173
		internal int iSize;

		// Token: 0x04001436 RID: 5174
		private InternalDecoderBestFitFallback oFallback;

		// Token: 0x04001437 RID: 5175
		private static object s_InternalSyncObject;
	}
}
