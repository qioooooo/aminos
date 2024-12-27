using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000147 RID: 327
	internal sealed class CopyNamespacesAction : Action
	{
		// Token: 0x06000E5F RID: 3679 RVA: 0x0004970C File Offset: 0x0004870C
		internal static CopyNamespacesAction GetAction()
		{
			return CopyNamespacesAction.s_Action;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00049714 File Offset: 0x00048714
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			while (processor.CanContinue)
			{
				switch (frame.State)
				{
				case 0:
					if (!frame.Node.MoveToFirstNamespace(XPathNamespaceScope.ExcludeXml))
					{
						frame.Finished();
						return;
					}
					frame.State = 2;
					break;
				case 1:
				case 3:
					return;
				case 2:
					break;
				case 4:
					if (!processor.EndEvent(XPathNodeType.Namespace))
					{
						return;
					}
					frame.State = 5;
					continue;
				case 5:
					if (frame.Node.MoveToNextNamespace(XPathNamespaceScope.ExcludeXml))
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
				if (!processor.BeginEvent(XPathNodeType.Namespace, null, frame.Node.LocalName, frame.Node.Value, false))
				{
					return;
				}
				frame.State = 4;
			}
		}

		// Token: 0x04000952 RID: 2386
		private const int BeginEvent = 2;

		// Token: 0x04000953 RID: 2387
		private const int TextEvent = 3;

		// Token: 0x04000954 RID: 2388
		private const int EndEvent = 4;

		// Token: 0x04000955 RID: 2389
		private const int Advance = 5;

		// Token: 0x04000956 RID: 2390
		private static CopyNamespacesAction s_Action = new CopyNamespacesAction();
	}
}
