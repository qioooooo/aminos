using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000477 RID: 1143
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class TemplatePropertyEntry : BuilderPropertyEntry
	{
		// Token: 0x060035BF RID: 13759 RVA: 0x000E7F97 File Offset: 0x000E6F97
		internal TemplatePropertyEntry()
		{
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000E7F9F File Offset: 0x000E6F9F
		internal TemplatePropertyEntry(bool bindableTemplate)
		{
			this._bindableTemplate = bindableTemplate;
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x060035C1 RID: 13761 RVA: 0x000E7FAE File Offset: 0x000E6FAE
		public bool BindableTemplate
		{
			get
			{
				return this._bindableTemplate;
			}
		}

		// Token: 0x04002554 RID: 9556
		private bool _bindableTemplate;
	}
}
