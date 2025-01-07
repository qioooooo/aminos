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
	internal sealed class DesignerActionPanel : ContainerControl
	{
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

		public Color ActiveLinkColor
		{
			get
			{
				return this._activeLinkColor;
			}
		}

		public Color BorderColor
		{
			get
			{
				return this._borderColor;
			}
		}

		private bool DropDownActive
		{
			get
			{
				return this._dropDownActive;
			}
		}

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

		public Color GradientDarkColor
		{
			get
			{
				return this._gradientDarkColor;
			}
		}

		public Color GradientLightColor
		{
			get
			{
				return this._gradientLightColor;
			}
		}

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

		public Color LinkColor
		{
			get
			{
				return this._linkColor;
			}
		}

		public Color SeparatorColor
		{
			get
			{
				return this._separatorColor;
			}
		}

		private IServiceProvider ServiceProvider
		{
			get
			{
				return this._serviceProvider;
			}
		}

		public Color TitleBarColor
		{
			get
			{
				return this._titleBarColor;
			}
		}

		public Color TitleBarTextColor
		{
			get
			{
				return this._titleBarTextColor;
			}
		}

		public Color TitleBarUnselectedColor
		{
			get
			{
				return this._titleBarUnselectedColor;
			}
		}

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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._toolTip.Dispose();
			}
			base.Dispose(disposing);
		}

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

		public override Size GetPreferredSize(Size proposedSize)
		{
			if (proposedSize.IsEmpty)
			{
				return proposedSize;
			}
			return this.DoLayout(proposedSize, true);
		}

		private static bool IsReadOnlyProperty(PropertyDescriptor pd)
		{
			return pd.IsReadOnly || pd.ComponentType.GetProperty(pd.Name).GetSetMethod() == null;
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateEditXPos();
		}

		private void OnFormActivated(object sender, EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[DesignerActionPanel.EventFormActivated];
			if (eventHandler != null)
			{
				eventHandler(sender, e);
			}
		}

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

		private void OnFormDeactivate(object sender, EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[DesignerActionPanel.EventFormDeactivate];
			if (eventHandler != null)
			{
				eventHandler(sender, e);
			}
		}

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

		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this._updatingTasks)
			{
				return;
			}
			this.DoLayout(base.Size, false);
		}

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

		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			base.PerformLayout();
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			DesignerActionPanel.Line focusedLine = this.FocusedLine;
			return (focusedLine != null && focusedLine.ProcessDialogKey(keyData)) || base.ProcessDialogKey(keyData);
		}

		protected override bool ProcessTabKey(bool forward)
		{
			return base.SelectNextControl(base.ActiveControl, forward, true, true, true);
		}

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

		private void SetDropDownActive(bool active)
		{
			this._dropDownActive = active;
		}

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

		public const string ExternDllGdi32 = "gdi32.dll";

		public const string ExternDllUser32 = "user32.dll";

		private const int EditInputWidth = 150;

		private const int ListBoxMaximumHeight = 200;

		private const int MinimumWidth = 150;

		private const int BottomPadding = 2;

		private const int TopPadding = 2;

		private const int LineLeftMargin = 5;

		private const int LineRightMargin = 4;

		private const int LineVerticalPadding = 7;

		private const int TextBoxTopPadding = 4;

		private const int SeparatorHorizontalPadding = 3;

		private const int TextBoxLineCenterMargin = 5;

		private const int TextBoxLineInnerPadding = 1;

		private const int EditorLineSwatchPadding = 1;

		private const int EditorLineButtonPadding = 1;

		private const int PanelHeaderVerticalPadding = 3;

		private const int PanelHeaderHorizontalPadding = 5;

		private const int TextBoxHeightFixup = 2;

		private static readonly object EventFormActivated = new object();

		private static readonly object EventFormDeactivate = new object();

		private CommandID[] _filteredCommandIDs;

		private ToolTip _toolTip;

		private List<DesignerActionPanel.Line> _lines;

		private List<int> _lineYPositions;

		private List<int> _lineHeights;

		private Color _gradientLightColor = SystemColors.Control;

		private Color _gradientDarkColor = SystemColors.Control;

		private Color _titleBarColor = SystemColors.ActiveCaption;

		private Color _titleBarUnselectedColor = SystemColors.InactiveCaption;

		private Color _titleBarTextColor = SystemColors.ActiveCaptionText;

		private Color _separatorColor = SystemColors.ControlDark;

		private Color _borderColor = SystemColors.ActiveBorder;

		private Color _linkColor = SystemColors.HotTrack;

		private Color _activeLinkColor = SystemColors.HotTrack;

		private IServiceProvider _serviceProvider;

		private bool _inMethodInvoke;

		private bool _updatingTasks;

		private bool _dropDownActive;

		private class LineInfo
		{
			public LineInfo(DesignerActionList list, DesignerActionItem item, DesignerActionPanel.Line line)
			{
				this.Line = line;
				this.Item = item;
				this.List = list;
			}

			public DesignerActionPanel.Line Line;

			public DesignerActionItem Item;

			public DesignerActionList List;
		}

		internal sealed class TypeDescriptorContext : ITypeDescriptorContext, IServiceProvider
		{
			public TypeDescriptorContext(IServiceProvider serviceProvider, PropertyDescriptor propDesc, object instance)
			{
				this._serviceProvider = serviceProvider;
				this._propDesc = propDesc;
				this._instance = instance;
			}

			private IComponentChangeService ComponentChangeService
			{
				get
				{
					return (IComponentChangeService)this._serviceProvider.GetService(typeof(IComponentChangeService));
				}
			}

			public IContainer Container
			{
				get
				{
					return (IContainer)this._serviceProvider.GetService(typeof(IContainer));
				}
			}

			public object Instance
			{
				get
				{
					return this._instance;
				}
			}

			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this._propDesc;
				}
			}

			public object GetService(Type serviceType)
			{
				return this._serviceProvider.GetService(serviceType);
			}

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

			public void OnComponentChanged()
			{
				if (this.ComponentChangeService != null)
				{
					this.ComponentChangeService.OnComponentChanged(this._instance, this._propDesc, null, null);
				}
			}

			private IServiceProvider _serviceProvider;

			private PropertyDescriptor _propDesc;

			private object _instance;
		}

		private abstract class Line
		{
			public Line(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
			{
				if (actionPanel == null)
				{
					throw new ArgumentNullException("actionPanel");
				}
				this._serviceProvider = serviceProvider;
				this._actionPanel = actionPanel;
			}

			protected DesignerActionPanel ActionPanel
			{
				get
				{
					return this._actionPanel;
				}
			}

			public abstract string FocusId { get; }

			protected IServiceProvider ServiceProvider
			{
				get
				{
					return this._serviceProvider;
				}
			}

			protected abstract void AddControls(List<Control> controls);

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

			public abstract void Focus();

			public abstract Size LayoutControls(int top, int width, bool measureOnly);

			public virtual void PaintLine(Graphics g, int lineWidth, int lineHeight)
			{
			}

			protected internal virtual bool ProcessDialogKey(Keys keyData)
			{
				return false;
			}

			internal void RemoveControls(Control.ControlCollection controls)
			{
				for (int i = 0; i < this._addedControls.Count; i++)
				{
					Control control = this._addedControls[i];
					control.Tag = null;
					controls.Remove(control);
				}
			}

			internal abstract void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex);

			private DesignerActionPanel _actionPanel;

			private List<Control> _addedControls;

			private IServiceProvider _serviceProvider;
		}

		private sealed class DesignerActionPanelHeaderItem : DesignerActionItem
		{
			public DesignerActionPanelHeaderItem(string title, string subtitle)
				: base(title, null, null)
			{
				this._subtitle = subtitle;
			}

			public string Subtitle
			{
				get
				{
					return this._subtitle;
				}
			}

			private string _subtitle;
		}

		private sealed class PanelHeaderLine : DesignerActionPanel.Line
		{
			public PanelHeaderLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
				actionPanel.FontChanged += this.OnParentControlFontChanged;
			}

			public sealed override string FocusId
			{
				get
				{
					return string.Empty;
				}
			}

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

			public sealed override void Focus()
			{
			}

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

			private void OnFormActivated(object sender, EventArgs e)
			{
				this._formActive = true;
				base.ActionPanel.Invalidate();
			}

			private void OnFormDeactivate(object sender, EventArgs e)
			{
				this._formActive = false;
				base.ActionPanel.Invalidate();
			}

			private void OnParentControlFontChanged(object sender, EventArgs e)
			{
				if (this._titleLabel != null && this._subtitleLabel != null)
				{
					this._titleLabel.Font = new Font(base.ActionPanel.Font, FontStyle.Bold);
					this._subtitleLabel.Font = base.ActionPanel.Font;
				}
			}

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

			private DesignerActionList _actionList;

			private DesignerActionPanel.DesignerActionPanelHeaderItem _panelHeaderItem;

			private Label _titleLabel;

			private Label _subtitleLabel;

			private bool _formActive;
		}

		private sealed class MethodLine : DesignerActionPanel.Line
		{
			public MethodLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			public sealed override string FocusId
			{
				get
				{
					return "METHOD:" + this._actionList.GetType().FullName + "." + this._methodItem.MemberName;
				}
			}

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

			public sealed override void Focus()
			{
				this._linkLabel.Focus();
			}

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

			internal override void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex)
			{
				this._actionList = actionList;
				this._methodItem = (DesignerActionMethodItem)actionItem;
				toolTip.SetToolTip(this._linkLabel, this._methodItem.Description);
				this._linkLabel.Text = DesignerActionPanel.StripAmpersands(this._methodItem.DisplayName);
				this._linkLabel.AccessibleDescription = actionItem.Description;
				this._linkLabel.TabIndex = currentTabIndex++;
			}

			private DesignerActionList _actionList;

			private DesignerActionMethodItem _methodItem;

			private DesignerActionPanel.MethodLine.MethodItemLinkLabel _linkLabel;

			private sealed class MethodItemLinkLabel : LinkLabel
			{
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

		private abstract class PropertyLine : DesignerActionPanel.Line
		{
			public PropertyLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			public sealed override string FocusId
			{
				get
				{
					return "PROPERTY:" + this._actionList.GetType().FullName + "." + this._propertyItem.MemberName;
				}
			}

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

			protected DesignerActionPropertyItem PropertyItem
			{
				get
				{
					return this._propertyItem;
				}
			}

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

			protected object Value
			{
				get
				{
					return this._value;
				}
			}

			protected abstract void OnPropertyTaskItemUpdated(ToolTip toolTip, ref int currentTabIndex);

			protected abstract void OnValueChanged();

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

			private DesignerActionList _actionList;

			private DesignerActionPropertyItem _propertyItem;

			private object _value;

			private bool _pushingValue;

			private PropertyDescriptor _propDesc;

			private ITypeDescriptorContext _typeDescriptorContext;
		}

		private sealed class CheckBoxPropertyLine : DesignerActionPanel.PropertyLine
		{
			public CheckBoxPropertyLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

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

			public sealed override void Focus()
			{
				this._checkBox.Focus();
			}

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

			private void OnCheckBoxCheckedChanged(object sender, EventArgs e)
			{
				base.SetValue(this._checkBox.Checked);
			}

			protected override void OnPropertyTaskItemUpdated(ToolTip toolTip, ref int currentTabIndex)
			{
				this._checkBox.Text = DesignerActionPanel.StripAmpersands(base.PropertyItem.DisplayName);
				this._checkBox.AccessibleDescription = base.PropertyItem.Description;
				this._checkBox.TabIndex = currentTabIndex++;
				toolTip.SetToolTip(this._checkBox, base.PropertyItem.Description);
			}

			protected override void OnValueChanged()
			{
				this._checkBox.Checked = (bool)base.Value;
			}

			private CheckBox _checkBox;
		}

		private class TextBoxPropertyLine : DesignerActionPanel.PropertyLine
		{
			public TextBoxPropertyLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			protected Control EditControl
			{
				get
				{
					return this._editControl;
				}
			}

			protected Point EditRegionLocation
			{
				get
				{
					return this._editRegionLocation;
				}
			}

			protected Point EditRegionRelativeLocation
			{
				get
				{
					return this._editRegionRelativeLocation;
				}
			}

			protected Size EditRegionSize
			{
				get
				{
					return this._editRegionSize;
				}
			}

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

			public sealed override void Focus()
			{
				this._editControl.Focus();
			}

			internal int GetEditRegionXPos()
			{
				if (string.IsNullOrEmpty(this._label.Text))
				{
					return 5;
				}
				return 5 + this._label.GetPreferredSize(new Size(int.MaxValue, int.MaxValue)).Width + 5;
			}

			protected virtual int GetTextBoxLeftPadding(int textBoxHeight)
			{
				return 1;
			}

			protected virtual int GetTextBoxRightPadding(int textBoxHeight)
			{
				return 1;
			}

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

			protected virtual bool IsReadOnly()
			{
				return DesignerActionPanel.IsReadOnlyProperty(base.PropertyDescriptor);
			}

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

			protected virtual void OnReadOnlyTextBoxLabelClick(object sender, MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.Focus();
				}
			}

			private void OnReadOnlyTextBoxLabelEnter(object sender, EventArgs e)
			{
				this._readOnlyTextBoxLabel.ForeColor = SystemColors.HighlightText;
				this._readOnlyTextBoxLabel.BackColor = SystemColors.Highlight;
			}

			private void OnReadOnlyTextBoxLabelLeave(object sender, EventArgs e)
			{
				this._readOnlyTextBoxLabel.ForeColor = SystemColors.WindowText;
				this._readOnlyTextBoxLabel.BackColor = SystemColors.Window;
			}

			protected TypeConverter.StandardValuesCollection GetStandardValues()
			{
				TypeConverter converter = base.PropertyDescriptor.Converter;
				if (converter != null && converter.GetStandardValuesSupported(base.TypeDescriptorContext))
				{
					return converter.GetStandardValues(base.TypeDescriptorContext);
				}
				return null;
			}

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

			private void OnReadOnlyTextBoxLabelKeyDown(object sender, KeyEventArgs e)
			{
				this.OnEditControlKeyDown(e);
			}

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

			private void OnTextBoxLostFocus(object sender, EventArgs e)
			{
				if (base.ActionPanel.DropDownActive)
				{
					return;
				}
				this.UpdateValue();
			}

			private void OnTextBoxTextChanged(object sender, EventArgs e)
			{
				this._textBoxDirty = true;
			}

			protected override void OnValueChanged()
			{
				this._editControl.Text = base.PropertyDescriptor.Converter.ConvertToString(base.TypeDescriptorContext, base.Value);
			}

			public override void PaintLine(Graphics g, int lineWidth, int lineHeight)
			{
				Rectangle rectangle = new Rectangle(this.EditRegionRelativeLocation, this.EditRegionSize);
				g.FillRectangle(SystemBrushes.Window, rectangle);
				g.DrawRectangle(SystemPens.ControlDark, rectangle);
			}

			internal void SetEditRegionXPos(int xPos)
			{
				if (!string.IsNullOrEmpty(this._label.Text))
				{
					this._editXPos = xPos;
					return;
				}
				this._editXPos = 5;
			}

			private void UpdateValue()
			{
				if (this._textBoxDirty)
				{
					base.SetValue(this._editControl.Text);
					this._textBoxDirty = false;
				}
			}

			private TextBox _textBox;

			private DesignerActionPanel.TextBoxPropertyLine.EditorLabel _readOnlyTextBoxLabel;

			private Control _editControl;

			private Label _label;

			private int _editXPos;

			private bool _textBoxDirty;

			private Point _editRegionLocation;

			private Point _editRegionRelativeLocation;

			private Size _editRegionSize;

			private sealed class EditorLabel : Label
			{
				public EditorLabel()
				{
					base.SetStyle(ControlStyles.Selectable, true);
				}

				protected override AccessibleObject CreateAccessibilityInstance()
				{
					return new DesignerActionPanel.TextBoxPropertyLine.EditorLabel.EditorLabelAccessibleObject(this);
				}

				protected override void OnGotFocus(EventArgs e)
				{
					base.OnGotFocus(e);
					base.AccessibilityNotifyClients(AccessibleEvents.Focus, 0, -1);
				}

				protected override bool IsInputKey(Keys keyData)
				{
					return keyData == Keys.Down || keyData == Keys.Up || base.IsInputKey(keyData);
				}

				private sealed class EditorLabelAccessibleObject : Control.ControlAccessibleObject
				{
					public EditorLabelAccessibleObject(DesignerActionPanel.TextBoxPropertyLine.EditorLabel owner)
						: base(owner)
					{
					}

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

		private sealed class EditorPropertyLine : DesignerActionPanel.TextBoxPropertyLine, IWindowsFormsEditorService, IServiceProvider
		{
			public EditorPropertyLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

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

			protected override void AddControls(List<Control> controls)
			{
				base.AddControls(controls);
				this._button = new DesignerActionPanel.EditorPropertyLine.EditorButton();
				this._button.Click += this.OnButtonClick;
				this._button.GotFocus += this.OnButtonGotFocus;
				controls.Add(this._button);
			}

			private void CloseDropDown()
			{
				if (this._dropDownHolder != null)
				{
					this._dropDownHolder.Visible = false;
				}
			}

			protected override int GetTextBoxLeftPadding(int textBoxHeight)
			{
				if (this._hasSwatch)
				{
					return base.GetTextBoxLeftPadding(textBoxHeight) + textBoxHeight + 2;
				}
				return base.GetTextBoxLeftPadding(textBoxHeight);
			}

			protected override int GetTextBoxRightPadding(int textBoxHeight)
			{
				return base.GetTextBoxRightPadding(textBoxHeight) + textBoxHeight + 2;
			}

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

			private void OnButtonClick(object sender, EventArgs e)
			{
				this.ActivateDropDown();
			}

			private void OnButtonGotFocus(object sender, EventArgs e)
			{
				if (!this._button.Ellipsis)
				{
					this.Focus();
				}
			}

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

			private void OnListBoxSelectedIndexChanged(object sender, EventArgs e)
			{
				if (this._ignoreNextSelectChange)
				{
					this._ignoreNextSelectChange = false;
					return;
				}
				this.CloseDropDown();
			}

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

			protected override void OnValueChanged()
			{
				base.OnValueChanged();
				this._swatch = null;
				if (this._hasSwatch)
				{
					base.ActionPanel.Invalidate(new Rectangle(base.EditRegionLocation, base.EditRegionSize), false);
				}
			}

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

			protected internal override bool ProcessDialogKey(Keys keyData)
			{
				if (!this._button.Focused && !this._button.Ellipsis && !base.ActionPanel.DropDownActive && (keyData == (Keys.Back | Keys.Space | Keys.Alt) || keyData == (Keys.RButton | Keys.MButton | Keys.Space | Keys.Alt) || keyData == Keys.F4))
				{
					this.ActivateDropDown();
					return true;
				}
				return base.ProcessDialogKey(keyData);
			}

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

			void IWindowsFormsEditorService.CloseDropDown()
			{
				this.CloseDropDown();
			}

			void IWindowsFormsEditorService.DropDownControl(Control control)
			{
				this.ShowDropDown(control, base.ActionPanel.BorderColor);
			}

			DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
			{
				IUIService iuiservice = (IUIService)base.ServiceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					return iuiservice.ShowDialog(dialog);
				}
				return dialog.ShowDialog();
			}

			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IWindowsFormsEditorService))
				{
					return this;
				}
				return base.ServiceProvider.GetService(serviceType);
			}

			private DesignerActionPanel.EditorPropertyLine.EditorButton _button;

			private UITypeEditor _editor;

			private bool _hasSwatch;

			private Image _swatch;

			private DesignerActionPanel.EditorPropertyLine.FlyoutDialog _dropDownHolder;

			private bool _ignoreNextSelectChange;

			private bool _ignoreDropDownValue;

			internal class FlyoutDialog : Form
			{
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

				public virtual void FocusComponent()
				{
					if (this._hostedControl != null && base.Visible)
					{
						this._hostedControl.Focus();
					}
				}

				public void DoModalLoop()
				{
					while (base.Visible)
					{
						Application.DoEvents();
						DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 250, 255, 4);
					}
				}

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

				protected override bool ProcessDialogKey(Keys keyData)
				{
					if (keyData == (Keys.Back | Keys.Space | Keys.Alt) || keyData == (Keys.RButton | Keys.MButton | Keys.Space | Keys.Alt) || keyData == Keys.F4)
					{
						base.Visible = false;
						return true;
					}
					return base.ProcessDialogKey(keyData);
				}

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

				private Control _hostedControl;

				private Control _parentControl;
			}

			private class DropDownHolder : DesignerActionPanel.EditorPropertyLine.FlyoutDialog
			{
				public DropDownHolder(Control hostedControl, Control parentControl, Color borderColor, Font font, DesignerActionPanel.EditorPropertyLine parent)
					: base(hostedControl, parentControl, borderColor, font)
				{
					this._parent = parent;
					this._parent.ActionPanel.SetDropDownActive(true);
				}

				protected override void OnClosed(EventArgs e)
				{
					base.OnClosed(e);
					this._parent.ActionPanel.SetDropDownActive(false);
				}

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

				private DesignerActionPanel.EditorPropertyLine _parent;
			}

			private static class NativeMethods
			{
				public const int WM_ACTIVATE = 6;

				public const int WM_CANCELMODE = 31;

				public const int WM_MOUSEACTIVATE = 33;

				public const int WM_NCLBUTTONDOWN = 161;

				public const int WM_NCRBUTTONDOWN = 164;

				public const int WM_NCMBUTTONDOWN = 167;

				public const int WM_LBUTTONDOWN = 513;

				public const int WM_RBUTTONDOWN = 516;

				public const int WM_MBUTTONDOWN = 519;

				public const int WA_INACTIVE = 0;

				public const int WA_ACTIVE = 1;

				public const int WS_EX_TOOLWINDOW = 128;

				public const int WS_POPUP = -2147483648;

				public const int WS_BORDER = 8388608;

				public const int GWL_HWNDPARENT = -8;

				public const int QS_KEY = 1;

				public const int QS_MOUSEMOVE = 2;

				public const int QS_MOUSEBUTTON = 4;

				public const int QS_POSTMESSAGE = 8;

				public const int QS_TIMER = 16;

				public const int QS_PAINT = 32;

				public const int QS_SENDMESSAGE = 64;

				public const int QS_HOTKEY = 128;

				public const int QS_ALLPOSTMESSAGE = 256;

				public const int QS_MOUSE = 6;

				public const int QS_INPUT = 7;

				public const int QS_ALLEVENTS = 191;

				public const int QS_ALLINPUT = 255;

				public const int CS_SAVEBITS = 2048;

				public const int MWMO_INPUTAVAILABLE = 4;

				internal static class Util
				{
					public static int LOWORD(int n)
					{
						return n & 65535;
					}
				}

				public static class CommonHandles
				{
					public static HandleCollector GdiHandleCollector = new HandleCollector("GDI", 500);

					public static HandleCollector HdcHandleCollector = new HandleCollector("HDC", 2);
				}

				[StructLayout(LayoutKind.Sequential)]
				public class SIZE
				{
					public int cx;

					public int cy;
				}

				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
				public struct TEXTMETRIC
				{
					public int tmHeight;

					public int tmAscent;

					public int tmDescent;

					public int tmInternalLeading;

					public int tmExternalLeading;

					public int tmAveCharWidth;

					public int tmMaxCharWidth;

					public int tmWeight;

					public int tmOverhang;

					public int tmDigitizedAspectX;

					public int tmDigitizedAspectY;

					public char tmFirstChar;

					public char tmLastChar;

					public char tmDefaultChar;

					public char tmBreakChar;

					public byte tmItalic;

					public byte tmUnderlined;

					public byte tmStruckOut;

					public byte tmPitchAndFamily;

					public byte tmCharSet;
				}

				public struct TEXTMETRICA
				{
					public int tmHeight;

					public int tmAscent;

					public int tmDescent;

					public int tmInternalLeading;

					public int tmExternalLeading;

					public int tmAveCharWidth;

					public int tmMaxCharWidth;

					public int tmWeight;

					public int tmOverhang;

					public int tmDigitizedAspectX;

					public int tmDigitizedAspectY;

					public byte tmFirstChar;

					public byte tmLastChar;

					public byte tmDefaultChar;

					public byte tmBreakChar;

					public byte tmItalic;

					public byte tmUnderlined;

					public byte tmStruckOut;

					public byte tmPitchAndFamily;

					public byte tmCharSet;
				}
			}

			private static class SafeNativeMethods
			{
				[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
				private static extern bool IntDeleteObject(HandleRef hObject);

				public static bool DeleteObject(HandleRef hObject)
				{
					DesignerActionPanel.EditorPropertyLine.NativeMethods.CommonHandles.GdiHandleCollector.Remove();
					return DesignerActionPanel.EditorPropertyLine.SafeNativeMethods.IntDeleteObject(hObject);
				}

				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern bool ReleaseCapture();

				[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
				public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

				[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
				public static extern int GetTextExtentPoint32(HandleRef hDC, string str, int len, [In] [Out] DesignerActionPanel.EditorPropertyLine.NativeMethods.SIZE size);

				[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
				public static extern int GetTextMetricsW(HandleRef hDC, [In] [Out] ref DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRIC lptm);

				[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
				public static extern int GetTextMetricsA(HandleRef hDC, [In] [Out] ref DesignerActionPanel.EditorPropertyLine.NativeMethods.TEXTMETRICA lptm);

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

			private static class UnsafeNativeMethods
			{
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr GetWindowLong(HandleRef hWnd, int nIndex);

				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr GetCapture();

				[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true)]
				private static extern IntPtr IntGetDC(HandleRef hWnd);

				public static IntPtr GetDC(HandleRef hWnd)
				{
					DesignerActionPanel.EditorPropertyLine.NativeMethods.CommonHandles.HdcHandleCollector.Add();
					return DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.IntGetDC(hWnd);
				}

				[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true)]
				private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

				public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
				{
					DesignerActionPanel.EditorPropertyLine.NativeMethods.CommonHandles.HdcHandleCollector.Remove();
					return DesignerActionPanel.EditorPropertyLine.UnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
				}
			}

			internal sealed class EditorButton : Button
			{
				protected override void OnMouseDown(MouseEventArgs e)
				{
					base.OnMouseDown(e);
					if (e.Button == MouseButtons.Left)
					{
						this._mouseDown = true;
					}
				}

				protected override void OnMouseEnter(EventArgs e)
				{
					base.OnMouseEnter(e);
					this._mouseOver = true;
				}

				protected override void OnMouseLeave(EventArgs e)
				{
					base.OnMouseLeave(e);
					this._mouseOver = false;
				}

				protected override void OnMouseUp(MouseEventArgs e)
				{
					base.OnMouseUp(e);
					if (e.Button == MouseButtons.Left)
					{
						this._mouseDown = false;
					}
				}

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

				public void ResetMouseStates()
				{
					this._mouseDown = false;
					this._mouseOver = false;
					base.Invalidate();
				}

				private bool _mouseOver;

				private bool _mouseDown;

				private bool _ellipsis;
			}
		}

		private class TextLine : DesignerActionPanel.Line
		{
			public TextLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
				actionPanel.FontChanged += this.OnParentControlFontChanged;
			}

			public sealed override string FocusId
			{
				get
				{
					return string.Empty;
				}
			}

			protected override void AddControls(List<Control> controls)
			{
				this._label = new Label();
				this._label.BackColor = Color.Transparent;
				this._label.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
				this._label.UseMnemonic = false;
				controls.Add(this._label);
			}

			public sealed override void Focus()
			{
			}

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

			private void OnParentControlFontChanged(object sender, EventArgs e)
			{
				if (this._label.Font != null)
				{
					this._label.Font = this.GetFont();
				}
			}

			protected virtual Font GetFont()
			{
				return base.ActionPanel.Font;
			}

			internal override void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex)
			{
				this._textItem = (DesignerActionTextItem)actionItem;
				this._label.Text = DesignerActionPanel.StripAmpersands(this._textItem.DisplayName);
				this._label.Font = this.GetFont();
				this._label.TabIndex = currentTabIndex++;
				toolTip.SetToolTip(this._label, this._textItem.Description);
			}

			private Label _label;

			private DesignerActionTextItem _textItem;
		}

		private sealed class HeaderLine : DesignerActionPanel.TextLine
		{
			public HeaderLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: base(serviceProvider, actionPanel)
			{
			}

			protected override Font GetFont()
			{
				return new Font(base.ActionPanel.Font, FontStyle.Bold);
			}
		}

		private sealed class SeparatorLine : DesignerActionPanel.Line
		{
			public SeparatorLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel)
				: this(serviceProvider, actionPanel, false)
			{
			}

			public SeparatorLine(IServiceProvider serviceProvider, DesignerActionPanel actionPanel, bool isSubSeparator)
				: base(serviceProvider, actionPanel)
			{
				this._isSubSeparator = isSubSeparator;
			}

			public sealed override string FocusId
			{
				get
				{
					return string.Empty;
				}
			}

			protected override void AddControls(List<Control> controls)
			{
			}

			public sealed override void Focus()
			{
			}

			public override Size LayoutControls(int top, int width, bool measureOnly)
			{
				return new Size(150, 1);
			}

			public override void PaintLine(Graphics g, int lineWidth, int lineHeight)
			{
				using (Pen pen = new Pen(base.ActionPanel.SeparatorColor))
				{
					g.DrawLine(pen, 3, 0, lineWidth - 4, 0);
				}
			}

			internal override void UpdateActionItem(DesignerActionList actionList, DesignerActionItem actionItem, ToolTip toolTip, ref int currentTabIndex)
			{
			}

			private bool _isSubSeparator;
		}
	}
}
