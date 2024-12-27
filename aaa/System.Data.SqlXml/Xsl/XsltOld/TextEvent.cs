using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000138 RID: 312
	internal class TextEvent : Event
	{
		// Token: 0x06000DB4 RID: 3508 RVA: 0x0004733C File Offset: 0x0004633C
		protected TextEvent()
		{
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00047344 File Offset: 0x00046344
		public TextEvent(string text)
		{
			this.text = text;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00047354 File Offset: 0x00046354
		public TextEvent(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			this.text = input.Value;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0004737A File Offset: 0x0004637A
		public override bool Output(Processor processor, ActionFrame frame)
		{
			return processor.TextEvent(this.text);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00047388 File Offset: 0x00046388
		public virtual string Evaluate(Processor processor, ActionFrame frame)
		{
			return this.text;
		}

		// Token: 0x04000904 RID: 2308
		private string text;
	}
}
