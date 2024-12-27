using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x0200015C RID: 348
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class ClientBuildManagerParameter
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x000464CF File Offset: 0x000454CF
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x000464D7 File Offset: 0x000454D7
		public PrecompilationFlags PrecompilationFlags
		{
			get
			{
				return this._precompilationFlags;
			}
			set
			{
				this._precompilationFlags = value;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x000464E0 File Offset: 0x000454E0
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x000464E8 File Offset: 0x000454E8
		public string StrongNameKeyFile
		{
			get
			{
				return this._strongNameKeyFile;
			}
			set
			{
				this._strongNameKeyFile = value;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x000464F1 File Offset: 0x000454F1
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x000464F9 File Offset: 0x000454F9
		public string StrongNameKeyContainer
		{
			get
			{
				return this._strongNameKeyContainer;
			}
			set
			{
				this._strongNameKeyContainer = value;
			}
		}

		// Token: 0x04001611 RID: 5649
		private string _strongNameKeyFile;

		// Token: 0x04001612 RID: 5650
		private string _strongNameKeyContainer;

		// Token: 0x04001613 RID: 5651
		private PrecompilationFlags _precompilationFlags;
	}
}
