using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200019E RID: 414
	internal class BuiltInRuleTextAction : Action
	{
		// Token: 0x06001183 RID: 4483 RVA: 0x00054624 File Offset: 0x00053624
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
			{
				string text = processor.ValueOf(frame.NodeSet.Current);
				if (processor.TextEvent(text, false))
				{
					frame.Finished();
					return;
				}
				frame.StoredOutput = text;
				frame.State = 2;
				return;
			}
			case 1:
				break;
			case 2:
				processor.TextEvent(frame.StoredOutput);
				frame.Finished();
				break;
			default:
				return;
			}
		}

		// Token: 0x04000BCF RID: 3023
		private const int ResultStored = 2;
	}
}
