using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200039C RID: 924
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CompiledBindableTemplateBuilder : IBindableTemplate, ITemplate
	{
		// Token: 0x06002D12 RID: 11538 RVA: 0x000CA703 File Offset: 0x000C9703
		public CompiledBindableTemplateBuilder(BuildTemplateMethod buildTemplateMethod, ExtractTemplateValuesMethod extractTemplateValuesMethod)
		{
			this._buildTemplateMethod = buildTemplateMethod;
			this._extractTemplateValuesMethod = extractTemplateValuesMethod;
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000CA719 File Offset: 0x000C9719
		public IOrderedDictionary ExtractValues(Control container)
		{
			if (this._extractTemplateValuesMethod != null)
			{
				return this._extractTemplateValuesMethod(container);
			}
			return new OrderedDictionary();
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000CA735 File Offset: 0x000C9735
		public void InstantiateIn(Control container)
		{
			this._buildTemplateMethod(container);
		}

		// Token: 0x040020D6 RID: 8406
		private BuildTemplateMethod _buildTemplateMethod;

		// Token: 0x040020D7 RID: 8407
		private ExtractTemplateValuesMethod _extractTemplateValuesMethod;
	}
}
