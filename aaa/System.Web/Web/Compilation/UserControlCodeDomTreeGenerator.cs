using System;
using System.CodeDom;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000197 RID: 407
	internal class UserControlCodeDomTreeGenerator : TemplateControlCodeDomTreeGenerator
	{
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x0004D034 File Offset: 0x0004C034
		private UserControlParser Parser
		{
			get
			{
				return this._ucParser;
			}
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x0004D03C File Offset: 0x0004C03C
		internal UserControlCodeDomTreeGenerator(UserControlParser ucParser)
			: base(ucParser)
		{
			this._ucParser = ucParser;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0004D04C File Offset: 0x0004C04C
		protected override void GenerateClassAttributes()
		{
			base.GenerateClassAttributes();
			if (this._sourceDataClass != null && this.Parser.OutputCacheParameters != null)
			{
				OutputCacheParameters outputCacheParameters = this.Parser.OutputCacheParameters;
				if (outputCacheParameters.Duration > 0)
				{
					CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.Web.UI.PartialCachingAttribute");
					CodeAttributeArgument codeAttributeArgument = new CodeAttributeArgument(new CodePrimitiveExpression(outputCacheParameters.Duration));
					codeAttributeDeclaration.Arguments.Add(codeAttributeArgument);
					codeAttributeArgument = new CodeAttributeArgument(new CodePrimitiveExpression(outputCacheParameters.VaryByParam));
					codeAttributeDeclaration.Arguments.Add(codeAttributeArgument);
					codeAttributeArgument = new CodeAttributeArgument(new CodePrimitiveExpression(outputCacheParameters.VaryByControl));
					codeAttributeDeclaration.Arguments.Add(codeAttributeArgument);
					codeAttributeArgument = new CodeAttributeArgument(new CodePrimitiveExpression(outputCacheParameters.VaryByCustom));
					codeAttributeDeclaration.Arguments.Add(codeAttributeArgument);
					codeAttributeArgument = new CodeAttributeArgument(new CodePrimitiveExpression(outputCacheParameters.SqlDependency));
					codeAttributeDeclaration.Arguments.Add(codeAttributeArgument);
					codeAttributeArgument = new CodeAttributeArgument(new CodePrimitiveExpression(this.Parser.FSharedPartialCaching));
					codeAttributeDeclaration.Arguments.Add(codeAttributeArgument);
					this._sourceDataClass.CustomAttributes.Add(codeAttributeDeclaration);
				}
			}
		}

		// Token: 0x04001694 RID: 5780
		protected UserControlParser _ucParser;
	}
}
