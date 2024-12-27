using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000AA RID: 170
	public class ForestTrustDomainInfoCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060005B1 RID: 1457 RVA: 0x00020899 File Offset: 0x0001F899
		internal ForestTrustDomainInfoCollection()
		{
		}

		// Token: 0x17000159 RID: 345
		public ForestTrustDomainInformation this[int index]
		{
			get
			{
				return (ForestTrustDomainInformation)base.InnerList[index];
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000208B4 File Offset: 0x0001F8B4
		public bool Contains(ForestTrustDomainInformation information)
		{
			if (information == null)
			{
				throw new ArgumentNullException("information");
			}
			return base.InnerList.Contains(information);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000208D0 File Offset: 0x0001F8D0
		public int IndexOf(ForestTrustDomainInformation information)
		{
			if (information == null)
			{
				throw new ArgumentNullException("information");
			}
			return base.InnerList.IndexOf(information);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x000208EC File Offset: 0x0001F8EC
		public void CopyTo(ForestTrustDomainInformation[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000208FB File Offset: 0x0001F8FB
		internal int Add(ForestTrustDomainInformation info)
		{
			return base.InnerList.Add(info);
		}
	}
}
