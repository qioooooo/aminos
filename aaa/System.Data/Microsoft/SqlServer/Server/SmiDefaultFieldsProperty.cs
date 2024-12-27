using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000048 RID: 72
	internal class SmiDefaultFieldsProperty : SmiMetaDataProperty
	{
		// Token: 0x06000280 RID: 640 RVA: 0x001CD18C File Offset: 0x001CC58C
		internal SmiDefaultFieldsProperty(IList<bool> defaultFields)
		{
			this._defaults = new List<bool>(defaultFields).AsReadOnly();
		}

		// Token: 0x17000046 RID: 70
		internal bool this[int ordinal]
		{
			get
			{
				return this._defaults.Count > ordinal && this._defaults[ordinal];
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x001CD1DC File Offset: 0x001CC5DC
		[Conditional("DEBUG")]
		internal void CheckCount(int countToMatch)
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x001CD1EC File Offset: 0x001CC5EC
		internal override string TraceString()
		{
			string text = "DefaultFields(";
			bool flag = false;
			for (int i = 0; i < this._defaults.Count; i++)
			{
				if (flag)
				{
					text += ",";
				}
				else
				{
					flag = true;
				}
				if (this._defaults[i])
				{
					text += i;
				}
			}
			return text + ")";
		}

		// Token: 0x040005ED RID: 1517
		private IList<bool> _defaults;
	}
}
