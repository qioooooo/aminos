using System;

namespace System.Configuration
{
	// Token: 0x02000061 RID: 97
	internal class EmptyImpersonationContext : IDisposable
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00013264 File Offset: 0x00012264
		internal static IDisposable GetStaticInstance()
		{
			if (EmptyImpersonationContext.s_emptyImpersonationContext == null)
			{
				EmptyImpersonationContext.s_emptyImpersonationContext = new EmptyImpersonationContext();
			}
			return EmptyImpersonationContext.s_emptyImpersonationContext;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001327C File Offset: 0x0001227C
		public void Dispose()
		{
		}

		// Token: 0x040002F1 RID: 753
		private static IDisposable s_emptyImpersonationContext;
	}
}
