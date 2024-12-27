using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000A9 RID: 169
	public class ForestTrustRelationshipCollisionCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x00020829 File Offset: 0x0001F829
		internal ForestTrustRelationshipCollisionCollection()
		{
		}

		// Token: 0x17000158 RID: 344
		public ForestTrustRelationshipCollision this[int index]
		{
			get
			{
				return (ForestTrustRelationshipCollision)base.InnerList[index];
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00020844 File Offset: 0x0001F844
		public bool Contains(ForestTrustRelationshipCollision collision)
		{
			if (collision == null)
			{
				throw new ArgumentNullException("collision");
			}
			return base.InnerList.Contains(collision);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00020860 File Offset: 0x0001F860
		public int IndexOf(ForestTrustRelationshipCollision collision)
		{
			if (collision == null)
			{
				throw new ArgumentNullException("collision");
			}
			return base.InnerList.IndexOf(collision);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0002087C File Offset: 0x0001F87C
		public void CopyTo(ForestTrustRelationshipCollision[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0002088B File Offset: 0x0001F88B
		internal int Add(ForestTrustRelationshipCollision collision)
		{
			return base.InnerList.Add(collision);
		}
	}
}
