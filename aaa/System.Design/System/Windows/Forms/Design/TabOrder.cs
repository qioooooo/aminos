using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002A3 RID: 675
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	internal class TabOrder : Control, IMouseHandler, IMenuStatusHandler
	{
		// Token: 0x06001956 RID: 6486 RVA: 0x000886EC File Offset: 0x000876EC
		public TabOrder(IDesignerHost host)
		{
			this.host = host;
			IUIService iuiservice = (IUIService)host.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				this.tabFont = (Font)iuiservice.Styles["DialogFont"];
			}
			else
			{
				this.tabFont = Control.DefaultFont;
			}
			this.tabFont = new Font(this.tabFont, FontStyle.Bold);
			this.selSize = DesignerUtils.GetAdornmentDimensions(AdornmentType.GrabHandle).Width;
			this.drawString = new StringBuilder(12);
			this.highlightTextBrush = new SolidBrush(SystemColors.HighlightText);
			this.highlightPen = new Pen(SystemColors.Highlight);
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureInfo.CurrentCulture.GetFormat(typeof(NumberFormatInfo));
			if (numberFormatInfo != null)
			{
				this.decimalSep = numberFormatInfo.NumberDecimalSeparator;
			}
			else
			{
				this.decimalSep = ".";
			}
			this.tabProperties = new Hashtable();
			base.SetStyle(ControlStyles.Opaque, true);
			IOverlayService overlayService = (IOverlayService)host.GetService(typeof(IOverlayService));
			if (overlayService != null)
			{
				overlayService.PushOverlay(this);
			}
			IHelpService helpService = (IHelpService)host.GetService(typeof(IHelpService));
			if (helpService != null)
			{
				helpService.AddContextAttribute("Keyword", "TabOrderView", HelpKeywordType.FilterKeyword);
			}
			this.commands = new MenuCommand[]
			{
				new MenuCommand(new EventHandler(this.OnKeyCancel), MenuCommands.KeyCancel),
				new MenuCommand(new EventHandler(this.OnKeyDefault), MenuCommands.KeyDefaultAction),
				new MenuCommand(new EventHandler(this.OnKeyPrevious), MenuCommands.KeyMoveUp),
				new MenuCommand(new EventHandler(this.OnKeyNext), MenuCommands.KeyMoveDown),
				new MenuCommand(new EventHandler(this.OnKeyPrevious), MenuCommands.KeyMoveLeft),
				new MenuCommand(new EventHandler(this.OnKeyNext), MenuCommands.KeyMoveRight),
				new MenuCommand(new EventHandler(this.OnKeyNext), MenuCommands.KeySelectNext),
				new MenuCommand(new EventHandler(this.OnKeyPrevious), MenuCommands.KeySelectPrevious)
			};
			this.newCommands = new MenuCommand[]
			{
				new MenuCommand(new EventHandler(this.OnKeyDefault), MenuCommands.KeyTabOrderSelect)
			};
			IMenuCommandService menuCommandService = (IMenuCommandService)host.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				foreach (MenuCommand menuCommand in this.newCommands)
				{
					menuCommandService.AddCommand(menuCommand);
				}
			}
			IEventHandlerService eventHandlerService = (IEventHandlerService)host.GetService(typeof(IEventHandlerService));
			if (eventHandlerService != null)
			{
				eventHandlerService.PushHandler(this);
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)host.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentAdded += this.OnComponentAddRemove;
				componentChangeService.ComponentRemoved += this.OnComponentAddRemove;
				componentChangeService.ComponentChanged += this.OnComponentChanged;
			}
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x000889F8 File Offset: 0x000879F8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.region != null)
				{
					this.region.Dispose();
					this.region = null;
				}
				if (this.host != null)
				{
					IOverlayService overlayService = (IOverlayService)this.host.GetService(typeof(IOverlayService));
					if (overlayService != null)
					{
						overlayService.RemoveOverlay(this);
					}
					IEventHandlerService eventHandlerService = (IEventHandlerService)this.host.GetService(typeof(IEventHandlerService));
					if (eventHandlerService != null)
					{
						eventHandlerService.PopHandler(this);
					}
					IMenuCommandService menuCommandService = (IMenuCommandService)this.host.GetService(typeof(IMenuCommandService));
					if (menuCommandService != null)
					{
						foreach (MenuCommand menuCommand in this.newCommands)
						{
							menuCommandService.RemoveCommand(menuCommand);
						}
					}
					IComponentChangeService componentChangeService = (IComponentChangeService)this.host.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentAdded -= this.OnComponentAddRemove;
						componentChangeService.ComponentRemoved -= this.OnComponentAddRemove;
						componentChangeService.ComponentChanged -= this.OnComponentChanged;
					}
					IHelpService helpService = (IHelpService)this.host.GetService(typeof(IHelpService));
					if (helpService != null)
					{
						helpService.RemoveContextAttribute("Keyword", "TabOrderView");
					}
					this.host = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00088B58 File Offset: 0x00087B58
		private void DrawTabs(IList tabs, Graphics gr, bool fRegion)
		{
			IEnumerator enumerator = tabs.GetEnumerator();
			int num = 0;
			Rectangle rectangle = Rectangle.Empty;
			Size size = Size.Empty;
			Font font = this.tabFont;
			if (fRegion)
			{
				this.region = new Region(new Rectangle(0, 0, 0, 0));
			}
			if (this.ctlHover != null)
			{
				Rectangle convertedBounds = this.GetConvertedBounds(this.ctlHover);
				Rectangle rectangle2 = convertedBounds;
				rectangle2.Inflate(this.selSize, this.selSize);
				if (fRegion)
				{
					this.region = new Region(rectangle2);
					this.region.Exclude(convertedBounds);
				}
				else
				{
					Control parent = this.ctlHover.Parent;
					Color backColor = parent.BackColor;
					Region clip = gr.Clip;
					gr.ExcludeClip(convertedBounds);
					gr.FillRectangle(new SolidBrush(backColor), rectangle2);
					ControlPaint.DrawSelectionFrame(gr, false, rectangle2, convertedBounds, backColor);
					gr.Clip = clip;
				}
			}
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Control control = (Control)obj;
				rectangle = this.GetConvertedBounds(control);
				this.drawString.Length = 0;
				Control control2 = this.GetSitedParent(control);
				Control control3 = (Control)this.host.RootComponent;
				while (control2 != control3 && control2 != null)
				{
					this.drawString.Insert(0, this.decimalSep);
					this.drawString.Insert(0, control2.TabIndex.ToString(CultureInfo.CurrentCulture));
					control2 = this.GetSitedParent(control2);
				}
				this.drawString.Insert(0, ' ');
				this.drawString.Append(control.TabIndex.ToString(CultureInfo.CurrentCulture));
				this.drawString.Append(' ');
				if (((PropertyDescriptor)this.tabProperties[control]).IsReadOnly)
				{
					this.drawString.Append(SR.GetString("WindowsFormsTabOrderReadOnly"));
					this.drawString.Append(' ');
				}
				string text = this.drawString.ToString();
				size = Size.Ceiling(gr.MeasureString(text, font));
				rectangle.Width = size.Width + 2;
				rectangle.Height = size.Height + 2;
				this.tabGlyphs[num++] = rectangle;
				if (fRegion)
				{
					this.region.Union(rectangle);
				}
				else
				{
					Brush highlight;
					Pen highlightText;
					Color color;
					if (this.tabComplete.IndexOf(control) != -1)
					{
						highlight = this.highlightTextBrush;
						highlightText = this.highlightPen;
						color = SystemColors.Highlight;
					}
					else
					{
						highlight = SystemBrushes.Highlight;
						highlightText = SystemPens.HighlightText;
						color = SystemColors.HighlightText;
					}
					gr.FillRectangle(highlight, rectangle);
					gr.DrawRectangle(highlightText, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					Brush brush = new SolidBrush(color);
					gr.DrawString(text, font, brush, (float)(rectangle.X + 1), (float)(rectangle.Y + 1));
					brush.Dispose();
				}
			}
			if (fRegion)
			{
				Control control = (Control)this.host.RootComponent;
				rectangle = this.GetConvertedBounds(control);
				this.region.Intersect(rectangle);
				base.Region = this.region;
			}
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x00088E80 File Offset: 0x00087E80
		private Control GetControlAtPoint(IList tabs, int x, int y)
		{
			IEnumerator enumerator = tabs.GetEnumerator();
			Control control = null;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Control control2 = (Control)obj;
				Control sitedParent = this.GetSitedParent(control2);
				Rectangle bounds = control2.Bounds;
				if (sitedParent.RectangleToScreen(bounds).Contains(x, y))
				{
					control = control2;
				}
			}
			return control;
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x00088ED4 File Offset: 0x00087ED4
		private Rectangle GetConvertedBounds(Control ctl)
		{
			Control parent = ctl.Parent;
			Rectangle rectangle = ctl.Bounds;
			rectangle = parent.RectangleToScreen(rectangle);
			return base.RectangleToClient(rectangle);
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00088F00 File Offset: 0x00087F00
		private int GetMaxControlCount(Control ctl)
		{
			int num = 0;
			for (int i = 0; i < ctl.Controls.Count; i++)
			{
				if (this.GetTabbable(ctl.Controls[i]))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00088F40 File Offset: 0x00087F40
		private Control GetSitedParent(Control child)
		{
			Control control;
			for (control = child.Parent; control != null; control = control.Parent)
			{
				ISite site = control.Site;
				IContainer container = null;
				if (site != null)
				{
					container = site.Container;
				}
				container = DesignerUtils.CheckForNestedContainer(container);
				if (site != null && container == this.host)
				{
					break;
				}
			}
			return control;
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00088F88 File Offset: 0x00087F88
		private void GetTabbing(Control ctl, IList tabs)
		{
			int count = ctl.Controls.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				Control control = ctl.Controls[i];
				if (this.GetSitedParent(control) != null && this.GetTabbable(control))
				{
					tabs.Add(control);
				}
				if (control.Controls.Count > 0)
				{
					this.GetTabbing(control, tabs);
				}
			}
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00088FEC File Offset: 0x00087FEC
		private bool GetTabbable(Control control)
		{
			for (Control control2 = control; control2 != null; control2 = control2.Parent)
			{
				if (!control2.Visible)
				{
					return false;
				}
			}
			ISite site = control.Site;
			if (site == null || site.Container != this.host)
			{
				return false;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["TabIndex"];
			if (propertyDescriptor == null || !propertyDescriptor.IsBrowsable)
			{
				return false;
			}
			this.tabProperties[control] = propertyDescriptor;
			return true;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x00089058 File Offset: 0x00088058
		private void OnComponentAddRemove(object sender, ComponentEventArgs ce)
		{
			this.ctlHover = null;
			this.tabControls = null;
			this.tabGlyphs = null;
			if (this.tabComplete != null)
			{
				this.tabComplete.Clear();
			}
			if (this.tabNext != null)
			{
				this.tabNext.Clear();
			}
			if (this.region != null)
			{
				this.region.Dispose();
				this.region = null;
			}
			base.Invalidate();
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x000890C0 File Offset: 0x000880C0
		private void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			this.tabControls = null;
			this.tabGlyphs = null;
			if (this.region != null)
			{
				this.region.Dispose();
				this.region = null;
			}
			base.Invalidate();
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x000890F0 File Offset: 0x000880F0
		private void OnKeyCancel(object sender, EventArgs e)
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)this.host.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				MenuCommand menuCommand = menuCommandService.FindCommand(StandardCommands.TabOrder);
				if (menuCommand != null)
				{
					menuCommand.Invoke();
				}
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00089130 File Offset: 0x00088130
		private void OnKeyDefault(object sender, EventArgs e)
		{
			if (this.ctlHover != null)
			{
				this.SetNextTabIndex(this.ctlHover);
				this.RotateControls(true);
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0008914D File Offset: 0x0008814D
		private void OnKeyNext(object sender, EventArgs e)
		{
			this.RotateControls(true);
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00089156 File Offset: 0x00088156
		private void OnKeyPrevious(object sender, EventArgs e)
		{
			this.RotateControls(false);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0008915F File Offset: 0x0008815F
		public virtual void OnMouseDoubleClick(IComponent component)
		{
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x00089161 File Offset: 0x00088161
		public virtual void OnMouseDown(IComponent component, MouseButtons button, int x, int y)
		{
			if (this.ctlHover != null)
			{
				this.SetNextTabIndex(this.ctlHover);
			}
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x00089177 File Offset: 0x00088177
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.ctlHover != null)
			{
				this.SetNextTabIndex(this.ctlHover);
			}
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x00089194 File Offset: 0x00088194
		public virtual void OnMouseHover(IComponent component)
		{
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x00089198 File Offset: 0x00088198
		public virtual void OnMouseMove(IComponent component, int x, int y)
		{
			if (this.tabControls != null)
			{
				Control controlAtPoint = this.GetControlAtPoint(this.tabControls, x, y);
				this.SetNewHover(controlAtPoint);
			}
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x000891C4 File Offset: 0x000881C4
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.tabGlyphs != null)
			{
				Control control = null;
				for (int i = 0; i < this.tabGlyphs.Length; i++)
				{
					if (this.tabGlyphs[i].Contains(e.X, e.Y))
					{
						control = (Control)this.tabControls[i];
					}
				}
				this.SetNewHover(control);
			}
			this.SetAppropriateCursor();
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00089233 File Offset: 0x00088233
		public virtual void OnMouseUp(IComponent component, MouseButtons button)
		{
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00089235 File Offset: 0x00088235
		private void SetAppropriateCursor()
		{
			if (this.ctlHover != null)
			{
				Cursor.Current = Cursors.Cross;
				return;
			}
			Cursor.Current = Cursors.Default;
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00089254 File Offset: 0x00088254
		public virtual void OnSetCursor(IComponent component)
		{
			this.SetAppropriateCursor();
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0008925C File Offset: 0x0008825C
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this.tabControls == null)
			{
				this.tabControls = new ArrayList();
				this.GetTabbing((Control)this.host.RootComponent, this.tabControls);
				this.tabGlyphs = new Rectangle[this.tabControls.Count];
			}
			if (this.tabComplete == null)
			{
				this.tabComplete = new ArrayList();
			}
			if (this.tabNext == null)
			{
				this.tabNext = new Hashtable();
			}
			if (this.region == null)
			{
				this.DrawTabs(this.tabControls, e.Graphics, true);
			}
			this.DrawTabs(this.tabControls, e.Graphics, false);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0008930C File Offset: 0x0008830C
		public bool OverrideInvoke(MenuCommand cmd)
		{
			for (int i = 0; i < this.commands.Length; i++)
			{
				if (this.commands[i].CommandID.Equals(cmd.CommandID))
				{
					this.commands[i].Invoke();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00089358 File Offset: 0x00088358
		public bool OverrideStatus(MenuCommand cmd)
		{
			for (int i = 0; i < this.commands.Length; i++)
			{
				if (this.commands[i].CommandID.Equals(cmd.CommandID))
				{
					cmd.Enabled = this.commands[i].Enabled;
					return true;
				}
			}
			if (!cmd.CommandID.Equals(StandardCommands.TabOrder))
			{
				cmd.Enabled = false;
				return true;
			}
			return false;
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x000893C4 File Offset: 0x000883C4
		private void RotateControls(bool forward)
		{
			Control control = this.ctlHover;
			Control control2 = (Control)this.host.RootComponent;
			if (control == null)
			{
				control = control2;
			}
			while ((control = control2.GetNextControl(control, forward)) != null && !this.GetTabbable(control))
			{
			}
			this.SetNewHover(control);
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0008940C File Offset: 0x0008840C
		private void SetNewHover(Control ctl)
		{
			if (this.ctlHover != ctl)
			{
				if (this.ctlHover != null)
				{
					if (this.region != null)
					{
						this.region.Dispose();
						this.region = null;
					}
					Rectangle convertedBounds = this.GetConvertedBounds(this.ctlHover);
					convertedBounds.Inflate(this.selSize, this.selSize);
					base.Invalidate(convertedBounds);
				}
				this.ctlHover = ctl;
				if (this.ctlHover != null)
				{
					if (this.region != null)
					{
						this.region.Dispose();
						this.region = null;
					}
					Rectangle convertedBounds2 = this.GetConvertedBounds(this.ctlHover);
					convertedBounds2.Inflate(this.selSize, this.selSize);
					base.Invalidate(convertedBounds2);
				}
			}
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x000894C0 File Offset: 0x000884C0
		private void SetNextTabIndex(Control ctl)
		{
			if (this.tabControls != null)
			{
				Control sitedParent = this.GetSitedParent(ctl);
				object obj = this.tabNext[sitedParent];
				if (this.tabComplete.IndexOf(ctl) == -1)
				{
					this.tabComplete.Add(ctl);
				}
				int num;
				if (obj != null)
				{
					num = (int)obj;
				}
				else
				{
					num = 0;
				}
				try
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)this.tabProperties[ctl];
					if (propertyDescriptor != null)
					{
						int num2 = num + 1;
						if (propertyDescriptor.IsReadOnly)
						{
							num2 = (int)propertyDescriptor.GetValue(ctl) + 1;
						}
						int maxControlCount = this.GetMaxControlCount(sitedParent);
						if (num2 >= maxControlCount)
						{
							num2 = 0;
						}
						this.tabNext[sitedParent] = num2;
						if (this.tabComplete.Count == this.tabControls.Count)
						{
							this.tabComplete.Clear();
						}
						if (!propertyDescriptor.IsReadOnly)
						{
							try
							{
								propertyDescriptor.SetValue(ctl, num);
								goto IL_00EB;
							}
							catch (Exception)
							{
								goto IL_00EB;
							}
						}
						base.Invalidate();
					}
					IL_00EB:;
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x0400148D RID: 5261
		private IDesignerHost host;

		// Token: 0x0400148E RID: 5262
		private Control ctlHover;

		// Token: 0x0400148F RID: 5263
		private ArrayList tabControls;

		// Token: 0x04001490 RID: 5264
		private Rectangle[] tabGlyphs;

		// Token: 0x04001491 RID: 5265
		private ArrayList tabComplete;

		// Token: 0x04001492 RID: 5266
		private Hashtable tabNext;

		// Token: 0x04001493 RID: 5267
		private Font tabFont;

		// Token: 0x04001494 RID: 5268
		private StringBuilder drawString;

		// Token: 0x04001495 RID: 5269
		private Brush highlightTextBrush;

		// Token: 0x04001496 RID: 5270
		private Pen highlightPen;

		// Token: 0x04001497 RID: 5271
		private int selSize;

		// Token: 0x04001498 RID: 5272
		private Hashtable tabProperties;

		// Token: 0x04001499 RID: 5273
		private Region region;

		// Token: 0x0400149A RID: 5274
		private MenuCommand[] commands;

		// Token: 0x0400149B RID: 5275
		private MenuCommand[] newCommands;

		// Token: 0x0400149C RID: 5276
		private string decimalSep;
	}
}
