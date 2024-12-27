using System;

namespace System.Xml.Xsl
{
	// Token: 0x02000007 RID: 7
	internal struct StringPair
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002317 File Offset: 0x00001317
		public StringPair(string left, string right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002327 File Offset: 0x00001327
		public string Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000232F File Offset: 0x0000132F
		public string Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x040000AC RID: 172
		private string left;

		// Token: 0x040000AD RID: 173
		private string right;
	}
}
