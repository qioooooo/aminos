using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050D RID: 1293
	[ComVisible(true)]
	public interface ICustomFactory
	{
		// Token: 0x06003293 RID: 12947
		MarshalByRefObject CreateInstance(Type serverType);
	}
}
