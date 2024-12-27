using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000165 RID: 357
	internal abstract class TemplateBaseAction : ContainerAction
	{
		// Token: 0x06000F07 RID: 3847 RVA: 0x0004BC78 File Offset: 0x0004AC78
		public int AllocateVariableSlot()
		{
			int num = this.variableFreeSlot;
			this.variableFreeSlot++;
			if (this.variableCount < this.variableFreeSlot)
			{
				this.variableCount = this.variableFreeSlot;
			}
			return num;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0004BCB5 File Offset: 0x0004ACB5
		public void ReleaseVariableSlots(int n)
		{
		}

		// Token: 0x040009BC RID: 2492
		protected int variableCount;

		// Token: 0x040009BD RID: 2493
		private int variableFreeSlot;
	}
}
