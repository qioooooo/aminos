using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x020000F3 RID: 243
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class CategoryNameCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000D7D RID: 3453 RVA: 0x000280C3 File Offset: 0x000270C3
		public CategoryNameCollection(CategoryNameCollection value)
		{
			base.InnerList.AddRange(value);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x000280D7 File Offset: 0x000270D7
		public CategoryNameCollection(string[] value)
		{
			base.InnerList.AddRange(value);
		}

		// Token: 0x17000370 RID: 880
		public string this[int index]
		{
			get
			{
				return (string)base.InnerList[index];
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x000280FE File Offset: 0x000270FE
		public bool Contains(string value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002810C File Offset: 0x0002710C
		public void CopyTo(string[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0002811B File Offset: 0x0002711B
		public int IndexOf(string value)
		{
			return base.InnerList.IndexOf(value);
		}
	}
}
