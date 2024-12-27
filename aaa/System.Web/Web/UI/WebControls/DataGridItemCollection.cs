using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200053D RID: 1341
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataGridItemCollection : ICollection, IEnumerable
	{
		// Token: 0x06004223 RID: 16931 RVA: 0x0011218F File Offset: 0x0011118F
		public DataGridItemCollection(ArrayList items)
		{
			this.items = items;
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06004224 RID: 16932 RVA: 0x0011219E File Offset: 0x0011119E
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06004225 RID: 16933 RVA: 0x001121AB File Offset: 0x001111AB
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06004226 RID: 16934 RVA: 0x001121AE File Offset: 0x001111AE
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x06004227 RID: 16935 RVA: 0x001121B1 File Offset: 0x001111B1
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000FFB RID: 4091
		public DataGridItem this[int index]
		{
			get
			{
				return (DataGridItem)this.items[index];
			}
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x001121C8 File Offset: 0x001111C8
		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x001121F8 File Offset: 0x001111F8
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x040028F4 RID: 10484
		private ArrayList items;
	}
}
