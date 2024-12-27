using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200008B RID: 139
	public class ApplicationPartitionCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000445 RID: 1093 RVA: 0x00017DA0 File Offset: 0x00016DA0
		internal ApplicationPartitionCollection()
		{
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00017DA8 File Offset: 0x00016DA8
		internal ApplicationPartitionCollection(ArrayList values)
		{
			if (values != null)
			{
				base.InnerList.AddRange(values);
			}
		}

		// Token: 0x17000108 RID: 264
		public ApplicationPartition this[int index]
		{
			get
			{
				return (ApplicationPartition)base.InnerList[index];
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00017DD4 File Offset: 0x00016DD4
		public bool Contains(ApplicationPartition applicationPartition)
		{
			if (applicationPartition == null)
			{
				throw new ArgumentNullException("applicationPartition");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ApplicationPartition applicationPartition2 = (ApplicationPartition)base.InnerList[i];
				if (Utils.Compare(applicationPartition2.Name, applicationPartition.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00017E30 File Offset: 0x00016E30
		public int IndexOf(ApplicationPartition applicationPartition)
		{
			if (applicationPartition == null)
			{
				throw new ArgumentNullException("applicationPartition");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ApplicationPartition applicationPartition2 = (ApplicationPartition)base.InnerList[i];
				if (Utils.Compare(applicationPartition2.Name, applicationPartition.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00017E89 File Offset: 0x00016E89
		public void CopyTo(ApplicationPartition[] applicationPartitions, int index)
		{
			base.InnerList.CopyTo(applicationPartitions, index);
		}
	}
}
