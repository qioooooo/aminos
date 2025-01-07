using System;
using System.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	public class ScrollableControlDesigner : ParentControlDesigner
	{
		protected override bool GetHitTest(Point pt)
		{
			if (base.GetHitTest(pt))
			{
				return true;
			}
			ScrollableControl scrollableControl = (ScrollableControl)this.Control;
			if (scrollableControl.IsHandleCreated && scrollableControl.AutoScroll)
			{
				int num = (int)NativeMethods.SendMessage(scrollableControl.Handle, 132, (IntPtr)0, (IntPtr)NativeMethods.Util.MAKELPARAM(pt.X, pt.Y));
				if (num == 7 || num == 6)
				{
					return true;
				}
			}
			return false;
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			switch (m.Msg)
			{
			case 276:
			case 277:
				if (this.selManager == null)
				{
					this.selManager = this.GetService(typeof(SelectionManager)) as SelectionManager;
				}
				if (this.selManager != null)
				{
					this.selManager.Refresh();
				}
				this.Control.Invalidate();
				this.Control.Update();
				return;
			default:
				return;
			}
		}

		private SelectionManager selManager;
	}
}
