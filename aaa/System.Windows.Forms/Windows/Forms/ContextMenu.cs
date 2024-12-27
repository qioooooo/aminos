using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020002A1 RID: 673
	[DefaultEvent("Popup")]
	public class ContextMenu : Menu
	{
		// Token: 0x06002468 RID: 9320 RVA: 0x000541E3 File Offset: 0x000531E3
		public ContextMenu()
			: base(null)
		{
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000541F3 File Offset: 0x000531F3
		public ContextMenu(MenuItem[] menuItems)
			: base(menuItems)
		{
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x00054203 File Offset: 0x00053203
		[SRDescription("ContextMenuSourceControlDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control SourceControl
		{
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return this.sourceControl;
			}
		}

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x0600246B RID: 9323 RVA: 0x0005420B File Offset: 0x0005320B
		// (remove) Token: 0x0600246C RID: 9324 RVA: 0x00054224 File Offset: 0x00053224
		[SRDescription("MenuItemOnInitDescr")]
		public event EventHandler Popup
		{
			add
			{
				this.onPopup = (EventHandler)Delegate.Combine(this.onPopup, value);
			}
			remove
			{
				this.onPopup = (EventHandler)Delegate.Remove(this.onPopup, value);
			}
		}

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x0600246D RID: 9325 RVA: 0x0005423D File Offset: 0x0005323D
		// (remove) Token: 0x0600246E RID: 9326 RVA: 0x00054256 File Offset: 0x00053256
		[SRDescription("ContextMenuCollapseDescr")]
		public event EventHandler Collapse
		{
			add
			{
				this.onCollapse = (EventHandler)Delegate.Combine(this.onCollapse, value);
			}
			remove
			{
				this.onCollapse = (EventHandler)Delegate.Remove(this.onCollapse, value);
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x0005426F File Offset: 0x0005326F
		// (set) Token: 0x06002470 RID: 9328 RVA: 0x00054298 File Offset: 0x00053298
		[Localizable(true)]
		[SRDescription("MenuRightToLeftDescr")]
		[DefaultValue(RightToLeft.No)]
		public virtual RightToLeft RightToLeft
		{
			get
			{
				if (RightToLeft.Inherit != this.rightToLeft)
				{
					return this.rightToLeft;
				}
				if (this.sourceControl != null)
				{
					return this.sourceControl.RightToLeft;
				}
				return RightToLeft.No;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("RightToLeft", (int)value, typeof(RightToLeft));
				}
				if (this.RightToLeft != value)
				{
					this.rightToLeft = value;
					base.UpdateRtl(value == RightToLeft.Yes);
				}
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000542E5 File Offset: 0x000532E5
		internal override bool RenderIsRightToLeft
		{
			get
			{
				return this.rightToLeft == RightToLeft.Yes;
			}
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000542F0 File Offset: 0x000532F0
		protected internal virtual void OnPopup(EventArgs e)
		{
			if (this.onPopup != null)
			{
				this.onPopup(this, e);
			}
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x00054307 File Offset: 0x00053307
		protected internal virtual void OnCollapse(EventArgs e)
		{
			if (this.onCollapse != null)
			{
				this.onCollapse(this, e);
			}
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x0005431E File Offset: 0x0005331E
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal virtual bool ProcessCmdKey(ref Message msg, Keys keyData, Control control)
		{
			this.sourceControl = control;
			return this.ProcessCmdKey(ref msg, keyData);
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x0005432F File Offset: 0x0005332F
		private void ResetRightToLeft()
		{
			this.RightToLeft = RightToLeft.No;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x00054338 File Offset: 0x00053338
		internal virtual bool ShouldSerializeRightToLeft()
		{
			return RightToLeft.Inherit != this.rightToLeft;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x00054346 File Offset: 0x00053346
		public void Show(Control control, Point pos)
		{
			this.Show(control, pos, 66);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x00054352 File Offset: 0x00053352
		public void Show(Control control, Point pos, LeftRightAlignment alignment)
		{
			if (alignment == LeftRightAlignment.Left)
			{
				this.Show(control, pos, 74);
				return;
			}
			this.Show(control, pos, 66);
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0005436C File Offset: 0x0005336C
		private void Show(Control control, Point pos, int flags)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (!control.IsHandleCreated || !control.Visible)
			{
				throw new ArgumentException(SR.GetString("ContextMenuInvalidParent"), "control");
			}
			this.sourceControl = control;
			this.OnPopup(EventArgs.Empty);
			pos = control.PointToScreen(pos);
			SafeNativeMethods.TrackPopupMenuEx(new HandleRef(this, base.Handle), flags, pos.X, pos.Y, new HandleRef(control, control.Handle), null);
		}

		// Token: 0x040015B3 RID: 5555
		private EventHandler onPopup;

		// Token: 0x040015B4 RID: 5556
		private EventHandler onCollapse;

		// Token: 0x040015B5 RID: 5557
		internal Control sourceControl;

		// Token: 0x040015B6 RID: 5558
		private RightToLeft rightToLeft = RightToLeft.Inherit;
	}
}
