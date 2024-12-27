using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000148 RID: 328
	internal sealed class CopyNodeSetAction : Action
	{
		// Token: 0x06000E63 RID: 3683 RVA: 0x000497EF File Offset: 0x000487EF
		internal static CopyNodeSetAction GetAction()
		{
			return CopyNodeSetAction.s_Action;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x000497F8 File Offset: 0x000487F8
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			while (processor.CanContinue)
			{
				switch (frame.State)
				{
				case 0:
					if (!frame.NextNode(processor))
					{
						frame.Finished();
						return;
					}
					frame.State = 2;
					break;
				case 1:
					return;
				case 2:
					break;
				case 3:
				{
					XPathNodeType nodeType = frame.Node.NodeType;
					if (nodeType == XPathNodeType.Element || nodeType == XPathNodeType.Root)
					{
						processor.PushActionFrame(CopyNamespacesAction.GetAction(), frame.NodeSet);
						frame.State = 4;
						return;
					}
					if (!CopyNodeSetAction.SendTextEvent(processor, frame.Node))
					{
						return;
					}
					frame.State = 7;
					continue;
				}
				case 4:
					processor.PushActionFrame(CopyAttributesAction.GetAction(), frame.NodeSet);
					frame.State = 5;
					return;
				case 5:
					if (frame.Node.HasChildren)
					{
						processor.PushActionFrame(CopyNodeSetAction.GetAction(), frame.Node.SelectChildren(XPathNodeType.All));
						frame.State = 6;
						return;
					}
					frame.State = 7;
					goto IL_0103;
				case 6:
					frame.State = 7;
					continue;
				case 7:
					goto IL_0103;
				default:
					return;
				}
				if (!CopyNodeSetAction.SendBeginEvent(processor, frame.Node))
				{
					return;
				}
				frame.State = 3;
				continue;
				IL_0103:
				if (!CopyNodeSetAction.SendEndEvent(processor, frame.Node))
				{
					return;
				}
				frame.State = 0;
			}
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00049929 File Offset: 0x00048929
		private static bool SendBeginEvent(Processor processor, XPathNavigator node)
		{
			return processor.CopyBeginEvent(node, node.IsEmptyElement);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00049938 File Offset: 0x00048938
		private static bool SendTextEvent(Processor processor, XPathNavigator node)
		{
			return processor.CopyTextEvent(node);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00049941 File Offset: 0x00048941
		private static bool SendEndEvent(Processor processor, XPathNavigator node)
		{
			return processor.CopyEndEvent(node);
		}

		// Token: 0x04000957 RID: 2391
		private const int BeginEvent = 2;

		// Token: 0x04000958 RID: 2392
		private const int Contents = 3;

		// Token: 0x04000959 RID: 2393
		private const int Namespaces = 4;

		// Token: 0x0400095A RID: 2394
		private const int Attributes = 5;

		// Token: 0x0400095B RID: 2395
		private const int Subtree = 6;

		// Token: 0x0400095C RID: 2396
		private const int EndEvent = 7;

		// Token: 0x0400095D RID: 2397
		private static CopyNodeSetAction s_Action = new CopyNodeSetAction();
	}
}
