using System;
using System.Security.Policy;

namespace System.Security.Permissions
{
	// Token: 0x0200063D RID: 1597
	[Serializable]
	internal sealed class StrongName2
	{
		// Token: 0x06003A0F RID: 14863 RVA: 0x000C43BD File Offset: 0x000C33BD
		public StrongName2(StrongNamePublicKeyBlob publicKeyBlob, string name, Version version)
		{
			this.m_publicKeyBlob = publicKeyBlob;
			this.m_name = name;
			this.m_version = version;
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x000C43DA File Offset: 0x000C33DA
		public StrongName2 Copy()
		{
			return new StrongName2(this.m_publicKeyBlob, this.m_name, this.m_version);
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x000C43F4 File Offset: 0x000C33F4
		public bool IsSubsetOf(StrongName2 target)
		{
			return this.m_publicKeyBlob == null || (this.m_publicKeyBlob.Equals(target.m_publicKeyBlob) && (this.m_name == null || (target.m_name != null && StrongName.CompareNames(target.m_name, this.m_name))) && (this.m_version == null || (target.m_version != null && target.m_version.CompareTo(this.m_version) == 0)));
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x000C446B File Offset: 0x000C346B
		public StrongName2 Intersect(StrongName2 target)
		{
			if (target.IsSubsetOf(this))
			{
				return target.Copy();
			}
			if (this.IsSubsetOf(target))
			{
				return this.Copy();
			}
			return null;
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x000C448E File Offset: 0x000C348E
		public bool Equals(StrongName2 target)
		{
			return target.IsSubsetOf(this) && this.IsSubsetOf(target);
		}

		// Token: 0x04001E04 RID: 7684
		public StrongNamePublicKeyBlob m_publicKeyBlob;

		// Token: 0x04001E05 RID: 7685
		public string m_name;

		// Token: 0x04001E06 RID: 7686
		public Version m_version;
	}
}
