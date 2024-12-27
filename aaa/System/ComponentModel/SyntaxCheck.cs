using System;
using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200013E RID: 318
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public static class SyntaxCheck
	{
		// Token: 0x06000A59 RID: 2649 RVA: 0x00023F09 File Offset: 0x00022F09
		public static bool CheckMachineName(string value)
		{
			if (value == null)
			{
				return false;
			}
			value = value.Trim();
			return !value.Equals(string.Empty) && value.IndexOf('\\') == -1;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00023F32 File Offset: 0x00022F32
		public static bool CheckPath(string value)
		{
			if (value == null)
			{
				return false;
			}
			value = value.Trim();
			return !value.Equals(string.Empty) && value.StartsWith("\\\\");
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00023F5B File Offset: 0x00022F5B
		public static bool CheckRootedPath(string value)
		{
			if (value == null)
			{
				return false;
			}
			value = value.Trim();
			return !value.Equals(string.Empty) && Path.IsPathRooted(value);
		}
	}
}
