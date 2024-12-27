using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000137 RID: 311
	internal abstract class Event
	{
		// Token: 0x06000DAF RID: 3503 RVA: 0x00047323 File Offset: 0x00046323
		public virtual void ReplaceNamespaceAlias(Compiler compiler)
		{
		}

		// Token: 0x06000DB0 RID: 3504
		public abstract bool Output(Processor processor, ActionFrame frame);

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00047325 File Offset: 0x00046325
		internal void OnInstructionExecute(Processor processor)
		{
			processor.OnInstructionExecute();
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0004732D File Offset: 0x0004632D
		internal virtual DbgData DbgData
		{
			get
			{
				return DbgData.Empty;
			}
		}
	}
}
