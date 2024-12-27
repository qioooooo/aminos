using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000350 RID: 848
	[Serializable]
	internal class FixupHolder
	{
		// Token: 0x06002213 RID: 8723 RVA: 0x000568E8 File Offset: 0x000558E8
		internal FixupHolder(long id, object fixupInfo, int fixupType)
		{
			this.m_id = id;
			this.m_fixupInfo = fixupInfo;
			this.m_fixupType = fixupType;
		}

		// Token: 0x04000E21 RID: 3617
		internal const int ArrayFixup = 1;

		// Token: 0x04000E22 RID: 3618
		internal const int MemberFixup = 2;

		// Token: 0x04000E23 RID: 3619
		internal const int DelayedFixup = 4;

		// Token: 0x04000E24 RID: 3620
		internal long m_id;

		// Token: 0x04000E25 RID: 3621
		internal object m_fixupInfo;

		// Token: 0x04000E26 RID: 3622
		internal int m_fixupType;
	}
}
