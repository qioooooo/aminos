using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200017A RID: 378
	internal abstract class TemplateControlBuildProvider : BaseTemplateBuildProvider
	{
		// Token: 0x06001086 RID: 4230 RVA: 0x00049373 File Offset: 0x00048373
		internal virtual DependencyParser CreateDependencyParser()
		{
			return null;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00049378 File Offset: 0x00048378
		internal override ICollection GetBuildResultVirtualPathDependencies()
		{
			DependencyParser dependencyParser = this.CreateDependencyParser();
			if (dependencyParser == null)
			{
				return null;
			}
			dependencyParser.Init(base.VirtualPathObject);
			return dependencyParser.GetVirtualPathDependencies();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x000493A3 File Offset: 0x000483A3
		internal override BuildResult CreateBuildResult(CompilerResults results)
		{
			if (base.Parser.RequiresCompilation)
			{
				return base.CreateBuildResult(results);
			}
			return this.CreateNoCompileBuildResult();
		}

		// Token: 0x06001089 RID: 4233
		internal abstract BuildResultNoCompileTemplateControl CreateNoCompileBuildResult();
	}
}
