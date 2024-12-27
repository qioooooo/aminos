using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x0200013E RID: 318
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class BuildProviderAppliesToAttribute : Attribute
	{
		// Token: 0x06000EF1 RID: 3825 RVA: 0x0004371C File Offset: 0x0004271C
		public BuildProviderAppliesToAttribute(BuildProviderAppliesTo appliesTo)
		{
			this._appliesTo = appliesTo;
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x0004372B File Offset: 0x0004272B
		public BuildProviderAppliesTo AppliesTo
		{
			get
			{
				return this._appliesTo;
			}
		}

		// Token: 0x040015B4 RID: 5556
		private BuildProviderAppliesTo _appliesTo;
	}
}
