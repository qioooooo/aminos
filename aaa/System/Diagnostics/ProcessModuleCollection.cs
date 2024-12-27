using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000787 RID: 1927
	public class ProcessModuleCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06003B69 RID: 15209 RVA: 0x000FD97E File Offset: 0x000FC97E
		protected ProcessModuleCollection()
		{
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x000FD986 File Offset: 0x000FC986
		public ProcessModuleCollection(ProcessModule[] processModules)
		{
			base.InnerList.AddRange(processModules);
		}

		// Token: 0x17000DE8 RID: 3560
		public ProcessModule this[int index]
		{
			get
			{
				return (ProcessModule)base.InnerList[index];
			}
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x000FD9AD File Offset: 0x000FC9AD
		public int IndexOf(ProcessModule module)
		{
			return base.InnerList.IndexOf(module);
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x000FD9BB File Offset: 0x000FC9BB
		public bool Contains(ProcessModule module)
		{
			return base.InnerList.Contains(module);
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x000FD9C9 File Offset: 0x000FC9C9
		public void CopyTo(ProcessModule[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}
