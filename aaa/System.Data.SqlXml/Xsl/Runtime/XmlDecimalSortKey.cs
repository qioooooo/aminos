using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C2 RID: 194
	internal class XmlDecimalSortKey : XmlSortKey
	{
		// Token: 0x06000960 RID: 2400 RVA: 0x0002C348 File Offset: 0x0002B348
		public XmlDecimalSortKey(decimal value, XmlCollation collation)
		{
			this.decVal = (collation.DescendingOrder ? (-value) : value);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002C368 File Offset: 0x0002B368
		public override int CompareTo(object obj)
		{
			XmlDecimalSortKey xmlDecimalSortKey = obj as XmlDecimalSortKey;
			if (xmlDecimalSortKey == null)
			{
				return base.CompareToEmpty(obj);
			}
			int num = decimal.Compare(this.decVal, xmlDecimalSortKey.decVal);
			if (num == 0)
			{
				return base.BreakSortingTie(xmlDecimalSortKey);
			}
			return num;
		}

		// Token: 0x040005C2 RID: 1474
		private decimal decVal;
	}
}
