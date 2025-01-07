using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal class TableLayoutPanelBehavior : Behavior
	{
		internal TableLayoutPanelBehavior(TableLayoutPanel panel, TableLayoutPanelDesigner designer, IServiceProvider serviceProvider)
		{
			this.table = panel;
			this.designer = designer;
			this.serviceProvider = serviceProvider;
			this.behaviorService = serviceProvider.GetService(typeof(BehaviorService)) as BehaviorService;
			if (this.behaviorService == null)
			{
				return;
			}
			this.pushedBehavior = false;
			this.lastMouseLoc = Point.Empty;
		}

		private void FinishResize()
		{
			this.pushedBehavior = false;
			this.behaviorService.PopBehavior(this);
			this.lastMouseLoc = Point.Empty;
			this.styles = null;
			IComponentChangeService componentChangeService = this.serviceProvider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (componentChangeService != null && this.changedProp != null)
			{
				componentChangeService.OnComponentChanged(this.table, this.changedProp, null, null);
				this.changedProp = null;
			}
			SelectionManager selectionManager = this.serviceProvider.GetService(typeof(SelectionManager)) as SelectionManager;
			if (selectionManager != null)
			{
				selectionManager.Refresh();
			}
		}

		public override void OnLoseCapture(Glyph g, EventArgs e)
		{
			if (this.pushedBehavior)
			{
				this.FinishResize();
				if (this.resizeTransaction != null)
				{
					DesignerTransaction designerTransaction = this.resizeTransaction;
					this.resizeTransaction = null;
					using (designerTransaction)
					{
						designerTransaction.Cancel();
					}
				}
			}
		}

		public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (button == MouseButtons.Left && g is TableLayoutPanelResizeGlyph)
			{
				this.tableGlyph = g as TableLayoutPanelResizeGlyph;
				ISelectionService selectionService = this.serviceProvider.GetService(typeof(ISelectionService)) as ISelectionService;
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new object[] { this.designer.Component }, SelectionTypes.Click);
				}
				bool flag = this.tableGlyph.Type == TableLayoutPanelResizeGlyph.TableLayoutResizeType.Column;
				this.lastMouseLoc = mouseLoc;
				this.resizeProp = TypeDescriptor.GetProperties(this.tableGlyph.Style)[flag ? "Width" : "Height"];
				IComponentChangeService componentChangeService = this.serviceProvider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null)
				{
					this.changedProp = TypeDescriptor.GetProperties(this.table)[flag ? "ColumnStyles" : "RowStyles"];
					int[] array = (flag ? this.table.GetColumnWidths() : this.table.GetRowHeights());
					if (this.changedProp != null)
					{
						this.GetActiveStyleCollection(flag);
						if (this.styles != null && this.CanResizeStyle(array))
						{
							IDesignerHost designerHost = this.serviceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
							if (designerHost != null)
							{
								this.resizeTransaction = designerHost.CreateTransaction(SR.GetString("TableLayoutPanelRowColResize", new object[]
								{
									flag ? "Column" : "Row",
									this.designer.Control.Site.Name
								}));
							}
							try
							{
								int num = this.styles.IndexOf(this.tableGlyph.Style);
								this.rightStyle.index = this.IndexOfNextStealableStyle(true, num, array);
								this.rightStyle.style = (TableLayoutStyle)this.styles[this.rightStyle.index];
								this.rightStyle.styleProp = TypeDescriptor.GetProperties(this.rightStyle.style)[flag ? "Width" : "Height"];
								this.leftStyle.index = this.IndexOfNextStealableStyle(false, num, array);
								this.leftStyle.style = (TableLayoutStyle)this.styles[this.leftStyle.index];
								this.leftStyle.styleProp = TypeDescriptor.GetProperties(this.leftStyle.style)[flag ? "Width" : "Height"];
								componentChangeService.OnComponentChanging(this.table, this.changedProp);
								goto IL_02CD;
							}
							catch (CheckoutException ex)
							{
								if (CheckoutException.Canceled.Equals(ex) && this.resizeTransaction != null && !this.resizeTransaction.Canceled)
								{
									this.resizeTransaction.Cancel();
								}
								throw;
							}
						}
						return false;
					}
				}
				IL_02CD:
				this.behaviorService.PushCaptureBehavior(this);
				this.pushedBehavior = true;
			}
			return false;
		}

		private void GetActiveStyleCollection(bool isColumn)
		{
			if ((this.styles == null || isColumn != this.currentColumnStyles) && this.table != null)
			{
				this.styles = new ArrayList(this.changedProp.GetValue(this.table) as TableLayoutStyleCollection);
				this.currentColumnStyles = isColumn;
			}
		}

		private bool ColumnResize
		{
			get
			{
				bool flag = false;
				if (this.tableGlyph != null)
				{
					flag = this.tableGlyph.Type == TableLayoutPanelResizeGlyph.TableLayoutResizeType.Column;
				}
				return flag;
			}
		}

		private bool CanResizeStyle(int[] widths)
		{
			int num = ((IList)this.styles).IndexOf(this.tableGlyph.Style);
			if (num > -1 && num != this.styles.Count)
			{
				bool flag = this.IndexOfNextStealableStyle(true, num, widths) != -1;
				bool flag2 = this.IndexOfNextStealableStyle(false, num, widths) != -1;
				return flag && flag2;
			}
			return false;
		}

		private int IndexOfNextStealableStyle(bool forward, int startIndex, int[] widths)
		{
			int num = -1;
			if (this.styles != null)
			{
				if (forward)
				{
					for (int i = startIndex + 1; i < this.styles.Count; i++)
					{
						if (((TableLayoutStyle)this.styles[i]).SizeType != SizeType.AutoSize && widths[i] >= DesignerUtils.MINUMUMSTYLESIZEDRAG)
						{
							num = i;
							break;
						}
					}
				}
				else
				{
					for (int j = startIndex; j >= 0; j--)
					{
						if (((TableLayoutStyle)this.styles[j]).SizeType != SizeType.AutoSize && widths[j] >= DesignerUtils.MINUMUMSTYLESIZEDRAG)
						{
							num = j;
							break;
						}
					}
				}
			}
			return num;
		}

		public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (this.pushedBehavior)
			{
				bool columnResize = this.ColumnResize;
				this.GetActiveStyleCollection(columnResize);
				if (this.styles != null)
				{
					int index = this.rightStyle.index;
					int index2 = this.leftStyle.index;
					int num = (columnResize ? (mouseLoc.X - this.lastMouseLoc.X) : (mouseLoc.Y - this.lastMouseLoc.Y));
					if (columnResize && this.table.RightToLeft == RightToLeft.Yes)
					{
						num *= -1;
					}
					if (num == 0)
					{
						return false;
					}
					int[] array = (columnResize ? this.table.GetColumnWidths() : this.table.GetRowHeights());
					int[] array2 = array.Clone() as int[];
					array2[index] -= num;
					array2[index2] += num;
					if (array2[index] < DesignerUtils.MINUMUMSTYLESIZEDRAG || array2[index2] < DesignerUtils.MINUMUMSTYLESIZEDRAG)
					{
						return false;
					}
					this.table.SuspendLayout();
					int num2 = array[index];
					int num3 = array[index2];
					int num4 = 0;
					if (((TableLayoutStyle)this.styles[index]).SizeType == SizeType.Absolute && ((TableLayoutStyle)this.styles[index2]).SizeType == SizeType.Absolute)
					{
						float num5 = (float)array2[index];
						float num6 = (float)this.rightStyle.styleProp.GetValue(this.rightStyle.style);
						if (num6 != (float)array[index])
						{
							num5 = Math.Max(num6 - (float)num, (float)DesignerUtils.MINUMUMSTYLESIZEDRAG);
						}
						float num7 = (float)array2[index2];
						float num8 = (float)this.leftStyle.styleProp.GetValue(this.leftStyle.style);
						if (num8 != (float)array[index2])
						{
							num7 = Math.Max(num8 + (float)num, (float)DesignerUtils.MINUMUMSTYLESIZEDRAG);
						}
						this.rightStyle.styleProp.SetValue(this.rightStyle.style, num5);
						this.leftStyle.styleProp.SetValue(this.leftStyle.style, num7);
					}
					else if (((TableLayoutStyle)this.styles[index]).SizeType == SizeType.Percent && ((TableLayoutStyle)this.styles[index2]).SizeType == SizeType.Percent)
					{
						for (int i = 0; i < this.styles.Count; i++)
						{
							if (((TableLayoutStyle)this.styles[i]).SizeType == SizeType.Percent)
							{
								num4 += array[i];
							}
						}
						for (int j = 0; j < 2; j++)
						{
							int num9 = ((j == 0) ? index : index2);
							float num10 = (float)array2[num9] * 100f / (float)num4;
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.styles[num9])[columnResize ? "Width" : "Height"];
							if (propertyDescriptor != null)
							{
								propertyDescriptor.SetValue(this.styles[num9], num10);
							}
						}
					}
					else
					{
						int num11 = ((((TableLayoutStyle)this.styles[index]).SizeType == SizeType.Absolute) ? index : index2);
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(this.styles[num11])[columnResize ? "Width" : "Height"];
						if (propertyDescriptor2 != null)
						{
							float num12 = (float)array2[num11];
							float num13 = (float)propertyDescriptor2.GetValue(this.styles[num11]);
							if (num13 != (float)array[num11])
							{
								num12 = Math.Max((num11 == index) ? (num13 - (float)num) : (num13 + (float)num), (float)DesignerUtils.MINUMUMSTYLESIZEDRAG);
							}
							propertyDescriptor2.SetValue(this.styles[num11], num12);
						}
					}
					this.table.ResumeLayout(true);
					bool flag = true;
					int[] array3 = (columnResize ? this.table.GetColumnWidths() : this.table.GetRowHeights());
					for (int k = 0; k < array3.Length; k++)
					{
						if (array3[k] == array[k] && array2[k] != array[k])
						{
							flag = false;
						}
					}
					if (flag)
					{
						this.lastMouseLoc = mouseLoc;
					}
				}
				else
				{
					this.lastMouseLoc = mouseLoc;
				}
			}
			return false;
		}

		public override bool OnMouseUp(Glyph g, MouseButtons button)
		{
			if (this.pushedBehavior)
			{
				this.FinishResize();
				if (this.resizeTransaction != null)
				{
					DesignerTransaction designerTransaction = this.resizeTransaction;
					this.resizeTransaction = null;
					using (designerTransaction)
					{
						designerTransaction.Commit();
					}
					this.resizeProp = null;
				}
			}
			return false;
		}

		private TableLayoutPanelDesigner designer;

		private Point lastMouseLoc;

		private bool pushedBehavior;

		private BehaviorService behaviorService;

		private IServiceProvider serviceProvider;

		private TableLayoutPanelResizeGlyph tableGlyph;

		private DesignerTransaction resizeTransaction;

		private PropertyDescriptor resizeProp;

		private PropertyDescriptor changedProp;

		private TableLayoutPanel table;

		private TableLayoutPanelBehavior.StyleHelper rightStyle;

		private TableLayoutPanelBehavior.StyleHelper leftStyle;

		private ArrayList styles;

		private bool currentColumnStyles;

		private static readonly TraceSwitch tlpResizeSwitch;

		internal struct StyleHelper
		{
			public int index;

			public PropertyDescriptor styleProp;

			public TableLayoutStyle style;
		}
	}
}
