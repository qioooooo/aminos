using System;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000150 RID: 336
	internal class BuildResultNoCompileUserControl : BuildResultNoCompileTemplateControl
	{
		// Token: 0x06000F78 RID: 3960 RVA: 0x000452CC File Offset: 0x000442CC
		internal BuildResultNoCompileUserControl(Type baseType, TemplateParser parser)
			: base(baseType, parser)
		{
			UserControlParser userControlParser = (UserControlParser)parser;
			OutputCacheParameters outputCacheParameters = userControlParser.OutputCacheParameters;
			if (outputCacheParameters != null && outputCacheParameters.Duration > 0)
			{
				this._cachingAttribute = new PartialCachingAttribute(outputCacheParameters.Duration, outputCacheParameters.VaryByParam, outputCacheParameters.VaryByControl, outputCacheParameters.VaryByCustom, outputCacheParameters.SqlDependency, userControlParser.FSharedPartialCaching);
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x0004532A File Offset: 0x0004432A
		internal PartialCachingAttribute CachingAttribute
		{
			get
			{
				return this._cachingAttribute;
			}
		}

		// Token: 0x040015F0 RID: 5616
		private PartialCachingAttribute _cachingAttribute;
	}
}
