using System;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x0200014C RID: 332
	internal class BuildResultCompiledTemplateType : BuildResultCompiledType
	{
		// Token: 0x06000F65 RID: 3941 RVA: 0x00044FB8 File Offset: 0x00043FB8
		public BuildResultCompiledTemplateType()
		{
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00044FC0 File Offset: 0x00043FC0
		public BuildResultCompiledTemplateType(Type t)
			: base(t)
		{
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00044FC9 File Offset: 0x00043FC9
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.BuildResultCompiledTemplateType;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00044FCC File Offset: 0x00043FCC
		protected override void ComputeHashCode(HashCodeCombiner hashCodeCombiner)
		{
			base.ComputeHashCode(hashCodeCombiner);
			PagesSection pages = RuntimeConfig.GetConfig(base.VirtualPath).Pages;
			hashCodeCombiner.AddObject(Util.GetRecompilationHash(pages));
		}
	}
}
