using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C7 RID: 199
	internal class XmlDateTimeSortKey : XmlIntegerSortKey
	{
		// Token: 0x0600096B RID: 2411 RVA: 0x0002C67D File Offset: 0x0002B67D
		public XmlDateTimeSortKey(DateTime value, XmlCollation collation)
			: base(value.Ticks, collation)
		{
		}
	}
}
