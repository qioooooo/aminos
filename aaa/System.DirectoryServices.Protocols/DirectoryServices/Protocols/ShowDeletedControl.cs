using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000022 RID: 34
	public class ShowDeletedControl : DirectoryControl
	{
		// Token: 0x06000091 RID: 145 RVA: 0x0000449D File Offset: 0x0000349D
		public ShowDeletedControl()
			: base("1.2.840.113556.1.4.417", null, true, true)
		{
		}
	}
}
