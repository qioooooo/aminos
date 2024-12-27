using System;

namespace System.Drawing.Imaging
{
	// Token: 0x02000084 RID: 132
	public sealed class Encoder
	{
		// Token: 0x0600078C RID: 1932 RVA: 0x0001CB25 File Offset: 0x0001BB25
		public Encoder(Guid guid)
		{
			this.guid = guid;
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0001CB34 File Offset: 0x0001BB34
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x04000603 RID: 1539
		public static readonly Encoder Compression = new Encoder(new Guid(-526552163, -13100, 17646, new byte[] { 142, 186, 63, 191, 139, 228, 252, 88 }));

		// Token: 0x04000604 RID: 1540
		public static readonly Encoder ColorDepth = new Encoder(new Guid(1711829077, -21146, 19580, new byte[] { 154, 24, 56, 162, 49, 11, 131, 55 }));

		// Token: 0x04000605 RID: 1541
		public static readonly Encoder ScanMethod = new Encoder(new Guid(978200161, 12553, 20054, new byte[] { 133, 54, 66, 193, 86, 231, 220, 250 }));

		// Token: 0x04000606 RID: 1542
		public static readonly Encoder Version = new Encoder(new Guid(617712758, -32438, 16804, new byte[] { 191, 83, 28, 33, 156, 204, 247, 151 }));

		// Token: 0x04000607 RID: 1543
		public static readonly Encoder RenderMethod = new Encoder(new Guid(1833092410, 8858, 18469, new byte[] { 139, 183, 92, 153, 226, 185, 168, 184 }));

		// Token: 0x04000608 RID: 1544
		public static readonly Encoder Quality = new Encoder(new Guid(492561589, -1462, 17709, new byte[] { 156, 221, 93, 179, 81, 5, 231, 235 }));

		// Token: 0x04000609 RID: 1545
		public static readonly Encoder Transformation = new Encoder(new Guid(-1928416559, -23154, 20136, new byte[] { 170, 20, 16, 128, 116, 183, 182, 249 }));

		// Token: 0x0400060A RID: 1546
		public static readonly Encoder LuminanceTable = new Encoder(new Guid(-307020850, 614, 19063, new byte[] { 185, 4, 39, 33, 96, 153, 231, 23 }));

		// Token: 0x0400060B RID: 1547
		public static readonly Encoder ChrominanceTable = new Encoder(new Guid(-219916836, 2483, 17174, new byte[] { 130, 96, 103, 106, 218, 50, 72, 28 }));

		// Token: 0x0400060C RID: 1548
		public static readonly Encoder SaveFlag = new Encoder(new Guid(690120444, -21440, 18367, new byte[] { 140, 252, 168, 91, 137, 166, 85, 222 }));

		// Token: 0x0400060D RID: 1549
		private Guid guid;
	}
}
