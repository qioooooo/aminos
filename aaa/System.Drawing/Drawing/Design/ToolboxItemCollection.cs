using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x02000102 RID: 258
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ToolboxItemCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000E06 RID: 3590 RVA: 0x0002931B File Offset: 0x0002831B
		public ToolboxItemCollection(ToolboxItemCollection value)
		{
			base.InnerList.AddRange(value);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0002932F File Offset: 0x0002832F
		public ToolboxItemCollection(ToolboxItem[] value)
		{
			base.InnerList.AddRange(value);
		}

		// Token: 0x1700038D RID: 909
		public ToolboxItem this[int index]
		{
			get
			{
				return (ToolboxItem)base.InnerList[index];
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00029356 File Offset: 0x00028356
		public bool Contains(ToolboxItem value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00029364 File Offset: 0x00028364
		public void CopyTo(ToolboxItem[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00029373 File Offset: 0x00028373
		public int IndexOf(ToolboxItem value)
		{
			return base.InnerList.IndexOf(value);
		}
	}
}
