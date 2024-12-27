using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000D0 RID: 208
	internal abstract class MimeReflector
	{
		// Token: 0x06000593 RID: 1427
		internal abstract bool ReflectParameters();

		// Token: 0x06000594 RID: 1428
		internal abstract bool ReflectReturn();

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0001B5AA File Offset: 0x0001A5AA
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x0001B5B2 File Offset: 0x0001A5B2
		internal HttpProtocolReflector ReflectionContext
		{
			get
			{
				return this.protocol;
			}
			set
			{
				this.protocol = value;
			}
		}

		// Token: 0x04000425 RID: 1061
		private HttpProtocolReflector protocol;
	}
}
