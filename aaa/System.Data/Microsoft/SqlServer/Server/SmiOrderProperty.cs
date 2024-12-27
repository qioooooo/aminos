using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000046 RID: 70
	internal class SmiOrderProperty : SmiMetaDataProperty
	{
		// Token: 0x0600027B RID: 635 RVA: 0x001CD040 File Offset: 0x001CC440
		internal SmiOrderProperty(IList<SmiOrderProperty.SmiColumnOrder> columnOrders)
		{
			this._columns = new List<SmiOrderProperty.SmiColumnOrder>(columnOrders).AsReadOnly();
		}

		// Token: 0x17000045 RID: 69
		internal SmiOrderProperty.SmiColumnOrder this[int ordinal]
		{
			get
			{
				if (this._columns.Count <= ordinal)
				{
					return new SmiOrderProperty.SmiColumnOrder
					{
						Order = SortOrder.Unspecified,
						SortOrdinal = -1
					};
				}
				return this._columns[ordinal];
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x001CD0A8 File Offset: 0x001CC4A8
		[Conditional("DEBUG")]
		internal void CheckCount(int countToMatch)
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x001CD0B8 File Offset: 0x001CC4B8
		internal override string TraceString()
		{
			string text = "SortOrder(";
			bool flag = false;
			foreach (SmiOrderProperty.SmiColumnOrder smiColumnOrder in this._columns)
			{
				if (flag)
				{
					text += ",";
				}
				else
				{
					flag = true;
				}
				if (SortOrder.Unspecified != smiColumnOrder.Order)
				{
					text += smiColumnOrder.TraceString();
				}
			}
			text += ")";
			return text;
		}

		// Token: 0x040005EA RID: 1514
		private IList<SmiOrderProperty.SmiColumnOrder> _columns;

		// Token: 0x02000047 RID: 71
		internal struct SmiColumnOrder
		{
			// Token: 0x0600027F RID: 639 RVA: 0x001CD14C File Offset: 0x001CC54C
			internal string TraceString()
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}", new object[] { this.SortOrdinal, this.Order });
			}

			// Token: 0x040005EB RID: 1515
			internal int SortOrdinal;

			// Token: 0x040005EC RID: 1516
			internal SortOrder Order;
		}
	}
}
