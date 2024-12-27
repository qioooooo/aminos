using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200001D RID: 29
	public class LazyCommitControl : DirectoryControl
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004371 File Offset: 0x00003371
		public LazyCommitControl()
			: base("1.2.840.113556.1.4.619", null, true, true)
		{
		}
	}
}
