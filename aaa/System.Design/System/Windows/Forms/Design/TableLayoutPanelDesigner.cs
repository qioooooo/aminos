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
	// Token: 0x0200029F RID: 671
	internal class TableLayoutPanelDesigner : FlowPanelDesigner
	{
		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x000850F6 File Offset: 0x000840F6
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

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x00085123 File Offset: 0x00084123
		private TableLayoutColumnStyleCollection ColumnStyles
		{
			get
			{
				return this.Table.ColumnStyles;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x00085130 File Offset: 0x00084130
		private TableLayoutRowStyleCollection RowStyles
		{
			get
			{
				return this.Table.RowStyles;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x0008513D File Offset: 0x0008413D
		// (set) Token: 0x060018E7 RID: 6375 RVA: 0x0008514C File Offset: 0x0008414C
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

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x00085189 File Offset: 0x00084189
		// (set) Token: 0x060018E9 RID: 6377 RVA: 0x00085198 File Offset: 0x00084198
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

		// Token: 0x060018EA RID: 6378 RVA: 0x000851D8 File Offset: 0x000841D8
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

		// Token: 0x060018EB RID: 6379 RVA: 0x0008523C File Offset: 0x0008423C
		private bool ShouldSerializeColumnStyles()
		{
			return !this.IsLocalizable();
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x00085247 File Offset: 0x00084247
		private bool ShouldSerializeRowStyles()
		{
			return !this.IsLocalizable();
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060018ED RID: 6381 RVA: 0x00085252 File Offset: 0x00084252
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

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060018EE RID: 6382 RVA: 0x00085278 File Offset: 0x00084278
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

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x00085514 File Offset: 0x00084514
		private bool IsLoading
		{
			get
			{
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				return designerHost != null && designerHost.Loading;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x00085542 File Offset: 0x00084542
		internal TableLayoutPanel Table
		{
			get
			{
				return base.Component as TableLayoutPanel;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x00085550 File Offset: 0x00084550
		// (set) Token: 0x060018F2 RID: 6386 RVA: 0x000855D0 File Offset: 0x000845D0
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

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x000855DC File Offset: 0x000845DC
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

		// Token: 0x060018F4 RID: 6388 RVA: 0x000856E8 File Offset: 0x000846E8
		private void RefreshSmartTag()
		{
			DesignerActionUIService designerActionUIService = (DesignerActionUIService)this.GetService(typeof(DesignerActionUIService));
			if (designerActionUIService != null)
			{
				designerActionUIService.Refresh(base.Component);
			}
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x0008571C File Offset: 0x0008471C
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

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060018F6 RID: 6390 RVA: 0x00085799 File Offset: 0x00084799
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

		// Token: 0x060018F7 RID: 6391 RVA: 0x000857B0 File Offset: 0x000847B0
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

		// Token: 0x060018F8 RID: 6392 RVA: 0x00085A3D File Offset: 0x00084A3D
		private void BuildActionLists()
		{
			this.actionLists = new DesignerActionListCollection();
			this.actionLists.Add(new TableLayoutPanelDesigner.TableLayouPanelRowColumnActionList(this));
			this.actionLists[0].AutoShow = true;
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00085A70 File Offset: 0x00084A70
		private void RemoveControlInternal(Control c)
		{
			this.Table.ControlRemoved -= this.OnControlRemoved;
			this.Table.Controls.Remove(c);
			this.Table.ControlRemoved += this.OnControlRemoved;
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x00085ABC File Offset: 0x00084ABC
		private void AddControlInternal(Control c, int col, int row)
		{
			this.Table.ControlAdded -= this.OnControlAdded;
			this.Table.Controls.Add(c, col, row);
			this.Table.ControlAdded += this.OnControlAdded;
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00085B0C File Offset: 0x00084B0C
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

		// Token: 0x060018FC RID: 6396 RVA: 0x00085F00 File Offset: 0x00084F00
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

		// Token: 0x060018FD RID: 6397 RVA: 0x00085F78 File Offset: 0x00084F78
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

		// Token: 0x060018FE RID: 6398 RVA: 0x00086060 File Offset: 0x00085060
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

		// Token: 0x060018FF RID: 6399 RVA: 0x00086160 File Offset: 0x00085160
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

		// Token: 0x06001900 RID: 6400 RVA: 0x000862A0 File Offset: 0x000852A0
		internal void SuspendEnsureAvailableStyles()
		{
			this.ensureSuspendCount++;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x000862B0 File Offset: 0x000852B0
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

		// Token: 0x06001902 RID: 6402 RVA: 0x000862DC File Offset: 0x000852DC
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

		// Token: 0x06001903 RID: 6403 RVA: 0x00086424 File Offset: 0x00085424
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

		// Token: 0x06001904 RID: 6404 RVA: 0x00086464 File Offset: 0x00085464
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

		// Token: 0x06001905 RID: 6405 RVA: 0x0008662C File Offset: 0x0008562C
		private Point GetControlPosition(Control control)
		{
			TableLayoutPanelCellPosition positionFromControl = this.Table.GetPositionFromControl(control);
			if (positionFromControl.Row == -1 && positionFromControl.Column == -1)
			{
				return ControlDesigner.InvalidPoint;
			}
			return new Point(positionFromControl.Column, positionFromControl.Row);
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x00086674 File Offset: 0x00085674
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

		// Token: 0x06001907 RID: 6407 RVA: 0x000868E8 File Offset: 0x000858E8
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

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001908 RID: 6408 RVA: 0x00086A2B File Offset: 0x00085A2B
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

		// Token: 0x06001909 RID: 6409 RVA: 0x00086A53 File Offset: 0x00085A53
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			this.CreateEmptyTable();
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x00086A64 File Offset: 0x00085A64
		protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			this.rowCountBeforeAdd = Math.Max(0, this.Table.GetRowHeights().Length);
			this.colCountBeforeAdd = Math.Max(0, this.Table.GetColumnWidths().Length);
			return base.CreateToolCore(tool, x, y, width, height, hasLocation, hasSize);
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00086AB4 File Offset: 0x00085AB4
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

		// Token: 0x0600190C RID: 6412 RVA: 0x00086B88 File Offset: 0x00085B88
		private void OnControlRemoved(object sender, ControlEventArgs e)
		{
			if (e != null && e.Control != null)
			{
				this.Table.SetCellPosition(e.Control, new TableLayoutPanelCellPosition(-1, -1));
			}
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00086BB0 File Offset: 0x00085BB0
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

		// Token: 0x0600190E RID: 6414 RVA: 0x00086C6C File Offset: 0x00085C6C
		protected override void OnContextMenu(int x, int y)
		{
			Point cellPosition = this.GetCellPosition(new Point(x, y));
			this.curRow = cellPosition.Y;
			this.curCol = cellPosition.X;
			this.EnsureAvailableStyles();
			this.DesignerContextMenuStrip.Show(x, y);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00086CB8 File Offset: 0x00085CB8
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

		// Token: 0x06001910 RID: 6416 RVA: 0x00086CF9 File Offset: 0x00085CF9
		protected override void OnDragLeave(EventArgs e)
		{
			this.localDragControl = null;
			this.dragComps = null;
			base.OnDragLeave(e);
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00086D10 File Offset: 0x00085D10
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

		// Token: 0x06001912 RID: 6418 RVA: 0x00086E38 File Offset: 0x00085E38
		protected override void OnDragOver(DragEventArgs de)
		{
			if (!this.IsOverValidCell(true))
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			base.OnDragOver(de);
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001913 RID: 6419 RVA: 0x00086E54 File Offset: 0x00085E54
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

		// Token: 0x06001914 RID: 6420 RVA: 0x00086EF4 File Offset: 0x00085EF4
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

		// Token: 0x06001915 RID: 6421 RVA: 0x00086F40 File Offset: 0x00085F40
		private void OnComponentChanging(object sender, ComponentChangingEventArgs e)
		{
			Control control = e.Component as Control;
			if (control != null && control.Parent == base.Component && e.Member != null && this.DoesPropertyAffectPosition(e.Member))
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Controls"];
				this.compSvc.OnComponentChanging(base.Component, propertyDescriptor);
			}
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00086FA8 File Offset: 0x00085FA8
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

		// Token: 0x06001917 RID: 6423 RVA: 0x00087080 File Offset: 0x00086080
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

		// Token: 0x06001918 RID: 6424 RVA: 0x00087158 File Offset: 0x00086158
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

		// Token: 0x06001919 RID: 6425 RVA: 0x00087188 File Offset: 0x00086188
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

		// Token: 0x0600191A RID: 6426 RVA: 0x000871D4 File Offset: 0x000861D4
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

		// Token: 0x0600191B RID: 6427 RVA: 0x00087250 File Offset: 0x00086250
		protected override void OnMouseDragMove(int x, int y)
		{
			if (this.droppedCellPosition == ControlDesigner.InvalidPoint)
			{
				Cursor.Current = Cursors.No;
				return;
			}
			base.OnMouseDragMove(x, y);
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x00087277 File Offset: 0x00086277
		protected override void OnMouseDragEnd(bool cancel)
		{
			if (this.droppedCellPosition == ControlDesigner.InvalidPoint)
			{
				cancel = true;
			}
			base.OnMouseDragEnd(cancel);
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x00087298 File Offset: 0x00086298
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

		// Token: 0x0600191E RID: 6430 RVA: 0x000874D0 File Offset: 0x000864D0
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

		// Token: 0x0600191F RID: 6431 RVA: 0x000875C4 File Offset: 0x000865C4
		private void OnAddClick(object sender, EventArgs e)
		{
			this.OnAdd((bool)((ToolStripMenuItem)sender).Tag);
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x000875DC File Offset: 0x000865DC
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

		// Token: 0x06001921 RID: 6433 RVA: 0x0008771C File Offset: 0x0008671C
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

		// Token: 0x06001922 RID: 6434 RVA: 0x00087850 File Offset: 0x00086850
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

		// Token: 0x06001923 RID: 6435 RVA: 0x0008799C File Offset: 0x0008699C
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

		// Token: 0x06001924 RID: 6436 RVA: 0x00087AF0 File Offset: 0x00086AF0
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

		// Token: 0x06001925 RID: 6437 RVA: 0x00087BC4 File Offset: 0x00086BC4
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

		// Token: 0x06001926 RID: 6438 RVA: 0x00087DE8 File Offset: 0x00086DE8
		private void OnRemove(bool isRow)
		{
			this.OnRemoveInternal(isRow, isRow ? (this.Table.RowCount - 1) : (this.Table.ColumnCount - 1));
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x00087E10 File Offset: 0x00086E10
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

		// Token: 0x06001928 RID: 6440 RVA: 0x00087E80 File Offset: 0x00086E80
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

		// Token: 0x06001929 RID: 6441 RVA: 0x00088114 File Offset: 0x00087114
		private void OnAbsoluteClick(object sender, EventArgs e)
		{
			this.ChangeSizeType((bool)((ToolStripMenuItem)sender).Tag, SizeType.Absolute);
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0008812D File Offset: 0x0008712D
		private void OnPercentClick(object sender, EventArgs e)
		{
			this.ChangeSizeType((bool)((ToolStripMenuItem)sender).Tag, SizeType.Percent);
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x00088146 File Offset: 0x00087146
		private void OnAutoSizeClick(object sender, EventArgs e)
		{
			this.ChangeSizeType((bool)((ToolStripMenuItem)sender).Tag, SizeType.AutoSize);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x00088160 File Offset: 0x00087160
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

		// Token: 0x0600192D RID: 6445 RVA: 0x000881B8 File Offset: 0x000871B8
		private void OnVerbRemove(object sender, EventArgs e)
		{
			bool flag = ((DesignerVerb)sender).Text.Equals(SR.GetString("TableLayoutPanelDesignerRemoveRow"));
			this.OnRemove(flag);
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x000881E8 File Offset: 0x000871E8
		private void OnVerbAdd(object sender, EventArgs e)
		{
			bool flag = ((DesignerVerb)sender).Text.Equals(SR.GetString("TableLayoutPanelDesignerAddRow"));
			this.OnAdd(flag);
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00088217 File Offset: 0x00087217
		private void OnVerbEdit(object sender, EventArgs e)
		{
			this.OnEdit();
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x00088220 File Offset: 0x00087220
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

		// Token: 0x06001931 RID: 6449 RVA: 0x00088315 File Offset: 0x00087315
		private void Refresh()
		{
			base.BehaviorService.SyncSelection();
			if (this.Table != null)
			{
				this.Table.Invalidate(true);
			}
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00088336 File Offset: 0x00087336
		private void PropChanging(PropertyDescriptor prop)
		{
			if (this.compSvc != null && prop != null)
			{
				this.compSvc.OnComponentChanging(this.Table, prop);
			}
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00088355 File Offset: 0x00087355
		private void PropChanged(PropertyDescriptor prop)
		{
			if (this.compSvc != null && prop != null)
			{
				this.compSvc.OnComponentChanged(this.Table, prop, null, null);
			}
		}

		// Token: 0x04001474 RID: 5236
		private TableLayoutPanelBehavior tlpBehavior;

		// Token: 0x04001475 RID: 5237
		private Point droppedCellPosition = ControlDesigner.InvalidPoint;

		// Token: 0x04001476 RID: 5238
		private bool undoing;

		// Token: 0x04001477 RID: 5239
		private UndoEngine undoEngine;

		// Token: 0x04001478 RID: 5240
		private Control localDragControl;

		// Token: 0x04001479 RID: 5241
		private ArrayList dragComps;

		// Token: 0x0400147A RID: 5242
		private DesignerVerbCollection verbs;

		// Token: 0x0400147B RID: 5243
		private TableLayoutPanelDesigner.DesignerTableLayoutControlCollection controls;

		// Token: 0x0400147C RID: 5244
		private DesignerVerb removeRowVerb;

		// Token: 0x0400147D RID: 5245
		private DesignerVerb removeColVerb;

		// Token: 0x0400147E RID: 5246
		private DesignerActionListCollection actionLists;

		// Token: 0x0400147F RID: 5247
		private BaseContextMenuStrip designerContextMenuStrip;

		// Token: 0x04001480 RID: 5248
		private int curRow = -1;

		// Token: 0x04001481 RID: 5249
		private int curCol = -1;

		// Token: 0x04001482 RID: 5250
		private IComponentChangeService compSvc;

		// Token: 0x04001483 RID: 5251
		private PropertyDescriptor rowStyleProp;

		// Token: 0x04001484 RID: 5252
		private PropertyDescriptor colStyleProp;

		// Token: 0x04001485 RID: 5253
		private int rowCountBeforeAdd;

		// Token: 0x04001486 RID: 5254
		private int colCountBeforeAdd;

		// Token: 0x04001487 RID: 5255
		private ToolStripMenuItem contextMenuRow;

		// Token: 0x04001488 RID: 5256
		private ToolStripMenuItem contextMenuCol;

		// Token: 0x04001489 RID: 5257
		private int ensureSuspendCount;

		// Token: 0x0400148A RID: 5258
		private Dictionary<string, bool> extenderProperties;

		// Token: 0x020002A0 RID: 672
		private class TableLayouPanelRowColumnActionList : DesignerActionList
		{
			// Token: 0x06001935 RID: 6453 RVA: 0x00088397 File Offset: 0x00087397
			public TableLayouPanelRowColumnActionList(TableLayoutPanelDesigner owner)
				: base(owner.Component)
			{
				this.owner = owner;
			}

			// Token: 0x06001936 RID: 6454 RVA: 0x000883AC File Offset: 0x000873AC
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

			// Token: 0x06001937 RID: 6455 RVA: 0x00088477 File Offset: 0x00087477
			public void AddColumn()
			{
				this.owner.OnAdd(false);
			}

			// Token: 0x06001938 RID: 6456 RVA: 0x00088485 File Offset: 0x00087485
			public void AddRow()
			{
				this.owner.OnAdd(true);
			}

			// Token: 0x06001939 RID: 6457 RVA: 0x00088493 File Offset: 0x00087493
			public void RemoveColumn()
			{
				this.owner.OnRemove(false);
			}

			// Token: 0x0600193A RID: 6458 RVA: 0x000884A1 File Offset: 0x000874A1
			public void RemoveRow()
			{
				this.owner.OnRemove(true);
			}

			// Token: 0x0600193B RID: 6459 RVA: 0x000884AF File Offset: 0x000874AF
			public void EditRowAndCol()
			{
				this.owner.OnEdit();
			}

			// Token: 0x0400148B RID: 5259
			private TableLayoutPanelDesigner owner;
		}

		// Token: 0x020002A1 RID: 673
		[DesignerSerializer(typeof(TableLayoutPanelDesigner.DesignerTableLayoutControlCollectionCodeDomSerializer), typeof(CodeDomSerializer))]
		[ListBindable(false)]
		internal class DesignerTableLayoutControlCollection : TableLayoutControlCollection, IList, ICollection, IEnumerable
		{
			// Token: 0x0600193C RID: 6460 RVA: 0x000884BC File Offset: 0x000874BC
			public DesignerTableLayoutControlCollection(TableLayoutPanel owner)
				: base(owner)
			{
				this.realCollection = owner.Controls;
			}

			// Token: 0x17000450 RID: 1104
			// (get) Token: 0x0600193D RID: 6461 RVA: 0x000884D1 File Offset: 0x000874D1
			public override int Count
			{
				get
				{
					return this.realCollection.Count;
				}
			}

			// Token: 0x17000451 RID: 1105
			// (get) Token: 0x0600193E RID: 6462 RVA: 0x000884DE File Offset: 0x000874DE
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x0600193F RID: 6463 RVA: 0x000884E1 File Offset: 0x000874E1
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000453 RID: 1107
			// (get) Token: 0x06001940 RID: 6464 RVA: 0x000884E4 File Offset: 0x000874E4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000454 RID: 1108
			// (get) Token: 0x06001941 RID: 6465 RVA: 0x000884E7 File Offset: 0x000874E7
			public new bool IsReadOnly
			{
				get
				{
					return this.realCollection.IsReadOnly;
				}
			}

			// Token: 0x06001942 RID: 6466 RVA: 0x000884F4 File Offset: 0x000874F4
			int IList.Add(object control)
			{
				return ((IList)this.realCollection).Add(control);
			}

			// Token: 0x06001943 RID: 6467 RVA: 0x00088502 File Offset: 0x00087502
			public override void Add(Control c)
			{
				this.realCollection.Add(c);
			}

			// Token: 0x06001944 RID: 6468 RVA: 0x00088510 File Offset: 0x00087510
			public override void AddRange(Control[] controls)
			{
				this.realCollection.AddRange(controls);
			}

			// Token: 0x06001945 RID: 6469 RVA: 0x0008851E File Offset: 0x0008751E
			bool IList.Contains(object control)
			{
				return ((IList)this.realCollection).Contains(control);
			}

			// Token: 0x06001946 RID: 6470 RVA: 0x0008852C File Offset: 0x0008752C
			public new void CopyTo(Array dest, int index)
			{
				this.realCollection.CopyTo(dest, index);
			}

			// Token: 0x06001947 RID: 6471 RVA: 0x0008853B File Offset: 0x0008753B
			public override bool Equals(object other)
			{
				return this.realCollection.Equals(other);
			}

			// Token: 0x06001948 RID: 6472 RVA: 0x00088549 File Offset: 0x00087549
			public new IEnumerator GetEnumerator()
			{
				return this.realCollection.GetEnumerator();
			}

			// Token: 0x06001949 RID: 6473 RVA: 0x00088556 File Offset: 0x00087556
			public override int GetHashCode()
			{
				return this.realCollection.GetHashCode();
			}

			// Token: 0x0600194A RID: 6474 RVA: 0x00088563 File Offset: 0x00087563
			int IList.IndexOf(object control)
			{
				return ((IList)this.realCollection).IndexOf(control);
			}

			// Token: 0x0600194B RID: 6475 RVA: 0x00088571 File Offset: 0x00087571
			void IList.Insert(int index, object value)
			{
				((IList)this.realCollection).Insert(index, value);
			}

			// Token: 0x0600194C RID: 6476 RVA: 0x00088580 File Offset: 0x00087580
			void IList.Remove(object control)
			{
				((IList)this.realCollection).Remove(control);
			}

			// Token: 0x0600194D RID: 6477 RVA: 0x0008858E File Offset: 0x0008758E
			void IList.RemoveAt(int index)
			{
				((IList)this.realCollection).RemoveAt(index);
			}

			// Token: 0x17000455 RID: 1109
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

			// Token: 0x06001950 RID: 6480 RVA: 0x000885B1 File Offset: 0x000875B1
			public override void Add(Control control, int column, int row)
			{
				this.realCollection.Add(control, column, row);
			}

			// Token: 0x06001951 RID: 6481 RVA: 0x000885C1 File Offset: 0x000875C1
			public override int GetChildIndex(Control child, bool throwException)
			{
				return this.realCollection.GetChildIndex(child, throwException);
			}

			// Token: 0x06001952 RID: 6482 RVA: 0x000885D0 File Offset: 0x000875D0
			public override void SetChildIndex(Control child, int newIndex)
			{
				this.realCollection.SetChildIndex(child, newIndex);
			}

			// Token: 0x06001953 RID: 6483 RVA: 0x000885E0 File Offset: 0x000875E0
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

			// Token: 0x0400148C RID: 5260
			private TableLayoutControlCollection realCollection;
		}

		// Token: 0x020002A2 RID: 674
		internal class DesignerTableLayoutControlCollectionCodeDomSerializer : TableLayoutControlCollectionCodeDomSerializer
		{
			// Token: 0x06001954 RID: 6484 RVA: 0x00088650 File Offset: 0x00087650
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
