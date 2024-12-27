using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000133 RID: 307
	internal class ApplyTemplatesAction : ContainerAction
	{
		// Token: 0x06000D96 RID: 3478 RVA: 0x00046A6F File Offset: 0x00045A6F
		internal static ApplyTemplatesAction BuiltInRule()
		{
			return ApplyTemplatesAction.s_BuiltInRule;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00046A76 File Offset: 0x00045A76
		internal static ApplyTemplatesAction BuiltInRule(XmlQualifiedName mode)
		{
			if (!(mode == null) && !mode.IsEmpty)
			{
				return new ApplyTemplatesAction(mode);
			}
			return ApplyTemplatesAction.BuiltInRule();
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00046A95 File Offset: 0x00045A95
		internal ApplyTemplatesAction()
		{
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00046AA4 File Offset: 0x00045AA4
		private ApplyTemplatesAction(XmlQualifiedName mode)
		{
			this.mode = mode;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00046ABA File Offset: 0x00045ABA
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			this.CompileContent(compiler);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00046ACC File Offset: 0x00045ACC
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Select))
			{
				this.selectKey = compiler.AddQuery(value);
			}
			else
			{
				if (!Keywords.Equals(localName, compiler.Atoms.Mode))
				{
					return false;
				}
				if (compiler.AllowBuiltInMode && value == "*")
				{
					this.mode = Compiler.BuiltInMode;
				}
				else
				{
					this.mode = compiler.CreateXPathQName(value);
				}
			}
			return true;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00046B5C File Offset: 0x00045B5C
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
						if (Keywords.Equals(namespaceURI, input.Atoms.XsltNamespace))
						{
							if (Keywords.Equals(localName, input.Atoms.Sort))
							{
								base.AddAction(compiler.CreateSortAction());
							}
							else
							{
								if (!Keywords.Equals(localName, input.Atoms.WithParam))
								{
									goto IL_00BF;
								}
								WithParamAction withParamAction = compiler.CreateWithParamAction();
								base.CheckDuplicateParams(withParamAction.Name);
								base.AddAction(withParamAction);
							}
							compiler.PopScope();
							goto IL_00F3;
						}
						goto IL_00C6;
					}
					case XPathNodeType.SignificantWhitespace:
					case XPathNodeType.Whitespace:
					case XPathNodeType.ProcessingInstruction:
					case XPathNodeType.Comment:
						goto IL_00F3;
					}
					break;
					IL_00F3:
					if (!compiler.Advance())
					{
						goto Block_5;
					}
				}
				goto IL_00D5;
				IL_00BF:
				throw compiler.UnexpectedKeyword();
				IL_00C6:
				throw compiler.UnexpectedKeyword();
				IL_00D5:
				throw XsltException.Create("Xslt_InvalidContents", new string[] { "apply-templates" });
				Block_5:
				compiler.ToParent();
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00046C70 File Offset: 0x00045C70
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				processor.ResetParams();
				processor.InitSortArray();
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
				goto IL_00C2;
			case 4:
				goto IL_00DB;
			case 5:
				frame.State = 3;
				goto IL_00C2;
			default:
				return;
			}
			if (this.selectKey == -1)
			{
				if (!frame.Node.HasChildren)
				{
					frame.Finished();
					return;
				}
				frame.InitNewNodeSet(frame.Node.SelectChildren(XPathNodeType.All));
			}
			else
			{
				frame.InitNewNodeSet(processor.StartQuery(frame.NodeSet, this.selectKey));
			}
			if (processor.SortArray.Count != 0)
			{
				frame.SortNewNodeSet(processor, processor.SortArray);
			}
			frame.State = 3;
			IL_00C2:
			if (!frame.NewNextNode(processor))
			{
				frame.Finished();
				return;
			}
			frame.State = 4;
			IL_00DB:
			processor.PushTemplateLookup(frame.NewNodeSet, this.mode, null);
			frame.State = 5;
		}

		// Token: 0x040008F3 RID: 2291
		private const int ProcessedChildren = 2;

		// Token: 0x040008F4 RID: 2292
		private const int ProcessNextNode = 3;

		// Token: 0x040008F5 RID: 2293
		private const int PositionAdvanced = 4;

		// Token: 0x040008F6 RID: 2294
		private const int TemplateProcessed = 5;

		// Token: 0x040008F7 RID: 2295
		private int selectKey = -1;

		// Token: 0x040008F8 RID: 2296
		private XmlQualifiedName mode;

		// Token: 0x040008F9 RID: 2297
		private static ApplyTemplatesAction s_BuiltInRule = new ApplyTemplatesAction();
	}
}
