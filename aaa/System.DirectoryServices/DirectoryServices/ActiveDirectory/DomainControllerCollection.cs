using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200009D RID: 157
	public class DomainControllerCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000531 RID: 1329 RVA: 0x0001E518 File Offset: 0x0001D518
		internal DomainControllerCollection()
		{
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001E520 File Offset: 0x0001D520
		internal DomainControllerCollection(ArrayList values)
		{
			if (values != null)
			{
				base.InnerList.AddRange(values);
			}
		}

		// Token: 0x1700013C RID: 316
		public DomainController this[int index]
		{
			get
			{
				return (DomainController)base.InnerList[index];
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001E54C File Offset: 0x0001D54C
		public bool Contains(DomainController domainController)
		{
			if (domainController == null)
			{
				throw new ArgumentNullException("domainController");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				DomainController domainController2 = (DomainController)base.InnerList[i];
				if (Utils.Compare(domainController2.Name, domainController.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001E5A8 File Offset: 0x0001D5A8
		public int IndexOf(DomainController domainController)
		{
			if (domainController == null)
			{
				throw new ArgumentNullException("domainController");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				DomainController domainController2 = (DomainController)base.InnerList[i];
				if (Utils.Compare(domainController2.Name, domainController.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001E601 File Offset: 0x0001D601
		public void CopyTo(DomainController[] domainControllers, int index)
		{
			base.InnerList.CopyTo(domainControllers, index);
		}
	}
}
