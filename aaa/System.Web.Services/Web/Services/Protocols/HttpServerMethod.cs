using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000049 RID: 73
	internal class HttpServerMethod
	{
		// Token: 0x04000290 RID: 656
		internal string name;

		// Token: 0x04000291 RID: 657
		internal LogicalMethodInfo methodInfo;

		// Token: 0x04000292 RID: 658
		internal Type[] readerTypes;

		// Token: 0x04000293 RID: 659
		internal object[] readerInitializers;

		// Token: 0x04000294 RID: 660
		internal Type writerType;

		// Token: 0x04000295 RID: 661
		internal object writerInitializer;
	}
}
