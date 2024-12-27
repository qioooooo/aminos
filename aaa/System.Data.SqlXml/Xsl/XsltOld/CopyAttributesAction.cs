using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000145 RID: 325
	internal sealed class CopyAttributesAction : Action
	{
		// Token: 0x06000E52 RID: 3666 RVA: 0x000494DC File Offset: 0x000484DC
		internal static CopyAttributesAction GetAction()
		{
			return CopyAttributesAction.s_Action;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x000494E4 File Offset: 0x000484E4
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			while (processor.CanContinue)
			{
				switch (frame.State)
				{
				case 0:
					if (!frame.Node.HasAttributes || !frame.Node.MoveToFirstAttribute())
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
					if (!CopyAttributesAction.SendTextEvent(processor, frame.Node))
					{
						return;
					}
					frame.State = 4;
					continue;
				case 4:
					if (!CopyAttributesAction.SendEndEvent(processor, frame.Node))
					{
						return;
					}
					frame.State = 5;
					continue;
				case 5:
					if (frame.Node.MoveToNextAttribute())
					{
						frame.State = 2;
						continue;
					}
					frame.Node.MoveToParent();
					frame.Finished();
					return;
				default:
					return;
				}
				if (!CopyAttributesAction.SendBeginEvent(processor, frame.Node))
				{
					return;
				}
				frame.State = 3;
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000495C0 File Offset: 0x000485C0
		private static bool SendBeginEvent(Processor processor, XPathNavigator node)
		{
			return processor.BeginEvent(XPathNodeType.Attribute, node.Prefix, node.LocalName, node.NamespaceURI, false);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000495DC File Offset: 0x000485DC
		private static bool SendTextEvent(Processor processor, XPathNavigator node)
		{
			return processor.TextEvent(node.Value);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x000495EA File Offset: 0x000485EA
		private static bool SendEndEvent(Processor processor, XPathNavigator node)
		{
			return processor.EndEvent(XPathNodeType.Attribute);
		}

		// Token: 0x0400094B RID: 2379
		private const int BeginEvent = 2;

		// Token: 0x0400094C RID: 2380
		private const int TextEvent = 3;

		// Token: 0x0400094D RID: 2381
		private const int EndEvent = 4;

		// Token: 0x0400094E RID: 2382
		private const int Advance = 5;

		// Token: 0x0400094F RID: 2383
		private static CopyAttributesAction s_Action = new CopyAttributesAction();
	}
}
