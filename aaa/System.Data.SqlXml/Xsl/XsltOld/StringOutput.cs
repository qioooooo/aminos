using System;
using System.Text;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000194 RID: 404
	internal class StringOutput : SequentialOutput
	{
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00053A5F File Offset: 0x00052A5F
		internal string Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00053A67 File Offset: 0x00052A67
		internal StringOutput(Processor processor)
			: base(processor)
		{
			this.builder = new StringBuilder();
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00053A7B File Offset: 0x00052A7B
		internal override void Write(char outputChar)
		{
			this.builder.Append(outputChar);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00053A8A File Offset: 0x00052A8A
		internal override void Write(string outputText)
		{
			this.builder.Append(outputText);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00053A99 File Offset: 0x00052A99
		internal override void Close()
		{
			this.result = this.builder.ToString();
		}

		// Token: 0x04000BB5 RID: 2997
		private StringBuilder builder;

		// Token: 0x04000BB6 RID: 2998
		private string result;
	}
}
