using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020002AE RID: 686
	[DefaultEvent("Opening")]
	[ComVisible(true)]
	[SRDescription("DescriptionContextMenuStrip")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ContextMenuStrip : ToolStripDropDownMenu
	{
		// Token: 0x060025B3 RID: 9651 RVA: 0x000583B5 File Offset: 0x000573B5
		public ContextMenuStrip(IContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000583D2 File Offset: 0x000573D2
		public ContextMenuStrip()
		{
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000583DA File Offset: 0x000573DA
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000583E3 File Offset: 0x000573E3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("ContextMenuStripSourceControlDescr")]
		public Control SourceControl
		{
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return base.SourceControlInternal;
			}
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000583EC File Offset: 0x000573EC
		internal ContextMenuStrip Clone()
		{
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			contextMenuStrip.Events.AddHandlers(base.Events);
			contextMenuStrip.AutoClose = base.AutoClose;
			contextMenuStrip.AutoSize = this.AutoSize;
			contextMenuStrip.Bounds = base.Bounds;
			contextMenuStrip.ImageList = base.ImageList;
			contextMenuStrip.ShowCheckMargin = base.ShowCheckMargin;
			contextMenuStrip.ShowImageMargin = base.ShowImageMargin;
			for (int i = 0; i < this.Items.Count; i++)
			{
				ToolStripItem toolStripItem = this.Items[i];
				if (toolStripItem is ToolStripSeparator)
				{
					contextMenuStrip.Items.Add(new ToolStripSeparator());
				}
				else if (toolStripItem is ToolStripMenuItem)
				{
					ToolStripMenuItem toolStripMenuItem = toolStripItem as ToolStripMenuItem;
					contextMenuStrip.Items.Add(toolStripMenuItem.Clone());
				}
			}
			return contextMenuStrip;
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000584B8 File Offset: 0x000574B8
		internal void ShowInternal(Control source, Point location, bool isKeyboardActivated)
		{
			base.Show(source, location);
			if (isKeyboardActivated)
			{
				ToolStripManager.ModalMenuFilter.Instance.ShowUnderlines = true;
			}
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x000584D0 File Offset: 0x000574D0
		internal void ShowInTaskbar(int x, int y)
		{
			base.WorkingAreaConstrained = false;
			Rectangle rectangle = base.CalculateDropDownLocation(new Point(x, y), ToolStripDropDownDirection.AboveLeft);
			Rectangle bounds = Screen.FromRectangle(rectangle).Bounds;
			if (rectangle.Y < bounds.Y)
			{
				rectangle = base.CalculateDropDownLocation(new Point(x, y), ToolStripDropDownDirection.BelowLeft);
			}
			else if (rectangle.X < bounds.X)
			{
				rectangle = base.CalculateDropDownLocation(new Point(x, y), ToolStripDropDownDirection.AboveRight);
			}
			rectangle = WindowsFormsUtils.ConstrainToBounds(bounds, rectangle);
			base.Show(rectangle.X, rectangle.Y);
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x0005855B File Offset: 0x0005755B
		protected override void SetVisibleCore(bool visible)
		{
			if (!visible)
			{
				base.WorkingAreaConstrained = true;
			}
			base.SetVisibleCore(visible);
		}
	}
}
