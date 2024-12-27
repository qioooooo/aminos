using System;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x0200004C RID: 76
	internal class ModalPiece : FormPiece
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000F286 File Offset: 0x0000E286
		public UserInterfaceModalResult ModalResult
		{
			get
			{
				return this._modalResult;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000F290 File Offset: 0x0000E290
		public override bool OnClosing()
		{
			bool flag = base.OnClosing();
			this._modalEvent.Set();
			return flag;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000F2B1 File Offset: 0x0000E2B1
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this._modalEvent.Set();
		}

		// Token: 0x040001D6 RID: 470
		protected ManualResetEvent _modalEvent;

		// Token: 0x040001D7 RID: 471
		protected UserInterfaceModalResult _modalResult;
	}
}
