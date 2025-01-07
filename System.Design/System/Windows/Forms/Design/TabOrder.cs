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
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	internal class TabOrder : Control, IMouseHandler, IMenuStatusHandler
	{
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

		private Rectangle GetConvertedBounds(Control ctl)
		{
			Control parent = ctl.Parent;
			Rectangle rectangle = ctl.Bounds;
			rectangle = parent.RectangleToScreen(rectangle);
			return base.RectangleToClient(rectangle);
		}

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

		private void OnKeyDefault(object sender, EventArgs e)
		{
			if (this.ctlHover != null)
			{
				this.SetNextTabIndex(this.ctlHover);
				this.RotateControls(true);
			}
		}

		private void OnKeyNext(object sender, EventArgs e)
		{
			this.RotateControls(true);
		}

		private void OnKeyPrevious(object sender, EventArgs e)
		{
			this.RotateControls(false);
		}

		public virtual void OnMouseDoubleClick(IComponent component)
		{
		}

		public virtual void OnMouseDown(IComponent component, MouseButtons button, int x, int y)
		{
			if (this.ctlHover != null)
			{
				this.SetNextTabIndex(this.ctlHover);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.ctlHover != null)
			{
				this.SetNextTabIndex(this.ctlHover);
			}
		}

		public virtual void OnMouseHover(IComponent component)
		{
		}

		public virtual void OnMouseMove(IComponent component, int x, int y)
		{
			if (this.tabControls != null)
			{
				Control controlAtPoint = this.GetControlAtPoint(this.tabControls, x, y);
				this.SetNewHover(controlAtPoint);
			}
		}

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

		public virtual void OnMouseUp(IComponent component, MouseButtons button)
		{
		}

		private void SetAppropriateCursor()
		{
			if (this.ctlHover != null)
			{
				Cursor.Current = Cursors.Cross;
				return;
			}
			Cursor.Current = Cursors.Default;
		}

		public virtual void OnSetCursor(IComponent component)
		{
			this.SetAppropriateCursor();
		}

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

		private IDesignerHost host;

		private Control ctlHover;

		private ArrayList tabControls;

		private Rectangle[] tabGlyphs;

		private ArrayList tabComplete;

		private Hashtable tabNext;

		private Font tabFont;

		private StringBuilder drawString;

		private Brush highlightTextBrush;

		private Pen highlightPen;

		private int selSize;

		private Hashtable tabProperties;

		private Region region;

		private MenuCommand[] commands;

		private MenuCommand[] newCommands;

		private string decimalSep;
	}
}
