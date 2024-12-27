using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C3 RID: 195
	internal class XmlIntegerSortKey : XmlSortKey
	{
		// Token: 0x06000962 RID: 2402 RVA: 0x0002C3A5 File Offset: 0x0002B3A5
		public XmlIntegerSortKey(long value, XmlCollation collation)
		{
			this.longVal = (collation.DescendingOrder ? (~value) : value);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0002C3C0 File Offset: 0x0002B3C0
		public override int CompareTo(object obj)
		{
			XmlIntegerSortKey xmlIntegerSortKey = obj as XmlIntegerSortKey;
			if (xmlIntegerSortKey == null)
			{
				return base.CompareToEmpty(obj);
			}
			if (this.longVal == xmlIntegerSortKey.longVal)
			{
				return base.BreakSortingTie(xmlIntegerSortKey);
			}
			if (this.longVal >= xmlIntegerSortKey.longVal)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x040005C3 RID: 1475
		private long longVal;
	}
}
