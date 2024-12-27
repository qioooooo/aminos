using System;

namespace System.DirectoryServices
{
	// Token: 0x0200000E RID: 14
	internal sealed class ActiveDirectoryRightsTranslator
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002628 File Offset: 0x00001628
		internal static int AccessMaskFromRights(ActiveDirectoryRights adRights)
		{
			return (int)adRights;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000262B File Offset: 0x0000162B
		internal static ActiveDirectoryRights RightsFromAccessMask(int accessMask)
		{
			return (ActiveDirectoryRights)accessMask;
		}
	}
}
