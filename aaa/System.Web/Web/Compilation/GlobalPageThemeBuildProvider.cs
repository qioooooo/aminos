using System;
using System.Collections;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000185 RID: 389
	internal class GlobalPageThemeBuildProvider : PageThemeBuildProvider
	{
		// Token: 0x060010C5 RID: 4293 RVA: 0x0004AF68 File Offset: 0x00049F68
		internal GlobalPageThemeBuildProvider(VirtualPath virtualDirPath)
			: base(virtualDirPath)
		{
			this._virtualDirPath = virtualDirPath;
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x0004AF78 File Offset: 0x00049F78
		internal override string AssemblyNamePrefix
		{
			get
			{
				return "App_GlobalTheme_";
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x0004AF80 File Offset: 0x00049F80
		public override ICollection VirtualPathDependencies
		{
			get
			{
				ICollection virtualPathDependencies = base.VirtualPathDependencies;
				string fileName = this._virtualDirPath.FileName;
				CaseInsensitiveStringSet caseInsensitiveStringSet = new CaseInsensitiveStringSet();
				caseInsensitiveStringSet.AddCollection(virtualPathDependencies);
				string text = UrlPath.SimpleCombine(HttpRuntime.AppDomainAppVirtualPathString, "App_Themes");
				string text2 = text + '/' + fileName;
				if (HostingEnvironment.VirtualPathProvider.DirectoryExists(text2))
				{
					caseInsensitiveStringSet.Add(text2);
				}
				else
				{
					caseInsensitiveStringSet.Add(text);
				}
				return caseInsensitiveStringSet;
			}
		}

		// Token: 0x04001673 RID: 5747
		private VirtualPath _virtualDirPath;
	}
}
