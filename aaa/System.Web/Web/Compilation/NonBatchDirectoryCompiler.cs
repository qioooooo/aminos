using System;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x0200017F RID: 383
	internal class NonBatchDirectoryCompiler
	{
		// Token: 0x060010A3 RID: 4259 RVA: 0x00049A77 File Offset: 0x00048A77
		internal NonBatchDirectoryCompiler(VirtualDirectory vdir)
		{
			this._vdir = vdir;
			this._compConfig = RuntimeConfig.GetConfig(this._vdir.VirtualPath).Compilation;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00049AA4 File Offset: 0x00048AA4
		internal void Process()
		{
			foreach (object obj in this._vdir.Files)
			{
				VirtualFile virtualFile = (VirtualFile)obj;
				string extension = UrlPath.GetExtension(virtualFile.VirtualPath);
				Type buildProviderTypeFromExtension = CompilationUtil.GetBuildProviderTypeFromExtension(this._compConfig, extension, BuildProviderAppliesTo.Web, false);
				if (buildProviderTypeFromExtension != null && buildProviderTypeFromExtension != typeof(SourceFileBuildProvider) && buildProviderTypeFromExtension != typeof(ResXBuildProvider))
				{
					BuildManager.GetVPathBuildResult(virtualFile.VirtualPathObject);
				}
			}
		}

		// Token: 0x04001661 RID: 5729
		private CompilationSection _compConfig;

		// Token: 0x04001662 RID: 5730
		private VirtualDirectory _vdir;
	}
}
