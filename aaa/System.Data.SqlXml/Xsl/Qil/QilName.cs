using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000056 RID: 86
	internal class QilName : QilLiteral
	{
		// Token: 0x0600057B RID: 1403 RVA: 0x000214A6 File Offset: 0x000204A6
		public QilName(QilNodeType nodeType, string local, string uri, string prefix)
			: base(nodeType, null)
		{
			this.LocalName = local;
			this.NamespaceUri = uri;
			this.Prefix = prefix;
			base.Value = this;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x000214CD File Offset: 0x000204CD
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x000214D5 File Offset: 0x000204D5
		public string LocalName
		{
			get
			{
				return this.local;
			}
			set
			{
				this.local = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x000214DE File Offset: 0x000204DE
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x000214E6 File Offset: 0x000204E6
		public string NamespaceUri
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x000214EF File Offset: 0x000204EF
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x000214F7 File Offset: 0x000204F7
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00021500 File Offset: 0x00020500
		public string QualifiedName
		{
			get
			{
				if (this.prefix.Length == 0)
				{
					return this.local;
				}
				return this.prefix + ':' + this.local;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0002152E File Offset: 0x0002052E
		public override int GetHashCode()
		{
			return this.local.GetHashCode();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0002153C File Offset: 0x0002053C
		public override bool Equals(object other)
		{
			QilName qilName = other as QilName;
			return qilName != null && this.local == qilName.local && this.uri == qilName.uri;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0002157C File Offset: 0x0002057C
		public override string ToString()
		{
			if (this.prefix.Length != 0)
			{
				return string.Concat(new string[] { "{", this.uri, "}", this.prefix, ":", this.local });
			}
			if (this.uri.Length == 0)
			{
				return this.local;
			}
			return "{" + this.uri + "}" + this.local;
		}

		// Token: 0x04000399 RID: 921
		private string local;

		// Token: 0x0400039A RID: 922
		private string uri;

		// Token: 0x0400039B RID: 923
		private string prefix;
	}
}
