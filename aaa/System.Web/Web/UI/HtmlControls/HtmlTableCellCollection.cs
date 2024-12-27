using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004AD RID: 1197
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HtmlTableCellCollection : ICollection, IEnumerable
	{
		// Token: 0x06003810 RID: 14352 RVA: 0x000EFA67 File Offset: 0x000EEA67
		internal HtmlTableCellCollection(HtmlTableRow owner)
		{
			this.owner = owner;
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06003811 RID: 14353 RVA: 0x000EFA76 File Offset: 0x000EEA76
		public int Count
		{
			get
			{
				if (this.owner.HasControls())
				{
					return this.owner.Controls.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000C8D RID: 3213
		public HtmlTableCell this[int index]
		{
			get
			{
				return (HtmlTableCell)this.owner.Controls[index];
			}
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x000EFAAF File Offset: 0x000EEAAF
		public void Add(HtmlTableCell cell)
		{
			this.Insert(-1, cell);
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x000EFAB9 File Offset: 0x000EEAB9
		public void Insert(int index, HtmlTableCell cell)
		{
			this.owner.Controls.AddAt(index, cell);
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x000EFACD File Offset: 0x000EEACD
		public void Clear()
		{
			if (this.owner.HasControls())
			{
				this.owner.Controls.Clear();
			}
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x000EFAEC File Offset: 0x000EEAEC
		public IEnumerator GetEnumerator()
		{
			return this.owner.Controls.GetEnumerator();
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x000EFB00 File Offset: 0x000EEB00
		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06003818 RID: 14360 RVA: 0x000EFB30 File Offset: 0x000EEB30
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06003819 RID: 14361 RVA: 0x000EFB33 File Offset: 0x000EEB33
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x0600381A RID: 14362 RVA: 0x000EFB36 File Offset: 0x000EEB36
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x000EFB39 File Offset: 0x000EEB39
		public void Remove(HtmlTableCell cell)
		{
			this.owner.Controls.Remove(cell);
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x000EFB4C File Offset: 0x000EEB4C
		public void RemoveAt(int index)
		{
			this.owner.Controls.RemoveAt(index);
		}

		// Token: 0x040025D9 RID: 9689
		private HtmlTableRow owner;
	}
}
