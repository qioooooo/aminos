using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200012C RID: 300
	internal abstract class Action
	{
		// Token: 0x06000D38 RID: 3384
		internal abstract void Execute(Processor processor, ActionFrame frame);

		// Token: 0x06000D39 RID: 3385 RVA: 0x00044D38 File Offset: 0x00043D38
		internal virtual void ReplaceNamespaceAlias(Compiler compiler)
		{
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00044D3A File Offset: 0x00043D3A
		internal virtual DbgData GetDbgData(ActionFrame frame)
		{
			return DbgData.Empty;
		}

		// Token: 0x040008DF RID: 2271
		internal const int Initialized = 0;

		// Token: 0x040008E0 RID: 2272
		internal const int Finished = -1;
	}
}
