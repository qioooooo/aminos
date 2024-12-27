using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000144 RID: 324
	internal class CopyAction : ContainerAction
	{
		// Token: 0x06000E4E RID: 3662 RVA: 0x0004934C File Offset: 0x0004834C
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			if (compiler.Recurse())
			{
				base.CompileTemplate(compiler);
				compiler.ToParent();
			}
			if (this.containedActions == null)
			{
				this.empty = true;
			}
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0004937C File Offset: 0x0004837C
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.UseAttributeSets))
			{
				this.useAttributeSets = value;
				base.AddAction(compiler.CreateUseAttributeSetsAction());
				return true;
			}
			return false;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x000493CC File Offset: 0x000483CC
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			while (processor.CanContinue)
			{
				switch (frame.State)
				{
				case 0:
					if (Processor.IsRoot(frame.Node))
					{
						processor.PushActionFrame(frame);
						frame.State = 8;
						return;
					}
					if (!processor.CopyBeginEvent(frame.Node, this.empty))
					{
						return;
					}
					frame.State = 5;
					break;
				case 1:
				case 2:
				case 3:
				case 4:
					return;
				case 5:
					frame.State = 6;
					if (frame.Node.NodeType == XPathNodeType.Element)
					{
						processor.PushActionFrame(CopyNamespacesAction.GetAction(), frame.NodeSet);
						return;
					}
					break;
				case 6:
					if (frame.Node.NodeType == XPathNodeType.Element && !this.empty)
					{
						processor.PushActionFrame(frame);
						frame.State = 7;
						return;
					}
					if (!processor.CopyTextEvent(frame.Node))
					{
						return;
					}
					frame.State = 7;
					break;
				case 7:
					if (processor.CopyEndEvent(frame.Node))
					{
						frame.Finished();
						return;
					}
					return;
				case 8:
					frame.Finished();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x04000944 RID: 2372
		private const int CopyText = 4;

		// Token: 0x04000945 RID: 2373
		private const int NamespaceCopy = 5;

		// Token: 0x04000946 RID: 2374
		private const int ContentsCopy = 6;

		// Token: 0x04000947 RID: 2375
		private const int ProcessChildren = 7;

		// Token: 0x04000948 RID: 2376
		private const int ChildrenOnly = 8;

		// Token: 0x04000949 RID: 2377
		private string useAttributeSets;

		// Token: 0x0400094A RID: 2378
		private bool empty;
	}
}
