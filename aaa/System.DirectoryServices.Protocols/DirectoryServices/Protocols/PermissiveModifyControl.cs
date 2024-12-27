using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200001F RID: 31
	public class PermissiveModifyControl : DirectoryControl
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00004391 File Offset: 0x00003391
		public PermissiveModifyControl()
			: base("1.2.840.113556.1.4.1413", null, true, true)
		{
		}
	}
}
