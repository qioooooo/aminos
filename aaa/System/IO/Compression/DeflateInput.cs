using System;

namespace System.IO.Compression
{
	// Token: 0x02000206 RID: 518
	internal class DeflateInput
	{
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x0003A07E File Offset: 0x0003907E
		// (set) Token: 0x060011A6 RID: 4518 RVA: 0x0003A086 File Offset: 0x00039086
		internal byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
			set
			{
				this.buffer = value;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060011A7 RID: 4519 RVA: 0x0003A08F File Offset: 0x0003908F
		// (set) Token: 0x060011A8 RID: 4520 RVA: 0x0003A097 File Offset: 0x00039097
		internal int Count
		{
			get
			{
				return this.count;
			}
			set
			{
				this.count = value;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x0003A0A0 File Offset: 0x000390A0
		// (set) Token: 0x060011AA RID: 4522 RVA: 0x0003A0A8 File Offset: 0x000390A8
		internal int StartIndex
		{
			get
			{
				return this.startIndex;
			}
			set
			{
				this.startIndex = value;
			}
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0003A0B1 File Offset: 0x000390B1
		internal void ConsumeBytes(int n)
		{
			this.startIndex += n;
			this.count -= n;
		}

		// Token: 0x04000FE5 RID: 4069
		private byte[] buffer;

		// Token: 0x04000FE6 RID: 4070
		private int count;

		// Token: 0x04000FE7 RID: 4071
		private int startIndex;
	}
}
