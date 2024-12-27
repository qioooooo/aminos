using System;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200006A RID: 106
	internal class WhitespaceRule
	{
		// Token: 0x060006C3 RID: 1731 RVA: 0x00024795 File Offset: 0x00023795
		protected WhitespaceRule()
		{
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002479D File Offset: 0x0002379D
		public WhitespaceRule(string localName, string namespaceName, bool preserveSpace)
		{
			this.Init(localName, namespaceName, preserveSpace);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000247AE File Offset: 0x000237AE
		protected void Init(string localName, string namespaceName, bool preserveSpace)
		{
			this.localName = localName;
			this.namespaceName = namespaceName;
			this.preserveSpace = preserveSpace;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x000247C5 File Offset: 0x000237C5
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x000247CD File Offset: 0x000237CD
		public string LocalName
		{
			get
			{
				return this.localName;
			}
			set
			{
				this.localName = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x000247D6 File Offset: 0x000237D6
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x000247DE File Offset: 0x000237DE
		public string NamespaceName
		{
			get
			{
				return this.namespaceName;
			}
			set
			{
				this.namespaceName = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x000247E7 File Offset: 0x000237E7
		public bool PreserveSpace
		{
			get
			{
				return this.preserveSpace;
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000247EF File Offset: 0x000237EF
		public void GetObjectData(XmlQueryDataWriter writer)
		{
			writer.WriteStringQ(this.localName);
			writer.WriteStringQ(this.namespaceName);
			writer.Write(this.preserveSpace);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00024815 File Offset: 0x00023815
		public WhitespaceRule(XmlQueryDataReader reader)
		{
			this.localName = reader.ReadStringQ();
			this.namespaceName = reader.ReadStringQ();
			this.preserveSpace = reader.ReadBoolean();
		}

		// Token: 0x04000436 RID: 1078
		private string localName;

		// Token: 0x04000437 RID: 1079
		private string namespaceName;

		// Token: 0x04000438 RID: 1080
		private bool preserveSpace;
	}
}
