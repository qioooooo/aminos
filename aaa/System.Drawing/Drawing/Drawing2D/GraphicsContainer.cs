using System;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000B4 RID: 180
	public sealed class GraphicsContainer : MarshalByRefObject
	{
		// Token: 0x06000AF1 RID: 2801 RVA: 0x0002064B File Offset: 0x0001F64B
		internal GraphicsContainer(int graphicsContainer)
		{
			this.nativeGraphicsContainer = graphicsContainer;
		}

		// Token: 0x040009E0 RID: 2528
		internal int nativeGraphicsContainer;
	}
}
