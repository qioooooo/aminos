using System;

namespace Microsoft.Vsa
{
	// Token: 0x02000027 RID: 39
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	public abstract class BaseVsaStartup
	{
		// Token: 0x06000173 RID: 371 RVA: 0x00008096 File Offset: 0x00007096
		public void SetSite(IVsaSite site)
		{
			this.site = site;
		}

		// Token: 0x06000174 RID: 372
		public abstract void Startup();

		// Token: 0x06000175 RID: 373
		public abstract void Shutdown();

		// Token: 0x04000078 RID: 120
		protected IVsaSite site;
	}
}
