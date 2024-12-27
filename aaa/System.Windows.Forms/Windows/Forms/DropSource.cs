using System;

namespace System.Windows.Forms
{
	// Token: 0x020003D7 RID: 983
	internal class DropSource : UnsafeNativeMethods.IOleDropSource
	{
		// Token: 0x06003B02 RID: 15106 RVA: 0x000D5C26 File Offset: 0x000D4C26
		public DropSource(ISupportOleDropSource peer)
		{
			if (peer == null)
			{
				throw new ArgumentNullException("peer");
			}
			this.peer = peer;
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x000D5C44 File Offset: 0x000D4C44
		public int OleQueryContinueDrag(int fEscapePressed, int grfKeyState)
		{
			bool flag = fEscapePressed != 0;
			DragAction dragAction = DragAction.Continue;
			if (flag)
			{
				dragAction = DragAction.Cancel;
			}
			else if ((grfKeyState & 1) == 0 && (grfKeyState & 2) == 0 && (grfKeyState & 16) == 0)
			{
				dragAction = DragAction.Drop;
			}
			QueryContinueDragEventArgs queryContinueDragEventArgs = new QueryContinueDragEventArgs(grfKeyState, flag, dragAction);
			this.peer.OnQueryContinueDrag(queryContinueDragEventArgs);
			int num = 0;
			switch (queryContinueDragEventArgs.Action)
			{
			case DragAction.Drop:
				num = 262400;
				break;
			case DragAction.Cancel:
				num = 262401;
				break;
			}
			return num;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x000D5CB8 File Offset: 0x000D4CB8
		public int OleGiveFeedback(int dwEffect)
		{
			GiveFeedbackEventArgs giveFeedbackEventArgs = new GiveFeedbackEventArgs((DragDropEffects)dwEffect, true);
			this.peer.OnGiveFeedback(giveFeedbackEventArgs);
			if (giveFeedbackEventArgs.UseDefaultCursors)
			{
				return 262402;
			}
			return 0;
		}

		// Token: 0x04001D78 RID: 7544
		private const int DragDropSDrop = 262400;

		// Token: 0x04001D79 RID: 7545
		private const int DragDropSCancel = 262401;

		// Token: 0x04001D7A RID: 7546
		private const int DragDropSUseDefaultCursors = 262402;

		// Token: 0x04001D7B RID: 7547
		private ISupportOleDropSource peer;
	}
}
