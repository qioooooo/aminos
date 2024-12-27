using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x020000A0 RID: 160
	[DesignerCategory("code")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class JScriptCodeProvider : CodeDomProvider
	{
		// Token: 0x06000726 RID: 1830 RVA: 0x0003147E File Offset: 0x0003047E
		public JScriptCodeProvider()
		{
			this.generator = new JSCodeGenerator();
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00031491 File Offset: 0x00030491
		public override ICodeGenerator CreateGenerator()
		{
			return this.generator;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00031499 File Offset: 0x00030499
		public override ICodeCompiler CreateCompiler()
		{
			return this.generator;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x000314A1 File Offset: 0x000304A1
		public override string FileExtension
		{
			get
			{
				return "js";
			}
		}

		// Token: 0x04000323 RID: 803
		private JSCodeGenerator generator;
	}
}
