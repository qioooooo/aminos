using System;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x020003F4 RID: 1012
	[Serializable]
	public sealed class EncoderFallbackException : ArgumentException
	{
		// Token: 0x06002A18 RID: 10776 RVA: 0x00084194 File Offset: 0x00083194
		public EncoderFallbackException()
			: base(Environment.GetResourceString("Arg_ArgumentException"))
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000841B1 File Offset: 0x000831B1
		public EncoderFallbackException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000841C5 File Offset: 0x000831C5
		public EncoderFallbackException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000841DA File Offset: 0x000831DA
		internal EncoderFallbackException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000841E4 File Offset: 0x000831E4
		internal EncoderFallbackException(string message, char charUnknown, int index)
			: base(message)
		{
			this.charUnknown = charUnknown;
			this.index = index;
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000841FC File Offset: 0x000831FC
		internal EncoderFallbackException(string message, char charUnknownHigh, char charUnknownLow, int index)
			: base(message)
		{
			if (!char.IsHighSurrogate(charUnknownHigh))
			{
				throw new ArgumentOutOfRangeException("charUnknownHigh", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[] { 55296, 56319 }));
			}
			if (!char.IsLowSurrogate(charUnknownLow))
			{
				throw new ArgumentOutOfRangeException("CharUnknownLow", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[] { 56320, 57343 }));
			}
			this.charUnknownHigh = charUnknownHigh;
			this.charUnknownLow = charUnknownLow;
			this.index = index;
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002A1E RID: 10782 RVA: 0x000842A4 File Offset: 0x000832A4
		public char CharUnknown
		{
			get
			{
				return this.charUnknown;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x000842AC File Offset: 0x000832AC
		public char CharUnknownHigh
		{
			get
			{
				return this.charUnknownHigh;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002A20 RID: 10784 RVA: 0x000842B4 File Offset: 0x000832B4
		public char CharUnknownLow
		{
			get
			{
				return this.charUnknownLow;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x000842BC File Offset: 0x000832BC
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000842C4 File Offset: 0x000832C4
		public bool IsUnknownSurrogate()
		{
			return this.charUnknownHigh != '\0';
		}

		// Token: 0x04001456 RID: 5206
		private char charUnknown;

		// Token: 0x04001457 RID: 5207
		private char charUnknownHigh;

		// Token: 0x04001458 RID: 5208
		private char charUnknownLow;

		// Token: 0x04001459 RID: 5209
		private int index;
	}
}
