using System;

namespace System.Xml
{
	// Token: 0x020000E1 RID: 225
	public class XmlImplementation
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x0003C64B File Offset: 0x0003B64B
		public XmlImplementation()
			: this(new NameTable())
		{
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0003C658 File Offset: 0x0003B658
		public XmlImplementation(XmlNameTable nt)
		{
			this.nameTable = nt;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0003C667 File Offset: 0x0003B667
		public bool HasFeature(string strFeature, string strVersion)
		{
			return string.Compare("XML", strFeature, StringComparison.OrdinalIgnoreCase) == 0 && (strVersion == null || strVersion == "1.0" || strVersion == "2.0");
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0003C697 File Offset: 0x0003B697
		public virtual XmlDocument CreateDocument()
		{
			return new XmlDocument(this);
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0003C69F File Offset: 0x0003B69F
		internal XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x04000968 RID: 2408
		private XmlNameTable nameTable;
	}
}
