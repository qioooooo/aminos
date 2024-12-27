using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008EC RID: 2284
	[Flags]
	public enum ObjectAceFlags
	{
		// Token: 0x04002AF6 RID: 10998
		None = 0,
		// Token: 0x04002AF7 RID: 10999
		ObjectAceTypePresent = 1,
		// Token: 0x04002AF8 RID: 11000
		InheritedObjectAceTypePresent = 2
	}
}
