using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007A2 RID: 1954
	[ComVisible(true)]
	[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000", Name = "System.Runtime.Remoting")]
	public sealed class InternalRM
	{
		// Token: 0x06004621 RID: 17953 RVA: 0x000F039C File Offset: 0x000EF39C
		[Conditional("_LOGGING")]
		public static void InfoSoap(params object[] messages)
		{
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x000F039E File Offset: 0x000EF39E
		public static bool SoapCheckEnabled()
		{
			return BCLDebug.CheckEnabled("SOAP");
		}
	}
}
