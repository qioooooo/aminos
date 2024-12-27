using System;
using System.Globalization;

namespace System.Text
{
	// Token: 0x020003E9 RID: 1001
	public sealed class DecoderExceptionFallbackBuffer : DecoderFallbackBuffer
	{
		// Token: 0x060029C3 RID: 10691 RVA: 0x000831E9 File Offset: 0x000821E9
		public override bool Fallback(byte[] bytesUnknown, int index)
		{
			this.Throw(bytesUnknown, index);
			return true;
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000831F4 File Offset: 0x000821F4
		public override char GetNextChar()
		{
			return '\0';
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000831F7 File Offset: 0x000821F7
		public override bool MovePrevious()
		{
			return false;
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060029C6 RID: 10694 RVA: 0x000831FA File Offset: 0x000821FA
		public override int Remaining
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x00083200 File Offset: 0x00082200
		private void Throw(byte[] bytesUnknown, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(bytesUnknown.Length * 3);
			int num = 0;
			while (num < bytesUnknown.Length && num < 20)
			{
				stringBuilder.Append("[");
				stringBuilder.Append(bytesUnknown[num].ToString("X2", CultureInfo.InvariantCulture));
				stringBuilder.Append("]");
				num++;
			}
			if (num == 20)
			{
				stringBuilder.Append(" ...");
			}
			throw new DecoderFallbackException(Environment.GetResourceString("Argument_InvalidCodePageBytesIndex", new object[] { stringBuilder, index }), bytesUnknown, index);
		}
	}
}
