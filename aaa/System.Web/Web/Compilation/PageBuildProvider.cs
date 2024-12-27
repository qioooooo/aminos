using System;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000182 RID: 386
	[BuildProviderAppliesTo(BuildProviderAppliesTo.Web)]
	internal class PageBuildProvider : TemplateControlBuildProvider
	{
		// Token: 0x060010AE RID: 4270 RVA: 0x00049D34 File Offset: 0x00048D34
		internal override DependencyParser CreateDependencyParser()
		{
			return new PageDependencyParser();
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00049D3B File Offset: 0x00048D3B
		protected override TemplateParser CreateParser()
		{
			return new PageParser();
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00049D42 File Offset: 0x00048D42
		internal override BaseCodeDomTreeGenerator CreateCodeDomTreeGenerator(TemplateParser parser)
		{
			return new PageCodeDomTreeGenerator((PageParser)parser);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00049D4F File Offset: 0x00048D4F
		internal override BuildResultNoCompileTemplateControl CreateNoCompileBuildResult()
		{
			return new BuildResultNoCompilePage(base.Parser.BaseType, base.Parser);
		}
	}
}
