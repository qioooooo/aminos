using System;
using System.Collections;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E4 RID: 2020
	internal sealed class SerObjectInfoInit
	{
		// Token: 0x040024EB RID: 9451
		internal Hashtable seenBeforeTable = new Hashtable();

		// Token: 0x040024EC RID: 9452
		internal int objectInfoIdCount = 1;

		// Token: 0x040024ED RID: 9453
		internal SerStack oiPool = new SerStack("SerObjectInfo Pool");
	}
}
