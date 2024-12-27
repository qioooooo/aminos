using System;
using System.Reflection.Emit;

namespace System.Security
{
	// Token: 0x0200065B RID: 1627
	internal class FrameSecurityDescriptorWithResolver : FrameSecurityDescriptor
	{
		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x000C9008 File Offset: 0x000C8008
		public DynamicResolver Resolver
		{
			get
			{
				return this.m_resolver;
			}
		}

		// Token: 0x04001E66 RID: 7782
		private DynamicResolver m_resolver;
	}
}
