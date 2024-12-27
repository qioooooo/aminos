using System;
using System.Globalization;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C5 RID: 197
	internal class XmlStringSortKey : XmlSortKey
	{
		// Token: 0x06000966 RID: 2406 RVA: 0x0002C46A File Offset: 0x0002B46A
		public XmlStringSortKey(SortKey sortKey, bool descendingOrder)
		{
			this.sortKey = sortKey;
			this.descendingOrder = descendingOrder;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0002C480 File Offset: 0x0002B480
		public XmlStringSortKey(byte[] sortKey, bool descendingOrder)
		{
			this.sortKeyBytes = sortKey;
			this.descendingOrder = descendingOrder;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002C498 File Offset: 0x0002B498
		public override int CompareTo(object obj)
		{
			XmlStringSortKey xmlStringSortKey = obj as XmlStringSortKey;
			if (xmlStringSortKey == null)
			{
				return base.CompareToEmpty(obj);
			}
			int num;
			if (this.sortKey != null)
			{
				num = SortKey.Compare(this.sortKey, xmlStringSortKey.sortKey);
			}
			else
			{
				int num2 = ((this.sortKeyBytes.Length < xmlStringSortKey.sortKeyBytes.Length) ? this.sortKeyBytes.Length : xmlStringSortKey.sortKeyBytes.Length);
				for (int i = 0; i < num2; i++)
				{
					if (this.sortKeyBytes[i] < xmlStringSortKey.sortKeyBytes[i])
					{
						num = -1;
						goto IL_00BC;
					}
					if (this.sortKeyBytes[i] > xmlStringSortKey.sortKeyBytes[i])
					{
						num = 1;
						goto IL_00BC;
					}
				}
				if (this.sortKeyBytes.Length < xmlStringSortKey.sortKeyBytes.Length)
				{
					num = -1;
				}
				else if (this.sortKeyBytes.Length > xmlStringSortKey.sortKeyBytes.Length)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
			}
			IL_00BC:
			if (num == 0)
			{
				return base.BreakSortingTie(xmlStringSortKey);
			}
			if (!this.descendingOrder)
			{
				return num;
			}
			return -num;
		}

		// Token: 0x040005C5 RID: 1477
		private SortKey sortKey;

		// Token: 0x040005C6 RID: 1478
		private byte[] sortKeyBytes;

		// Token: 0x040005C7 RID: 1479
		private bool descendingOrder;
	}
}
