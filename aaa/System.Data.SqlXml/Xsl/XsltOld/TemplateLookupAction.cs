using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000197 RID: 407
	internal class TemplateLookupAction : Action
	{
		// Token: 0x06001168 RID: 4456 RVA: 0x00054167 File Offset: 0x00053167
		internal void Initialize(XmlQualifiedName mode, Stylesheet importsOf)
		{
			this.mode = mode;
			this.importsOf = importsOf;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00054178 File Offset: 0x00053178
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			Action action;
			if (this.mode != null)
			{
				action = ((this.importsOf == null) ? processor.Stylesheet.FindTemplate(processor, frame.Node, this.mode) : this.importsOf.FindTemplateImports(processor, frame.Node, this.mode));
			}
			else
			{
				action = ((this.importsOf == null) ? processor.Stylesheet.FindTemplate(processor, frame.Node) : this.importsOf.FindTemplateImports(processor, frame.Node));
			}
			if (action == null)
			{
				action = this.BuiltInTemplate(frame.Node);
			}
			if (action != null)
			{
				frame.SetAction(action);
				return;
			}
			frame.Finished();
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00054224 File Offset: 0x00053224
		internal Action BuiltInTemplate(XPathNavigator node)
		{
			Action action = null;
			switch (node.NodeType)
			{
			case XPathNodeType.Root:
			case XPathNodeType.Element:
				action = ApplyTemplatesAction.BuiltInRule(this.mode);
				break;
			case XPathNodeType.Attribute:
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
				action = ValueOfAction.BuiltInRule();
				break;
			}
			return action;
		}

		// Token: 0x04000BC4 RID: 3012
		protected XmlQualifiedName mode;

		// Token: 0x04000BC5 RID: 3013
		protected Stylesheet importsOf;
	}
}
