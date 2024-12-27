using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000485 RID: 1157
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ValidatorCollection : ICollection, IEnumerable
	{
		// Token: 0x0600366E RID: 13934 RVA: 0x000EB453 File Offset: 0x000EA453
		public ValidatorCollection()
		{
			this.data = new ArrayList();
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x000EB466 File Offset: 0x000EA466
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x17000C17 RID: 3095
		public IValidator this[int index]
		{
			get
			{
				return (IValidator)this.data[index];
			}
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x000EB486 File Offset: 0x000EA486
		public void Add(IValidator validator)
		{
			this.data.Add(validator);
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x000EB495 File Offset: 0x000EA495
		public bool Contains(IValidator validator)
		{
			return this.data.Contains(validator);
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x000EB4A3 File Offset: 0x000EA4A3
		public void Remove(IValidator validator)
		{
			this.data.Remove(validator);
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x000EB4B1 File Offset: 0x000EA4B1
		public IEnumerator GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x000EB4C0 File Offset: 0x000EA4C0
		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06003676 RID: 13942 RVA: 0x000EB4F0 File Offset: 0x000EA4F0
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06003677 RID: 13943 RVA: 0x000EB4F3 File Offset: 0x000EA4F3
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x000EB4F6 File Offset: 0x000EA4F6
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04002583 RID: 9603
		private ArrayList data;
	}
}
