using System;

namespace System.Drawing.Imaging
{
	// Token: 0x020000DD RID: 221
	public sealed class PropertyItem
	{
		// Token: 0x06000CCC RID: 3276 RVA: 0x00026573 File Offset: 0x00025573
		internal PropertyItem()
		{
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0002657B File Offset: 0x0002557B
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00026583 File Offset: 0x00025583
		public int Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0002658C File Offset: 0x0002558C
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x00026594 File Offset: 0x00025594
		public int Len
		{
			get
			{
				return this.len;
			}
			set
			{
				this.len = value;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0002659D File Offset: 0x0002559D
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x000265A5 File Offset: 0x000255A5
		public short Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x000265AE File Offset: 0x000255AE
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x000265B6 File Offset: 0x000255B6
		public byte[] Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04000AFF RID: 2815
		private int id;

		// Token: 0x04000B00 RID: 2816
		private int len;

		// Token: 0x04000B01 RID: 2817
		private short type;

		// Token: 0x04000B02 RID: 2818
		private byte[] value;
	}
}
