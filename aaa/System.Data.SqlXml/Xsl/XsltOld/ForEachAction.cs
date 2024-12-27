using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000156 RID: 342
	internal class ForEachAction : ContainerAction
	{
		// Token: 0x06000EC0 RID: 3776 RVA: 0x0004A43C File Offset: 0x0004943C
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckRequiredAttribute(compiler, this.selectKey != -1, "select");
			compiler.CanHaveApplyImports = false;
			if (compiler.Recurse())
			{
				this.CompileSortElements(compiler);
				base.CompileTemplate(compiler);
				compiler.ToParent();
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0004A48C File Offset: 0x0004948C
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Select))
			{
				this.selectKey = compiler.AddQuery(value);
				return true;
			}
			return false;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0004A4D8 File Offset: 0x000494D8
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				if (this.sortContainer != null)
				{
					processor.InitSortArray();
					processor.PushActionFrame(this.sortContainer, frame.NodeSet);
					frame.State = 2;
					return;
				}
				break;
			case 1:
				return;
			case 2:
				break;
			case 3:
				goto IL_0082;
			case 4:
				goto IL_009B;
			case 5:
				frame.State = 3;
				goto IL_0082;
			default:
				return;
			}
			frame.InitNewNodeSet(processor.StartQuery(frame.NodeSet, this.selectKey));
			if (this.sortContainer != null)
			{
				frame.SortNewNodeSet(processor, processor.SortArray);
			}
			frame.State = 3;
			IL_0082:
			if (!frame.NewNextNode(processor))
			{
				frame.Finished();
				return;
			}
			frame.State = 4;
			IL_009B:
			processor.PushActionFrame(frame, frame.NewNodeSet);
			frame.State = 5;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0004A5A0 File Offset: 0x000495A0
		protected void CompileSortElements(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			for (;;)
			{
				switch (input.NodeType)
				{
				case XPathNodeType.Element:
					if (!Keywords.Equals(input.NamespaceURI, input.Atoms.XsltNamespace) || !Keywords.Equals(input.LocalName, input.Atoms.Sort))
					{
						return;
					}
					if (this.sortContainer == null)
					{
						this.sortContainer = new ContainerAction();
					}
					this.sortContainer.AddAction(compiler.CreateSortAction());
					break;
				case XPathNodeType.Text:
					return;
				case XPathNodeType.SignificantWhitespace:
					base.AddEvent(compiler.CreateTextEvent());
					break;
				}
				if (!input.Advance())
				{
					return;
				}
			}
		}

		// Token: 0x0400097A RID: 2426
		private const int ProcessedSort = 2;

		// Token: 0x0400097B RID: 2427
		private const int ProcessNextNode = 3;

		// Token: 0x0400097C RID: 2428
		private const int PositionAdvanced = 4;

		// Token: 0x0400097D RID: 2429
		private const int ContentsProcessed = 5;

		// Token: 0x0400097E RID: 2430
		private int selectKey = -1;

		// Token: 0x0400097F RID: 2431
		private ContainerAction sortContainer;
	}
}
