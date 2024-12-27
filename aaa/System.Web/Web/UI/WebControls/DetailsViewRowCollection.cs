using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000569 RID: 1385
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DetailsViewRowCollection : ICollection, IEnumerable
	{
		// Token: 0x06004439 RID: 17465 RVA: 0x00119948 File Offset: 0x00118948
		public DetailsViewRowCollection(ArrayList rows)
		{
			this._rows = rows;
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x0600443A RID: 17466 RVA: 0x00119957 File Offset: 0x00118957
		public int Count
		{
			get
			{
				return this._rows.Count;
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x0600443B RID: 17467 RVA: 0x00119964 File Offset: 0x00118964
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x0600443C RID: 17468 RVA: 0x00119967 File Offset: 0x00118967
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x0600443D RID: 17469 RVA: 0x0011996A File Offset: 0x0011896A
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170010B0 RID: 4272
		public DetailsViewRow this[int index]
		{
			get
			{
				return (DetailsViewRow)this._rows[index];
			}
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x00119980 File Offset: 0x00118980
		public void CopyTo(DetailsViewRow[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0011998C File Offset: 0x0011898C
		void ICollection.CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x001199BC File Offset: 0x001189BC
		public IEnumerator GetEnumerator()
		{
			return this._rows.GetEnumerator();
		}

		// Token: 0x040029A6 RID: 10662
		private ArrayList _rows;
	}
}
