using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004B0 RID: 1200
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HtmlTableRowCollection : ICollection, IEnumerable
	{
		// Token: 0x06003833 RID: 14387 RVA: 0x000EFE8D File Offset: 0x000EEE8D
		internal HtmlTableRowCollection(HtmlTable owner)
		{
			this.owner = owner;
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06003834 RID: 14388 RVA: 0x000EFE9C File Offset: 0x000EEE9C
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

		// Token: 0x17000C9A RID: 3226
		public HtmlTableRow this[int index]
		{
			get
			{
				return (HtmlTableRow)this.owner.Controls[index];
			}
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x000EFED5 File Offset: 0x000EEED5
		public void Add(HtmlTableRow row)
		{
			this.Insert(-1, row);
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x000EFEDF File Offset: 0x000EEEDF
		public void Insert(int index, HtmlTableRow row)
		{
			this.owner.Controls.AddAt(index, row);
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x000EFEF3 File Offset: 0x000EEEF3
		public void Clear()
		{
			if (this.owner.HasControls())
			{
				this.owner.Controls.Clear();
			}
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x000EFF14 File Offset: 0x000EEF14
		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x0600383A RID: 14394 RVA: 0x000EFF44 File Offset: 0x000EEF44
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x0600383B RID: 14395 RVA: 0x000EFF47 File Offset: 0x000EEF47
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x0600383C RID: 14396 RVA: 0x000EFF4A File Offset: 0x000EEF4A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000EFF4D File Offset: 0x000EEF4D
		public IEnumerator GetEnumerator()
		{
			return this.owner.Controls.GetEnumerator();
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x000EFF5F File Offset: 0x000EEF5F
		public void Remove(HtmlTableRow row)
		{
			this.owner.Controls.Remove(row);
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000EFF72 File Offset: 0x000EEF72
		public void RemoveAt(int index)
		{
			this.owner.Controls.RemoveAt(index);
		}

		// Token: 0x040025DB RID: 9691
		private HtmlTable owner;
	}
}
