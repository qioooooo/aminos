using System;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006C4 RID: 1732
	internal class ArrayWithSize
	{
		// Token: 0x06003ECC RID: 16076 RVA: 0x000D7FCA File Offset: 0x000D6FCA
		internal ArrayWithSize(IDynamicMessageSink[] sinks, int count)
		{
			this.Sinks = sinks;
			this.Count = count;
		}

		// Token: 0x04001FB8 RID: 8120
		internal IDynamicMessageSink[] Sinks;

		// Token: 0x04001FB9 RID: 8121
		internal int Count;
	}
}
