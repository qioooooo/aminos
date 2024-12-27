using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200013E RID: 318
	internal class CommentAction : ContainerAction
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x00047B68 File Offset: 0x00046B68
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			if (compiler.Recurse())
			{
				base.CompileTemplate(compiler);
				compiler.ToParent();
			}
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00047B88 File Offset: 0x00046B88
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				if (!processor.BeginEvent(XPathNodeType.Comment, string.Empty, string.Empty, string.Empty, false))
				{
					return;
				}
				processor.PushActionFrame(frame);
				frame.State = 1;
				return;
			case 1:
				if (!processor.EndEvent(XPathNodeType.Comment))
				{
					return;
				}
				frame.Finished();
				return;
			default:
				return;
			}
		}
	}
}
