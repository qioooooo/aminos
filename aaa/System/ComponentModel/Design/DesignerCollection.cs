using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000173 RID: 371
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerCollection : ICollection, IEnumerable
	{
		// Token: 0x06000C05 RID: 3077 RVA: 0x000292E6 File Offset: 0x000282E6
		public DesignerCollection(IDesignerHost[] designers)
		{
			if (designers != null)
			{
				this.designers = new ArrayList(designers);
				return;
			}
			this.designers = new ArrayList();
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00029309 File Offset: 0x00028309
		public DesignerCollection(IList designers)
		{
			this.designers = designers;
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x00029318 File Offset: 0x00028318
		public int Count
		{
			get
			{
				return this.designers.Count;
			}
		}

		// Token: 0x1700026B RID: 619
		public virtual IDesignerHost this[int index]
		{
			get
			{
				return (IDesignerHost)this.designers[index];
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00029338 File Offset: 0x00028338
		public IEnumerator GetEnumerator()
		{
			return this.designers.GetEnumerator();
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00029345 File Offset: 0x00028345
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x0002934D File Offset: 0x0002834D
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00029350 File Offset: 0x00028350
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00029353 File Offset: 0x00028353
		void ICollection.CopyTo(Array array, int index)
		{
			this.designers.CopyTo(array, index);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00029362 File Offset: 0x00028362
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000ACB RID: 2763
		private IList designers;
	}
}
