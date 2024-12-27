using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000086 RID: 134
	public class AdamInstanceCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000418 RID: 1048 RVA: 0x0001648A File Offset: 0x0001548A
		internal AdamInstanceCollection()
		{
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00016492 File Offset: 0x00015492
		internal AdamInstanceCollection(ArrayList values)
		{
			if (values != null)
			{
				base.InnerList.AddRange(values);
			}
		}

		// Token: 0x17000103 RID: 259
		public AdamInstance this[int index]
		{
			get
			{
				return (AdamInstance)base.InnerList[index];
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000164BC File Offset: 0x000154BC
		public bool Contains(AdamInstance adamInstance)
		{
			if (adamInstance == null)
			{
				throw new ArgumentNullException("adamInstance");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				AdamInstance adamInstance2 = (AdamInstance)base.InnerList[i];
				if (Utils.Compare(adamInstance2.Name, adamInstance.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00016518 File Offset: 0x00015518
		public int IndexOf(AdamInstance adamInstance)
		{
			if (adamInstance == null)
			{
				throw new ArgumentNullException("adamInstance");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				AdamInstance adamInstance2 = (AdamInstance)base.InnerList[i];
				if (Utils.Compare(adamInstance2.Name, adamInstance.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00016571 File Offset: 0x00015571
		public void CopyTo(AdamInstance[] adamInstances, int index)
		{
			base.InnerList.CopyTo(adamInstances, index);
		}
	}
}
