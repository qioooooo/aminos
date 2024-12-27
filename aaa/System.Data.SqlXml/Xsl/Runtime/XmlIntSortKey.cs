using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C4 RID: 196
	internal class XmlIntSortKey : XmlSortKey
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x0002C406 File Offset: 0x0002B406
		public XmlIntSortKey(int value, XmlCollation collation)
		{
			this.intVal = (collation.DescendingOrder ? (~value) : value);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002C424 File Offset: 0x0002B424
		public override int CompareTo(object obj)
		{
			XmlIntSortKey xmlIntSortKey = obj as XmlIntSortKey;
			if (xmlIntSortKey == null)
			{
				return base.CompareToEmpty(obj);
			}
			if (this.intVal == xmlIntSortKey.intVal)
			{
				return base.BreakSortingTie(xmlIntSortKey);
			}
			if (this.intVal >= xmlIntSortKey.intVal)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x040005C4 RID: 1476
		private int intVal;
	}
}
