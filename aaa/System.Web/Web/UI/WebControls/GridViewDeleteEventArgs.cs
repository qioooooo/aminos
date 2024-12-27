using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005A0 RID: 1440
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewDeleteEventArgs : CancelEventArgs
	{
		// Token: 0x06004701 RID: 18177 RVA: 0x00123B4F File Offset: 0x00122B4F
		public GridViewDeleteEventArgs(int rowIndex)
			: base(false)
		{
			this._rowIndex = rowIndex;
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x06004702 RID: 18178 RVA: 0x00123B5F File Offset: 0x00122B5F
		public int RowIndex
		{
			get
			{
				return this._rowIndex;
			}
		}

		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x06004703 RID: 18179 RVA: 0x00123B67 File Offset: 0x00122B67
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

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x06004704 RID: 18180 RVA: 0x00123B82 File Offset: 0x00122B82
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

		// Token: 0x04002A7C RID: 10876
		private int _rowIndex;

		// Token: 0x04002A7D RID: 10877
		private OrderedDictionary _keys;

		// Token: 0x04002A7E RID: 10878
		private OrderedDictionary _values;
	}
}
