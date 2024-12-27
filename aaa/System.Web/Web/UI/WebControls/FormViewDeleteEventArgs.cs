using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000586 RID: 1414
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewDeleteEventArgs : CancelEventArgs
	{
		// Token: 0x060045C5 RID: 17861 RVA: 0x0011E95F File Offset: 0x0011D95F
		public FormViewDeleteEventArgs(int rowIndex)
			: base(false)
		{
			this._rowIndex = rowIndex;
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x060045C6 RID: 17862 RVA: 0x0011E96F File Offset: 0x0011D96F
		public int RowIndex
		{
			get
			{
				return this._rowIndex;
			}
		}

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x060045C7 RID: 17863 RVA: 0x0011E977 File Offset: 0x0011D977
		public IOrderedDictionary Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new OrderedDictionary();
				}
				return this._keys;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x060045C8 RID: 17864 RVA: 0x0011E992 File Offset: 0x0011D992
		public IOrderedDictionary Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new OrderedDictionary();
				}
				return this._values;
			}
		}

		// Token: 0x04002A13 RID: 10771
		private int _rowIndex;

		// Token: 0x04002A14 RID: 10772
		private OrderedDictionary _keys;

		// Token: 0x04002A15 RID: 10773
		private OrderedDictionary _values;
	}
}
