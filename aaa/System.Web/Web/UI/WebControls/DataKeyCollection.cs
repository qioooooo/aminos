using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200054A RID: 1354
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DataKeyCollection : ICollection, IEnumerable
	{
		// Token: 0x060042AB RID: 17067 RVA: 0x00113F26 File Offset: 0x00112F26
		public DataKeyCollection(ArrayList keys)
		{
			this.keys = keys;
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x060042AC RID: 17068 RVA: 0x00113F35 File Offset: 0x00112F35
		public int Count
		{
			get
			{
				return this.keys.Count;
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x00113F42 File Offset: 0x00112F42
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x060042AE RID: 17070 RVA: 0x00113F45 File Offset: 0x00112F45
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x060042AF RID: 17071 RVA: 0x00113F48 File Offset: 0x00112F48
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700102B RID: 4139
		public object this[int index]
		{
			get
			{
				return this.keys[index];
			}
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x00113F5C File Offset: 0x00112F5C
		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x00113F8C File Offset: 0x00112F8C
		public IEnumerator GetEnumerator()
		{
			return this.keys.GetEnumerator();
		}

		// Token: 0x04002922 RID: 10530
		private ArrayList keys;
	}
}
