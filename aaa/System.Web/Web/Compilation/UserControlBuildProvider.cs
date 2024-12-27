using System;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200017B RID: 379
	[BuildProviderAppliesTo(BuildProviderAppliesTo.Web | BuildProviderAppliesTo.Code)]
	internal class UserControlBuildProvider : TemplateControlBuildProvider
	{
		// Token: 0x0600108B RID: 4235 RVA: 0x000493C8 File Offset: 0x000483C8
		internal override DependencyParser CreateDependencyParser()
		{
			return new UserControlDependencyParser();
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x000493CF File Offset: 0x000483CF
		protected override TemplateParser CreateParser()
		{
			return new UserControlParser();
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x000493D6 File Offset: 0x000483D6
		internal override BaseCodeDomTreeGenerator CreateCodeDomTreeGenerator(TemplateParser parser)
		{
			return new UserControlCodeDomTreeGenerator((UserControlParser)parser);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x000493E3 File Offset: 0x000483E3
		internal override BuildResultNoCompileTemplateControl CreateNoCompileBuildResult()
		{
			return new BuildResultNoCompileUserControl(base.Parser.BaseType, base.Parser);
		}
	}
}
