using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000096 RID: 150
	public class DomainCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x0001C9F4 File Offset: 0x0001B9F4
		internal DomainCollection()
		{
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001C9FC File Offset: 0x0001B9FC
		internal DomainCollection(ArrayList values)
		{
			if (values != null)
			{
				for (int i = 0; i < values.Count; i++)
				{
					this.Add((Domain)values[i]);
				}
			}
		}

		// Token: 0x17000129 RID: 297
		public Domain this[int index]
		{
			get
			{
				return (Domain)base.InnerList[index];
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001CA4C File Offset: 0x0001BA4C
		public bool Contains(Domain domain)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				Domain domain2 = (Domain)base.InnerList[i];
				if (Utils.Compare(domain2.Name, domain.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001CAA8 File Offset: 0x0001BAA8
		public int IndexOf(Domain domain)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				Domain domain2 = (Domain)base.InnerList[i];
				if (Utils.Compare(domain2.Name, domain.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001CB01 File Offset: 0x0001BB01
		public void CopyTo(Domain[] domains, int index)
		{
			base.InnerList.CopyTo(domains, index);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001CB10 File Offset: 0x0001BB10
		internal int Add(Domain domain)
		{
			return base.InnerList.Add(domain);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001CB1E File Offset: 0x0001BB1E
		internal void Clear()
		{
			base.InnerList.Clear();
		}
	}
}
