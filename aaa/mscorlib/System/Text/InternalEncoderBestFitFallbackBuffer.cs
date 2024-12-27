using System;
using System.Threading;

namespace System.Text
{
	// Token: 0x020003F1 RID: 1009
	internal sealed class InternalEncoderBestFitFallbackBuffer : EncoderFallbackBuffer
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002A04 RID: 10756 RVA: 0x00083DA8 File Offset: 0x00082DA8
		private static object InternalSyncObject
		{
			get
			{
				if (InternalEncoderBestFitFallbackBuffer.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref InternalEncoderBestFitFallbackBuffer.s_InternalSyncObject, obj, null);
				}
				return InternalEncoderBestFitFallbackBuffer.s_InternalSyncObject;
			}
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x00083DD4 File Offset: 0x00082DD4
		public InternalEncoderBestFitFallbackBuffer(InternalEncoderBestFitFallback fallback)
		{
			this.oFallback = fallback;
			if (this.oFallback.arrayBestFit == null)
			{
				lock (InternalEncoderBestFitFallbackBuffer.InternalSyncObject)
				{
					if (this.oFallback.arrayBestFit == null)
					{
						this.oFallback.arrayBestFit = fallback.encoding.GetBestFitUnicodeToBytesData();
					}
				}
			}
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x00083E4C File Offset: 0x00082E4C
		public override bool Fallback(char charUnknown, int index)
		{
			this.iCount = (this.iSize = 1);
			this.cBestFit = this.TryBestFit(charUnknown);
			if (this.cBestFit == '\0')
			{
				this.cBestFit = '?';
			}
			return true;
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x00083E88 File Offset: 0x00082E88
		public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
		{
			if (!char.IsHighSurrogate(charUnknownHigh))
			{
				throw new ArgumentOutOfRangeException("charUnknownHigh", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[] { 55296, 56319 }));
			}
			if (!char.IsLowSurrogate(charUnknownLow))
			{
				throw new ArgumentOutOfRangeException("CharUnknownLow", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[] { 56320, 57343 }));
			}
			this.cBestFit = '?';
			this.iCount = (this.iSize = 2);
			return true;
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x00083F2C File Offset: 0x00082F2C
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

		// Token: 0x06002A09 RID: 10761 RVA: 0x00083F63 File Offset: 0x00082F63
		public override bool MovePrevious()
		{
			if (this.iCount >= 0)
			{
				this.iCount++;
			}
			return this.iCount >= 0 && this.iCount <= this.iSize;
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002A0A RID: 10762 RVA: 0x00083F98 File Offset: 0x00082F98
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

		// Token: 0x06002A0B RID: 10763 RVA: 0x00083FAB File Offset: 0x00082FAB
		public override void Reset()
		{
			this.iCount = -1;
			this.charStart = null;
			this.bFallingBack = false;
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x00083FC4 File Offset: 0x00082FC4
		private char TryBestFit(char cUnknown)
		{
			int num = 0;
			int num2 = this.oFallback.arrayBestFit.Length;
			int num3;
			while ((num3 = num2 - num) > 6)
			{
				int i = (num3 / 2 + num) & 65534;
				char c = this.oFallback.arrayBestFit[i];
				if (c == cUnknown)
				{
					return this.oFallback.arrayBestFit[i + 1];
				}
				if (c < cUnknown)
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
				if (this.oFallback.arrayBestFit[i] == cUnknown)
				{
					return this.oFallback.arrayBestFit[i + 1];
				}
			}
			return '\0';
		}

		// Token: 0x04001451 RID: 5201
		private char cBestFit;

		// Token: 0x04001452 RID: 5202
		private InternalEncoderBestFitFallback oFallback;

		// Token: 0x04001453 RID: 5203
		private int iCount = -1;

		// Token: 0x04001454 RID: 5204
		private int iSize;

		// Token: 0x04001455 RID: 5205
		private static object s_InternalSyncObject;
	}
}
