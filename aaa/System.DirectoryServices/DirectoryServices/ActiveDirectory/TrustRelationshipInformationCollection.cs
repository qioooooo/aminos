using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000F9 RID: 249
	public class TrustRelationshipInformationCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000755 RID: 1877 RVA: 0x0002757A File Offset: 0x0002657A
		internal TrustRelationshipInformationCollection()
		{
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00027584 File Offset: 0x00026584
		internal TrustRelationshipInformationCollection(DirectoryContext context, string source, ArrayList trusts)
		{
			for (int i = 0; i < trusts.Count; i++)
			{
				TrustObject trustObject = (TrustObject)trusts[i];
				if (trustObject.TrustType != TrustType.Forest && trustObject.TrustType != (TrustType)7)
				{
					TrustRelationshipInformation trustRelationshipInformation = new TrustRelationshipInformation(context, source, trustObject);
					this.Add(trustRelationshipInformation);
				}
			}
		}

		// Token: 0x170001CB RID: 459
		public TrustRelationshipInformation this[int index]
		{
			get
			{
				return (TrustRelationshipInformation)base.InnerList[index];
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000275EB File Offset: 0x000265EB
		public bool Contains(TrustRelationshipInformation information)
		{
			if (information == null)
			{
				throw new ArgumentNullException("information");
			}
			return base.InnerList.Contains(information);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00027607 File Offset: 0x00026607
		public int IndexOf(TrustRelationshipInformation information)
		{
			if (information == null)
			{
				throw new ArgumentNullException("information");
			}
			return base.InnerList.IndexOf(information);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00027623 File Offset: 0x00026623
		public void CopyTo(TrustRelationshipInformation[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00027632 File Offset: 0x00026632
		internal int Add(TrustRelationshipInformation info)
		{
			return base.InnerList.Add(info);
		}
	}
}
