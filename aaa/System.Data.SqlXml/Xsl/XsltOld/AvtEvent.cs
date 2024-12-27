using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000139 RID: 313
	internal sealed class AvtEvent : TextEvent
	{
		// Token: 0x06000DB9 RID: 3513 RVA: 0x00047390 File Offset: 0x00046390
		public AvtEvent(int key)
		{
			this.key = key;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0004739F File Offset: 0x0004639F
		public override bool Output(Processor processor, ActionFrame frame)
		{
			return processor.TextEvent(processor.EvaluateString(frame, this.key));
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x000473B4 File Offset: 0x000463B4
		public override string Evaluate(Processor processor, ActionFrame frame)
		{
			return processor.EvaluateString(frame, this.key);
		}

		// Token: 0x04000905 RID: 2309
		private int key;
	}
}
