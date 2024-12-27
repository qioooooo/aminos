using System;
using System.Windows.Forms;

namespace System.Deployment.Application
{
	// Token: 0x0200004B RID: 75
	internal class FormPiece : Panel
	{
		// Token: 0x0600025E RID: 606 RVA: 0x0000F27B File Offset: 0x0000E27B
		public virtual bool OnClosing()
		{
			return true;
		}
	}
}
