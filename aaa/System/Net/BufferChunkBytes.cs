using System;

namespace System.Net
{
	// Token: 0x020004C7 RID: 1223
	internal struct BufferChunkBytes : IReadChunkBytes
	{
		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000962BC File Offset: 0x000952BC
		// (set) Token: 0x060025B7 RID: 9655 RVA: 0x000962F9 File Offset: 0x000952F9
		public int NextByte
		{
			get
			{
				if (this.Count != 0)
				{
					this.Count--;
					return (int)this.Buffer[this.Offset++];
				}
				return -1;
			}
			set
			{
				this.Count++;
				this.Offset--;
				this.Buffer[this.Offset] = (byte)value;
			}
		}

		// Token: 0x04002573 RID: 9587
		public byte[] Buffer;

		// Token: 0x04002574 RID: 9588
		public int Offset;

		// Token: 0x04002575 RID: 9589
		public int Count;
	}
}
