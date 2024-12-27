using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x02000478 RID: 1144
	[ComVisible(true)]
	public interface ISecurityEncodable
	{
		// Token: 0x06002E0D RID: 11789
		SecurityElement ToXml();

		// Token: 0x06002E0E RID: 11790
		void FromXml(SecurityElement e);
	}
}
