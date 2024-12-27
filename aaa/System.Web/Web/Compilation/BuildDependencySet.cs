using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x02000132 RID: 306
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class BuildDependencySet
	{
		// Token: 0x06000E15 RID: 3605 RVA: 0x0003FF12 File Offset: 0x0003EF12
		internal BuildDependencySet(BuildResult result)
		{
			this._result = result;
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0003FF21 File Offset: 0x0003EF21
		public string HashCode
		{
			get
			{
				return this._result.VirtualPathDependenciesHash;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0003FF2E File Offset: 0x0003EF2E
		public IEnumerable VirtualPaths
		{
			get
			{
				return this._result.VirtualPathDependencies;
			}
		}

		// Token: 0x0400155D RID: 5469
		private BuildResult _result;
	}
}
