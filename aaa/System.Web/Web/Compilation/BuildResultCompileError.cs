using System;

namespace System.Web.Compilation
{
	// Token: 0x02000143 RID: 323
	internal class BuildResultCompileError : BuildResult
	{
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0004494B File Offset: 0x0004394B
		internal HttpCompileException CompileException
		{
			get
			{
				return this._compileException;
			}
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00044953 File Offset: 0x00043953
		internal BuildResultCompileError(VirtualPath virtualPath, HttpCompileException compileException)
		{
			base.VirtualPath = virtualPath;
			this._compileException = compileException;
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00044969 File Offset: 0x00043969
		internal override bool CacheToDisk
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0004496C File Offset: 0x0004396C
		internal override DateTime MemoryCacheExpiration
		{
			get
			{
				return DateTime.UtcNow.AddSeconds(10.0);
			}
		}

		// Token: 0x040015DD RID: 5597
		private HttpCompileException _compileException;
	}
}
