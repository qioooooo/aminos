using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class TableLayoutPanelDesigner : FlowPanelDesigner
	{
		private TableLayoutPanelBehavior Behavior
		{
			get
			{
				if (this.tlpBehavior == null)
				{
					this.tlpBehavior = new TableLayoutPanelBehavior(this.Table, this, base.Component.Site);
				}
				return this.tlpBehavior;
			}
		}

		private TableLayoutColumnStyleCollection ColumnStyles
		{
			get
			{
				return this.Table.ColumnStyles;
			}
		}

		private TableLayoutRowStyleCollection RowStyles
		{
			get
			{
				return this.Table.RowStyles;
			}
		}

		public int RowCount
		{
			get
			{
				return this.Table.RowCount;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException(SR.GetString("TableLayoutPanelDesignerInvalidColumnRowCount", new object[] { "RowCount" }));
				}
				this.Table.RowCount = value;
			}
		}

		public int ColumnCount
		{
			get
			{
				return this.Table.ColumnCount;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException(SR.GetString("TableLayoutPanelDesignerInvalidColumnRowCount", new object[] { "ColumnCount" }));
				}
				this.Table.ColumnCount = value;
			}
		}

		private bool IsLocalizable()
		{
			IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(designerHost.RootComponent)["Localizable"];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool))
				{
					return (bool)propertyDescriptor.GetValue(designerHost.RootComponent);
				}
			}
			return false;
		}

		private bool ShouldSerializeColumnStyles()
		{
			return !this.IsLocalizable();
		}

		private bool ShouldSerializeRowStyles()
		{
			return !this.IsLocalizable();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		private TableLayoutPanelDesigner.DesignerTableLayoutControlCollection Controls
		{
			get
			{
				if (this.controls == null)
				{
					this.controls = new TableLayoutPanelDesigner.DesignerTableLayoutControlCollection((TableLayoutPanel)this.Control);
				}
				return this.controls;
			}
		}

		private ContextMenuStrip DesignerContextMenuStrip
		{
			get
			{
				if (this.designerContextMenuStrip == null)
				{
					this.designerContextMenuStrip = new BaseContextMenuStrip(base.Component.Site, this.Table);
					ContextMenuStripGroup contextMenuStripGroup = this.designerContextMenuStrip.Groups["Verbs"];
					foreach (object obj in this.Verbs)
					{
						DesignerVerb designerVerb = (DesignerVerb)obj;
						if (!designerVerb.Text.Equals(SR.GetString("TableLayoutPanelDesignerEditRowAndCol")))
						{
							foreach (ToolStripItem toolStripItem in contextMenuStripGroup.Items)
							{
								if (toolStripItem.Text.Equals(designerVerb.Text))
								{
									contextMenuStripGroup.Items.Remove(toolStripItem);
									break;
								}
							}
						}
					}
					ToolStripDropDownMenu toolStripDropDownMenu = this.BuildMenu(true);
					ToolStripDropDownMenu toolStripDropDownMenu2 = this.BuildMenu(false);
					this.contextMenuRow = new ToolStripMenuItem();
					this.contextMenuRow.DropDown = toolStripDropDownMenu;
					this.contextMenuRow.Text = SR.GetString("TableLayoutPanelDesignerRowMenu");
					this.contextMenuCol = new ToolStripMenuItem();
					this.contextMenuCol.DropDown = toolStripDropDownMenu2;
					this.contextMenuCol.Text = SR.GetString("TableLayoutPanelDesignerColMenu");
					contextMenuStripGroup.Items.Insert(0, this.contextMenuCol);
					contextMenuStripGroup.Items.Insert(0, this.contextMenuRow);
					contextMenuStripGroup = this.designerContextMenuStrip.Groups["Edit"];
					foreach (ToolStripItem toolStripItem2 in contextMenuStripGroup.Items)
					{
						if (toolStripItem2.Text.Equals(SR.GetString("ContextMenuCut")))
						{
							toolStripItem2.Text = SR.GetString("TableLayoutPanelDesignerContextMenuCut");
						}
						else if (toolStripItem2.Text.Equals(SR.GetString("ContextMenuCopy")))
						{
							toolStripItem2.Text = SR.GetString("TableLayoutPanelDesignerContextMenuCopy");
						}
						else if (toolStripItem2.Text.Equals(SR.GetString("ContextMenuDelete")))
						{
							toolStripItem2.Text = SR.GetString("TableLayoutPanelDesignerContextMenuDelete");
						}
					}
				}
				bool flag = this.IsOverValidCell(false);
				this.contextMenuRow.Enabled = flag;
				this.contextMenuCol.Enabled = flag;
				return this.designerContextMenuStrip;
			}
		}

		private bool IsLoading
		{
			get
			{
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				return designerHost != null && designerHost.Loading;
			}
		}

		internal TableLayoutPanel Table
		{
			get
			{
				return base.Component as TableLayoutPanel;
			}
		}

		private bool Undoing
		{
			get
			{
				if (this.undoEngine == null)
				{
					this.undoEngine = this.GetService(typeof(UndoEngine)) as UndoEngine;
					if (this.undoEngine != null)
					{
						this.undoEngine.Undoing += this.OnUndoing;
						if (this.undoEngine.UndoInProgress)
						{
							this.undoing = true;
							this.undoEngine.Undone += this.OnUndone;
						}
					}
				}
				return this.undoing;
			}
			set
			{
				this.undoing = value;
			}
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.removeColVerb = new DesignerVerb(SR.GetString("TableLayoutPanelDesignerRemoveColumn"), new EventHandler(this.OnVerbRemove));
					this.removeRowVerb = new DesignerVerb(SR.GetString("TableLayoutPanelDesignerRemoveRow"), new EventHandler(this.OnVerbRemove));
					this.verbs = new DesignerVerbCollection();
					this.verbs.Add(new DesignerVerb(SR.GetString("TableLayoutPanelDesignerAddColumn"), new EventHandler(this.OnVerbAdd)));
					this.verbs.Add(new DesignerVerb(SR.GetString("TableLayoutPanelDesignerAddRow"), new EventHandler(this.OnVerbAdd)));
					this.verbs.Add(this.removeColVerb);
					this.verbs.Add(this.removeRowVerb);
					this.verbs.Add(new DesignerVerb(SR.GetString("TableLayoutPanelDesignerEditRowAndCol"), new EventHandler(this.OnVerbEdit)));
					this.CheckVerbStatus();
				}
				return this.verbs;
			}
		}

		private void RefreshSmartTag()
		{
			DesignerActionUIService designerActionUIService = (DesignerActionUIService)this.GetService(typeof(DesignerActionUIService));
			if (designerActionUIService != null)
			{
				designerActionUIService.Refresh(base.Component);
			}
		}

		private void CheckVerbStatus()
		{
			if (this.Table != null)
			{
				if (this.removeColVerb != null)
				{
					bool flag = this.Table.ColumnCount > 1;
					if (this.removeColVerb.Enabled != flag)
					{
						this.removeColVerb.Enabled = flag;
					}
				}
				if (this.removeRowVerb != null)
				{
					bool flag2 = this.Table.RowCount > 1;
					if (this.removeRowVerb.Enabled != flag2)
					{
						this.removeRowVerb.Enabled = flag2;
					}
				}
				this.RefreshSmartTag();
			}
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this.actionLists == null)
				{
					this.BuildActionLists();
				}
				return this.actionLists;
			}
		}

		private ToolStripDropDownMenu BuildMenu(bool isRow)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
			ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem();
			ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem();
			ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
			ToolStripLabel toolStripLabel = new ToolStripLabel();
			ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem();
			ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem();
			ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem();
			toolStripMenuItem.Text = SR.GetString("TableLayoutPanelDesignerAddMenu");
			toolStripMenuItem.Tag = isRow;
			toolStripMenuItem.Name = "add";
			toolStripMenuItem.Click += this.OnAddClick;
			toolStripMenuItem2.Text = SR.GetString("TableLayoutPanelDesignerInsertMenu");
			toolStripMenuItem2.Tag = isRow;
			toolStripMenuItem2.Name = "insert";
			toolStripMenuItem2.Click += this.OnInsertClick;
			toolStripMenuItem3.Text = SR.GetString("TableLayoutPanelDesignerDeleteMenu");
			toolStripMenuItem3.Tag = isRow;
			toolStripMenuItem3.Name = "delete";
			toolStripMenuItem3.Click += this.OnDeleteClick;
			toolStripLabel.Text = SR.GetString("TableLayoutPanelDesignerLabelMenu");
			if (SR.GetString("TableLayoutPanelDesignerDontBoldLabel") == "0")
			{
				toolStripLabel.Font = new Font(toolStripLabel.Font, FontStyle.Bold);
			}
			toolStripLabel.Name = "sizemode";
			toolStripMenuItem4.Text = SR.GetString("TableLayoutPanelDesignerAbsoluteMenu");
			toolStripMenuItem4.Tag = isRow;
			toolStripMenuItem4.Name = "absolute";
			toolStripMenuItem4.Click += this.OnAbsoluteClick;
			toolStripMenuItem5.Text = SR.GetString("TableLayoutPanelDesignerPercentageMenu");
			toolStripMenuItem5.Tag = isRow;
			toolStripMenuItem5.Name = "percent";
			toolStripMenuItem5.Click += this.OnPercentClick;
			toolStripMenuItem6.Text = SR.GetString("TableLayoutPanelDesignerAutoSizeMenu");
			toolStripMenuItem6.Tag = isRow;
			toolStripMenuItem6.Name = "autosize";
			toolStripMenuItem6.Click += this.OnAutoSizeClick;
			ToolStripDropDownMenu toolStripDropDownMenu = new ToolStripDropDownMenu();
			toolStripDropDownMenu.Items.AddRange(new ToolStripItem[] { toolStripMenuItem, toolStripMenuItem2, toolStripMenuItem3, toolStripSeparator, toolStripLabel, toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6 });
			toolStripDropDownMenu.Tag = isRow;
			toolStripDropDownMenu.Opening += this.OnRowColMenuOpening;
			IUIService iuiservice = this.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				toolStripDropDownMenu.Renderer = (ToolStripProfessionalRenderer)iuiservice.Styles["VsRenderer"];
			}
			return toolStripDropDownMenu;
		}

		private void BuildActionLists()
		{
			this.actionLists = new DesignerActionListCollection();
			this.actionLists.Add(new TableLayoutPanelDesigner.TableLayouPanelRowColumnActionList(this));
			this.actionLists[0].AutoShow = true;
		}

		private void RemoveControlInternal(Control c)
		{
			this.Table.ControlRemoved -= this.OnControlRemoved;
			this.Table.Controls.Remove(c);
			this.Table.ControlRemoved += this.OnControlRemoved;
		}

		private void AddControlInternal(Control c, int col, int row)
		{
			this.Table.ControlAdded -= this.OnControlAdded;
			this.Table.Controls.Add(c, col, row);
			this.Table.ControlAdded += this.OnControlAdded;
		}

		private void ControlAddedInternal(Control control, Point newControlPosition, bool localReposition, bool fullTable, DragEventArgs de)
		{
			if (fullTable)
			{
				if (this.Table.GrowStyle == TableLayoutPanelGrowStyle.AddRows)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Table)["RowCount"];
					if (propertyDescriptor != null)
					{
						propertyDescriptor.SetValue(this.Table, this.Table.GetRowHeights().Length);
					}
					newControlPosition.X = 0;
					newControlPosition.Y = this.Table.RowCount - 1;
				}
				else if (this.Table.GrowStyle == TableLayoutPanelGrowStyle.AddColumns)
				{
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(this.Table)["ColumnCount"];
					if (propertyDescriptor2 != null)
					{
						propertyDescriptor2.SetValue(this.Table, this.Table.GetColumnWidths().Length);
					}
					newControlPosition.X = this.Table.ColumnCount - 1;
					newControlPosition.Y = 0;
				}
			}
			DesignerTransaction designerTransaction = null;
			PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(this.Table)["Controls"];
			try
			{
				bool flag = de != null && de.Effect == DragDropEffects.Copy && localReposition;
				Control control2 = ((TableLayoutPanel)this.Control).GetControlFromPosition(newControlPosition.X, newControlPosition.Y);
				if (flag)
				{
					IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
					if (designerHost != null)
					{
						designerTransaction = designerHost.CreateTransaction(SR.GetString("BehaviorServiceCopyControl", new object[] { control.Site.Name }));
					}
					this.PropChanging(propertyDescriptor3);
				}
				else if (control2 != null && !control2.Equals(control))
				{
					if (localReposition)
					{
						IDesignerHost designerHost2 = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
						if (designerHost2 != null)
						{
							designerTransaction = designerHost2.CreateTransaction(SR.GetString("TableLayoutPanelDesignerControlsSwapped", new object[]
							{
								control.Site.Name,
								control2.Site.Name
							}));
						}
						this.PropChanging(propertyDescriptor3);
						this.RemoveControlInternal(control2);
					}
					else
					{
						this.PropChanging(propertyDescriptor3);
						control2 = null;
					}
				}
				else
				{
					if (localReposition)
					{
						IDesignerHost designerHost3 = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
						if (designerHost3 != null)
						{
							designerTransaction = designerHost3.CreateTransaction(SR.GetString("BehaviorServiceMoveControl", new object[] { control.Site.Name }));
						}
					}
					control2 = null;
					this.PropChanging(propertyDescriptor3);
				}
				if (flag)
				{
					ArrayList arrayList = DesignerUtils.CopyDragObjects(new ArrayList { control }, base.Component.Site) as ArrayList;
					control = arrayList[0] as Control;
				}
				if (localReposition)
				{
					Point controlPosition = this.GetControlPosition(control);
					if (controlPosition != ControlDesigner.InvalidPoint)
					{
						this.RemoveControlInternal(control);
						if (controlPosition != newControlPosition && control2 != null)
						{
							this.AddControlInternal(control2, controlPosition.X, controlPosition.Y);
						}
					}
				}
				if (localReposition)
				{
					this.AddControlInternal(control, newControlPosition.X, newControlPosition.Y);
				}
				else
				{
					this.Table.SetCellPosition(control, new TableLayoutPanelCellPosition(newControlPosition.X, newControlPosition.Y));
				}
				this.PropChanged(propertyDescriptor3);
				if (de != null)
				{
					base.OnDragComplete(de);
				}
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
					designerTransaction = null;
				}
				if (flag)
				{
					ISelectionService selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
					if (selectionService != null)
					{
						selectionService.SetSelectedComponents(new object[] { control }, SelectionTypes.Replace | SelectionTypes.Click);
					}
				}
			}
			catch (ArgumentException ex)
			{
				IUIService iuiservice = this.GetService(typeof(IUIService)) as IUIService;
				if (iuiservice != null)
				{
					iuiservice.ShowError(ex);
				}
			}
			catch (Exception ex2)
			{
				if (ClientUtils.IsCriticalException(ex2))
				{
					throw;
				}
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
				}
			}
		}

		private void CreateEmptyTable()
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Table)["ColumnCount"];
			if (propertyDescriptor != null)
			{
				propertyDescriptor.SetValue(this.Table, DesignerUtils.DEFAULTCOLUMNCOUNT);
			}
			PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(this.Table)["RowCount"];
			if (propertyDescriptor2 != null)
			{
				propertyDescriptor2.SetValue(this.Table, DesignerUtils.DEFAULTROWCOUNT);
			}
			this.EnsureAvailableStyles();
			this.InitializeNewStyles();
		}

		private void InitializeNewStyles()
		{
			Size size = this.Table.Size;
			this.Table.ColumnStyles[0].SizeType = SizeType.Percent;
			this.Table.ColumnStyles[0].Width = (float)DesignerUtils.MINIMUMSTYLEPERCENT;
			this.Table.ColumnStyles[1].SizeType = SizeType.Percent;
			this.Table.ColumnStyles[1].Width = (float)DesignerUtils.MINIMUMSTYLEPERCENT;
			this.Table.RowStyles[0].SizeType = SizeType.Percent;
			this.Table.RowStyles[0].Height = (float)DesignerUtils.MINIMUMSTYLEPERCENT;
			this.Table.RowStyles[1].SizeType = SizeType.Percent;
			this.Table.RowStyles[1].Height = (float)DesignerUtils.MINIMUMSTYLEPERCENT;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					designerHost.TransactionClosing -= this.OnTransactionClosing;
				}
				if (this.undoEngine != null)
				{
					if (this.Undoing)
					{
						this.undoEngine.Undone -= this.OnUndone;
					}
					this.undoEngine.Undoing -= this.OnUndoing;
				}
				if (this.compSvc != null)
				{
					this.compSvc.ComponentChanged -= this.OnComponentChanged;
					this.compSvc.ComponentChanging -= this.OnComponentChanging;
				}
				if (this.Table != null)
				{
					this.Table.ControlAdded -= this.OnControlAdded;
					this.Table.ControlRemoved -= this.OnControlRemoved;
				}
				this.rowStyleProp = null;
				this.colStyleProp = null;
			}
			base.Dispose(disposing);
		}

		protected override void DrawBorder(Graphics graphics)
		{
			if (this.Table.CellBorderStyle != TableLayoutPanelCellBorderStyle.None)
			{
				return;
			}
			base.DrawBorder(graphics);
			Rectangle displayRectangle = this.Control.DisplayRectangle;
			displayRectangle.Width--;
			displayRectangle.Height--;
			int[] columnWidths = this.Table.GetColumnWidths();
			int[] rowHeights = this.Table.GetRowHeights();
			using (Pen borderPen = base.BorderPen)
			{
				if (columnWidths.Length > 1)
				{
					bool flag = this.Table.RightToLeft == RightToLeft.Yes;
					int num = (flag ? displayRectangle.Right : displayRectangle.Left);
					for (int i = 0; i < columnWidths.Length - 1; i++)
					{
						if (flag)
						{
							num -= columnWidths[i];
						}
						else
						{
							num += columnWidths[i];
						}
						graphics.DrawLine(borderPen, num, displayRectangle.Top, num, displayRectangle.Bottom);
					}
				}
				if (rowHeights.Length > 1)
				{
					int num2 = displayRectangle.Top;
					for (int j = 0; j < rowHeights.Length - 1; j++)
					{
						num2 += rowHeights[j];
						graphics.DrawLine(borderPen, displayRectangle.Left, num2, displayRectangle.Right, num2);
					}
				}
			}
		}

		internal void SuspendEnsureAvailableStyles()
		{
			this.ensureSuspendCount++;
		}

		internal void ResumeEnsureAvailableStyles(bool performEnsure)
		{
			if (this.ensureSuspendCount > 0)
			{
				this.ensureSuspendCount--;
				if (this.ensureSuspendCount == 0 && performEnsure)
				{
					this.EnsureAvailableStyles();
				}
			}
		}

		private bool EnsureAvailableStyles()
		{
			if (this.IsLoading || this.Undoing || this.ensureSuspendCount > 0)
			{
				return false;
			}
			int[] columnWidths = this.Table.GetColumnWidths();
			int[] rowHeights = this.Table.GetRowHeights();
			this.Table.SuspendLayout();
			try
			{
				if (columnWidths.Length > this.Table.ColumnStyles.Count)
				{
					int num = columnWidths.Length - this.Table.ColumnStyles.Count;
					this.PropChanging(this.rowStyleProp);
					for (int i = 0; i < num; i++)
					{
						this.Table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, (float)DesignerUtils.MINIMUMSTYLESIZE));
					}
					this.PropChanged(this.rowStyleProp);
				}
				if (rowHeights.Length > this.Table.RowStyles.Count)
				{
					int num2 = rowHeights.Length - this.Table.RowStyles.Count;
					this.PropChanging(this.colStyleProp);
					for (int j = 0; j < num2; j++)
					{
						this.Table.RowStyles.Add(new RowStyle(SizeType.Absolute, (float)DesignerUtils.MINIMUMSTYLESIZE));
					}
					this.PropChanged(this.colStyleProp);
				}
			}
			finally
			{
				this.Table.ResumeLayout();
			}
			return true;
		}

		private Control ExtractControlFromDragEvent(DragEventArgs de)
		{
			DropSourceBehavior.BehaviorDataObject behaviorDataObject = de.Data as DropSourceBehavior.BehaviorDataObject;
			if (behaviorDataObject != null)
			{
				this.dragComps = new ArrayList(behaviorDataObject.DragComponents);
				return this.dragComps[0] as Control;
			}
			return null;
		}

		private Point GetCellPosition(Point pos)
		{
			int[] rowHeights = this.Table.GetRowHeights();
			int[] columnWidths = this.Table.GetColumnWidths();
			Point point = this.Table.PointToScreen(this.Table.DisplayRectangle.Location);
			Rectangle rectangle = new Rectangle(point, this.Table.DisplayRectangle.Size);
			Point point2 = new Point(-1, -1);
			bool flag = this.Table.RightToLeft == RightToLeft.Yes;
			int num = rectangle.X;
			if (flag)
			{
				if (pos.X <= rectangle.X)
				{
					point2.X = columnWidths.Length;
				}
				else if (pos.X < rectangle.Right)
				{
					num = rectangle.Right;
					for (int i = 0; i < columnWidths.Length; i++)
					{
						point2.X = i;
						if (pos.X >= num - columnWidths[i])
						{
							break;
						}
						num -= columnWidths[i];
					}
				}
			}
			else if (pos.X >= rectangle.Right)
			{
				point2.X = columnWidths.Length;
			}
			else if (pos.X > rectangle.X)
			{
				for (int j = 0; j < columnWidths.Length; j++)
				{
					point2.X = j;
					if (pos.X <= num + columnWidths[j])
					{
						break;
					}
					num += columnWidths[j];
				}
			}
			num = rectangle.Y;
			if (pos.Y >= rectangle.Bottom)
			{
				point2.Y = rowHeights.Length;
			}
			else if (pos.Y > rectangle.Y)
			{
				for (int k = 0; k < rowHeights.Length; k++)
				{
					if (pos.Y <= num + rowHeights[k])
					{
						point2.Y = k;
						break;
					}
					num += rowHeights[k];
				}
			}
			return point2;
		}

		private Point GetControlPosition(Control control)
		{
			TableLayoutPanelCellPosition positionFromControl = this.Table.GetPositionFromControl(control);
			if (positionFromControl.Row == -1 && positionFromControl.Column == -1)
			{
				return ControlDesigner.InvalidPoint;
			}
			return new Point(positionFromControl.Column, positionFromControl.Row);
		}

		public override GlyphCollection GetGlyphs(GlyphSelectionType selectionType)
		{
			GlyphCollection glyphs = base.GetGlyphs(selectionType);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Locked"];
			bool flag = propertyDescriptor != null && (bool)propertyDescriptor.GetValue(base.Component);
			bool flag2 = this.EnsureAvailableStyles();
			if (selectionType != GlyphSelectionType.NotSelected && !flag && this.InheritanceAttribute != InheritanceAttribute.InheritedReadOnly)
			{
				Point point = base.BehaviorService.MapAdornerWindowPoint(this.Table.Handle, this.Table.DisplayRectangle.Location);
				Rectangle rectangle = new Rectangle(point, this.Table.DisplayRectangle.Size);
				Point point2 = base.BehaviorService.ControlToAdornerWindow(this.Control);
				Rectangle rectangle2 = new Rectangle(point2, this.Control.ClientSize);
				int[] columnWidths = this.Table.GetColumnWidths();
				int[] rowHeights = this.Table.GetRowHeights();
				int num = DesignerUtils.RESIZEGLYPHSIZE / 2;
				bool flag3 = this.Table.RightToLeft == RightToLeft.Yes;
				int num2 = (flag3 ? rectangle.Right : rectangle.X);
				if (flag2)
				{
					for (int i = 0; i < columnWidths.Length - 1; i++)
					{
						if (columnWidths[i] != 0)
						{
							if (flag3)
							{
								num2 -= columnWidths[i];
							}
							else
							{
								num2 += columnWidths[i];
							}
							Rectangle rectangle3 = new Rectangle(num2 - num, rectangle2.Top, DesignerUtils.RESIZEGLYPHSIZE, rectangle2.Height);
							if (rectangle2.Contains(rectangle3) && this.Table.ColumnStyles[i] != null)
							{
								TableLayoutPanelResizeGlyph tableLayoutPanelResizeGlyph = new TableLayoutPanelResizeGlyph(rectangle3, this.Table.ColumnStyles[i], Cursors.VSplit, this.Behavior);
								glyphs.Add(tableLayoutPanelResizeGlyph);
							}
						}
					}
					num2 = rectangle.Y;
					for (int j = 0; j < rowHeights.Length - 1; j++)
					{
						if (rowHeights[j] != 0)
						{
							num2 += rowHeights[j];
							Rectangle rectangle4 = new Rectangle(rectangle2.Left, num2 - num, rectangle2.Width, DesignerUtils.RESIZEGLYPHSIZE);
							if (rectangle2.Contains(rectangle4) && this.Table.RowStyles[j] != null)
							{
								TableLayoutPanelResizeGlyph tableLayoutPanelResizeGlyph2 = new TableLayoutPanelResizeGlyph(rectangle4, this.Table.RowStyles[j], Cursors.HSplit, this.Behavior);
								glyphs.Add(tableLayoutPanelResizeGlyph2);
							}
						}
					}
				}
			}
			return glyphs;
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				designerHost.TransactionClosing += this.OnTransactionClosing;
				this.compSvc = designerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			}
			if (this.compSvc != null)
			{
				this.compSvc.ComponentChanging += this.OnComponentChanging;
				this.compSvc.ComponentChanged += this.OnComponentChanged;
			}
			this.Control.ControlAdded += this.OnControlAdded;
			this.Control.ControlRemoved += this.OnControlRemoved;
			this.rowStyleProp = TypeDescriptor.GetProperties(this.Table)["RowStyles"];
			this.colStyleProp = TypeDescriptor.GetProperties(this.Table)["ColumnStyles"];
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				for (int i = 0; i < this.Control.Controls.Count; i++)
				{
					TypeDescriptor.AddAttributes(this.Control.Controls[i], new Attribute[] { InheritanceAttribute.InheritedReadOnly });
				}
			}
		}

		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (base.InheritanceAttribute == InheritanceAttribute.Inherited || base.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
				{
					return InheritanceAttribute.InheritedReadOnly;
				}
				return base.InheritanceAttribute;
			}
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			this.CreateEmptyTable();
		}

		protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			this.rowCountBeforeAdd = Math.Max(0, this.Table.GetRowHeights().Length);
			this.colCountBeforeAdd = Math.Max(0, this.Table.GetColumnWidths().Length);
			return base.CreateToolCore(tool, x, y, width, height, hasLocation, hasSize);
		}

		private void OnControlAdded(object sender, ControlEventArgs e)
		{
			if (this.IsLoading || this.Undoing)
			{
				return;
			}
			int num = 0;
			int[] rowHeights = this.Table.GetRowHeights();
			int[] columnWidths = this.Table.GetColumnWidths();
			for (int i = 0; i < rowHeights.Length; i++)
			{
				for (int j = 0; j < columnWidths.Length; j++)
				{
					if (this.Table.GetControlFromPosition(j, i) != null)
					{
						num++;
					}
				}
			}
			bool flag = num - 1 >= Math.Max(1, this.colCountBeforeAdd) * Math.Max(1, this.rowCountBeforeAdd);
			if (this.droppedCellPosition == ControlDesigner.InvalidPoint)
			{
				this.droppedCellPosition = this.GetControlPosition(e.Control);
			}
			this.ControlAddedInternal(e.Control, this.droppedCellPosition, false, flag, null);
			this.droppedCellPosition = ControlDesigner.InvalidPoint;
		}

		private void OnControlRemoved(object sender, ControlEventArgs e)
		{
			if (e != null && e.Control != null)
			{
				this.Table.SetCellPosition(e.Control, new TableLayoutPanelCellPosition(-1, -1));
			}
		}

		private bool IsOverValidCell(bool dragOp)
		{
			Point cellPosition = this.GetCellPosition(Control.MousePosition);
			int[] rowHeights = this.Table.GetRowHeights();
			int[] columnWidths = this.Table.GetColumnWidths();
			if (cellPosition.Y < 0 || cellPosition.Y >= rowHeights.Length || cellPosition.X < 0 || cellPosition.X >= columnWidths.Length)
			{
				return false;
			}
			if (dragOp)
			{
				Control controlFromPosition = ((TableLayoutPanel)this.Control).GetControlFromPosition(cellPosition.X, cellPosition.Y);
				if ((controlFromPosition != null && this.localDragControl == null) || (this.localDragControl != null && this.dragComps.Count > 1) || (this.localDragControl != null && controlFromPosition != null && Control.ModifierKeys == Keys.Control))
				{
					return false;
				}
			}
			return true;
		}

		protected override void OnContextMenu(int x, int y)
		{
			Point cellPosition = this.GetCellPosition(new Point(x, y));
			this.curRow = cellPosition.Y;
			this.curCol = cellPosition.X;
			this.EnsureAvailableStyles();
			this.DesignerContextMenuStrip.Show(x, y);
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			base.OnDragEnter(de);
			if (this.localDragControl == null)
			{
				Control control = this.ExtractControlFromDragEvent(de);
				if (control != null && this.Table.Controls.Contains(control))
				{
					this.localDragControl = control;
				}
			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			this.localDragControl = null;
			this.dragComps = null;
			base.OnDragLeave(e);
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			this.droppedCellPosition = this.GetCellPosition(Control.MousePosition);
			if (this.localDragControl != null)
			{
				this.ControlAddedInternal(this.localDragControl, this.droppedCellPosition, true, false, de);
				this.localDragControl = null;
			}
			else
			{
				this.rowCountBeforeAdd = Math.Max(0, this.Table.GetRowHeights().Length);
				this.colCountBeforeAdd = Math.Max(0, this.Table.GetColumnWidths().Length);
				base.OnDragDrop(de);
				if (this.dragComps != null)
				{
					foreach (object obj in this.dragComps)
					{
						Control control = (Control)obj;
						if (control != null)
						{
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["ColumnSpan"];
							PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(control)["RowSpan"];
							if (propertyDescriptor != null)
							{
								propertyDescriptor.SetValue(control, 1);
							}
							if (propertyDescriptor2 != null)
							{
								propertyDescriptor2.SetValue(control, 1);
							}
						}
					}
				}
			}
			this.droppedCellPosition = ControlDesigner.InvalidPoint;
			this.dragComps = null;
		}

		protected override void OnDragOver(DragEventArgs de)
		{
			if (!this.IsOverValidCell(true))
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			base.OnDragOver(de);
		}

		private Dictionary<string, bool> ExtenderProperties
		{
			get
			{
				if (this.extenderProperties == null && base.Component != null)
				{
					this.extenderProperties = new Dictionary<string, bool>();
					AttributeCollection attributes = TypeDescriptor.GetAttributes(base.Component.GetType());
					foreach (object obj in attributes)
					{
						Attribute attribute = (Attribute)obj;
						ProvidePropertyAttribute providePropertyAttribute = attribute as ProvidePropertyAttribute;
						if (providePropertyAttribute != null)
						{
							this.extenderProperties[providePropertyAttribute.PropertyName] = true;
						}
					}
				}
				return this.extenderProperties;
			}
		}

		private bool DoesPropertyAffectPosition(MemberDescriptor member)
		{
			bool flag = false;
			DesignerSerializationVisibilityAttribute designerSerializationVisibilityAttribute = member.Attributes[typeof(DesignerSerializationVisibilityAttribute)] as DesignerSerializationVisibilityAttribute;
			if (designerSerializationVisibilityAttribute != null)
			{
				flag = designerSerializationVisibilityAttribute.Visibility == DesignerSerializationVisibility.Hidden && this.ExtenderProperties.ContainsKey(member.Name);
			}
			return flag;
		}

		private void OnComponentChanging(object sender, ComponentChangingEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && control.Parent == base.Component && e.Member != null && this.DoesPropertyAffectPosition(e.Member))
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Controls"];
				this.compSvc.OnComponentChanging(base.Component, propertyDescriptor);
			}
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Component != null)
			{
				Control control = e.Component as Control;
				if (control != null && control.Parent != null && control.Parent.Equals(this.Control) && e.Member != null && (e.Member.Name == "Row" || e.Member.Name == "Column"))
				{
					this.EnsureAvailableStyles();
				}
				if (control != null && control.Parent == base.Component && e.Member != null && this.DoesPropertyAffectPosition(e.Member))
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Controls"];
					this.compSvc.OnComponentChanged(base.Component, propertyDescriptor, null, null);
				}
			}
			this.CheckVerbStatus();
		}

		private void OnTransactionClosing(object sender, DesignerTransactionCloseEventArgs e)
		{
			ISelectionService selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
			if (selectionService != null && this.Table != null)
			{
				ICollection selectedComponents = selectionService.GetSelectedComponents();
				bool flag = false;
				foreach (object obj in selectedComponents)
				{
					Control control = obj as Control;
					if (control != null && control.Parent == this.Table)
					{
						flag = true;
						break;
					}
				}
				if (selectionService.GetComponentSelected(this.Table) || flag)
				{
					this.Table.SuspendLayout();
					this.EnsureAvailableStyles();
					this.Table.ResumeLayout(false);
					this.Table.PerformLayout();
				}
			}
		}

		private void OnUndoing(object sender, EventArgs e)
		{
			if (!this.Undoing)
			{
				if (this.undoEngine != null)
				{
					this.undoEngine.Undone += this.OnUndone;
				}
				this.Undoing = true;
			}
		}

		private void OnUndone(object sender, EventArgs e)
		{
			if (this.Undoing)
			{
				if (this.undoEngine != null)
				{
					this.undoEngine.Undone -= this.OnUndone;
				}
				this.Undoing = false;
				bool flag = this.EnsureAvailableStyles();
				if (flag)
				{
					this.Refresh();
				}
			}
		}

		protected override void OnMouseDragBegin(int x, int y)
		{
			if (this.IsOverValidCell(true))
			{
				IToolboxService toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
				if (toolboxService != null && toolboxService.GetSelectedToolboxItem((IDesignerHost)this.GetService(typeof(IDesignerHost))) != null)
				{
					this.droppedCellPosition = this.GetCellPosition(Control.MousePosition);
				}
			}
			else
			{
				this.droppedCellPosition = ControlDesigner.InvalidPoint;
				Cursor.Current = Cursors.No;
			}
			base.OnMouseDragBegin(x, y);
		}

		protected override void OnMouseDragMove(int x, int y)
		{
			if (this.droppedCellPosition == ControlDesigner.InvalidPoint)
			{
				Cursor.Current = Cursors.No;
				return;
			}
			base.OnMouseDragMove(x, y);
		}

		protected override void OnMouseDragEnd(bool cancel)
		{
			if (this.droppedCellPosition == ControlDesigner.InvalidPoint)
			{
				cancel = true;
			}
			base.OnMouseDragEnd(cancel);
		}

		private void OnRowColMenuOpening(object sender, CancelEventArgs e)
		{
			e.Cancel = false;
			ToolStripDropDownMenu toolStripDropDownMenu = sender as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null)
			{
				int num = 0;
				ISelectionService selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
				if (selectionService != null)
				{
					num = selectionService.SelectionCount;
				}
				bool flag = num == 1 && this.InheritanceAttribute != InheritanceAttribute.InheritedReadOnly;
				toolStripDropDownMenu.Items["add"].Enabled = flag;
				toolStripDropDownMenu.Items["insert"].Enabled = flag;
				toolStripDropDownMenu.Items["delete"].Enabled = flag;
				toolStripDropDownMenu.Items["sizemode"].Enabled = flag;
				toolStripDropDownMenu.Items["absolute"].Enabled = flag;
				toolStripDropDownMenu.Items["percent"].Enabled = flag;
				toolStripDropDownMenu.Items["autosize"].Enabled = flag;
				if (num == 1)
				{
					((ToolStripMenuItem)toolStripDropDownMenu.Items["absolute"]).Checked = false;
					((ToolStripMenuItem)toolStripDropDownMenu.Items["percent"]).Checked = false;
					((ToolStripMenuItem)toolStripDropDownMenu.Items["autosize"]).Checked = false;
					bool flag2 = (bool)toolStripDropDownMenu.Tag;
					switch (flag2 ? this.Table.RowStyles[this.curRow].SizeType : this.Table.ColumnStyles[this.curCol].SizeType)
					{
					case SizeType.AutoSize:
						((ToolStripMenuItem)toolStripDropDownMenu.Items["autosize"]).Checked = true;
						break;
					case SizeType.Absolute:
						((ToolStripMenuItem)toolStripDropDownMenu.Items["absolute"]).Checked = true;
						break;
					case SizeType.Percent:
						((ToolStripMenuItem)toolStripDropDownMenu.Items["percent"]).Checked = true;
						break;
					}
					if ((flag2 ? this.Table.RowCount : this.Table.ColumnCount) < 2)
					{
						toolStripDropDownMenu.Items["delete"].Enabled = false;
					}
				}
			}
		}

		private void OnAdd(bool isRow)
		{
			IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost != null && this.Table.Site != null)
			{
				using (DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString(isRow ? "TableLayoutPanelDesignerAddRowUndoUnit" : "TableLayoutPanelDesignerAddColumnUndoUnit", new object[] { this.Table.Site.Name })))
				{
					try
					{
						this.Table.SuspendLayout();
						this.InsertRowCol(isRow, isRow ? this.Table.RowCount : this.Table.ColumnCount);
						this.Table.ResumeLayout();
						designerTransaction.Commit();
					}
					catch (CheckoutException ex)
					{
						if (!CheckoutException.Canceled.Equals(ex))
						{
							throw;
						}
						if (designerTransaction != null)
						{
							designerTransaction.Cancel();
						}
					}
				}
			}
		}

		private void OnAddClick(object sender, EventArgs e)
		{
			this.OnAdd((bool)((ToolStripMenuItem)sender).Tag);
		}

		internal void InsertRowCol(bool isRow, int index)
		{
			try
			{
				if (isRow)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Table)["RowCount"];
					if (propertyDescriptor != null)
					{
						this.PropChanging(this.rowStyleProp);
						this.Table.RowStyles.Insert(index, new RowStyle(SizeType.Absolute, (float)DesignerUtils.MINIMUMSTYLESIZE));
						this.PropChanged(this.rowStyleProp);
						propertyDescriptor.SetValue(this.Table, this.Table.RowCount + 1);
					}
				}
				else
				{
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(this.Table)["ColumnCount"];
					if (propertyDescriptor2 != null)
					{
						this.PropChanging(this.colStyleProp);
						this.Table.ColumnStyles.Insert(index, new ColumnStyle(SizeType.Absolute, (float)DesignerUtils.MINIMUMSTYLESIZE));
						this.PropChanged(this.colStyleProp);
						propertyDescriptor2.SetValue(this.Table, this.Table.ColumnCount + 1);
					}
				}
			}
			catch (InvalidOperationException ex)
			{
				IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
				iuiservice.ShowError(ex.Message);
			}
			base.BehaviorService.Invalidate(base.BehaviorService.ControlRectInAdornerWindow(this.Control));
		}

		internal void FixUpControlsOnInsert(bool isRow, int index)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Table)["Controls"];
			this.PropChanging(propertyDescriptor);
			foreach (object obj in this.Table.Controls)
			{
				Control control = (Control)obj;
				int num = (isRow ? this.Table.GetRow(control) : this.Table.GetColumn(control));
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(control)[isRow ? "Row" : "Column"];
				PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(control)[isRow ? "RowSpan" : "ColumnSpan"];
				if (num != -1)
				{
					if (num >= index)
					{
						if (propertyDescriptor2 != null)
						{
							propertyDescriptor2.SetValue(control, num + 1);
						}
					}
					else
					{
						int num2 = (isRow ? this.Table.GetRowSpan(control) : this.Table.GetColumnSpan(control));
						if (num + num2 > index && propertyDescriptor3 != null)
						{
							propertyDescriptor3.SetValue(control, num2 + 1);
						}
					}
				}
			}
			this.PropChanged(propertyDescriptor);
		}

		private void OnInsertClick(object sender, EventArgs e)
		{
			IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost != null && this.Table.Site != null)
			{
				bool flag = (bool)((ToolStripMenuItem)sender).Tag;
				using (DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString(flag ? "TableLayoutPanelDesignerAddRowUndoUnit" : "TableLayoutPanelDesignerAddColumnUndoUnit", new object[] { this.Table.Site.Name })))
				{
					try
					{
						this.Table.SuspendLayout();
						this.InsertRowCol(flag, flag ? this.curRow : this.curCol);
						this.FixUpControlsOnInsert(flag, flag ? this.curRow : this.curCol);
						this.Table.ResumeLayout();
						designerTransaction.Commit();
					}
					catch (CheckoutException ex)
					{
						if (!CheckoutException.Canceled.Equals(ex))
						{
							throw;
						}
						if (designerTransaction != null)
						{
							designerTransaction.Cancel();
						}
					}
					catch (InvalidOperationException ex2)
					{
						IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
						iuiservice.ShowError(ex2.Message);
					}
				}
			}
		}

		internal void FixUpControlsOnDelete(bool isRow, int index, ArrayList deleteList)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Table)["Controls"];
			this.PropChanging(propertyDescriptor);
			foreach (object obj in this.Table.Controls)
			{
				Control control = (Control)obj;
				int num = (isRow ? this.Table.GetRow(control) : this.Table.GetColumn(control));
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(control)[isRow ? "Row" : "Column"];
				PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(control)[isRow ? "RowSpan" : "ColumnSpan"];
				if (num == index)
				{
					if (!deleteList.Contains(control))
					{
						deleteList.Add(control);
					}
				}
				else if (num != -1 && !deleteList.Contains(control))
				{
					if (num > index)
					{
						if (propertyDescriptor2 != null)
						{
							propertyDescriptor2.SetValue(control, num - 1);
						}
					}
					else
					{
						int num2 = (isRow ? this.Table.GetRowSpan(control) : this.Table.GetColumnSpan(control));
						if (num + num2 > index && propertyDescriptor3 != null)
						{
							propertyDescriptor3.SetValue(control, num2 - 1);
						}
					}
				}
			}
			this.PropChanged(propertyDescriptor);
		}

		internal void DeleteRowCol(bool isRow, int index)
		{
			if (isRow)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Table)["RowCount"];
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(this.Table, this.Table.RowCount - 1);
					this.PropChanging(this.rowStyleProp);
					this.Table.RowStyles.RemoveAt(index);
					this.PropChanged(this.rowStyleProp);
					return;
				}
			}
			else
			{
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(this.Table)["ColumnCount"];
				if (propertyDescriptor2 != null)
				{
					propertyDescriptor2.SetValue(this.Table, this.Table.ColumnCount - 1);
					this.PropChanging(this.colStyleProp);
					this.Table.ColumnStyles.RemoveAt(index);
					this.PropChanged(this.colStyleProp);
				}
			}
		}

		private void OnRemoveInternal(bool isRow, int index)
		{
			if ((isRow ? this.Table.RowCount : this.Table.ColumnCount) < 2)
			{
				return;
			}
			IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost != null && this.Table.Site != null)
			{
				using (DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString(isRow ? "TableLayoutPanelDesignerRemoveRowUndoUnit" : "TableLayoutPanelDesignerRemoveColumnUndoUnit", new object[] { this.Table.Site.Name })))
				{
					try
					{
						this.Table.SuspendLayout();
						ArrayList arrayList = new ArrayList();
						this.FixUpControlsOnDelete(isRow, index, arrayList);
						this.DeleteRowCol(isRow, index);
						if (arrayList.Count > 0)
						{
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.Table)["Controls"];
							this.PropChanging(propertyDescriptor);
							foreach (object obj in arrayList)
							{
								ArrayList arrayList2 = new ArrayList();
								DesignerUtils.GetAssociatedComponents((IComponent)obj, designerHost, arrayList2);
								foreach (object obj2 in arrayList2)
								{
									IComponent component = (IComponent)obj2;
									this.compSvc.OnComponentChanging(component, null);
								}
								designerHost.DestroyComponent(obj as Component);
							}
							this.PropChanged(propertyDescriptor);
						}
						this.Table.ResumeLayout();
						designerTransaction.Commit();
					}
					catch (CheckoutException ex)
					{
						if (!CheckoutException.Canceled.Equals(ex))
						{
							throw;
						}
						if (designerTransaction != null)
						{
							designerTransaction.Cancel();
						}
					}
				}
			}
		}

		private void OnRemove(bool isRow)
		{
			this.OnRemoveInternal(isRow, isRow ? (this.Table.RowCount - 1) : (this.Table.ColumnCount - 1));
		}

		private void OnDeleteClick(object sender, EventArgs e)
		{
			try
			{
				bool flag = (bool)((ToolStripMenuItem)sender).Tag;
				this.OnRemoveInternal(flag, flag ? this.curRow : this.curCol);
			}
			catch (InvalidOperationException ex)
			{
				IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
				iuiservice.ShowError(ex.Message);
			}
		}

		private void ChangeSizeType(bool isRow, SizeType newType)
		{
			try
			{
				TableLayoutStyleCollection tableLayoutStyleCollection;
				if (isRow)
				{
					tableLayoutStyleCollection = this.Table.RowStyles;
				}
				else
				{
					tableLayoutStyleCollection = this.Table.ColumnStyles;
				}
				int num = (isRow ? this.curRow : this.curCol);
				if (tableLayoutStyleCollection[num].SizeType != newType)
				{
					int[] rowHeights = this.Table.GetRowHeights();
					int[] columnWidths = this.Table.GetColumnWidths();
					if ((!isRow || rowHeights.Length >= num - 1) && (isRow || columnWidths.Length >= num - 1))
					{
						IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
						if (designerHost != null && this.Table.Site != null)
						{
							using (DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("TableLayoutPanelDesignerChangeSizeTypeUndoUnit", new object[] { this.Table.Site.Name })))
							{
								try
								{
									this.Table.SuspendLayout();
									this.PropChanging(isRow ? this.rowStyleProp : this.colStyleProp);
									switch (newType)
									{
									case SizeType.AutoSize:
										tableLayoutStyleCollection[num].SizeType = SizeType.AutoSize;
										break;
									case SizeType.Absolute:
										tableLayoutStyleCollection[num].SizeType = SizeType.Absolute;
										if (isRow)
										{
											this.Table.RowStyles[num].Height = (float)rowHeights[num];
										}
										else
										{
											this.Table.ColumnStyles[num].Width = (float)columnWidths[num];
										}
										break;
									case SizeType.Percent:
										tableLayoutStyleCollection[num].SizeType = SizeType.Percent;
										if (isRow)
										{
											this.Table.RowStyles[num].Height = (float)DesignerUtils.MINIMUMSTYLEPERCENT;
										}
										else
										{
											this.Table.ColumnStyles[num].Width = (float)DesignerUtils.MINIMUMSTYLEPERCENT;
										}
										break;
									}
									this.PropChanged(isRow ? this.rowStyleProp : this.colStyleProp);
									this.Table.ResumeLayout();
									designerTransaction.Commit();
								}
								catch (CheckoutException ex)
								{
									if (!CheckoutException.Canceled.Equals(ex))
									{
										throw;
									}
									if (designerTransaction != null)
									{
										designerTransaction.Cancel();
									}
								}
							}
						}
					}
				}
			}
			catch (InvalidOperationException ex2)
			{
				IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
				iuiservice.ShowError(ex2.Message);
			}
		}

		private void OnAbsoluteClick(object sender, EventArgs e)
		{
			this.ChangeSizeType((bool)((ToolStripMenuItem)sender).Tag, SizeType.Absolute);
		}

		private void OnPercentClick(object sender, EventArgs e)
		{
			this.ChangeSizeType((bool)((ToolStripMenuItem)sender).Tag, SizeType.Percent);
		}

		private void OnAutoSizeClick(object sender, EventArgs e)
		{
			this.ChangeSizeType((bool)((ToolStripMenuItem)sender).Tag, SizeType.AutoSize);
		}

		private void OnEdit()
		{
			try
			{
				EditorServiceContext.EditValue(this, this.Table, "ColumnStyles");
			}
			catch (InvalidOperationException ex)
			{
				IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
				iuiservice.ShowError(ex.Message);
			}
		}

		private void OnVerbRemove(object sender, EventArgs e)
		{
			bool flag = ((DesignerVerb)sender).Text.Equals(SR.GetString("TableLayoutPanelDesignerRemoveRow"));
			this.OnRemove(flag);
		}

		private void OnVerbAdd(object sender, EventArgs e)
		{
			bool flag = ((DesignerVerb)sender).Text.Equals(SR.GetString("TableLayoutPanelDesignerAddRow"));
			this.OnAdd(flag);
		}

		private void OnVerbEdit(object sender, EventArgs e)
		{
			this.OnEdit();
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "ColumnStyles", "RowStyles", "ColumnCount", "RowCount" };
			Attribute[] array2 = new Attribute[]
			{
				new BrowsableAttribute(true)
			};
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(TableLayoutPanelDesigner), propertyDescriptor, array2);
				}
			}
			PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties["Controls"];
			if (propertyDescriptor2 != null)
			{
				Attribute[] array3 = new Attribute[propertyDescriptor2.Attributes.Count];
				propertyDescriptor2.Attributes.CopyTo(array3, 0);
				properties["Controls"] = TypeDescriptor.CreateProperty(typeof(TableLayoutPanelDesigner), "Controls", typeof(TableLayoutPanelDesigner.DesignerTableLayoutControlCollection), array3);
			}
		}

		private void Refresh()
		{
			base.BehaviorService.SyncSelection();
			if (this.Table != null)
			{
				this.Table.Invalidate(true);
			}
		}

		private void PropChanging(PropertyDescriptor prop)
		{
			if (this.compSvc != null && prop != null)
			{
				this.compSvc.OnComponentChanging(this.Table, prop);
			}
		}

		private void PropChanged(PropertyDescriptor prop)
		{
			if (this.compSvc != null && prop != null)
			{
				this.compSvc.OnComponentChanged(this.Table, prop, null, null);
			}
		}

		private TableLayoutPanelBehavior tlpBehavior;

		private Point droppedCellPosition = ControlDesigner.InvalidPoint;

		private bool undoing;

		private UndoEngine undoEngine;

		private Control localDragControl;

		private ArrayList dragComps;

		private DesignerVerbCollection verbs;

		private TableLayoutPanelDesigner.DesignerTableLayoutControlCollection controls;

		private DesignerVerb removeRowVerb;

		private DesignerVerb removeColVerb;

		private DesignerActionListCollection actionLists;

		private BaseContextMenuStrip designerContextMenuStrip;

		private int curRow = -1;

		private int curCol = -1;

		private IComponentChangeService compSvc;

		private PropertyDescriptor rowStyleProp;

		private PropertyDescriptor colStyleProp;

		private int rowCountBeforeAdd;

		private int colCountBeforeAdd;

		private ToolStripMenuItem contextMenuRow;

		private ToolStripMenuItem contextMenuCol;

		private int ensureSuspendCount;

		private Dictionary<string, bool> extenderProperties;

		private class TableLayouPanelRowColumnActionList : DesignerActionList
		{
			public TableLayouPanelRowColumnActionList(TableLayoutPanelDesigner owner)
				: base(owner.Component)
			{
				this.owner = owner;
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "AddColumn", SR.GetString("TableLayoutPanelDesignerAddColumn"), false));
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "AddRow", SR.GetString("TableLayoutPanelDesignerAddRow"), false));
				if (this.owner.Table.ColumnCount > 1)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "RemoveColumn", SR.GetString("TableLayoutPanelDesignerRemoveColumn"), false));
				}
				if (this.owner.Table.RowCount > 1)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "RemoveRow", SR.GetString("TableLayoutPanelDesignerRemoveRow"), false));
				}
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditRowAndCol", SR.GetString("TableLayoutPanelDesignerEditRowAndCol"), false));
				return designerActionItemCollection;
			}

			public void AddColumn()
			{
				this.owner.OnAdd(false);
			}

			public void AddRow()
			{
				this.owner.OnAdd(true);
			}

			public void RemoveColumn()
			{
				this.owner.OnRemove(false);
			}

			public void RemoveRow()
			{
				this.owner.OnRemove(true);
			}

			public void EditRowAndCol()
			{
				this.owner.OnEdit();
			}

			private TableLayoutPanelDesigner owner;
		}

		[DesignerSerializer(typeof(TableLayoutPanelDesigner.DesignerTableLayoutControlCollectionCodeDomSerializer), typeof(CodeDomSerializer))]
		[ListBindable(false)]
		internal class DesignerTableLayoutControlCollection : TableLayoutControlCollection, IList, ICollection, IEnumerable
		{
			public DesignerTableLayoutControlCollection(TableLayoutPanel owner)
				: base(owner)
			{
				this.realCollection = owner.Controls;
			}

			public override int Count
			{
				get
				{
					return this.realCollection.Count;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			public new bool IsReadOnly
			{
				get
				{
					return this.realCollection.IsReadOnly;
				}
			}

			int IList.Add(object control)
			{
				return ((IList)this.realCollection).Add(control);
			}

			public override void Add(Control c)
			{
				this.realCollection.Add(c);
			}

			public override void AddRange(Control[] controls)
			{
				this.realCollection.AddRange(controls);
			}

			bool IList.Contains(object control)
			{
				return ((IList)this.realCollection).Contains(control);
			}

			public new void CopyTo(Array dest, int index)
			{
				this.realCollection.CopyTo(dest, index);
			}

			public override bool Equals(object other)
			{
				return this.realCollection.Equals(other);
			}

			public new IEnumerator GetEnumerator()
			{
				return this.realCollection.GetEnumerator();
			}

			public override int GetHashCode()
			{
				return this.realCollection.GetHashCode();
			}

			int IList.IndexOf(object control)
			{
				return ((IList)this.realCollection).IndexOf(control);
			}

			void IList.Insert(int index, object value)
			{
				((IList)this.realCollection).Insert(index, value);
			}

			void IList.Remove(object control)
			{
				((IList)this.realCollection).Remove(control);
			}

			void IList.RemoveAt(int index)
			{
				((IList)this.realCollection).RemoveAt(index);
			}

			object IList.this[int index]
			{
				get
				{
					return ((IList)this.realCollection)[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			public override void Add(Control control, int column, int row)
			{
				this.realCollection.Add(control, column, row);
			}

			public override int GetChildIndex(Control child, bool throwException)
			{
				return this.realCollection.GetChildIndex(child, throwException);
			}

			public override void SetChildIndex(Control child, int newIndex)
			{
				this.realCollection.SetChildIndex(child, newIndex);
			}

			public override void Clear()
			{
				for (int i = this.realCollection.Count - 1; i >= 0; i--)
				{
					if (this.realCollection[i] != null && this.realCollection[i].Site != null && TypeDescriptor.GetAttributes(this.realCollection[i]).Contains(InheritanceAttribute.NotInherited))
					{
						this.realCollection.RemoveAt(i);
					}
				}
			}

			private TableLayoutControlCollection realCollection;
		}

		internal class DesignerTableLayoutControlCollectionCodeDomSerializer : TableLayoutControlCollectionCodeDomSerializer
		{
			protected override object SerializeCollection(IDesignerSerializationManager manager, CodeExpression targetExpression, Type targetType, ICollection originalCollection, ICollection valuesToSerialize)
			{
				ArrayList arrayList = new ArrayList();
				if (valuesToSerialize != null && valuesToSerialize.Count > 0)
				{
					foreach (object obj in valuesToSerialize)
					{
						IComponent component = obj as IComponent;
						if (component != null && component.Site != null && !(component.Site is INestedSite))
						{
							arrayList.Add(component);
						}
					}
				}
				return base.SerializeCollection(manager, targetExpression, targetType, originalCollection, arrayList);
			}
		}
	}
}
