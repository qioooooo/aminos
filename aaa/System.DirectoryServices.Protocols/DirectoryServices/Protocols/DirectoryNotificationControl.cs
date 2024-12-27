using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200001E RID: 30
	public class DirectoryNotificationControl : DirectoryControl
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00004381 File Offset: 0x00003381
		public DirectoryNotificationControl()
			: base("1.2.840.113556.1.4.528", null, true, true)
		{
		}
	}
}
