using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200013C RID: 316
	internal class CallTemplateAction : ContainerAction
	{
		// Token: 0x06000DD4 RID: 3540 RVA: 0x000477F9 File Offset: 0x000467F9
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckRequiredAttribute(compiler, this.name, "name");
			this.CompileContent(compiler);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0004781C File Offset: 0x0004681C
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Name))
			{
				this.name = compiler.CreateXPathQName(value);
				return true;
			}
			return false;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00047868 File Offset: 0x00046868
		private void CompileContent(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			if (compiler.Recurse())
			{
				for (;;)
				{
					switch (input.NodeType)
					{
					case XPathNodeType.Element:
					{
						compiler.PushNamespaceScope();
						string namespaceURI = input.NamespaceURI;
						string localName = input.LocalName;
						if (Keywords.Equals(namespaceURI, input.Atoms.XsltNamespace) && Keywords.Equals(localName, input.Atoms.WithParam))
						{
							WithParamAction withParamAction = compiler.CreateWithParamAction();
							base.CheckDuplicateParams(withParamAction.Name);
							base.AddAction(withParamAction);
							compiler.PopScope();
							goto IL_00C8;
						}
						goto IL_009B;
					}
					case XPathNodeType.SignificantWhitespace:
					case XPathNodeType.Whitespace:
					case XPathNodeType.ProcessingInstruction:
					case XPathNodeType.Comment:
						goto IL_00C8;
					}
					break;
					IL_00C8:
					if (!compiler.Advance())
					{
						goto Block_4;
					}
				}
				goto IL_00AA;
				IL_009B:
				throw compiler.UnexpectedKeyword();
				IL_00AA:
				throw XsltException.Create("Xslt_InvalidContents", new string[] { "call-template" });
				Block_4:
				compiler.ToParent();
			}
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00047950 File Offset: 0x00046950
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				processor.ResetParams();
				if (this.containedActions != null && this.containedActions.Count > 0)
				{
					processor.PushActionFrame(frame);
					frame.State = 2;
					return;
				}
				break;
			case 1:
				return;
			case 2:
				break;
			case 3:
				frame.Finished();
				return;
			default:
				return;
			}
			TemplateAction templateAction = processor.Stylesheet.FindTemplate(this.name);
			if (templateAction != null)
			{
				frame.State = 3;
				processor.PushActionFrame(templateAction, frame.NodeSet);
				return;
			}
			throw XsltException.Create("Xslt_InvalidCallTemplate", new string[] { this.name.ToString() });
		}

		// Token: 0x04000918 RID: 2328
		private const int ProcessedChildren = 2;

		// Token: 0x04000919 RID: 2329
		private const int ProcessedTemplate = 3;

		// Token: 0x0400091A RID: 2330
		private XmlQualifiedName name;
	}
}
