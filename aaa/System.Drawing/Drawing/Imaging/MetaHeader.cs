using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000D1 RID: 209
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public sealed class MetaHeader
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x000256AB File Offset: 0x000246AB
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x000256B3 File Offset: 0x000246B3
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

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x000256BC File Offset: 0x000246BC
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x000256C4 File Offset: 0x000246C4
		public short HeaderSize
		{
			get
			{
				return this.headerSize;
			}
			set
			{
				this.headerSize = value;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x000256CD File Offset: 0x000246CD
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x000256D5 File Offset: 0x000246D5
		public short Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x000256DE File Offset: 0x000246DE
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x000256E6 File Offset: 0x000246E6
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x000256EF File Offset: 0x000246EF
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x000256F7 File Offset: 0x000246F7
		public short NoObjects
		{
			get
			{
				return this.noObjects;
			}
			set
			{
				this.noObjects = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00025700 File Offset: 0x00024700
		// (set) Token: 0x06000C77 RID: 3191 RVA: 0x00025708 File Offset: 0x00024708
		public int MaxRecord
		{
			get
			{
				return this.maxRecord;
			}
			set
			{
				this.maxRecord = value;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00025711 File Offset: 0x00024711
		// (set) Token: 0x06000C79 RID: 3193 RVA: 0x00025719 File Offset: 0x00024719
		public short NoParameters
		{
			get
			{
				return this.noParameters;
			}
			set
			{
				this.noParameters = value;
			}
		}

		// Token: 0x04000ABB RID: 2747
		private short type;

		// Token: 0x04000ABC RID: 2748
		private short headerSize;

		// Token: 0x04000ABD RID: 2749
		private short version;

		// Token: 0x04000ABE RID: 2750
		private int size;

		// Token: 0x04000ABF RID: 2751
		private short noObjects;

		// Token: 0x04000AC0 RID: 2752
		private int maxRecord;

		// Token: 0x04000AC1 RID: 2753
		private short noParameters;
	}
}
