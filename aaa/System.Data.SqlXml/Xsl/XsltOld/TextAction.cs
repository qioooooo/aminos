using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200016C RID: 364
	internal class TextAction : CompiledAction
	{
		// Token: 0x06000F3A RID: 3898 RVA: 0x0004CC3E File Offset: 0x0004BC3E
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			this.CompileContent(compiler);
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0004CC50 File Offset: 0x0004BC50
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.DisableOutputEscaping))
			{
				this.disableOutputEscaping = compiler.GetYesNo(value);
				return true;
			}
			return false;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0004CC9C File Offset: 0x0004BC9C
		private void CompileContent(Compiler compiler)
		{
			if (compiler.Recurse())
			{
				NavigatorInput input = compiler.Input;
				this.text = string.Empty;
				for (;;)
				{
					switch (input.NodeType)
					{
					case XPathNodeType.Text:
					case XPathNodeType.SignificantWhitespace:
					case XPathNodeType.Whitespace:
						this.text += input.Value;
						goto IL_005F;
					case XPathNodeType.ProcessingInstruction:
					case XPathNodeType.Comment:
						goto IL_005F;
					}
					break;
					IL_005F:
					if (!compiler.Advance())
					{
						goto Block_3;
					}
				}
				throw compiler.UnexpectedKeyword();
				Block_3:
				compiler.ToParent();
			}
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0004CD18 File Offset: 0x0004BD18
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			int state = frame.State;
			if (state != 0)
			{
				return;
			}
			if (processor.TextEvent(this.text, this.disableOutputEscaping))
			{
				frame.Finished();
			}
		}

		// Token: 0x040009DB RID: 2523
		private bool disableOutputEscaping;

		// Token: 0x040009DC RID: 2524
		private string text;
	}
}
