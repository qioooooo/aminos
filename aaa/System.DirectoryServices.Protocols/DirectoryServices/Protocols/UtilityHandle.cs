using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000015 RID: 21
	internal class UtilityHandle
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00003B1C File Offset: 0x00002B1C
		public static ConnectionHandle GetHandle()
		{
			return UtilityHandle.handle;
		}

		// Token: 0x040000D9 RID: 217
		private static ConnectionHandle handle = new ConnectionHandle();
	}
}
