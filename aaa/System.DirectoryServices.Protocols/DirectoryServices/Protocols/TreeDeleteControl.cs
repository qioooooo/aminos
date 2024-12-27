using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000023 RID: 35
	public class TreeDeleteControl : DirectoryControl
	{
		// Token: 0x06000092 RID: 146 RVA: 0x000044AD File Offset: 0x000034AD
		public TreeDeleteControl()
			: base("1.2.840.113556.1.4.805", null, true, true)
		{
		}
	}
}
