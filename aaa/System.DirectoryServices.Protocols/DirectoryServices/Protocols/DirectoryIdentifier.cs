using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000032 RID: 50
	public abstract class DirectoryIdentifier
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x000053A4 File Offset: 0x000043A4
		protected DirectoryIdentifier()
		{
			Utility.CheckOSVersion();
		}
	}
}
