using System;
using System.Collections;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200002F RID: 47
	internal sealed class SerObjectInfoInit
	{
		// Token: 0x04000219 RID: 537
		internal Hashtable seenBeforeTable = new Hashtable();

		// Token: 0x0400021A RID: 538
		internal int objectInfoIdCount = 1;

		// Token: 0x0400021B RID: 539
		internal SerStack oiPool = new SerStack("SerObjectInfo Pool");
	}
}
