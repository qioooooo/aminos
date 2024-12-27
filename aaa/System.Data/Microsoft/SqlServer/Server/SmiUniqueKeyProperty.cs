using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000045 RID: 69
	internal class SmiUniqueKeyProperty : SmiMetaDataProperty
	{
		// Token: 0x06000277 RID: 631 RVA: 0x001CCF74 File Offset: 0x001CC374
		internal SmiUniqueKeyProperty(IList<bool> columnIsKey)
		{
			this._columns = new List<bool>(columnIsKey).AsReadOnly();
		}

		// Token: 0x17000044 RID: 68
		internal bool this[int ordinal]
		{
			get
			{
				return this._columns.Count > ordinal && this._columns[ordinal];
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x001CCFC4 File Offset: 0x001CC3C4
		[Conditional("DEBUG")]
		internal void CheckCount(int countToMatch)
		{
		}

		// Token: 0x0600027A RID: 634 RVA: 0x001CCFD4 File Offset: 0x001CC3D4
		internal override string TraceString()
		{
			string text = "UniqueKey(";
			bool flag = false;
			for (int i = 0; i < this._columns.Count; i++)
			{
				if (flag)
				{
					text += ",";
				}
				else
				{
					flag = true;
				}
				if (this._columns[i])
				{
					text += i.ToString(CultureInfo.InvariantCulture);
				}
			}
			return text + ")";
		}

		// Token: 0x040005E9 RID: 1513
		private IList<bool> _columns;
	}
}
