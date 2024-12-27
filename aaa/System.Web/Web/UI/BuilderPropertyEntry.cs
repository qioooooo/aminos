using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200039F RID: 927
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class BuilderPropertyEntry : PropertyEntry
	{
		// Token: 0x06002D3E RID: 11582 RVA: 0x000CA8E2 File Offset: 0x000C98E2
		internal BuilderPropertyEntry()
		{
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000CA8EA File Offset: 0x000C98EA
		// (set) Token: 0x06002D40 RID: 11584 RVA: 0x000CA8F2 File Offset: 0x000C98F2
		public ControlBuilder Builder
		{
			get
			{
				return this._builder;
			}
			set
			{
				this._builder = value;
			}
		}

		// Token: 0x040020EA RID: 8426
		private ControlBuilder _builder;
	}
}
