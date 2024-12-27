using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005A7 RID: 1447
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewRowCollection : ICollection, IEnumerable
	{
		// Token: 0x06004724 RID: 18212 RVA: 0x00123C9A File Offset: 0x00122C9A
		public GridViewRowCollection(ArrayList rows)
		{
			this._rows = rows;
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06004725 RID: 18213 RVA: 0x00123CA9 File Offset: 0x00122CA9
		public int Count
		{
			get
			{
				return this._rows.Count;
			}
		}

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x06004726 RID: 18214 RVA: 0x00123CB6 File Offset: 0x00122CB6
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x06004727 RID: 18215 RVA: 0x00123CB9 File Offset: 0x00122CB9
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x06004728 RID: 18216 RVA: 0x00123CBC File Offset: 0x00122CBC
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700118A RID: 4490
		public GridViewRow this[int index]
		{
			get
			{
				return (GridViewRow)this._rows[index];
			}
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x00123CD2 File Offset: 0x00122CD2
		public void CopyTo(GridViewRow[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x00123CDC File Offset: 0x00122CDC
		void ICollection.CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x00123D0C File Offset: 0x00122D0C
		public IEnumerator GetEnumerator()
		{
			return this._rows.GetEnumerator();
		}

		// Token: 0x04002A86 RID: 10886
		private ArrayList _rows;
	}
}
