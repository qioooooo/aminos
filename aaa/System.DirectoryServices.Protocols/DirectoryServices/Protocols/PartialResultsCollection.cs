using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000047 RID: 71
	public class PartialResultsCollection : ReadOnlyCollectionBase
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00006F10 File Offset: 0x00005F10
		internal PartialResultsCollection()
		{
		}

		// Token: 0x17000068 RID: 104
		public object this[int index]
		{
			get
			{
				return base.InnerList[index];
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006F26 File Offset: 0x00005F26
		internal int Add(object value)
		{
			return base.InnerList.Add(value);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006F34 File Offset: 0x00005F34
		public bool Contains(object value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006F42 File Offset: 0x00005F42
		public int IndexOf(object value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006F50 File Offset: 0x00005F50
		public void CopyTo(object[] values, int index)
		{
			base.InnerList.CopyTo(values, index);
		}
	}
}
