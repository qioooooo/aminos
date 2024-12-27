using System;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000B7 RID: 183
	public sealed class GraphicsState : MarshalByRefObject
	{
		// Token: 0x06000B65 RID: 2917 RVA: 0x00022398 File Offset: 0x00021398
		internal GraphicsState(int nativeState)
		{
			this.nativeState = nativeState;
		}

		// Token: 0x040009E3 RID: 2531
		internal int nativeState;
	}
}
