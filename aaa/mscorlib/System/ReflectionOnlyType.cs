using System;

namespace System
{
	// Token: 0x020000FE RID: 254
	[Serializable]
	internal class ReflectionOnlyType : RuntimeType
	{
		// Token: 0x06000EA6 RID: 3750 RVA: 0x0002BD2E File Offset: 0x0002AD2E
		private ReflectionOnlyType()
		{
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0002BD36 File Offset: 0x0002AD36
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInReflectionOnly"));
			}
		}
	}
}
