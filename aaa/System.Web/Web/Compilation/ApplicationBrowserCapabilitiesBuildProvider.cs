using System;
using System.Web.Hosting;

namespace System.Web.Compilation
{
	// Token: 0x02000131 RID: 305
	internal class ApplicationBrowserCapabilitiesBuildProvider : BuildProvider
	{
		// Token: 0x06000E12 RID: 3602 RVA: 0x0003FECD File Offset: 0x0003EECD
		internal ApplicationBrowserCapabilitiesBuildProvider()
		{
			this._codeGenerator = new ApplicationBrowserCapabilitiesCodeGenerator(this);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0003FEE4 File Offset: 0x0003EEE4
		internal void AddFile(string virtualPath)
		{
			string text = HostingEnvironment.MapPathInternal(virtualPath);
			this._codeGenerator.AddFile(text);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0003FF04 File Offset: 0x0003EF04
		public override void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			this._codeGenerator.GenerateCode(assemblyBuilder);
		}

		// Token: 0x0400155C RID: 5468
		private ApplicationBrowserCapabilitiesCodeGenerator _codeGenerator;
	}
}
