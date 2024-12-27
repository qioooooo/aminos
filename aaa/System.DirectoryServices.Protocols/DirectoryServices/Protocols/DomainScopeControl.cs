using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200001B RID: 27
	public class DomainScopeControl : DirectoryControl
	{
		// Token: 0x0600007E RID: 126 RVA: 0x000042D7 File Offset: 0x000032D7
		public DomainScopeControl()
			: base("1.2.840.113556.1.4.1339", null, true, true)
		{
		}
	}
}
