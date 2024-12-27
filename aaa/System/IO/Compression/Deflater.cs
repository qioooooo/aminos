using System;

namespace System.IO.Compression
{
	// Token: 0x02000200 RID: 512
	internal class Deflater
	{
		// Token: 0x06001163 RID: 4451 RVA: 0x00038D48 File Offset: 0x00037D48
		public Deflater(bool doGZip)
		{
			this.encoder = new FastEncoder(doGZip);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00038D5C File Offset: 0x00037D5C
		public void SetInput(byte[] input, int startIndex, int count)
		{
			this.encoder.SetInput(input, startIndex, count);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00038D6C File Offset: 0x00037D6C
		public int GetDeflateOutput(byte[] output)
		{
			return this.encoder.GetCompressedOutput(output);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00038D7A File Offset: 0x00037D7A
		public bool NeedsInput()
		{
			return this.encoder.NeedsInput();
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00038D87 File Offset: 0x00037D87
		public int Finish(byte[] output)
		{
			return this.encoder.Finish(output);
		}

		// Token: 0x04000FBF RID: 4031
		private FastEncoder encoder;
	}
}
