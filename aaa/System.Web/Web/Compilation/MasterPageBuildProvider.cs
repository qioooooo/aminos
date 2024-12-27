using System;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200017C RID: 380
	[BuildProviderAppliesTo(BuildProviderAppliesTo.Web | BuildProviderAppliesTo.Code)]
	internal class MasterPageBuildProvider : UserControlBuildProvider
	{
		// Token: 0x06001090 RID: 4240 RVA: 0x00049403 File Offset: 0x00048403
		internal override DependencyParser CreateDependencyParser()
		{
			return new MasterPageDependencyParser();
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0004940A File Offset: 0x0004840A
		protected override TemplateParser CreateParser()
		{
			return new MasterPageParser();
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00049411 File Offset: 0x00048411
		internal override BaseCodeDomTreeGenerator CreateCodeDomTreeGenerator(TemplateParser parser)
		{
			return new MasterPageCodeDomTreeGenerator((MasterPageParser)parser);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0004941E File Offset: 0x0004841E
		internal override BuildResultNoCompileTemplateControl CreateNoCompileBuildResult()
		{
			return new BuildResultNoCompileMasterPage(base.Parser.BaseType, base.Parser);
		}
	}
}
