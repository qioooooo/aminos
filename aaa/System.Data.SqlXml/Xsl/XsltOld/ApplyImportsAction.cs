using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000131 RID: 305
	internal class ApplyImportsAction : CompiledAction
	{
		// Token: 0x06000D75 RID: 3445 RVA: 0x000453F7 File Offset: 0x000443F7
		internal override void Compile(Compiler compiler)
		{
			base.CheckEmpty(compiler);
			if (!compiler.CanHaveApplyImports)
			{
				throw XsltException.Create("Xslt_ApplyImports", new string[0]);
			}
			this.mode = compiler.CurrentMode;
			this.stylesheet = compiler.CompiledStylesheet;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00045434 File Offset: 0x00044434
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				processor.PushTemplateLookup(frame.NodeSet, this.mode, this.stylesheet);
				frame.State = 2;
				return;
			case 1:
				break;
			case 2:
				frame.Finished();
				break;
			default:
				return;
			}
		}

		// Token: 0x040008EC RID: 2284
		private const int TemplateProcessed = 2;

		// Token: 0x040008ED RID: 2285
		private XmlQualifiedName mode;

		// Token: 0x040008EE RID: 2286
		private Stylesheet stylesheet;
	}
}
