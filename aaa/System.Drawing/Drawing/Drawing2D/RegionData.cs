using System;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000E0 RID: 224
	public sealed class RegionData
	{
		// Token: 0x06000CFD RID: 3325 RVA: 0x00026C5D File Offset: 0x00025C5D
		internal RegionData(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00026C6C File Offset: 0x00025C6C
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x00026C74 File Offset: 0x00025C74
		public byte[] Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		// Token: 0x04000B0D RID: 2829
		private byte[] data;
	}
}
