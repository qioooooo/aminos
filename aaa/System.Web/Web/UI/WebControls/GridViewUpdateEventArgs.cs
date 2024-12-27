using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005B0 RID: 1456
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewUpdateEventArgs : CancelEventArgs
	{
		// Token: 0x06004754 RID: 18260 RVA: 0x00123E4A File Offset: 0x00122E4A
		public GridViewUpdateEventArgs(int rowIndex)
			: base(false)
		{
			this._rowIndex = rowIndex;
		}

		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x06004755 RID: 18261 RVA: 0x00123E5A File Offset: 0x00122E5A
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

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x06004756 RID: 18262 RVA: 0x00123E75 File Offset: 0x00122E75
		public IOrderedDictionary NewValues
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

		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x06004757 RID: 18263 RVA: 0x00123E90 File Offset: 0x00122E90
		public IOrderedDictionary OldValues
		{
			get
			{
				if (this._oldValues == null)
				{
					this._oldValues = new OrderedDictionary();
				}
				return this._oldValues;
			}
		}

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x06004758 RID: 18264 RVA: 0x00123EAB File Offset: 0x00122EAB
		public int RowIndex
		{
			get
			{
				return this._rowIndex;
			}
		}

		// Token: 0x04002A92 RID: 10898
		private int _rowIndex;

		// Token: 0x04002A93 RID: 10899
		private OrderedDictionary _values;

		// Token: 0x04002A94 RID: 10900
		private OrderedDictionary _keys;

		// Token: 0x04002A95 RID: 10901
		private OrderedDictionary _oldValues;
	}
}
