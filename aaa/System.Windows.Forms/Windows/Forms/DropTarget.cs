using System;
using System.Runtime.InteropServices.ComTypes;

namespace System.Windows.Forms
{
	// Token: 0x020003D8 RID: 984
	internal class DropTarget : UnsafeNativeMethods.IOleDropTarget
	{
		// Token: 0x06003B05 RID: 15109 RVA: 0x000D5CE8 File Offset: 0x000D4CE8
		public DropTarget(IDropTarget owner)
		{
			this.owner = owner;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000D5CF8 File Offset: 0x000D4CF8
		private DragEventArgs CreateDragEventArgs(object pDataObj, int grfKeyState, NativeMethods.POINTL pt, int pdwEffect)
		{
			IDataObject dataObject;
			if (pDataObj == null)
			{
				dataObject = this.lastDataObject;
			}
			else if (pDataObj is IDataObject)
			{
				dataObject = (IDataObject)pDataObj;
			}
			else
			{
				if (!(pDataObj is IDataObject))
				{
					return null;
				}
				dataObject = new DataObject(pDataObj);
			}
			DragEventArgs dragEventArgs = new DragEventArgs(dataObject, grfKeyState, pt.x, pt.y, (DragDropEffects)pdwEffect, this.lastEffect);
			this.lastDataObject = dataObject;
			return dragEventArgs;
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x000D5D5C File Offset: 0x000D4D5C
		int UnsafeNativeMethods.IOleDropTarget.OleDragEnter(object pDataObj, int grfKeyState, long pt, ref int pdwEffect)
		{
			DragEventArgs dragEventArgs = this.CreateDragEventArgs(pDataObj, grfKeyState, new NativeMethods.POINTL
			{
				x = this.GetX(pt),
				y = this.GetY(pt)
			}, pdwEffect);
			if (dragEventArgs != null)
			{
				this.owner.OnDragEnter(dragEventArgs);
				pdwEffect = (int)dragEventArgs.Effect;
				this.lastEffect = dragEventArgs.Effect;
			}
			else
			{
				pdwEffect = 0;
			}
			return 0;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x000D5DC4 File Offset: 0x000D4DC4
		int UnsafeNativeMethods.IOleDropTarget.OleDragOver(int grfKeyState, long pt, ref int pdwEffect)
		{
			DragEventArgs dragEventArgs = this.CreateDragEventArgs(null, grfKeyState, new NativeMethods.POINTL
			{
				x = this.GetX(pt),
				y = this.GetY(pt)
			}, pdwEffect);
			this.owner.OnDragOver(dragEventArgs);
			pdwEffect = (int)dragEventArgs.Effect;
			this.lastEffect = dragEventArgs.Effect;
			return 0;
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x000D5E1E File Offset: 0x000D4E1E
		int UnsafeNativeMethods.IOleDropTarget.OleDragLeave()
		{
			this.owner.OnDragLeave(EventArgs.Empty);
			return 0;
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x000D5E34 File Offset: 0x000D4E34
		int UnsafeNativeMethods.IOleDropTarget.OleDrop(object pDataObj, int grfKeyState, long pt, ref int pdwEffect)
		{
			DragEventArgs dragEventArgs = this.CreateDragEventArgs(pDataObj, grfKeyState, new NativeMethods.POINTL
			{
				x = this.GetX(pt),
				y = this.GetY(pt)
			}, pdwEffect);
			if (dragEventArgs != null)
			{
				this.owner.OnDragDrop(dragEventArgs);
				pdwEffect = (int)dragEventArgs.Effect;
			}
			else
			{
				pdwEffect = 0;
			}
			this.lastEffect = DragDropEffects.None;
			this.lastDataObject = null;
			return 0;
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x000D5E9B File Offset: 0x000D4E9B
		private int GetX(long pt)
		{
			return (int)(pt & (long)((ulong)(-1)));
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x000D5EA2 File Offset: 0x000D4EA2
		private int GetY(long pt)
		{
			return (int)((pt >> 32) & (long)((ulong)(-1)));
		}

		// Token: 0x04001D7C RID: 7548
		private IDataObject lastDataObject;

		// Token: 0x04001D7D RID: 7549
		private DragDropEffects lastEffect;

		// Token: 0x04001D7E RID: 7550
		private IDropTarget owner;
	}
}
