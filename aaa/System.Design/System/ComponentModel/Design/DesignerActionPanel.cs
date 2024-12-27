using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace System.ComponentModel.Design
{
	// Token: 0x02000107 RID: 263
	internal sealed class DesignerActionPanel : ContainerControl
	{
		// Token: 0x06000ABA RID: 2746 RVA: 0x0002955C File Offset: 0x0002855C
		public DesignerActionPanel(IServiceProvider serviceProvider)
		{
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.Opaque, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			this._serviceProvider = serviceProvider;
			this._lines = new List<DesignerActionPanel.Line>();
			this._lineHeights = new List<int>();
			this._lineYPositions = new List<int>();
			this._toolTip = new ToolTip();
			IUIService iuiservice = (IUIService)this.ServiceProvider.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				this.Font = (Font)iuiservice.Styles["DialogFont"];
				if (iuiservice.Styles["VsColorPanelGradientDark"] is Color)
				{
					this._gradientDarkColor = (Color)iuiservice.Styles["VsColorPanelGradientDark"];
				}
				if (iuiservice.Styles["VsColorPanelGradientLight"] is Color)
				{
					this._gradientLightColor = (Color)iuiservice.Styles["VsColorPanelGradientLight"];
				}
				if (iuiservice.Styles["VsColorPanelHyperLink"] is Color)
				{
					this._linkColor = (Color)iuiservice.Styles["VsColorPanelHyperLink"];
				}
				if (iuiservice.Styles["VsColorPanelHyperLinkPressed"] is Color)
				{
					this._activeLinkColor = (Color)iuiservice.Styles["VsColorPanelHyperLinkPressed"];
				}
				if (iuiservice.Styles["VsColorPanelTitleBar"] is Color)
				{
					this._titleBarColor = (Color)iuiservice.Styles["VsColorPanelTitleBar"];
				}
				if (iuiservice.Styles["VsColorPanelTitleBarUnselected"] is Color)
				{
					this._titleBarUnselectedColor = (Color)iuiservice.Styles["VsColorPanelTitleBarUnselected"];
				}
				if (iuiservice.Styles["VsColorPanelTitleBarText"] is Color)
				{
					this._titleBarTextColor = (Color)iuiservice.Styles["VsColorPanelTitleBarText"];
				}
				if (iuiservice.Styles["VsColorPanelBorder"] is Color)
				{
					this._borderColor = (Color)iuiservice.Styles["VsColorPanelBorder"];
				}
				if (iuiservice.Styles["VsColorPanelSeparator"] is Color)
				{
					this._separatorColor = (Color)iuiservice.Styles["VsColorPanelSeparator"];
				}
			}
			this.MinimumSize = new Size(150, 0);
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00029845 File Offset: 0x00028845
		public Color ActiveLinkColor
		{
			get
			{
				return this._activeLinkColor;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0002984D File Offset: 0x0002884D
		public Color BorderColor
		{
			get
			{
				return this._borderColor;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00029855 File Offset: 0x00028855
		private bool DropDownActive
		{
			get
			{
				return this._dropDownActive;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00029860 File Offset: 0x00028860
		public CommandID[] FilteredCommandIDs
		{
			get
			{
				if (this._filteredCommandIDs == null)
				{
					this._filteredCommandIDs = new CommandID[]
					{
						StandardCommands.Copy,
						StandardCommands.Cut,
						StandardCommands.Delete,
						StandardCommands.F1Help,
						StandardCommands.Paste,
						StandardCommands.Redo,
						StandardCommands.SelectAll,
						StandardCommands.Undo,
						MenuCommands.KeyCancel,
						MenuCommands.KeyReverseCancel,
						MenuCommands.KeyDefaultAction,
						MenuCommands.KeyEnd,
						MenuCommands.KeyHome,
						MenuCommands.KeyMoveDown,
						MenuCommands.KeyMoveLeft,
						MenuCommands.KeyMoveRight,
						MenuCommands.KeyMoveUp,
						MenuCommands.KeyNudgeDown,
						MenuCommands.KeyNudgeHeightDecrease,
						MenuCommands.KeyNudgeHeightIncrease,
						MenuCommands.KeyNudgeLeft,
						MenuCommands.KeyNudgeRight,
						MenuCommands.KeyNudgeUp,
						MenuCommands.KeyNudgeWidthDecrease,
						MenuCommands.KeyNudgeWidthIncrease,
						MenuCommands.KeySizeHeightDecrease,
						MenuCommands.KeySizeHeightIncrease,
						MenuCommands.KeySizeWidthDecrease,
						MenuCommands.KeySizeWidthIncrease,
						MenuCommands.KeySelectNext,
						MenuCommands.KeySelectPrevious,
						MenuCommands.KeyShiftEnd,
						MenuCommands.KeyShiftHome
					};
				}
				return this._filteredCommandIDs;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000299B0 File Offset: 0x000289B0
		private DesignerActionPanel.Line FocusedLine
		{
			get
			{
				Control activeControl = base.ActiveControl;
				if (activeControl != null)
				{
					return activeControl.Tag as DesignerActionPanel.Line;
				}
				return null;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x000299D4 File Offset: 0x000289D4
		public Color GradientDarkColor
		{
			get
			{
				return this._gradientDarkColor;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x000299DC File Offset: 0x000289DC
		public Color GradientLightColor
		{
			get
			{
				return this._gradientLightColor;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x000299E4 File Offset: 0x000289E4
		// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x000299EC File Offset: 0x000289EC
		public bool InMethodInvoke
		{
			get
			{
				return this._inMethodInvoke;
			}
			internal set
			{
				this._inMethodInvoke = value;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x000299F5 File Offset: 0x000289F5
		public Color LinkColor
		{
			get
			{
				return this._linkColor;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000299FD File Offset: 0x000289FD
		public Color SeparatorColor
		{
			get
			{
				return this._separatorColor;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x00029A05 File Offset: 0x00028A05
		private IServiceProvider ServiceProvider
		{
			get
			{
				return this._serviceProvider;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00029A0D File Offset: 0x00028A0D
		public Color TitleBarColor
		{
			get
			{
				return this._titleBarColor;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00029A15 File Offset: 0x00028A15
		public Color TitleBarTextColor
		{
			get
			{
				return this._titleBarTextColor;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00029A1D File Offset: 0x00028A1D
		public Color TitleBarUnselectedColor
		{
			get
			{
				return this._titleBarUnselectedColor;
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000ACA RID: 2762 RVA: 0x00029A25 File Offset: 0x00028A25
		// (remove) Token: 0x06000ACB RID: 2763 RVA: 0x00029A38 File Offset: 0x00028A38
		private event EventHandler FormActivated
		{
			add
			{
				base.Events.AddHandler(DesignerActionPanel.EventFormActivated, value);
			}
			remove
			{
				base.Events.RemoveHandler(DesignerActionPanel.EventFormActivated, value);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000ACC RID: 2764 RVA: 0x00029A4B File Offset: 0x00028A4B
		// (remove) Token: 0x06000ACD RID: 2765 RVA: 0x00029A5E File Offset: 0x00028A5E
		private event EventHandler FormDeactivate
		{
			add
			{
				base.Events.AddHandler(DesignerActionPanel.EventFormDeactivate, value);
			}
			remove
			{
				base.Events.RemoveHandler(DesignerActionPanel.EventFormDeactivate, value);
			}
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00029A74 File Offset: 0x00028A74
		private void AddToCategories(DesignerActionPanel.LineInfo lineInfo, ListDictionary categories)
		{
			string text = lineInfo.Item.Category;
			if (text == null)
			{
				text = string.Empty;
			}
			ListDictionary listDictionary = (ListDictionary)categories[text];
			if (listDictionary == null)
			{
				listDictionary = new ListDictionary();
				categories.Add(text, listDictionary);
			}
			List<DesignerActionPanel.LineInfo> list = (List<DesignerActionPanel.LineInfo>)listDictionary[lineInfo.List];
			if (list == null)
			{
				list = new List<DesignerActionPanel.LineInfo>();
				listDictionary.Add(lineInfo.List, list);
			}
			list.Add(lineInfo);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00029AE4 File Offset: 0x00028AE4
		public static Point ComputePreferredDesktopLocation(Rectangle rectangleAnchor, Size sizePanel, out DockStyle edgeToDock)
		{
			Rectangle workingArea = Screen.FromPoint(rectangleAnchor.Location).WorkingArea;
			bool flag = true;
			bool flag2 = false;
			if (rectangleAnchor.Right + sizePanel.Width > workingArea.Right)
			{
				flag = false;
				if (rectangleAnchor.Left - sizePanel.Width < workingArea.Left)
				{
					flag2 = true;
				}
			}
			bool flag3 = flag;
			bool flag4 = false;
			if (flag3)
			{
				if (rectangleAnchor.Bottom + sizePanel.Height > workingArea.Bottom)
				{
					flag3 = false;
					if (rectangleAnchor.Top - sizePanel.Height < workingArea.Top)
					{
						flag4 = true;
					}
				}
			}
			else if (rectangleAnchor.Top - sizePanel.Height < workingArea.Top)
			{
				flag3 = true;
				if (rectangleAnchor.Bottom + sizePanel.Height > workingArea.Bottom)
				{
					flag4 = true;
				}
			}
			if (flag4)
			{
				flag2 = false;
			}
			int num = 0;
			int num2 = 0;
			edgeToDock = DockStyle.None;
			if (flag2 && flag3)
			{
				num = workingArea.Left;
				num2 = rectangleAnchor.Bottom;
				edgeToDock = DockStyle.Bottom;
			}
			else if (flag2 && !flag3)
			{
				num = workingArea.Left;
				num2 = rectangleAnchor.Top - sizePanel.Height;
				edgeToDock = DockStyle.Top;
			}
			else if (flag && flag4)
			{
				num = rectangleAnchor.Right;
				num2 = workingArea.Top;
				edgeToDock = DockStyle.Right;
			}
			else if (flag && flag3)
			{
				num = rectangleAnchor.Right;
				num2 = rectangleAnchor.Top;
				edgeToDock = DockStyle.Right;
			}
			else if (flag && !flag3)
			{
				num = rectangleAnchor.Right;
				num2 = rectangleAnchor.Bottom - sizePanel.Height;
				edgeToDock = DockStyle.Right;
			}
			else if (!flag && flag4)
			{
				num = rectangleAnchor.Left - sizePanel.Width;
				num2 = workingArea.Top;
				edgeToDock = DockStyle.Left;
			}
			else if (!flag && flag3)
			{
				num = rectangleAnchor.Left - sizePanel.Width;
				num2 = rectangleAnchor.Top;
				edgeToDock = DockStyle.Left;
			}
			else if (!flag && !flag3)
			{
				num = rectangleAnchor.Right - sizePanel.Width;
				num2 = rectangleAnchor.Top - sizePanel.Height;
				edgeToDock = DockStyle.Top;
			}
			return new Point(num, num2);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00029CF1 File Offset: 0x00028CF1
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._toolTip.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00029D08 File Offset: 0x00028D08
		private Size DoLayout(Size proposedSize, bool measureOnly)
		{
			if (base.Disposing || base.IsDisposed)
			{
				return Size.Empty;
			}
			int num = 150;
			int num2 = 0;
			base.SuspendLayout();
			try
			{
				this._lineYPositions.Clear();
				this._lineHeights.Clear();
				for (int i = 0; i < this._lines.Count; i++)
				{
					DesignerActionPanel.Line line = this._lines[i];
					this._lineYPositions.Add(num2);
					Size size = line.LayoutControls(num2, proposedSize.Width, measureOnly);
					num = Math.Max(num, size.Width);
					this._lineHeights.Add(size.Height);
					num2 += size.Height;
				}
			}
			finally
			{
				base.ResumeLayout(!measureOnly);
			}
			return new Size(num, num2 + 2);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00029DE0 File Offset: 0x00028DE0
		public override Size GetPreferredSize(Size proposedSize)
		{
			if (proposedSize.IsEmpty)
			{
				return proposedSize;
			}
			return this.DoLayout(proposedSize, true);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00029DF5 File Offset: 0x00028DF5
		private static bool IsReadOnlyProperty(PropertyDescriptor pd)
		{
			return pd.IsReadOnly || pd.ComponentType.GetProperty(pd.Name).GetSetMethod() == null;
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00029E1A File Offset: 0x00028E1A
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateEditXPos();
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00029E2C File Offset: 0x00028E2C
		private void OnFormActivated(object sender, EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[DesignerActionPanel.EventFormActivated];
			if (eventHandler != null)
			{
				eventHandler(sender, e);
			}
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00029E5C File Offset: 0x00028E5C
		private void OnFormClosing(object sender, CancelEventArgs e)
		{
			if (!e.Cancel && base.TopLevelControl != null)
			{
				Form form = (Form)base.TopLevelControl;
				if (form != null)
				{
					form.Activated -= this.OnFormActivated;
					form.Deactivate -= this.OnFormDeactivate;
					form.Closing -= this.OnFormClosing;
				}
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00029EC0 File Offset: 0x00028EC0
		private void OnFormDeactivate(object sender, EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[DesignerActionPanel.EventFormDeactivate];
			if (eventHandler != null)
			{
				eventHandler(sender, e);
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00029EF0 File Offset: 0x00028EF0
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			Form form = base.TopLevelControl as Form;
			if (form != null)
			{
				form.Activated += this.OnFormActivated;
				form.Deactivate += this.OnFormDeactivate;
				form.Closing += this.OnFormClosing;
			}
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00029F49 File Offset: 0x00028F49
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this._updatingTasks)
			{
				return;
			}
			this.DoLayout(base.Size, false);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00029F64 File Offset: 0x00028F64
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this._updatingTasks)
			{
				return;
			}
			Rectangle bounds = base.Bounds;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, this.GradientDarkColor, this.GradientLightColor, LinearGradientMode.Horizontal))
				{
					e.Graphics.FillRectangle(linearGradientBrush, base.ClientRectangle);
					goto IL_0084;
				}
			}
			using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(bounds, this.GradientLightColor, this.GradientDarkColor, LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(linearGradientBrush2, base.ClientRectangle);
			}
			IL_0084:
			using (Pen pen = new Pen(this.BorderColor))
			{
				e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, base.Width - 1, base.Height - 1));
			}
			Rectangle clipRectangle = e.ClipRectangle;
			int num = 0;
			while (num < this._lineYPositions.Count - 1 && this._lineYPositions[num + 1] <= clipRectangle.Top)
			{
				num++;
			}
			Graphics graphics = e.Graphics;
			for (int i = num; i < this._lineYPositions.Count; i++)
			{
				DesignerActionPanel.Line line = this._lines[i];
				int num2 = this._lineYPositions[i];
				int num3 = this._lineHeights[i];
				int width = base.Width;
				graphics.SetClip(new Rectangle(0, num2, width, num3));
				graphics.TranslateTransform(0f, (float)num2);
				line.PaintLine(graphics, width, num3);
				graphics.ResetTransform();
				if (num2 + num3 > clipRectangle.Bottom)
				{
					return;
				}
			}
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002A138 File Offset: 0x00029138
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			base.PerformLayout();
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0002A148 File Offset: 0x00029148
		protected override bool ProcessDialogKey(Keys keyData)
		{
			DesignerActionPanel.Line focusedLine = this.FocusedLine;
			return (focusedLine != null && focusedLine.ProcessDialogKey(keyData)) || base.ProcessDialogKey(keyData);
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002A171 File Offset: 0x00029171
		protected override bool ProcessTabKey(bool forward)
		{
			return base.SelectNextControl(base.ActiveControl, forward, true, true, true);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002A184 File Offset: 0x00029184
		private void ProcessLists(DesignerActionListCollection lists, ListDictionary categories)
		{
			if (lists == null)
			{
				return;
			}
			foreach (object obj in lists)
			{
				DesignerActionList designerActionList = (DesignerActionList)obj;
				if (designerActionList != null)
				{
					IEnumerable sortedActionItems = designerActionList.GetSortedActionItems();
					if (sortedActionItems != null)
					{
						foreach (object obj2 in sortedActionItems)
						{
							DesignerActionItem designerActionItem = (DesignerActionItem)obj2;
							if (designerActionItem != null)
							{
								DesignerActionPanel.LineInfo lineInfo = this.ProcessTaskItem(designerActionList, designerActionItem);
								if (lineInfo != null)
								{
									this.AddToCategories(lineInfo, categories);
									IComponent component = null;
									DesignerActionPropertyItem designerActionPropertyItem = designerActionItem as DesignerActionPropertyItem;
									if (designerActionPropertyItem != null)
									{
										component = designerActionPropertyItem.RelatedComponent;
									}
									else
									{
										DesignerActionMethodItem designerActionMethodItem = designerActionItem as DesignerActionMethodItem;
										if (designerActionMethodItem != null)
										{
											component = designerActionMethodItem.RelatedComponent;
										}
									}
									if (component != null)
									{
										IEnumerable<DesignerActionPanel.LineInfo> enumerable = this.ProcessRelatedTaskItems(component);
										if (enumerable != null)
										{
											foreach (DesignerActionPanel.LineInfo lineInfo2 in enumerable)
											{
												this.AddToCategories(lineInfo2, categories);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002A2DC File Offset: 0x000292DC
		private IEnumerable<DesignerActionPanel.LineInfo> ProcessRelatedTaskItems(IComponent relatedComponent)
		{
			DesignerActionListCollection designerActionListCollection = null;
			DesignerActionService designerActionService = (DesignerActionService)this.ServiceProvider.GetService(typeof(DesignerActionService));
			if (designerActionService != null)
			{
				designerActionListCollection = designerActionService.GetComponentActions(relatedComponent);
			}
			else
			{
				IServiceProvider serviceProvider = relatedComponent.Site;
				if (serviceProvider == null)
				{
					serviceProvider = this.ServiceProvider;
				}
				IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					ComponentDesigner componentDesigner = designerHost.GetDesigner(relatedComponent) as ComponentDesigner;
					if (componentDesigner != null)
					{
						designerActionListCollection = componentDesigner.ActionLists;
					}
				}
			}
			List<DesignerActionPanel.LineInfo> list = new List<DesignerActionPanel.LineInfo>();
			if (designerActionListCollection != null)
			{
				foreach (object obj in designerActionListCollection)
				{
					DesignerActionList designerActionList = (DesignerActionList)obj;
					if (designerActionList != null)
					{
						designerActionList.GetType();
						IEnumerable sortedActionItems = designerActionList.GetSortedActionItems();
						if (sortedActionItems != null)
						{
							foreach (object obj2 in sortedActionItems)
							{
								DesignerActionItem designerActionItem = (DesignerActionItem)obj2;
								if (designerActionItem != null && designerActionItem.AllowAssociate)
								{
									DesignerActionPanel.LineInfo lineInfo = this.ProcessTaskItem(designerActionList, designerActionItem);
									if (lineInfo != null)
									{
										list.Add(lineInfo);
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002A438 File Offset: 0x00029438
		private DesignerActionPanel.LineInfo ProcessTaskItem(DesignerActionList list, DesignerActionItem item)
		{
			DesignerActionPanel.Line line;
			if (item is DesignerActionMethodItem)
			{
				line = new DesignerActionPanel.MethodLine(this._serviceProvider, this);
			}
			else if (item is DesignerActionPropertyItem)
			{
				DesignerActionPropertyItem designerActionPropertyItem = (DesignerActionPropertyItem)item;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(list)[designerActionPropertyItem.MemberName];
				if (propertyDescriptor == null)
				{
					throw new InvalidOperationException(SR.GetString("DesignerActionPanel_CouldNotFindProperty", new object[]
					{
						designerActionPropertyItem.MemberName,
						list.GetType().FullName
					}));
				}
				DesignerActionPanel.TypeDescriptorContext typeDescriptorContext = new DesignerActionPanel.TypeDescriptorContext(this._serviceProvider, propertyDescriptor, list);
				UITypeEditor uitypeEditor = (UITypeEditor)propertyDescriptor.GetEditor(typeof(UITypeEditor));
				bool standardValuesSupported = propertyDescriptor.Converter.GetStandardValuesSupported(typeDescriptorContext);
				if (uitypeEditor == null)
				{
					if (propertyDescriptor.PropertyType == typeof(bool))
					{
						if (DesignerActionPanel.IsReadOnlyProperty(propertyDescriptor))
						{
							line = new DesignerActionPanel.TextBoxPropertyLine(this._serviceProvider, this);
						}
						else
						{
							line = new DesignerActionPanel.CheckBoxPropertyLine(this._serviceProvider, this);
						}
					}
					else if (standardValuesSupported)
					{
						line = new DesignerActionPanel.EditorPropertyLine(this._serviceProvider, this);
					}
					else
					{
						line = new DesignerActionPanel.TextBoxPropertyLine(this._serviceProvider, this);
					}
				}
				else
				{
					line = new DesignerActionPanel.EditorPropertyLine(this._serviceProvider, this);
				}
			}
			else
			{
				if (!(item is DesignerActionTextItem))
				{
					return null;
				}
				if (item is DesignerActionHeaderItem)
				{
					line = new DesignerActionPanel.HeaderLine(this._serviceProvider, this);
				}
				else
				{
					line = new DesignerActionPanel.TextLine(this._serviceProvider, this);
				}
			}
			return new DesignerActionPanel.LineInfo(list, item, line);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002A593 File Offset: 0x00029593
		private void SetDropDownActive(bool active)
		{
			this._dropDownActive = active;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002A59C File Offset: 0x0002959C
		private void ShowError(string errorMessage)
		{
			IUIService iuiservice = (IUIService)this.ServiceProvider.GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				iuiservice.ShowError(errorMessage);
				return;
			}
			MessageBoxOptions messageBoxOptions = (MessageBoxOptions)0;
			if (SR.GetString("RTL") != "RTL_False")
			{
				messageBoxOptions = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			MessageBox.Show(this, errorMessage, SR.GetString("UIServiceHelper_ErrorCaption"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, messageBoxOptions);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002A604 File Offset: 0x00029604
		private static string StripAmpersands(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(s.Length);
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '&')
				{
					i++;
					if (i == s.Length)
					{
						stringBuilder.Append('&');
						break;
					}
				}
				stringBuilder.Append(s[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002A674 File Offset: 0x00029674
		private void UpdateEditXPos()
		{
			int num = 0;
			for (int i = 0; i < this._lines.Count; i++)
			{
				DesignerActionPanel.TextBoxPropertyLine textBoxPropertyLine = this._lines[i] as DesignerActionPanel.TextBoxPropertyLine;
				if (textBoxPropertyLine != null)
				{
					num = Math.Max(num, textBoxPropertyLine.GetEditRegionXPos());
				}
			}
			for (int j = 0; j < this._lines.Count; j++)
			{
				DesignerActionPanel.TextBoxPropertyLine textBoxPropertyLine2 = this._lines[j] as DesignerActionPanel.TextBoxPropertyLine;
				if (textBoxPropertyLine2 != null)
				{
					textBoxPropertyLine2.SetEditRegionXPos(num);
				}
			}
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002A6F0 File Offset: 0x000296F0
		public void UpdateTasks(DesignerActionListCollection actionLists, DesignerActionListCollection serviceActionLists, string title, string subtitle)
		{
			this._updatingTasks = true;
			base.SuspendLayout();
			try
			{
				base.AccessibleName = title;
				base.AccessibleDescription = subtitle;
				string text = string.Empty;
				DesignerActionPanel.Line focusedLine = this.FocusedLine;
				if (focusedLine != null)
				{
					text = focusedLine.FocusId;
				}
				ListDictionary listDictionary = new ListDictionary();
				this.ProcessLists(actionLists, listDictionary);
				this.ProcessLists(serviceActionLists, listDictionary);
				List<DesignerActionPanel.LineInfo> list = new List<DesignerActionPanel.LineInfo>();
				list.Add(new DesignerActionPanel.LineInfo(null, new DesignerActionPanel.DesignerActionPanelHeaderItem(title, subtitle), new DesignerActionPanel.PanelHeaderLine(this._serviceProvider, this)));
				int num = 0;
				foreach (object obj in listDictionary.Values)
				{
					ListDictionary listDictionary2 = (ListDictionary)obj;
					int num2 = 0;
					foreach (object obj2 in listDictionary2.Values)
					{
						List<DesignerActionPanel.LineInfo> list2 = (List<DesignerActionPanel.LineInfo>)obj2;
						for (int i = 0; i < list2.Count; i++)
						{
							list.Add(list2[i]);
						}
						num2++;
						if (num2 < listDictionary2.Count)
						{
							list.Add(new DesignerActionPanel.LineInfo(null, null, new DesignerActionPanel.SeparatorLine(this._serviceProvider, this, true)));
						}
					}
					num++;
					if (num < listDictionary.Count)
					{
						list.Add(new DesignerActionPanel.LineInfo(null, null, new DesignerActionPanel.SeparatorLine(this._serviceProvider, this)));
					}
				}
				int num3 = 0;
				for (int j = 0; j < list.Count; j++)
				{
					DesignerActionPanel.LineInfo lineInfo = list[j];
					DesignerActionPanel.Line line = lineInfo.Line;
					bool flag = false;
					if (j < this._lines.Count)
					{
						DesignerActionPanel.Line line2 = this._lines[j];
						if (line2.GetType() == line.GetType())
						{
							line2.UpdateActionItem(lineInfo.List, lineInfo.Item, this._toolTip, ref num3);
							flag = true;
						}
						else
						{
							line2.RemoveControls(base.Controls);
							this._lines.RemoveAt(j);
						}
					}
					if (!flag)
					{
						List<Control> controls = line.GetControls();
						Control[] array = new Control[controls.Count];
						controls.CopyTo(array);
						base.Controls.AddRange(array);
						line.UpdateActionItem(lineInfo.List, lineInfo.Item, this._toolTip, ref num3);
						this._lines.Insert(j, line);
					}
				}
				for (int k = this._lines.Count - 1; k >= list.Count; k--)
				{
					DesignerActionPanel.Line line3 = this._lines[k];
					line3.RemoveControls(base.Controls);
					this._lines.RemoveAt(k);
				}
				if (!string.IsNullOrEmpty(text))
				{
					foreach (DesignerActionPanel.Line line4 in this._lines)
					{
						if (string.Equals(line4.FocusId, text, StringComparison.Ordinal))
						{
							line4.Focus();
						}
					}
				}
			}
			finally
			{
				this.UpdateEditXPos();
				this._updatingTasks = false;
				base.ResumeLayout(true);
			}
			base.Invalidate();
		}

		// Token: 0x04000DA2 RID: 3490
		public const string ExternDllGdi32 = "gdi32.dll";

		// Token: 0x04000DA3 RID: 3491
		public const string ExternDllUser32 = "user32.dll";

		// Token: 0x04000DA4 RID: 3492
		private const int EditInputWidth = 150;

		// Token: 0x04000DA5 RID: 3493
		private const int ListBoxMaximumHeight = 200;

		// Token: 0x04000DA6 RID: 3494
		private const int MinimumWidth = 150;

		// Token: 0x04000DA7 RID: 3495
		private const int BottomPadding = 2;

		// Token: 0x04000DA8 RID: 3496
		private const int TopPadding = 2;

		// Token: 0x04000DA9 RID: 3497
		private const int LineLeftMargin = 5;

		// Token: 0x04000DAA RID: 3498
		private const int LineRightMargin = 4;

		// Token: 0x04000DAB RID: 3499
		private const int LineVerticalPadding = 7;

		// Token: 0x04000DAC RID: 3500
		private const int TextBoxTopPadding = 4;

		// Token: 0x04000DAD RID: 3501
		private const int SeparatorHorizontalPadding = 3;

		// Token: 0x04000DAE RID: 3502
		private const int TextBoxLineCenterMargin = 5;

		// Token: 0x04000DAF RID: 3503
		private const int TextBoxLineInnerPadding = 1;

		// Token: 0x04000DB0 RID: 3504
		private const int EditorLineSwatchPadding = 1;

		// Token: 0x04000DB1 RID: 3505
		private const int EditorLineButtonPadding = 1;

		// Token: 0x04000DB2 RID: 3506
		private const int PanelHeaderVerticalPadding = 3;

		// Token: 0x04000DB3 RID: 3507
		private const int PanelHeaderHorizontalPadding = 5;

		// Token: 0x04000DB4 RID: 3508
		private const int TextBoxHeightFixup = 2;

		// Token: 0x04000DB5 RID: 3509
		private static readonly object EventFormActivated = new object();

		// Token: 0x04000DB6 RID: 3510
		private static readonly object EventFormDeactivate = new object();

		// Token: 0x04000DB7 RID: 3511
		private CommandID[] _filteredCommandIDs;

		// Token: 0x04000DB8 RID: 3512
		private ToolTip _toolTip;

		// Token: 0x04000DB9 RID: 3513
		private List<DesignerActionPanel.Line> _lines;

		// Token: 0x04000DBA RID: 3514
		private List<int> _lineYPositions;

		// Token: 0x04000DBB RID: 3515
		private List<int> _lineHeights;

		// Token: 0x04000DBC RID: 3516
		private Color _gradientLightColor = SystemColors.Control;

		// Token: 0x04000DBD RID: 3517
		private Color _gradientDarkColor = SystemColors.Control;

		// Token: 0x04000DBE RID: 3518
		private Color _titleBarColor = SystemColors.ActiveCaption;

		// Token: 0x04000DBF RID: 3519
		private Color _titleBarUnselectedColor = SystemColors.InactiveCaption;

		// Token: 0x04000DC0 RID: 3520
		private Color _titleBarTextColor = SystemColors.ActiveCaptionText;

		// Token: 0x04000DC1 RID: 3521
		private Color _separatorColor = SystemColors.ControlDark;

		// Token: 0x04000DC2 RID: 3522
		private Color _borderColor = SystemColors.ActiveBorder;

		// Token: 0x04000DC3 RID: 3523
		private Color _linkColor = SystemColors.HotTrack;

		// Token: 0x04000DC4 RID: 3524
		private Color _activeLinkColor = SystemColors.HotTrack;

		// Token: 0x04000DC5 RID: 3525
		private IServiceProvider _serviceProvider;

		// Token: 0x04000DC6 RID: 3526
		private bool _inMethodInvoke;

		// Token: 0x04000DC7 RID: 3527
		private bool _updatingTasks;

		// Token: 0x04000DC8 RID: 3528
		private bool _dropDownActive;

		// Token: 0x02000108 RID: 264
		private class LineInfo
		{
			// Token: 0x06000AE7 RID: 2791 RVA: 0x0002AA9A File Offset: 0x00029A9A
			public LineInfo(DesignerActionList list, DesignerActionItem item, DesignerActionPanel.Line line)
			{
				this.Line = line;
				this.Item = item;
				this.List = list;
			}

			// Token: 0x04000DC9 RID: 3529
			public DesignerActionPanel.Line Line;

			// Token: 0x04000DCA RID: 3530
			public DesignerActionItem Item;

			// Token: 0x04000DCB RID: 3531
			public DesignerActionList List;
		}

		// Token: 0x02000109 RID: 265
		internal sealed class TypeDescriptorContext : ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x06000AE8 RID: 2792 RVA: 0x0002AAB7 File Offset: 0x00029AB7
			public TypeDescriptorContext(IServiceProvider serviceProvider, PropertyDescriptor propDesc, object instance)
			{
				this._serviceProvider = serviceProvider;
				this._propDesc = propDesc;
				this._instance = instance;
			}

			// Token: 0x1700018C RID: 396
			// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x0002AAD4 File Offset: 0x00029AD4
			private IComponentChangeService ComponentChangeService
			{
				get
				{
					return (IComponentChangeService)this._serviceProvider.GetService(typeof(IComponentChangeService));
				}
			}

			// Token: 0x1700018D RID: 397
			// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0002AAF0 File Offset: 0x00029AF0
			public IContainer Container
			{
				get
				{
					return (IContainer)this._serviceProvider.GetService(typeof(IContainer));
				}
			}

			// Token: 0x1700018E RID: 398
			// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0002AB0C File Offset: 0x00029B0C
			public object Instance
			{
				get
				{
					return this._instance;
				}
			}

			// Token: 0x1700018F RID: 399
			// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0002AB14 File Offset: 0x00029B14
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this._propDesc;
				}
			}

			// Token: 0x06000AED RID: 2797 RVA: 0x0002AB1C File Offset: 0x00029B1C
			public object GetService(Type serviceType)
			{
				return this._serviceProvider.GetService(serviceType);
			}

			// Token: 0x06000AEE RID: 2798 RVA: 0x0002AB2C File Offset: 0x00029B2C
			public bool OnComponentChanging()
			{
				if (this.ComponentChangeService != null)
				{
					try
					{
						this.ComponentChangeService.OnComponentChanging(this._instance, this._propDesc);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return false;
						}
						throw ex;
					}
					return true;
				}
				return true;
			}

			// Token: 0x06000AEF RID: 2799 RVA: 0x0002AB7C File Offset: 0x00029B7C
			public void OnComponentChanged()
			{
				if (this.ComponentChangeService != null)
				{
					this.ComponentChangeService.OnComponentChanged(this._instance, this._propDesc, null, null);
				}
			}

			// Token: 0x04000DCC RID: 3532
			private IServiceProvider _serviceProvider;

			// Token: 0x04000DCD RID: 3533
			private PropertyDescriptor _propDesc;

			// Token: 0x04000DCE RID: 3534
			private object _instance;
		}

		// Token: 0x0200010A RID: 266
		private abstract class Line
		{
			// Token: 0x06000AF0 RID: 2800 RVA: 0x0002AB9F File Offset: 0x00029B9F
			public Line(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
			{
				if (actionPanel == null)
				{
					throw new ArgumentNullException("actionPanel");
				}
				this._serviceProvider = serviceProvider;
				this._actionPanel = actionPanel;
			}

			// Token: 0x17000190 RID: 400
			// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0002ABC3 File Offset: 0x00029BC3
			protected DesignerActionPanel ActionPanel
			{
				get
				{
					return this._actionPanel;
				}
			}

			// Token: 0x17000191 RID: 401
			// (get) Token: 0x06000AF2 RID: 2802
			public abstract string FocusId { get; }

			// Token: 0x17000192 RID: 402
			// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0002ABCB File Offset: 0x00029BCB
			protected IServiceProvider ServiceProvider
			{
				get
				{
					return this._serviceProvider;
				}
			}

			// Token: 0x06000AF4 RID: 2804
			protected abstract void AddControls(List<Control> controls);

			// Token: 0x06000AF5 RID: 2805 RVA: 0x0002ABD4 File Offset: 0x00029BD4
			internal List<Control> GetControls()
			{
				this._addedControls = new List<Control>();
				this.AddControls(this._addedControls);
				foreach (Control control in this._addedControls)
				{
					control.Tag = this;
				}
				return this._addedControls;
			}

			// Token: 0x06000AF6 RID: 2806
			public abstract void Focus();

			// Token: 0x06000AF7 RID: 2807
			public abstract Size LayoutControls(int top, int width, bool measureOnly);

			// Token: 0x06000AF8 RID: 2808 RVA: 0x0002AC44 File Offset: 0x00029C44
			public virtual void PaintLine(Graphics g, int lineWidth, int lineHeight)
			{
			}

			// Token: 0x06000AF9 RID: 2809 RVA: 0x0002AC46 File Offset: 0x00029C46
			protected internal virtual bool ProcessDialogKey(Keys keyData)
			{
				return false;
			}

			// Token: 0x06000AFA RID: 2810 RVA: 0x0002AC4C File Offset: 0x00029C4C
			internal void RemoveControls(Control.ControlCollection controls)
			{
				for (int i = 0; i < this._addedControls.Count; i++)
				{
					Control control = this._addedControls[i];
					control.Tag = null;
					controls.Remove(control);
				}
			}

			// Token: 0x06000AFB RID: 2811
			internal abstract void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex);

			// Token: 0x04000DCF RID: 3535
			private DesignerActionPanel _actionPanel;

			// Token: 0x04000DD0 RID: 3536
			private List<Control> _addedControls;

			// Token: 0x04000DD1 RID: 3537
			private IServiceProvider _serviceProvider;
		}

		// Token: 0x0200010B RID: 267
		private sealed class DesignerActionPanelHeaderItem : DesignerActionItem
		{
			// Token: 0x06000AFC RID: 2812 RVA: 0x0002AC8A File Offset: 0x00029C8A
			public DesignerActionPanelHeaderItem(string title, string subtitle)
				: base(title, null, null)
			{
				this._subtitle = subtitle;
			}

			// Token: 0x17000193 RID: 403
			// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0002AC9C File Offset: 0x00029C9C
			public string Subtitle
			{
				get
				{
					return this._subtitle;
				}
			}

			// Token: 0x04000DD2 RID: 3538
			private string _subtitle;
		}

		// Token: 0x0200010C RID: 268
		private sealed class PanelHeaderLine : DesignerActionPanel.Line
		{
			// Token: 0x06000AFE RID: 2814 RVA: 0x0002ACA4 File Offset: 0x00029CA4
			public PanelHeaderLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
				actionPanel.FontChanged += this.OnParentControlFontChanged;
			}

			// Token: 0x17000194 RID: 404
			// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0002ACC0 File Offset: 0x00029CC0
			public sealed override string FocusId
			{
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x06000B00 RID: 2816 RVA: 0x0002ACC8 File Offset: 0x00029CC8
			protected override void AddControls(List<Control> controls)
			{
				this._titleLabel = new Label();
				this._titleLabel.BackColor = Color.Transparent;
				this._titleLabel.ForeColor = base.ActionPanel.TitleBarTextColor;
				this._titleLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
				this._titleLabel.UseMnemonic = false;
				this._subtitleLabel = new Label();
				this._subtitleLabel.BackColor = Color.Transparent;
				this._subtitleLabel.ForeColor = base.ActionPanel.TitleBarTextColor;
				this._subtitleLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
				this._subtitleLabel.UseMnemonic = false;
				controls.Add(this._titleLabel);
				controls.Add(this._subtitleLabel);
				base.ActionPanel.FormActivated += this.OnFormActivated;
				base.ActionPanel.FormDeactivate += this.OnFormDeactivate;
			}

			// Token: 0x06000B01 RID: 2817 RVA: 0x0002ADAF File Offset: 0x00029DAF
			public sealed override void Focus()
			{
			}

			// Token: 0x06000B02 RID: 2818 RVA: 0x0002ADB4 File Offset: 0x00029DB4
			public override Size LayoutControls(int top, int width, bool measureOnly)
			{
				Size preferredSize = this._titleLabel.GetPreferredSize(new Size(int.MaxValue, int.MaxValue));
				Size size = Size.Empty;
				if (!string.IsNullOrEmpty(this._panelHeaderItem.Subtitle))
				{
					size = this._subtitleLabel.GetPreferredSize(new Size(int.MaxValue, int.MaxValue));
				}
				if (!measureOnly)
				{
					this._titleLabel.Location = new Point(5, top + 3);
					this._titleLabel.Size = preferredSize;
					this._subtitleLabel.Location = new Point(5, top + 6 + preferredSize.Height);
					this._subtitleLabel.Size = size;
				}
				int num = Math.Max(preferredSize.Width, size.Width) + 10;
				int num2 = (size.IsEmpty ? (preferredSize.Height + 6) : (preferredSize.Height + size.Height + 9));
				return new Size(num + 2, num2 + 1);
			}

			// Token: 0x06000B03 RID: 2819 RVA: 0x0002AEA3 File Offset: 0x00029EA3
			private void OnFormActivated(object sender, EventArgs e)
			{
				this._formActive = true;
				base.ActionPanel.Invalidate();
			}

			// Token: 0x06000B04 RID: 2820 RVA: 0x0002AEB7 File Offset: 0x00029EB7
			private void OnFormDeactivate(object sender, EventArgs e)
			{
				this._formActive = false;
				base.ActionPanel.Invalidate();
			}

			// Token: 0x06000B05 RID: 2821 RVA: 0x0002AECC File Offset: 0x00029ECC
			private void OnParentControlFontChanged(object sender, EventArgs e)
			{
				if (this._titleLabel != null && this._subtitleLabel != null)
				{
					this._titleLabel.Font = new Font(base.ActionPanel.Font, FontStyle.Bold);
					this._subtitleLabel.Font = base.ActionPanel.Font;
				}
			}

			// Token: 0x06000B06 RID: 2822 RVA: 0x0002AF1C File Offset: 0x00029F1C
			public override void PaintLine(Graphics g, int lineWidth, int lineHeight)
			{
				Color color = ((this._formActive || base.ActionPanel.DropDownActive) ? base.ActionPanel.TitleBarColor : base.ActionPanel.TitleBarUnselectedColor);
				using (SolidBrush solidBrush = new SolidBrush(color))
				{
					g.FillRectangle(solidBrush, 1, 1, lineWidth - 2, lineHeight - 1);
				}
				using (Pen pen = new Pen(base.ActionPanel.BorderColor))
				{
					g.DrawLine(pen, 0, lineHeight - 1, lineWidth, lineHeight - 1);
				}
			}

			// Token: 0x06000B07 RID: 2823 RVA: 0x0002AFC4 File Offset: 0x00029FC4
			internal override void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex)
			{
				this._actionList = actionList;
				this._panelHeaderItem = (DesignerActionPanel.DesignerActionPanelHeaderItem)actionItem;
				this._titleLabel.Text = this._panelHeaderItem.DisplayName;
				this._titleLabel.TabIndex = currentTabIndex++;
				this._subtitleLabel.Text = this._panelHeaderItem.Subtitle;
				this._subtitleLabel.TabIndex = currentTabIndex++;
				this._subtitleLabel.Visible = this._subtitleLabel.Text.Length != 0;
				this.OnParentControlFontChanged(null, EventArgs.Empty);
			}

			// Token: 0x04000DD3 RID: 3539
			private DesignerActionList _actionList;

			// Token: 0x04000DD4 RID: 3540
			private DesignerActionPanel.DesignerActionPanelHeaderItem _panelHeaderItem;

			// Token: 0x04000DD5 RID: 3541
			private Label _titleLabel;

			// Token: 0x04000DD6 RID: 3542
			private Label _subtitleLabel;

			// Token: 0x04000DD7 RID: 3543
			private bool _formActive;
		}

		// Token: 0x0200010D RID: 269
		private sealed class MethodLine : DesignerActionPanel.Line
		{
			// Token: 0x06000B08 RID: 2824 RVA: 0x0002B067 File Offset: 0x0002A067
			public MethodLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			// Token: 0x17000195 RID: 405
			// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0002B071 File Offset: 0x0002A071
			public sealed override string FocusId
			{
				get
				{
					return "METHOD:" + this._actionList.GetType().FullName + "." + this._methodItem.MemberName;
				}
			}

			// Token: 0x06000B0A RID: 2826 RVA: 0x0002B0A0 File Offset: 0x0002A0A0
			protected override void AddControls(List<Control> controls)
			{
				this._linkLabel = new DesignerActionPanel.MethodLine.MethodItemLinkLabel();
				this._linkLabel.ActiveLinkColor = base.ActionPanel.ActiveLinkColor;
				this._linkLabel.AutoSize = false;
				this._linkLabel.BackColor = Color.Transparent;
				this._linkLabel.LinkBehavior = LinkBehavior.HoverUnderline;
				this._linkLabel.LinkColor = base.ActionPanel.LinkColor;
				this._linkLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
				this._linkLabel.UseMnemonic = false;
				this._linkLabel.VisitedLinkColor = base.ActionPanel.LinkColor;
				this._linkLabel.LinkClicked += this.OnLinkLabelLinkClicked;
				controls.Add(this._linkLabel);
			}

			// Token: 0x06000B0B RID: 2827 RVA: 0x0002B15E File Offset: 0x0002A15E
			public sealed override void Focus()
			{
				this._linkLabel.Focus();
			}

			// Token: 0x06000B0C RID: 2828 RVA: 0x0002B16C File Offset: 0x0002A16C
			public override Size LayoutControls(int top, int width, bool measureOnly)
			{
				Size preferredSize = this._linkLabel.GetPreferredSize(new Size(int.MaxValue, int.MaxValue));
				if (!measureOnly)
				{
					this._linkLabel.Location = new Point(5, top + 3);
					this._linkLabel.Size = preferredSize;
				}
				return preferredSize + new Size(9, 7);
			}

			// Token: 0x06000B0D RID: 2829 RVA: 0x0002B1C8 File Offset: 0x0002A1C8
			private void OnLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.ActionPanel.InMethodInvoke = true;
				try
				{
					this._methodItem.Invoke();
				}
				catch (Exception innerException)
				{
					if (innerException is TargetInvocationException)
					{
						innerException = innerException.InnerException;
					}
					base.ActionPanel.ShowError(SR.GetString("DesignerActionPanel_ErrorInvokingAction", new object[]
					{
						this._methodItem.DisplayName,
						Environment.NewLine + innerException.Message
					}));
				}
				finally
				{
					base.ActionPanel.InMethodInvoke = false;
				}
			}

			// Token: 0x06000B0E RID: 2830 RVA: 0x0002B26C File Offset: 0x0002A26C
			internal override void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex)
			{
				this._actionList = actionList;
				this._methodItem = (DesignerActionMethodItem)actionItem;
				toolTip.SetToolTip(this._linkLabel, this._methodItem.Description);
				this._linkLabel.Text = DesignerActionPanel.StripAmpersands(this._methodItem.DisplayName);
				this._linkLabel.AccessibleDescription = actionItem.Description;
				this._linkLabel.TabIndex = currentTabIndex++;
			}

			// Token: 0x04000DD8 RID: 3544
			private DesignerActionList _actionList;

			// Token: 0x04000DD9 RID: 3545
			private DesignerActionMethodItem _methodItem;

			// Token: 0x04000DDA RID: 3546
			private DesignerActionPanel.MethodLine.MethodItemLinkLabel _linkLabel;

			// Token: 0x0200010E RID: 270
			private sealed class MethodItemLinkLabel : LinkLabel
			{
				// Token: 0x06000B0F RID: 2831 RVA: 0x0002B2E4 File Offset: 0x0002A2E4
				protected override bool ProcessDialogKey(Keys keyData)
				{
					if ((keyData & Keys.Control) == Keys.Control)
					{
						Keys keys = keyData & Keys.KeyCode;
						Keys keys2 = keys;
						if (keys2 == Keys.Tab)
						{
							return false;
						}
					}
					return base.ProcessDialogKey(keyData);
				}
			}
		}

		// Token: 0x0200010F RID: 271
		private abstract class PropertyLine : DesignerActionPanel.Line
		{
			// Token: 0x06000B11 RID: 2833 RVA: 0x0002B31F File Offset: 0x0002A31F
			public PropertyLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			// Token: 0x17000196 RID: 406
			// (get) Token: 0x06000B12 RID: 2834 RVA: 0x0002B329 File Offset: 0x0002A329
			public sealed override string FocusId
			{
				get
				{
					return "PROPERTY:" + this._actionList.GetType().FullName + "." + this._propertyItem.MemberName;
				}
			}

			// Token: 0x17000197 RID: 407
			// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0002B355 File Offset: 0x0002A355
			protected PropertyDescriptor PropertyDescriptor
			{
				get
				{
					if (this._propDesc == null)
					{
						this._propDesc = TypeDescriptor.GetProperties(this._actionList)[this._propertyItem.MemberName];
					}
					return this._propDesc;
				}
			}

			// Token: 0x17000198 RID: 408
			// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0002B386 File Offset: 0x0002A386
			protected DesignerActionPropertyItem PropertyItem
			{
				get
				{
					return this._propertyItem;
				}
			}

			// Token: 0x17000199 RID: 409
			// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0002B38E File Offset: 0x0002A38E
			protected ITypeDescriptorContext TypeDescriptorContext
			{
				get
				{
					if (this._typeDescriptorContext == null)
					{
						this._typeDescriptorContext = new DesignerActionPanel.TypeDescriptorContext(base.ServiceProvider, this.PropertyDescriptor, this._actionList);
					}
					return this._typeDescriptorContext;
				}
			}

			// Token: 0x1700019A RID: 410
			// (get) Token: 0x06000B16 RID: 2838 RVA: 0x0002B3BB File Offset: 0x0002A3BB
			protected object Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x06000B17 RID: 2839
			protected abstract void OnPropertyTaskItemUpdated(ToolTip toolTip, ref int currentTabIndex);

			// Token: 0x06000B18 RID: 2840
			protected abstract void OnValueChanged();

			// Token: 0x06000B19 RID: 2841 RVA: 0x0002B3C4 File Offset: 0x0002A3C4
			protected void SetValue(object newValue)
			{
				if (this._pushingValue || base.ActionPanel.DropDownActive)
				{
					return;
				}
				this._pushingValue = true;
				try
				{
					if (newValue != null)
					{
						Type type = newValue.GetType();
						if (!this.PropertyDescriptor.PropertyType.IsAssignableFrom(type) && this.PropertyDescriptor.Converter != null)
						{
							if (!this.PropertyDescriptor.Converter.CanConvertFrom(this._typeDescriptorContext, type))
							{
								base.ActionPanel.ShowError(SR.GetString("DesignerActionPanel_CouldNotConvertValue", new object[]
								{
									newValue,
									this._propDesc.PropertyType
								}));
								return;
							}
							newValue = this.PropertyDescriptor.Converter.ConvertFrom(this._typeDescriptorContext, CultureInfo.CurrentCulture, newValue);
						}
					}
					if (!object.Equals(this._value, newValue))
					{
						this.PropertyDescriptor.SetValue(this._actionList, newValue);
						this._value = this.PropertyDescriptor.GetValue(this._actionList);
						this.OnValueChanged();
					}
				}
				catch (Exception innerException)
				{
					if (innerException is TargetInvocationException)
					{
						innerException = innerException.InnerException;
					}
					base.ActionPanel.ShowError(SR.GetString("DesignerActionPanel_ErrorSettingValue", new object[]
					{
						newValue,
						this.PropertyDescriptor.Name,
						innerException.Message
					}));
				}
				catch
				{
				}
				finally
				{
					this._pushingValue = false;
				}
			}

			// Token: 0x06000B1A RID: 2842 RVA: 0x0002B568 File Offset: 0x0002A568
			internal sealed override void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex)
			{
				this._actionList = actionList;
				this._propertyItem = (DesignerActionPropertyItem)actionItem;
				this._propDesc = null;
				this._typeDescriptorContext = null;
				this._value = this.PropertyDescriptor.GetValue(actionList);
				this.OnPropertyTaskItemUpdated(toolTip, ref currentTabIndex);
				this._pushingValue = true;
				try
				{
					this.OnValueChanged();
				}
				finally
				{
					this._pushingValue = false;
				}
			}

			// Token: 0x04000DDB RID: 3547
			private DesignerActionList _actionList;

			// Token: 0x04000DDC RID: 3548
			private DesignerActionPropertyItem _propertyItem;

			// Token: 0x04000DDD RID: 3549
			private object _value;

			// Token: 0x04000DDE RID: 3550
			private bool _pushingValue;

			// Token: 0x04000DDF RID: 3551
			private PropertyDescriptor _propDesc;

			// Token: 0x04000DE0 RID: 3552
			private ITypeDescriptorContext _typeDescriptorContext;
		}

		// Token: 0x02000110 RID: 272
		private sealed class CheckBoxPropertyLine : DesignerActionPanel.PropertyLine
		{
			// Token: 0x06000B1B RID: 2843 RVA: 0x0002B5D8 File Offset: 0x0002A5D8
			public CheckBoxPropertyLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			// Token: 0x06000B1C RID: 2844 RVA: 0x0002B5E4 File Offset: 0x0002A5E4
			protected override void AddControls(List<Control> controls)
			{
				this._checkBox = new CheckBox();
				this._checkBox.BackColor = Color.Transparent;
				this._checkBox.CheckAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
				this._checkBox.CheckedChanged += this.OnCheckBoxCheckedChanged;
				this._checkBox.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
				this._checkBox.UseMnemonic = false;
				controls.Add(this._checkBox);
			}

			// Token: 0x06000B1D RID: 2845 RVA: 0x0002B655 File Offset: 0x0002A655
			public sealed override void Focus()
			{
				this._checkBox.Focus();
			}

			// Token: 0x06000B1E RID: 2846 RVA: 0x0002B664 File Offset: 0x0002A664
			public override Size LayoutControls(int top, int width, bool measureOnly)
			{
				Size preferredSize = this._checkBox.GetPreferredSize(new Size(int.MaxValue, int.MaxValue));
				if (!measureOnly)
				{
					this._checkBox.Location = new Point(5, top + 3);
					this._checkBox.Size = preferredSize;
				}
				return preferredSize + new Size(9, 7);
			}

			// Token: 0x06000B1F RID: 2847 RVA: 0x0002B6BD File Offset: 0x0002A6BD
			private void OnCheckBoxCheckedChanged(object sender, EventArgs e)
			{
				base.SetValue(this._checkBox.Checked);
			}

			// Token: 0x06000B20 RID: 2848 RVA: 0x0002B6D8 File Offset: 0x0002A6D8
			protected override void OnPropertyTaskItemUpdated(ToolTip toolTip, ref int currentTabIndex)
			{
				this._checkBox.Text = DesignerActionPanel.StripAmpersands(base.PropertyItem.DisplayName);
				this._checkBox.AccessibleDescription = base.PropertyItem.Description;
				this._checkBox.TabIndex = currentTabIndex++;
				toolTip.SetToolTip(this._checkBox, base.PropertyItem.Description);
			}

			// Token: 0x06000B21 RID: 2849 RVA: 0x0002B741 File Offset: 0x0002A741
			protected override void OnValueChanged()
			{
				this._checkBox.Checked = (bool)base.Value;
			}

			// Token: 0x04000DE1 RID: 3553
			private CheckBox _checkBox;
		}

		// Token: 0x02000111 RID: 273
		private class TextBoxPropertyLine : DesignerActionPanel.PropertyLine
		{
			// Token: 0x06000B22 RID: 2850 RVA: 0x0002B759 File Offset: 0x0002A759
			public TextBoxPropertyLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			// Token: 0x1700019B RID: 411
			// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0002B763 File Offset: 0x0002A763
			protected Control EditControl
			{
				get
				{
					return this._editControl;
				}
			}

			// Token: 0x1700019C RID: 412
			// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0002B76B File Offset: 0x0002A76B
			protected Point EditRegionLocation
			{
				get
				{
					return this._editRegionLocation;
				}
			}

			// Token: 0x1700019D RID: 413
			// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0002B773 File Offset: 0x0002A773
			protected Point EditRegionRelativeLocation
			{
				get
				{
					return this._editRegionRelativeLocation;
				}
			}

			// Token: 0x1700019E RID: 414
			// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0002B77B File Offset: 0x0002A77B
			protected Size EditRegionSize
			{
				get
				{
					return this._editRegionSize;
				}
			}

			// Token: 0x06000B27 RID: 2855 RVA: 0x0002B784 File Offset: 0x0002A784
			protected override void AddControls(List<Control> controls)
			{
				this._label = new Label();
				this._label.BackColor = Color.Transparent;
				this._label.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
				this._label.UseMnemonic = false;
				this._readOnlyTextBoxLabel = new DesignerActionPanel.TextBoxPropertyLine.EditorLabel();
				this._readOnlyTextBoxLabel.BackColor = Color.Transparent;
				this._readOnlyTextBoxLabel.TabStop = true;
				this._readOnlyTextBoxLabel.TextAlign = global::System.Drawing.ContentAlignment.TopLeft;
				this._readOnlyTextBoxLabel.UseMnemonic = false;
				this._readOnlyTextBoxLabel.Visible = false;
				this._readOnlyTextBoxLabel.MouseClick += this.OnReadOnlyTextBoxLabelClick;
				this._readOnlyTextBoxLabel.Enter += this.OnReadOnlyTextBoxLabelEnter;
				this._readOnlyTextBoxLabel.Leave += this.OnReadOnlyTextBoxLabelLeave;
				this._readOnlyTextBoxLabel.KeyDown += this.OnReadOnlyTextBoxLabelKeyDown;
				this._textBox = new TextBox();
				this._textBox.BorderStyle = BorderStyle.None;
				this._textBox.TextAlign = HorizontalAlignment.Left;
				this._textBox.Visible = false;
				this._textBox.TextChanged += this.OnTextBoxTextChanged;
				this._textBox.KeyDown += this.OnTextBoxKeyDown;
				this._textBox.LostFocus += this.OnTextBoxLostFocus;
				controls.Add(this._readOnlyTextBoxLabel);
				controls.Add(this._textBox);
				controls.Add(this._label);
			}

			// Token: 0x06000B28 RID: 2856 RVA: 0x0002B905 File Offset: 0x0002A905
			public sealed override void Focus()
			{
				this._editControl.Focus();
			}

			// Token: 0x06000B29 RID: 2857 RVA: 0x0002B914 File Offset: 0x0002A914
			internal int GetEditRegionXPos()
			{
				if (string.IsNullOrEmpty(this._label.Text))
				{
					return 5;
				}
				return 5 + this._label.GetPreferredSize(new Size(int.MaxValue, int.MaxValue)).Width + 5;
			}

			// Token: 0x06000B2A RID: 2858 RVA: 0x0002B95B File Offset: 0x0002A95B
			protected virtual int GetTextBoxLeftPadding(int textBoxHeight)
			{
				return 1;
			}

			// Token: 0x06000B2B RID: 2859 RVA: 0x0002B95E File Offset: 0x0002A95E
			protected virtual int GetTextBoxRightPadding(int textBoxHeight)
			{
				return 1;
			}

			// Token: 0x06000B2C RID: 2860 RVA: 0x0002B964 File Offset: 0x0002A964
			public override Size LayoutControls(int top, int width, bool measureOnly)
			{
				int num = this._textBox.GetPreferredSize(new Size(int.MaxValue, int.MaxValue)).Height;
				num += 2;
				int num2 = num + 7 + 2 + 2;
				int num3 = Math.Max(this._editXPos, this.GetEditRegionXPos());
				int num4 = num3 + 150 + 4;
				width = Math.Max(width, num4);
				int num5 = width - num4;
				if (!measureOnly)
				{
					this._editRegionLocation = new Point(num3, top + 4);
					this._editRegionRelativeLocation = new Point(num3, 4);
					this._editRegionSize = new Size(150 + num5, num + 2);
					this._label.Location = new Point(5, top);
					int width2 = this._label.GetPreferredSize(new Size(int.MaxValue, int.MaxValue)).Width;
					this._label.Size = new Size(width2, num2);
					int num6 = 0;
					if (this._editControl is TextBox)
					{
						num6 = 2;
					}
					this._editControl.Location = new Point(this._editRegionLocation.X + this.GetTextBoxLeftPadding(num) + 1 + num6, this._editRegionLocation.Y + 1 + 1);
					this._editControl.Width = this._editRegionSize.Width - this.GetTextBoxRightPadding(num) - this.GetTextBoxLeftPadding(num) - num6;
					this._editControl.Height = this._editRegionSize.Height - 2 - 1;
				}
				return new Size(width, num2);
			}

			// Token: 0x06000B2D RID: 2861 RVA: 0x0002BAE1 File Offset: 0x0002AAE1
			protected virtual bool IsReadOnly()
			{
				return DesignerActionPanel.IsReadOnlyProperty(base.PropertyDescriptor);
			}

			// Token: 0x06000B2E RID: 2862 RVA: 0x0002BAF0 File Offset: 0x0002AAF0
			protected override void OnPropertyTaskItemUpdated(ToolTip toolTip, ref int currentTabIndex)
			{
				this._label.Text = DesignerActionPanel.StripAmpersands(base.PropertyItem.DisplayName);
				this._label.TabIndex = currentTabIndex++;
				toolTip.SetToolTip(this._label, base.PropertyItem.Description);
				this._textBoxDirty = false;
				if (this.IsReadOnly())
				{
					this._readOnlyTextBoxLabel.Visible = true;
					this._textBox.Visible = false;
					this._textBox.Location = new Point(int.MaxValue, int.MaxValue);
					this._editControl = this._readOnlyTextBoxLabel;
				}
				else
				{
					this._readOnlyTextBoxLabel.Visible = false;
					this._readOnlyTextBoxLabel.Location = new Point(int.MaxValue, int.MaxValue);
					this._textBox.Visible = true;
					this._editControl = this._textBox;
				}
				this._editControl.AccessibleDescription = base.PropertyItem.Description;
				this._editControl.AccessibleName = DesignerActionPanel.StripAmpersands(base.PropertyItem.DisplayName);
				this._editControl.TabIndex = currentTabIndex++;
				this._editControl.BringToFront();
			}

			// Token: 0x06000B2F RID: 2863 RVA: 0x0002BC20 File Offset: 0x0002AC20
			protected virtual void OnReadOnlyTextBoxLabelClick(object sender, MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.Focus();
				}
			}

			// Token: 0x06000B30 RID: 2864 RVA: 0x0002BC35 File Offset: 0x0002AC35
			private void OnReadOnlyTextBoxLabelEnter(object sender, EventArgs e)
			{
				this._readOnlyTextBoxLabel.ForeColor = SystemColors.HighlightText;
				this._readOnlyTextBoxLabel.BackColor = SystemColors.Highlight;
			}

			// Token: 0x06000B31 RID: 2865 RVA: 0x0002BC57 File Offset: 0x0002AC57
			private void OnReadOnlyTextBoxLabelLeave(object sender, EventArgs e)
			{
				this._readOnlyTextBoxLabel.ForeColor = SystemColors.WindowText;
				this._readOnlyTextBoxLabel.BackColor = SystemColors.Window;
			}

			// Token: 0x06000B32 RID: 2866 RVA: 0x0002BC7C File Offset: 0x0002AC7C
			protected TypeConverter.StandardValuesCollection GetStandardValues()
			{
				TypeConverter converter = base.PropertyDescriptor.Converter;
				if (converter != null && converter.GetStandardValuesSupported(base.TypeDescriptorContext))
				{
					return converter.GetStandardValues(base.TypeDescriptorContext);
				}
				return null;
			}

			// Token: 0x06000B33 RID: 2867 RVA: 0x0002BCB4 File Offset: 0x0002ACB4
			private void OnEditControlKeyDown(KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Down)
				{
					e.Handled = true;
					TypeConverter.StandardValuesCollection standardValues = this.GetStandardValues();
					if (standardValues != null)
					{
						for (int i = 0; i < standardValues.Count; i++)
						{
							if (object.Equals(base.Value, standardValues[i]))
							{
								if (i < standardValues.Count - 1)
								{
									base.SetValue(standardValues[i + 1]);
								}
								return;
							}
						}
						if (standardValues.Count > 0)
						{
							base.SetValue(standardValues[0]);
						}
					}
					return;
				}
				if (e.KeyCode == Keys.Up)
				{
					e.Handled = true;
					TypeConverter.StandardValuesCollection standardValues2 = this.GetStandardValues();
					if (standardValues2 != null)
					{
						for (int j = 0; j < standardValues2.Count; j++)
						{
							if (object.Equals(base.Value, standardValues2[j]))
							{
								if (j > 0)
								{
									base.SetValue(standardValues2[j - 1]);
								}
								return;
							}
						}
						if (standardValues2.Count > 0)
						{
							base.SetValue(standardValues2[standardValues2.Count - 1]);
						}
					}
				}
			}

			// Token: 0x06000B34 RID: 2868 RVA: 0x0002BDA4 File Offset: 0x0002ADA4
			private void OnReadOnlyTextBoxLabelKeyDown(object sender, KeyEventArgs e)
			{
				this.OnEditControlKeyDown(e);
			}

			// Token: 0x06000B35 RID: 2869 RVA: 0x0002BDAD File Offset: 0x0002ADAD
			private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
			{
				if (base.ActionPanel.DropDownActive)
				{
					return;
				}
				if (e.KeyCode == Keys.Return)
				{
					this.UpdateValue();
					e.Handled = true;
					return;
				}
				this.OnEditControlKeyDown(e);
			}

			// Token: 0x06000B36 RID: 2870 RVA: 0x0002BDDC File Offset: 0x0002ADDC
			private void OnTextBoxLostFocus(object sender, EventArgs e)
			{
				if (base.ActionPanel.DropDownActive)
				{
					return;
				}
				this.UpdateValue();
			}

			// Token: 0x06000B37 RID: 2871 RVA: 0x0002BDF2 File Offset: 0x0002ADF2
			private void OnTextBoxTextChanged(object sender, EventArgs e)
			{
				this._textBoxDirty = true;
			}

			// Token: 0x06000B38 RID: 2872 RVA: 0x0002BDFB File Offset: 0x0002ADFB
			protected override void OnValueChanged()
			{
				this._editControl.Text = base.PropertyDescriptor.Converter.ConvertToString(base.TypeDescriptorContext, base.Value);
			}

			// Token: 0x06000B39 RID: 2873 RVA: 0x0002BE24 File Offset: 0x0002AE24
			public override void PaintLine(Graphics g, int lineWidth, int lineHeight)
			{
				Rectangle rectangle = new Rectangle(this.EditRegionRelativeLocation, this.EditRegionSize);
				g.FillRectangle(SystemBrushes.Window, rectangle);
				g.DrawRectangle(SystemPens.ControlDark, rectangle);
			}

			// Token: 0x06000B3A RID: 2874 RVA: 0x0002BE5C File Offset: 0x0002AE5C
			internal void SetEditRegionXPos(int xPos)
			{
				if (!string.IsNullOrEmpty(this._label.Text))
				{
					this._editXPos = xPos;
					return;
				}
				this._editXPos = 5;
			}

			// Token: 0x06000B3B RID: 2875 RVA: 0x0002BE7F File Offset: 0x0002AE7F
			private void UpdateValue()
			{
				if (this._textBoxDirty)
				{
					base.SetValue(this._editControl.Text);
					this._textBoxDirty = false;
				}
			}

			// Token: 0x04000DE2 RID: 3554
			private TextBox _textBox;

			// Token: 0x04000DE3 RID: 3555
			private DesignerActionPanel.TextBoxPropertyLine.EditorLabel _readOnlyTextBoxLabel;

			// Token: 0x04000DE4 RID: 3556
			private Control _editControl;

			// Token: 0x04000DE5 RID: 3557
			private Label _label;

			// Token: 0x04000DE6 RID: 3558
			private int _editXPos;

			// Token: 0x04000DE7 RID: 3559
			private bool _textBoxDirty;

			// Token: 0x04000DE8 RID: 3560
			private Point _editRegionLocation;

			// Token: 0x04000DE9 RID: 3561
			private Point _editRegionRelativeLocation;

			// Token: 0x04000DEA RID: 3562
			private Size _editRegionSize;

			// Token: 0x02000112 RID: 274
			private sealed class EditorLabel : Label
			{
				// Token: 0x06000B3C RID: 2876 RVA: 0x0002BEA1 File Offset: 0x0002AEA1
				public EditorLabel()
				{
					base.SetStyle(ControlStyles.Selectable, true);
				}

				// Token: 0x06000B3D RID: 2877 RVA: 0x0002BEB5 File Offset: 0x0002AEB5
				protected override AccessibleObject CreateAccessibilityInstance()
				{
					return new DesignerActionPanel.TextBoxPropertyLine.EditorLabel.EditorLabelAccessibleObject(this);
				}

				// Token: 0x06000B3E RID: 2878 RVA: 0x0002BEBD File Offset: 0x0002AEBD
				protected override void OnGotFocus(EventArgs e)
				{
					base.OnGotFocus(e);
					base.AccessibilityNotifyClients(AccessibleEvents.Focus, 0, -1);
				}

				// Token: 0x06000B3F RID: 2879 RVA: 0x0002BED3 File Offset: 0x0002AED3
				protected override bool IsInputKey(Keys keyData)
				{
					return keyData == Keys.Down || keyData == Keys.Up || base.IsInputKey(keyData);
				}

				// Token: 0x02000113 RID: 275
				private sealed class EditorLabelAccessibleObject : Control.ControlAccessibleObject
				{
					// Token: 0x06000B40 RID: 2880 RVA: 0x0002BEE8 File Offset: 0x0002AEE8
					public EditorLabelAccessibleObject(DesignerActionPanel.TextBoxPropertyLine.EditorLabel owner)
						: base(owner)
					{
					}

					// Token: 0x1700019F RID: 415
					// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0002BEF1 File Offset: 0x0002AEF1
					public override string Value
					{
						get
						{
							return base.Owner.Text;
						}
					}
				}
			}
		}

		// Token: 0x02000114 RID: 276
		private sealed class EditorPropertyLine : DesignerActionPanel.TextBoxPropertyLine, IWindowsFormsEditorService, IServiceProvider
		{
			// Token: 0x06000B42 RID: 2882 RVA: 0x0002BEFE File Offset: 0x0002AEFE
			public EditorPropertyLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			// Token: 0x06000B43 RID: 2883 RVA: 0x0002BF08 File Offset: 0x0002AF08
			private void ActivateDropDown()
			{
				if (this._editor != null)
				{
					try
					{
						object obj = this._editor.EditValue(base.TypeDescriptorContext, this, base.Value);
						base.SetValue(obj);
						return;
					}
					catch (Exception ex)
					{
						base.ActionPanel.ShowError(SR.GetString("DesignerActionPanel_ErrorActivatingDropDown", new object[] { ex.Message }));
						return;
					}
				}
				ListBox listBox = new ListBox();
				listBox.BorderStyle = BorderStyle.None;
				listBox.IntegralHeight = false;
				listBox.Font = base.ActionPanel.Font;
				listBox.SelectedIndexChanged += this.OnListBoxSelectedIndexChanged;
				listBox.KeyDown += this.OnListBoxKeyDown;
				TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues();
				if (standardValues != null)
				{
					foreach (object obj2 in standardValues)
					{
						string text = base.PropertyDescriptor.Converter.ConvertToString(base.TypeDescriptorContext, CultureInfo.CurrentCulture, obj2);
						listBox.Items.Add(text);
						if (obj2 != null && obj2.Equals(base.Value))
						{
							listBox.SelectedItem = text;
						}
					}
				}
				int num = 0;
				IntPtr dc = DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.GetDC(new HandleRef(listBox, listBox.Handle));
				IntPtr intPtr = listBox.Font.ToHfont();
				DesignerActionPanel.EditorPropertyLine.NativeMethods.CommonHandles.GdiHandleCollector.Add();
				DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRIC textmetric = default(DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRIC);
				try
				{
					intPtr = DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.SelectObject(new HandleRef(listBox, dc), new HandleRef(listBox.Font, intPtr));
					if (listBox.Items.Count > 0)
					{
						DesignerActionPanel.EditorPropertyLine.NativeMethods.SIZE size = new DesignerActionPanel.EditorPropertyLine.NativeMethods.SIZE();
						foreach (object obj3 in listBox.Items)
						{
							string text2 = (string)obj3;
							DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.GetTextExtentPoint32(new HandleRef(listBox, dc), text2, text2.Length, size);
							num = Math.Max(size.cx, num);
						}
					}
					DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.GetTextMetrics(new HandleRef(listBox, dc), ref textmetric);
					num += 2 + textmetric.tmMaxCharWidth + SystemInformation.VerticalScrollBarWidth;
					intPtr = DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.SelectObject(new HandleRef(listBox, dc), new HandleRef(listBox.Font, intPtr));
				}
				finally
				{
					DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.DeleteObject(new HandleRef(listBox.Font, intPtr));
					DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.ReleaseDC(new HandleRef(listBox, listBox.Handle), new HandleRef(listBox, dc));
				}
				listBox.Height = Math.Max(textmetric.tmHeight + 2, Math.Min(200, listBox.PreferredHeight));
				listBox.Width = Math.Max(num, base.EditRegionSize.Width);
				this._ignoreDropDownValue = false;
				try
				{
					this.ShowDropDown(listBox, SystemColors.ControlDark);
				}
				finally
				{
					listBox.SelectedIndexChanged -= this.OnListBoxSelectedIndexChanged;
					listBox.KeyDown -= this.OnListBoxKeyDown;
				}
				if (!this._ignoreDropDownValue && listBox.SelectedItem != null)
				{
					base.SetValue(listBox.SelectedItem);
				}
			}

			// Token: 0x06000B44 RID: 2884 RVA: 0x0002C254 File Offset: 0x0002B254
			protected override void AddControls(List<Control> controls)
			{
				base.AddControls(controls);
				this._button = new DesignerActionPanel.EditorPropertyLine.EditorButton();
				this._button.Click += this.OnButtonClick;
				this._button.GotFocus += this.OnButtonGotFocus;
				controls.Add(this._button);
			}

			// Token: 0x06000B45 RID: 2885 RVA: 0x0002C2AD File Offset: 0x0002B2AD
			private void CloseDropDown()
			{
				if (this._dropDownHolder != null)
				{
					this._dropDownHolder.Visible = false;
				}
			}

			// Token: 0x06000B46 RID: 2886 RVA: 0x0002C2C3 File Offset: 0x0002B2C3
			protected override int GetTextBoxLeftPadding(int textBoxHeight)
			{
				if (this._hasSwatch)
				{
					return base.GetTextBoxLeftPadding(textBoxHeight) + textBoxHeight + 2;
				}
				return base.GetTextBoxLeftPadding(textBoxHeight);
			}

			// Token: 0x06000B47 RID: 2887 RVA: 0x0002C2E0 File Offset: 0x0002B2E0
			protected override int GetTextBoxRightPadding(int textBoxHeight)
			{
				return base.GetTextBoxRightPadding(textBoxHeight) + textBoxHeight + 2;
			}

			// Token: 0x06000B48 RID: 2888 RVA: 0x0002C2F0 File Offset: 0x0002B2F0
			protected override bool IsReadOnly()
			{
				if (base.IsReadOnly())
				{
					return true;
				}
				bool flag = !base.PropertyDescriptor.Converter.CanConvertFrom(base.TypeDescriptorContext, typeof(string));
				bool flag2 = base.PropertyDescriptor.Converter.GetStandardValuesSupported(base.TypeDescriptorContext) && base.PropertyDescriptor.Converter.GetStandardValuesExclusive(base.TypeDescriptorContext);
				return flag || flag2;
			}

			// Token: 0x06000B49 RID: 2889 RVA: 0x0002C364 File Offset: 0x0002B364
			public override Size LayoutControls(int top, int width, bool measureOnly)
			{
				Size size = base.LayoutControls(top, width, measureOnly);
				if (!measureOnly)
				{
					int num = base.EditRegionSize.Height - 2 - 1;
					this._button.Location = new Point(base.EditRegionLocation.X + base.EditRegionSize.Width - num - 1, base.EditRegionLocation.Y + 1 + 1);
					this._button.Size = new Size(num, num);
				}
				return size;
			}

			// Token: 0x06000B4A RID: 2890 RVA: 0x0002C3E9 File Offset: 0x0002B3E9
			private void OnButtonClick(object sender, EventArgs e)
			{
				this.ActivateDropDown();
			}

			// Token: 0x06000B4B RID: 2891 RVA: 0x0002C3F1 File Offset: 0x0002B3F1
			private void OnButtonGotFocus(object sender, EventArgs e)
			{
				if (!this._button.Ellipsis)
				{
					this.Focus();
				}
			}

			// Token: 0x06000B4C RID: 2892 RVA: 0x0002C406 File Offset: 0x0002B406
			private void OnListBoxKeyDown(object sender, KeyEventArgs e)
			{
				if (e.KeyData == Keys.Return)
				{
					this._ignoreNextSelectChange = false;
					this.CloseDropDown();
					e.Handled = true;
					return;
				}
				this._ignoreNextSelectChange = true;
			}

			// Token: 0x06000B4D RID: 2893 RVA: 0x0002C42E File Offset: 0x0002B42E
			private void OnListBoxSelectedIndexChanged(object sender, EventArgs e)
			{
				if (this._ignoreNextSelectChange)
				{
					this._ignoreNextSelectChange = false;
					return;
				}
				this.CloseDropDown();
			}

			// Token: 0x06000B4E RID: 2894 RVA: 0x0002C448 File Offset: 0x0002B448
			protected override void OnPropertyTaskItemUpdated(ToolTip toolTip, ref int currentTabIndex)
			{
				this._editor = (UITypeEditor)base.PropertyDescriptor.GetEditor(typeof(UITypeEditor));
				base.OnPropertyTaskItemUpdated(toolTip, ref currentTabIndex);
				if (this._editor != null)
				{
					this._button.Ellipsis = this._editor.GetEditStyle(base.TypeDescriptorContext) == UITypeEditorEditStyle.Modal;
					this._hasSwatch = this._editor.GetPaintValueSupported(base.TypeDescriptorContext);
				}
				else
				{
					this._button.Ellipsis = false;
				}
				if (this._button.Ellipsis)
				{
					base.EditControl.AccessibleRole = (this.IsReadOnly() ? AccessibleRole.StaticText : AccessibleRole.Text);
				}
				else
				{
					base.EditControl.AccessibleRole = (this.IsReadOnly() ? AccessibleRole.DropList : AccessibleRole.ComboBox);
				}
				this._button.TabStop = this._button.Ellipsis;
				this._button.TabIndex = currentTabIndex++;
				this._button.AccessibleRole = (this._button.Ellipsis ? AccessibleRole.PushButton : AccessibleRole.ButtonDropDown);
				this._button.AccessibleDescription = base.EditControl.AccessibleDescription;
				this._button.AccessibleName = base.EditControl.AccessibleName;
			}

			// Token: 0x06000B4F RID: 2895 RVA: 0x0002C580 File Offset: 0x0002B580
			protected override void OnReadOnlyTextBoxLabelClick(object sender, MouseEventArgs e)
			{
				base.OnReadOnlyTextBoxLabelClick(sender, e);
				if (e.Button == MouseButtons.Left)
				{
					if (this._editor != null)
					{
						this._editor.GetEditStyle(base.TypeDescriptorContext);
					}
					if (base.ActionPanel.DropDownActive)
					{
						this._ignoreDropDownValue = true;
						this.CloseDropDown();
						return;
					}
					this.ActivateDropDown();
				}
			}

			// Token: 0x06000B50 RID: 2896 RVA: 0x0002C5DD File Offset: 0x0002B5DD
			protected override void OnValueChanged()
			{
				base.OnValueChanged();
				this._swatch = null;
				if (this._hasSwatch)
				{
					base.ActionPanel.Invalidate(new Rectangle(base.EditRegionLocation, base.EditRegionSize), false);
				}
			}

			// Token: 0x06000B51 RID: 2897 RVA: 0x0002C614 File Offset: 0x0002B614
			public override void PaintLine(Graphics g, int lineWidth, int lineHeight)
			{
				base.PaintLine(g, lineWidth, lineHeight);
				if (this._hasSwatch)
				{
					if (this._swatch == null)
					{
						int num = base.EditRegionSize.Height - 2;
						int num2 = num - 1;
						this._swatch = new Bitmap(num, num2);
						Rectangle rectangle = new Rectangle(1, 1, num - 2, num2 - 2);
						using (Graphics graphics = Graphics.FromImage(this._swatch))
						{
							this._editor.PaintValue(base.Value, graphics, rectangle);
							graphics.DrawRectangle(SystemPens.ControlDark, new Rectangle(0, 0, num - 1, num2 - 1));
						}
					}
					g.DrawImage(this._swatch, new Point(base.EditRegionRelativeLocation.X + 2, 6));
				}
			}

			// Token: 0x06000B52 RID: 2898 RVA: 0x0002C6E8 File Offset: 0x0002B6E8
			protected internal override bool ProcessDialogKey(Keys keyData)
			{
				if (!this._button.Focused && !this._button.Ellipsis && !base.ActionPanel.DropDownActive && (keyData == (Keys.Back | Keys.Space | Keys.Alt) || keyData == (Keys.RButton | Keys.MButton | Keys.Space | Keys.Alt) || keyData == Keys.F4))
				{
					this.ActivateDropDown();
					return true;
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06000B53 RID: 2899 RVA: 0x0002C740 File Offset: 0x0002B740
			private void ShowDropDown(Control hostedControl, Color borderColor)
			{
				hostedControl.Width = Math.Max(hostedControl.Width, base.EditRegionSize.Width - 2);
				this._dropDownHolder = new DesignerActionPanel.EditorPropertyLine.DropDownHolder(hostedControl, base.ActionPanel, borderColor, base.ActionPanel.Font, this);
				if (base.ActionPanel.RightToLeft != RightToLeft.Yes)
				{
					Rectangle rectangle = new Rectangle(Point.Empty, base.EditRegionSize);
					Size size = this._dropDownHolder.Size;
					Point point = base.ActionPanel.PointToScreen(base.EditRegionLocation);
					Rectangle workingArea = Screen.FromRectangle(base.ActionPanel.RectangleToScreen(rectangle)).WorkingArea;
					size.Width = Math.Max(rectangle.Width + 1, size.Width);
					point.X = Math.Min(workingArea.Right - size.Width, Math.Max(workingArea.X, point.X + rectangle.Right - size.Width));
					point.Y += rectangle.Y;
					if (workingArea.Bottom < size.Height + point.Y + rectangle.Height)
					{
						point.Y -= size.Height + 1;
					}
					else
					{
						point.Y += rectangle.Height;
					}
					this._dropDownHolder.Location = point;
				}
				else
				{
					this._dropDownHolder.RightToLeft = base.ActionPanel.RightToLeft;
					Rectangle rectangle2 = new Rectangle(Point.Empty, base.EditRegionSize);
					Size size2 = this._dropDownHolder.Size;
					Point point2 = base.ActionPanel.PointToScreen(base.EditRegionLocation);
					Rectangle workingArea2 = Screen.FromRectangle(base.ActionPanel.RectangleToScreen(rectangle2)).WorkingArea;
					size2.Width = Math.Max(rectangle2.Width + 1, size2.Width);
					point2.X = Math.Min(workingArea2.Right - size2.Width, Math.Max(workingArea2.X, point2.X - rectangle2.Width));
					point2.Y += rectangle2.Y;
					if (workingArea2.Bottom < size2.Height + point2.Y + rectangle2.Height)
					{
						point2.Y -= size2.Height + 1;
					}
					else
					{
						point2.Y += rectangle2.Height;
					}
					this._dropDownHolder.Location = point2;
				}
				base.ActionPanel.InMethodInvoke = true;
				base.ActionPanel.SetDropDownActive(true);
				try
				{
					this._dropDownHolder.ShowDropDown(this._button);
				}
				finally
				{
					this._button.ResetMouseStates();
					base.ActionPanel.SetDropDownActive(false);
					base.ActionPanel.InMethodInvoke = false;
				}
			}

			// Token: 0x06000B54 RID: 2900 RVA: 0x0002CA38 File Offset: 0x0002BA38
			void IWindowsFormsEditorService.CloseDropDown()
			{
				this.CloseDropDown();
			}

			// Token: 0x06000B55 RID: 2901 RVA: 0x0002CA40 File Offset: 0x0002BA40
			void IWindowsFormsEditorService.DropDownControl(Control control)
			{
				this.ShowDropDown(control, base.ActionPanel.BorderColor);
			}

			// Token: 0x06000B56 RID: 2902 RVA: 0x0002CA54 File Offset: 0x0002BA54
			DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
			{
				IUIService iuiservice = (IUIService)base.ServiceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					return iuiservice.ShowDialog(dialog);
				}
				return dialog.ShowDialog();
			}

			// Token: 0x06000B57 RID: 2903 RVA: 0x0002CA8D File Offset: 0x0002BA8D
			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IWindowsFormsEditorService))
				{
					return this;
				}
				return base.ServiceProvider.GetService(serviceType);
			}

			// Token: 0x04000DEB RID: 3563
			private DesignerActionPanel.EditorPropertyLine.EditorButton _button;

			// Token: 0x04000DEC RID: 3564
			private UITypeEditor _editor;

			// Token: 0x04000DED RID: 3565
			private bool _hasSwatch;

			// Token: 0x04000DEE RID: 3566
			private Image _swatch;

			// Token: 0x04000DEF RID: 3567
			private DesignerActionPanel.EditorPropertyLine.FlyoutDialog _dropDownHolder;

			// Token: 0x04000DF0 RID: 3568
			private bool _ignoreNextSelectChange;

			// Token: 0x04000DF1 RID: 3569
			private bool _ignoreDropDownValue;

			// Token: 0x02000115 RID: 277
			internal class FlyoutDialog : Form
			{
				// Token: 0x06000B58 RID: 2904 RVA: 0x0002CAAC File Offset: 0x0002BAAC
				public FlyoutDialog(Control hostedControl, Control parentControl, Color borderColor, Font font)
				{
					this._hostedControl = hostedControl;
					this._parentControl = parentControl;
					this.BackColor = SystemColors.Window;
					base.ControlBox = false;
					this.Font = font;
					base.FormBorderStyle = FormBorderStyle.None;
					base.MinimizeBox = false;
					base.MaximizeBox = false;
					base.ShowInTaskbar = false;
					base.StartPosition = FormStartPosition.Manual;
					this.Text = string.Empty;
					base.SuspendLayout();
					try
					{
						base.Controls.Add(hostedControl);
						int num = Math.Max(this._hostedControl.Width, SystemInformation.MinimumWindowSize.Width);
						int num2 = Math.Max(this._hostedControl.Height, SystemInformation.MinimizedWindowSize.Height);
						if (!borderColor.IsEmpty)
						{
							base.DockPadding.All = 1;
							this.BackColor = borderColor;
							num += 2;
							num2 += 4;
						}
						this._hostedControl.Dock = DockStyle.Fill;
						base.Width = num;
						base.Height = num2;
					}
					finally
					{
						base.ResumeLayout();
					}
				}

				// Token: 0x170001A0 RID: 416
				// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0002CBBC File Offset: 0x0002BBBC
				protected override CreateParams CreateParams
				{
					get
					{
						CreateParams createParams = base.CreateParams;
						createParams.ExStyle |= 128;
						createParams.Style |= -2139095040;
						createParams.ClassStyle |= 2048;
						if (this._parentControl != null && !this._parentControl.IsDisposed)
						{
							createParams.Parent = this._parentControl.Handle;
						}
						return createParams;
					}
				}

				// Token: 0x06000B5A RID: 2906 RVA: 0x0002CC2D File Offset: 0x0002BC2D
				public virtual void FocusComponent()
				{
					if (this._hostedControl != null && base.Visible)
					{
						this._hostedControl.Focus();
					}
				}

				// Token: 0x06000B5B RID: 2907 RVA: 0x0002CC4B File Offset: 0x0002BC4B
				public void DoModalLoop()
				{
					while (base.Visible)
					{
						Application.DoEvents();
						DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 250, 255, 4);
					}
				}

				// Token: 0x06000B5C RID: 2908 RVA: 0x0002CC74 File Offset: 0x0002BC74
				private bool OwnsWindow(IntPtr hWnd)
				{
					while (hWnd != IntPtr.Zero)
					{
						hWnd = DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.GetWindowLong(new HandleRef(null, hWnd), -8);
						if (hWnd == IntPtr.Zero)
						{
							return false;
						}
						if (hWnd == base.Handle)
						{
							return true;
						}
					}
					return false;
				}

				// Token: 0x06000B5D RID: 2909 RVA: 0x0002CCC0 File Offset: 0x0002BCC0
				protected override bool ProcessDialogKey(Keys keyData)
				{
					if (keyData == (Keys.Back | Keys.Space | Keys.Alt) || keyData == (Keys.RButton | Keys.MButton | Keys.Space | Keys.Alt) || keyData == Keys.F4)
					{
						base.Visible = false;
						return true;
					}
					return base.ProcessDialogKey(keyData);
				}

				// Token: 0x06000B5E RID: 2910 RVA: 0x0002CCE8 File Offset: 0x0002BCE8
				public void ShowDropDown(Control parent)
				{
					try
					{
						DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, new HandleRef(parent, parent.Handle));
						IntPtr capture = DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.GetCapture();
						if (capture != IntPtr.Zero)
						{
							DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.SendMessage(new HandleRef(null, capture), 31, 0, 0);
							DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.ReleaseCapture();
						}
						base.Visible = true;
						this.FocusComponent();
						this.DoModalLoop();
					}
					finally
					{
						DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -8, new HandleRef(null, IntPtr.Zero));
						if (parent != null && parent.Visible)
						{
							parent.Focus();
						}
					}
				}

				// Token: 0x06000B5F RID: 2911 RVA: 0x0002CD98 File Offset: 0x0002BD98
				protected override void WndProc(ref Message m)
				{
					if (m.Msg == 6 && base.Visible && DesignerActionPanel.EditorPropertyLine.NativeMethods.Util.LOWORD((int)m.WParam) == 0 && !this.OwnsWindow(m.LParam))
					{
						base.Visible = false;
						if (m.LParam == IntPtr.Zero)
						{
							Control topLevelControl = this._parentControl.TopLevelControl;
							ToolStripDropDown toolStripDropDown = topLevelControl as ToolStripDropDown;
							if (toolStripDropDown != null)
							{
								toolStripDropDown.Close();
								return;
							}
							if (topLevelControl != null)
							{
								topLevelControl.Visible = false;
							}
						}
						return;
					}
					base.WndProc(ref m);
				}

				// Token: 0x04000DF2 RID: 3570
				private Control _hostedControl;

				// Token: 0x04000DF3 RID: 3571
				private Control _parentControl;
			}

			// Token: 0x02000116 RID: 278
			private class DropDownHolder : DesignerActionPanel.EditorPropertyLine.FlyoutDialog
			{
				// Token: 0x06000B60 RID: 2912 RVA: 0x0002CE1E File Offset: 0x0002BE1E
				public DropDownHolder(Control hostedControl, Control parentControl, Color borderColor, Font font, DesignerActionPanel.EditorPropertyLine parent)
					: base(hostedControl, parentControl, borderColor, font)
				{
					this._parent = parent;
					this._parent.ActionPanel.SetDropDownActive(true);
				}

				// Token: 0x06000B61 RID: 2913 RVA: 0x0002CE44 File Offset: 0x0002BE44
				protected override void OnClosed(EventArgs e)
				{
					base.OnClosed(e);
					this._parent.ActionPanel.SetDropDownActive(false);
				}

				// Token: 0x06000B62 RID: 2914 RVA: 0x0002CE5E File Offset: 0x0002BE5E
				protected override bool ProcessDialogKey(Keys keyData)
				{
					if (keyData == Keys.Escape)
					{
						this._parent._ignoreDropDownValue = true;
						base.Visible = false;
						return true;
					}
					return base.ProcessDialogKey(keyData);
				}

				// Token: 0x04000DF4 RID: 3572
				private DesignerActionPanel.EditorPropertyLine _parent;
			}

			// Token: 0x02000117 RID: 279
			private static class NativeMethods
			{
				// Token: 0x04000DF5 RID: 3573
				public const int WM_ACTIVATE = 6;

				// Token: 0x04000DF6 RID: 3574
				public const int WM_CANCELMODE = 31;

				// Token: 0x04000DF7 RID: 3575
				public const int WM_MOUSEACTIVATE = 33;

				// Token: 0x04000DF8 RID: 3576
				public const int WM_NCLBUTTONDOWN = 161;

				// Token: 0x04000DF9 RID: 3577
				public const int WM_NCRBUTTONDOWN = 164;

				// Token: 0x04000DFA RID: 3578
				public const int WM_NCMBUTTONDOWN = 167;

				// Token: 0x04000DFB RID: 3579
				public const int WM_LBUTTONDOWN = 513;

				// Token: 0x04000DFC RID: 3580
				public const int WM_RBUTTONDOWN = 516;

				// Token: 0x04000DFD RID: 3581
				public const int WM_MBUTTONDOWN = 519;

				// Token: 0x04000DFE RID: 3582
				public const int WA_INACTIVE = 0;

				// Token: 0x04000DFF RID: 3583
				public const int WA_ACTIVE = 1;

				// Token: 0x04000E00 RID: 3584
				public const int WS_EX_TOOLWINDOW = 128;

				// Token: 0x04000E01 RID: 3585
				public const int WS_POPUP = -2147483648;

				// Token: 0x04000E02 RID: 3586
				public const int WS_BORDER = 8388608;

				// Token: 0x04000E03 RID: 3587
				public const int GWL_HWNDPARENT = -8;

				// Token: 0x04000E04 RID: 3588
				public const int QS_KEY = 1;

				// Token: 0x04000E05 RID: 3589
				public const int QS_MOUSEMOVE = 2;

				// Token: 0x04000E06 RID: 3590
				public const int QS_MOUSEBUTTON = 4;

				// Token: 0x04000E07 RID: 3591
				public const int QS_POSTMESSAGE = 8;

				// Token: 0x04000E08 RID: 3592
				public const int QS_TIMER = 16;

				// Token: 0x04000E09 RID: 3593
				public const int QS_PAINT = 32;

				// Token: 0x04000E0A RID: 3594
				public const int QS_SENDMESSAGE = 64;

				// Token: 0x04000E0B RID: 3595
				public const int QS_HOTKEY = 128;

				// Token: 0x04000E0C RID: 3596
				public const int QS_ALLPOSTMESSAGE = 256;

				// Token: 0x04000E0D RID: 3597
				public const int QS_MOUSE = 6;

				// Token: 0x04000E0E RID: 3598
				public const int QS_INPUT = 7;

				// Token: 0x04000E0F RID: 3599
				public const int QS_ALLEVENTS = 191;

				// Token: 0x04000E10 RID: 3600
				public const int QS_ALLINPUT = 255;

				// Token: 0x04000E11 RID: 3601
				public const int CS_SAVEBITS = 2048;

				// Token: 0x04000E12 RID: 3602
				public const int MWMO_INPUTAVAILABLE = 4;

				// Token: 0x02000118 RID: 280
				internal static class Util
				{
					// Token: 0x06000B63 RID: 2915 RVA: 0x0002CE81 File Offset: 0x0002BE81
					public static int LOWORD(int n)
					{
						return n & 65535;
					}
				}

				// Token: 0x02000119 RID: 281
				public static class CommonHandles
				{
					// Token: 0x04000E13 RID: 3603
					public static HandleCollector GdiHandleCollector = new HandleCollector("GDI", 500);

					// Token: 0x04000E14 RID: 3604
					public static HandleCollector HdcHandleCollector = new HandleCollector("HDC", 2);
				}

				// Token: 0x0200011A RID: 282
				[StructLayout(LayoutKind.Sequential)]
				public class SIZE
				{
					// Token: 0x04000E15 RID: 3605
					public int cx;

					// Token: 0x04000E16 RID: 3606
					public int cy;
				}

				// Token: 0x0200011B RID: 283
				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
				public struct TEXTMETRIC
				{
					// Token: 0x04000E17 RID: 3607
					public int tmHeight;

					// Token: 0x04000E18 RID: 3608
					public int tmAscent;

					// Token: 0x04000E19 RID: 3609
					public int tmDescent;

					// Token: 0x04000E1A RID: 3610
					public int tmInternalLeading;

					// Token: 0x04000E1B RID: 3611
					public int tmExternalLeading;

					// Token: 0x04000E1C RID: 3612
					public int tmAveCharWidth;

					// Token: 0x04000E1D RID: 3613
					public int tmMaxCharWidth;

					// Token: 0x04000E1E RID: 3614
					public int tmWeight;

					// Token: 0x04000E1F RID: 3615
					public int tmOverhang;

					// Token: 0x04000E20 RID: 3616
					public int tmDigitizedAspectX;

					// Token: 0x04000E21 RID: 3617
					public int tmDigitizedAspectY;

					// Token: 0x04000E22 RID: 3618
					public char tmFirstChar;

					// Token: 0x04000E23 RID: 3619
					public char tmLastChar;

					// Token: 0x04000E24 RID: 3620
					public char tmDefaultChar;

					// Token: 0x04000E25 RID: 3621
					public char tmBreakChar;

					// Token: 0x04000E26 RID: 3622
					public byte tmItalic;

					// Token: 0x04000E27 RID: 3623
					public byte tmUnderlined;

					// Token: 0x04000E28 RID: 3624
					public byte tmStruckOut;

					// Token: 0x04000E29 RID: 3625
					public byte tmPitchAndFamily;

					// Token: 0x04000E2A RID: 3626
					public byte tmCharSet;
				}

				// Token: 0x0200011C RID: 284
				public struct TEXTMETRICA
				{
					// Token: 0x04000E2B RID: 3627
					public int tmHeight;

					// Token: 0x04000E2C RID: 3628
					public int tmAscent;

					// Token: 0x04000E2D RID: 3629
					public int tmDescent;

					// Token: 0x04000E2E RID: 3630
					public int tmInternalLeading;

					// Token: 0x04000E2F RID: 3631
					public int tmExternalLeading;

					// Token: 0x04000E30 RID: 3632
					public int tmAveCharWidth;

					// Token: 0x04000E31 RID: 3633
					public int tmMaxCharWidth;

					// Token: 0x04000E32 RID: 3634
					public int tmWeight;

					// Token: 0x04000E33 RID: 3635
					public int tmOverhang;

					// Token: 0x04000E34 RID: 3636
					public int tmDigitizedAspectX;

					// Token: 0x04000E35 RID: 3637
					public int tmDigitizedAspectY;

					// Token: 0x04000E36 RID: 3638
					public byte tmFirstChar;

					// Token: 0x04000E37 RID: 3639
					public byte tmLastChar;

					// Token: 0x04000E38 RID: 3640
					public byte tmDefaultChar;

					// Token: 0x04000E39 RID: 3641
					public byte tmBreakChar;

					// Token: 0x04000E3A RID: 3642
					public byte tmItalic;

					// Token: 0x04000E3B RID: 3643
					public byte tmUnderlined;

					// Token: 0x04000E3C RID: 3644
					public byte tmStruckOut;

					// Token: 0x04000E3D RID: 3645
					public byte tmPitchAndFamily;

					// Token: 0x04000E3E RID: 3646
					public byte tmCharSet;
				}
			}

			// Token: 0x0200011D RID: 285
			private static class SafeNativeMethods
			{
				// Token: 0x06000B66 RID: 2918
				[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
				private static extern bool IntDeleteObject(HandleRef hObject);

				// Token: 0x06000B67 RID: 2919 RVA: 0x0002CEB8 File Offset: 0x0002BEB8
				public static bool DeleteObject(HandleRef hObject)
				{
					DesignerActionPanel.EditorPropertyLine.NativeMethods.CommonHandles.GdiHandleCollector.Remove();
					return DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.IntDeleteObject(hObject);
				}

				// Token: 0x06000B68 RID: 2920
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern bool ReleaseCapture();

				// Token: 0x06000B69 RID: 2921
				[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
				public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

				// Token: 0x06000B6A RID: 2922
				[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
				public static extern int GetTextExtentPoint32(HandleRef hDC, string str, int len, [In] [Out] DesignerActionPanel.EditorPropertyLine.NativeMethods.SIZE size);

				// Token: 0x06000B6B RID: 2923
				[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
				public static extern int GetTextMetricsW(HandleRef hDC, [In] [Out] ref DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRIC lptm);

				// Token: 0x06000B6C RID: 2924
				[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
				public static extern int GetTextMetricsA(HandleRef hDC, [In] [Out] ref DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRICA lptm);

				// Token: 0x06000B6D RID: 2925 RVA: 0x0002CECC File Offset: 0x0002BECC
				public static int GetTextMetrics(HandleRef hDC, ref DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRIC lptm)
				{
					if (Marshal.SystemDefaultCharSize == 1)
					{
						DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRICA textmetrica = default(DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRICA);
						int textMetricsA = DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.GetTextMetricsA(hDC, ref textmetrica);
						lptm.tmHeight = textmetrica.tmHeight;
						lptm.tmAscent = textmetrica.tmAscent;
						lptm.tmDescent = textmetrica.tmDescent;
						lptm.tmInternalLeading = textmetrica.tmInternalLeading;
						lptm.tmExternalLeading = textmetrica.tmExternalLeading;
						lptm.tmAveCharWidth = textmetrica.tmAveCharWidth;
						lptm.tmMaxCharWidth = textmetrica.tmMaxCharWidth;
						lptm.tmWeight = textmetrica.tmWeight;
						lptm.tmOverhang = textmetrica.tmOverhang;
						lptm.tmDigitizedAspectX = textmetrica.tmDigitizedAspectX;
						lptm.tmDigitizedAspectY = textmetrica.tmDigitizedAspectY;
						lptm.tmFirstChar = (char)textmetrica.tmFirstChar;
						lptm.tmLastChar = (char)textmetrica.tmLastChar;
						lptm.tmDefaultChar = (char)textmetrica.tmDefaultChar;
						lptm.tmBreakChar = (char)textmetrica.tmBreakChar;
						lptm.tmItalic = textmetrica.tmItalic;
						lptm.tmUnderlined = textmetrica.tmUnderlined;
						lptm.tmStruckOut = textmetrica.tmStruckOut;
						lptm.tmPitchAndFamily = textmetrica.tmPitchAndFamily;
						lptm.tmCharSet = textmetrica.tmCharSet;
						return textMetricsA;
					}
					return DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.GetTextMetricsW(hDC, ref lptm);
				}
			}

			// Token: 0x0200011E RID: 286
			private static class UnsafeNativeMethods
			{
				// Token: 0x06000B6E RID: 2926
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr GetWindowLong(HandleRef hWnd, int nIndex);

				// Token: 0x06000B6F RID: 2927
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

				// Token: 0x06000B70 RID: 2928
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

				// Token: 0x06000B71 RID: 2929
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

				// Token: 0x06000B72 RID: 2930
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr GetCapture();

				// Token: 0x06000B73 RID: 2931
				[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true)]
				private static extern IntPtr IntGetDC(HandleRef hWnd);

				// Token: 0x06000B74 RID: 2932 RVA: 0x0002D002 File Offset: 0x0002C002
				public static IntPtr GetDC(HandleRef hWnd)
				{
					DesignerActionPanel.EditorPropertyLine.NativeMethods.CommonHandles.HdcHandleCollector.Add();
					return DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.IntGetDC(hWnd);
				}

				// Token: 0x06000B75 RID: 2933
				[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true)]
				private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

				// Token: 0x06000B76 RID: 2934 RVA: 0x0002D014 File Offset: 0x0002C014
				public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
				{
					DesignerActionPanel.EditorPropertyLine.NativeMethods.CommonHandles.HdcHandleCollector.Remove();
					return DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
				}
			}

			// Token: 0x0200011F RID: 287
			internal sealed class EditorButton : Button
			{
				// Token: 0x06000B77 RID: 2935 RVA: 0x0002D027 File Offset: 0x0002C027
				protected override void OnMouseDown(MouseEventArgs e)
				{
					base.OnMouseDown(e);
					if (e.Button == MouseButtons.Left)
					{
						this._mouseDown = true;
					}
				}

				// Token: 0x06000B78 RID: 2936 RVA: 0x0002D044 File Offset: 0x0002C044
				protected override void OnMouseEnter(EventArgs e)
				{
					base.OnMouseEnter(e);
					this._mouseOver = true;
				}

				// Token: 0x06000B79 RID: 2937 RVA: 0x0002D054 File Offset: 0x0002C054
				protected override void OnMouseLeave(EventArgs e)
				{
					base.OnMouseLeave(e);
					this._mouseOver = false;
				}

				// Token: 0x06000B7A RID: 2938 RVA: 0x0002D064 File Offset: 0x0002C064
				protected override void OnMouseUp(MouseEventArgs e)
				{
					base.OnMouseUp(e);
					if (e.Button == MouseButtons.Left)
					{
						this._mouseDown = false;
					}
				}

				// Token: 0x170001A1 RID: 417
				// (get) Token: 0x06000B7B RID: 2939 RVA: 0x0002D081 File Offset: 0x0002C081
				// (set) Token: 0x06000B7C RID: 2940 RVA: 0x0002D089 File Offset: 0x0002C089
				public bool Ellipsis
				{
					get
					{
						return this._ellipsis;
					}
					set
					{
						this._ellipsis = value;
					}
				}

				// Token: 0x06000B7D RID: 2941 RVA: 0x0002D094 File Offset: 0x0002C094
				protected override void OnPaint(PaintEventArgs e)
				{
					Graphics graphics = e.Graphics;
					if (this._ellipsis)
					{
						PushButtonState pushButtonState = PushButtonState.Normal;
						if (this._mouseDown)
						{
							pushButtonState = PushButtonState.Pressed;
						}
						else if (this._mouseOver)
						{
							pushButtonState = PushButtonState.Hot;
						}
						ButtonRenderer.DrawButton(graphics, new Rectangle(-1, -1, base.Width + 2, base.Height + 2), "…", this.Font, this.Focused, pushButtonState);
						return;
					}
					if (ComboBoxRenderer.IsSupported)
					{
						ComboBoxState comboBoxState = ComboBoxState.Normal;
						if (base.Enabled)
						{
							if (this._mouseDown)
							{
								comboBoxState = ComboBoxState.Pressed;
							}
							else if (this._mouseOver)
							{
								comboBoxState = ComboBoxState.Hot;
							}
						}
						else
						{
							comboBoxState = ComboBoxState.Disabled;
						}
						ComboBoxRenderer.DrawDropDownButton(graphics, new Rectangle(0, 0, base.Width, base.Height), comboBoxState);
					}
					else
					{
						PushButtonState pushButtonState2 = PushButtonState.Normal;
						if (base.Enabled)
						{
							if (this._mouseDown)
							{
								pushButtonState2 = PushButtonState.Pressed;
							}
							else if (this._mouseOver)
							{
								pushButtonState2 = PushButtonState.Hot;
							}
						}
						else
						{
							pushButtonState2 = PushButtonState.Disabled;
						}
						ButtonRenderer.DrawButton(graphics, new Rectangle(-1, -1, base.Width + 2, base.Height + 2), string.Empty, this.Font, this.Focused, pushButtonState2);
						try
						{
							using (Icon icon = new Icon(typeof(DesignerActionPanel), "Arrow.ico"))
							{
								Bitmap bitmap = icon.ToBitmap();
								using (ImageAttributes imageAttributes = new ImageAttributes())
								{
									imageAttributes.SetRemapTable(new ColorMap[]
									{
										new ColorMap
										{
											OldColor = Color.Black,
											NewColor = SystemColors.WindowText
										}
									}, ColorAdjustType.Bitmap);
									int width = bitmap.Width;
									int height = bitmap.Height;
									graphics.DrawImage(bitmap, new Rectangle((base.Width - width + 1) / 2, (base.Height - height + 1) / 2, width, height), 0, 0, width, width, GraphicsUnit.Pixel, imageAttributes, null, IntPtr.Zero);
								}
							}
						}
						catch
						{
						}
					}
					if (this.Focused)
					{
						ControlPaint.DrawFocusRectangle(graphics, new Rectangle(2, 2, base.Width - 5, base.Height - 5));
					}
				}

				// Token: 0x06000B7E RID: 2942 RVA: 0x0002D2AC File Offset: 0x0002C2AC
				public void ResetMouseStates()
				{
					this._mouseDown = false;
					this._mouseOver = false;
					base.Invalidate();
				}

				// Token: 0x04000E3F RID: 3647
				private bool _mouseOver;

				// Token: 0x04000E40 RID: 3648
				private bool _mouseDown;

				// Token: 0x04000E41 RID: 3649
				private bool _ellipsis;
			}
		}

		// Token: 0x02000120 RID: 288
		private class TextLine : DesignerActionPanel.Line
		{
			// Token: 0x06000B80 RID: 2944 RVA: 0x0002D2CA File Offset: 0x0002C2CA
			public TextLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
				actionPanel.FontChanged += this.OnParentControlFontChanged;
			}

			// Token: 0x170001A2 RID: 418
			// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0002D2E6 File Offset: 0x0002C2E6
			public sealed override string FocusId
			{
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x06000B82 RID: 2946 RVA: 0x0002D2F0 File Offset: 0x0002C2F0
			protected override void AddControls(List<Control> controls)
			{
				this._label = new Label();
				this._label.BackColor = Color.Transparent;
				this._label.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
				this._label.UseMnemonic = false;
				controls.Add(this._label);
			}

			// Token: 0x06000B83 RID: 2947 RVA: 0x0002D33D File Offset: 0x0002C33D
			public sealed override void Focus()
			{
			}

			// Token: 0x06000B84 RID: 2948 RVA: 0x0002D340 File Offset: 0x0002C340
			public override Size LayoutControls(int top, int width, bool measureOnly)
			{
				Size preferredSize = this._label.GetPreferredSize(new Size(int.MaxValue, int.MaxValue));
				if (!measureOnly)
				{
					this._label.Location = new Point(5, top + 3);
					this._label.Size = preferredSize;
				}
				return preferredSize + new Size(9, 7);
			}

			// Token: 0x06000B85 RID: 2949 RVA: 0x0002D399 File Offset: 0x0002C399
			private void OnParentControlFontChanged(object sender, EventArgs e)
			{
				if (this._label.Font != null)
				{
					this._label.Font = this.GetFont();
				}
			}

			// Token: 0x06000B86 RID: 2950 RVA: 0x0002D3B9 File Offset: 0x0002C3B9
			protected virtual Font GetFont()
			{
				return base.ActionPanel.Font;
			}

			// Token: 0x06000B87 RID: 2951 RVA: 0x0002D3C8 File Offset: 0x0002C3C8
			internal override void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex)
			{
				this._textItem = (DesignerActionTextItem)actionItem;
				this._label.Text = DesignerActionPanel.StripAmpersands(this._textItem.DisplayName);
				this._label.Font = this.GetFont();
				this._label.TabIndex = currentTabIndex++;
				toolTip.SetToolTip(this._label, this._textItem.Description);
			}

			// Token: 0x04000E42 RID: 3650
			private Label _label;

			// Token: 0x04000E43 RID: 3651
			private DesignerActionTextItem _textItem;
		}

		// Token: 0x02000121 RID: 289
		private sealed class HeaderLine : DesignerActionPanel.TextLine
		{
			// Token: 0x06000B88 RID: 2952 RVA: 0x0002D439 File Offset: 0x0002C439
			public HeaderLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			// Token: 0x06000B89 RID: 2953 RVA: 0x0002D443 File Offset: 0x0002C443
			protected override Font GetFont()
			{
				return new Font(base.ActionPanel.Font, FontStyle.Bold);
			}
		}

		// Token: 0x02000122 RID: 290
		private sealed class SeparatorLine : DesignerActionPanel.Line
		{
			// Token: 0x06000B8A RID: 2954 RVA: 0x0002D456 File Offset: 0x0002C456
			public SeparatorLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: this(serviceProvider, actionPanel, false)
			{
			}

			// Token: 0x06000B8B RID: 2955 RVA: 0x0002D461 File Offset: 0x0002C461
			public SeparatorLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel, bool isSubSeparator)
				: base(serviceProvider, actionPanel)
			{
				this._isSubSeparator = isSubSeparator;
			}

			// Token: 0x170001A3 RID: 419
			// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0002D472 File Offset: 0x0002C472
			public sealed override string FocusId
			{
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x06000B8D RID: 2957 RVA: 0x0002D479 File Offset: 0x0002C479
			protected override void AddControls(List<Control> controls)
			{
			}

			// Token: 0x06000B8E RID: 2958 RVA: 0x0002D47B File Offset: 0x0002C47B
			public sealed override void Focus()
			{
			}

			// Token: 0x06000B8F RID: 2959 RVA: 0x0002D47D File Offset: 0x0002C47D
			public override Size LayoutControls(int top, int width, bool measureOnly)
			{
				return new Size(150, 1);
			}

			// Token: 0x06000B90 RID: 2960 RVA: 0x0002D48C File Offset: 0x0002C48C
			public override void PaintLine(Graphics g, int lineWidth, int lineHeight)
			{
				using (Pen pen = new Pen(base.ActionPanel.SeparatorColor))
				{
					g.DrawLine(pen, 3, 0, lineWidth - 4, 0);
				}
			}

			// Token: 0x06000B91 RID: 2961 RVA: 0x0002D4D4 File Offset: 0x0002C4D4
			internal override void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex)
			{
			}

			// Token: 0x04000E44 RID: 3652
			private bool _isSubSeparator;
		}
	}
}
