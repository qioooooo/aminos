using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C6 RID: 198
	internal class XmlDoubleSortKey : XmlSortKey
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x0002C578 File Offset: 0x0002B578
		public XmlDoubleSortKey(double value, XmlCollation collation)
		{
			if (double.IsNaN(value))
			{
				this.isNaN = true;
				this.dblVal = ((collation.EmptyGreatest != collation.DescendingOrder) ? double.PositiveInfinity : double.NegativeInfinity);
				return;
			}
			this.dblVal = (collation.DescendingOrder ? (-value) : value);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002C5D8 File Offset: 0x0002B5D8
		public override int CompareTo(object obj)
		{
			XmlDoubleSortKey xmlDoubleSortKey = obj as XmlDoubleSortKey;
			if (xmlDoubleSortKey == null)
			{
				if (this.isNaN)
				{
					return base.BreakSortingTie(obj as XmlSortKey);
				}
				return base.CompareToEmpty(obj);
			}
			else if (this.dblVal == xmlDoubleSortKey.dblVal)
			{
				if (this.isNaN)
				{
					if (xmlDoubleSortKey.isNaN)
					{
						return base.BreakSortingTie(xmlDoubleSortKey);
					}
					if (this.dblVal != double.NegativeInfinity)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					if (!xmlDoubleSortKey.isNaN)
					{
						return base.BreakSortingTie(xmlDoubleSortKey);
					}
					if (xmlDoubleSortKey.dblVal != double.NegativeInfinity)
					{
						return -1;
					}
					return 1;
				}
			}
			else
			{
				if (this.dblVal >= xmlDoubleSortKey.dblVal)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x040005C8 RID: 1480
		private double dblVal;

		// Token: 0x040005C9 RID: 1481
		private bool isNaN;
	}
}
