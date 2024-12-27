using System;
using System.Collections;
using System.Text;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000136 RID: 310
	internal sealed class Avt
	{
		// Token: 0x06000DAA RID: 3498 RVA: 0x0004723F File Offset: 0x0004623F
		private Avt(string constAvt)
		{
			this.constAvt = constAvt;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00047250 File Offset: 0x00046250
		private Avt(ArrayList eventList)
		{
			this.events = new TextEvent[eventList.Count];
			for (int i = 0; i < eventList.Count; i++)
			{
				this.events[i] = (TextEvent)eventList[i];
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00047299 File Offset: 0x00046299
		public bool IsConstant
		{
			get
			{
				return this.events == null;
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000472A4 File Offset: 0x000462A4
		internal string Evaluate(Processor processor, ActionFrame frame)
		{
			if (this.IsConstant)
			{
				return this.constAvt;
			}
			StringBuilder sharedStringBuilder = processor.GetSharedStringBuilder();
			for (int i = 0; i < this.events.Length; i++)
			{
				sharedStringBuilder.Append(this.events[i].Evaluate(processor, frame));
			}
			processor.ReleaseSharedStringBuilder();
			return sharedStringBuilder.ToString();
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x000472FC File Offset: 0x000462FC
		internal static Avt CompileAvt(Compiler compiler, string avtText)
		{
			bool flag;
			ArrayList arrayList = compiler.CompileAvt(avtText, out flag);
			if (!flag)
			{
				return new Avt(arrayList);
			}
			return new Avt(avtText);
		}

		// Token: 0x04000902 RID: 2306
		private string constAvt;

		// Token: 0x04000903 RID: 2307
		private TextEvent[] events;
	}
}
