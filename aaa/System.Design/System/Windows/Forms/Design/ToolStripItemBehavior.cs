using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002C5 RID: 709
	internal class ToolStripItemBehavior : Behavior
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x00092937 File Offset: 0x00091937
		private Control DropSource
		{
			get
			{
				if (this.dropSource == null)
				{
					this.dropSource = new Control();
				}
				return this.dropSource;
			}
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x00092954 File Offset: 0x00091954
		private bool CommonParent(ToolStripItem oldSelection, ToolStripItem newSelection)
		{
			ToolStrip currentParent = oldSelection.GetCurrentParent();
			ToolStrip currentParent2 = newSelection.GetCurrentParent();
			return currentParent == currentParent2;
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x00092974 File Offset: 0x00091974
		private void ClearInsertionMark(ToolStripItem item)
		{
			if (ToolStripDesigner.LastCursorPosition != Point.Empty && ToolStripDesigner.LastCursorPosition == Cursor.Position)
			{
				return;
			}
			ToolStripKeyboardHandlingService keyBoardHandlingService = this.GetKeyBoardHandlingService(item);
			if (keyBoardHandlingService != null && keyBoardHandlingService.TemplateNodeActive)
			{
				return;
			}
			Rectangle rectangle = Rectangle.Empty;
			if (item != null && item.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)item.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					rectangle = ToolStripItemBehavior.GetPaintingBounds(designerHost, item);
					rectangle.Inflate(1, 1);
					Region region = new Region(rectangle);
					try
					{
						rectangle.Inflate(-2, -2);
						region.Exclude(rectangle);
						BehaviorService behaviorService = this.GetBehaviorService(item);
						if (behaviorService != null && rectangle != Rectangle.Empty)
						{
							behaviorService.Invalidate(region);
						}
					}
					finally
					{
						region.Dispose();
						region = null;
					}
				}
			}
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00092A50 File Offset: 0x00091A50
		private void EnterInSituMode(ToolStripItemGlyph glyph)
		{
			if (glyph.ItemDesigner != null && !glyph.ItemDesigner.IsEditorActive)
			{
				glyph.ItemDesigner.ShowEditNode(false);
			}
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x00092A74 File Offset: 0x00091A74
		private ISelectionService GetSelectionService(ToolStripItem item)
		{
			if (item.Site != null)
			{
				return (ISelectionService)item.Site.GetService(typeof(ISelectionService));
			}
			return null;
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00092AA8 File Offset: 0x00091AA8
		private BehaviorService GetBehaviorService(ToolStripItem item)
		{
			if (item.Site != null)
			{
				return (BehaviorService)item.Site.GetService(typeof(BehaviorService));
			}
			return null;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x00092ADC File Offset: 0x00091ADC
		private ToolStripKeyboardHandlingService GetKeyBoardHandlingService(ToolStripItem item)
		{
			if (item.Site != null)
			{
				return (ToolStripKeyboardHandlingService)item.Site.GetService(typeof(ToolStripKeyboardHandlingService));
			}
			return null;
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x00092B10 File Offset: 0x00091B10
		private static Rectangle GetPaintingBounds(IDesignerHost designerHost, ToolStripItem item)
		{
			Rectangle rectangle = Rectangle.Empty;
			ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(item) as ToolStripItemDesigner;
			if (toolStripItemDesigner != null)
			{
				rectangle = toolStripItemDesigner.GetGlyphBounds();
				ToolStripDesignerUtils.GetAdjustedBounds(item, ref rectangle);
				rectangle.Inflate(1, 1);
				rectangle.Width--;
				rectangle.Height--;
			}
			return rectangle;
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00092B6C File Offset: 0x00091B6C
		private bool MouseHandlerPresent(ToolStripItem item)
		{
			IMouseHandler mouseHandler = null;
			if (this.eventSvc == null)
			{
				this.eventSvc = (IEventHandlerService)item.Site.GetService(typeof(IEventHandlerService));
			}
			if (this.eventSvc != null)
			{
				mouseHandler = (IMouseHandler)this.eventSvc.GetHandler(typeof(IMouseHandler));
			}
			return mouseHandler != null;
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x00092BD0 File Offset: 0x00091BD0
		private void OnDoubleClickTimerTick(object sender, EventArgs e)
		{
			if (this._timer != null)
			{
				this._timer.Enabled = false;
				this._timer.Tick -= this.OnDoubleClickTimerTick;
				this._timer.Dispose();
				this._timer = null;
				if (this.selectedGlyph != null && this.selectedGlyph.Item is ToolStripMenuItem)
				{
					this.EnterInSituMode(this.selectedGlyph);
				}
			}
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x00092C40 File Offset: 0x00091C40
		public override bool OnMouseDoubleClick(Glyph g, MouseButtons button, Point mouseLoc)
		{
			if (this.mouseUpFired)
			{
				this.doubleClickFired = true;
			}
			return false;
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x00092C54 File Offset: 0x00091C54
		public override bool OnMouseUp(Glyph g, MouseButtons button)
		{
			ToolStripItemGlyph toolStripItemGlyph = g as ToolStripItemGlyph;
			ToolStripItem item = toolStripItemGlyph.Item;
			if (this.MouseHandlerPresent(item))
			{
				return false;
			}
			this.SetParentDesignerValuesForDragDrop(item, false, Point.Empty);
			if (this.doubleClickFired)
			{
				if (toolStripItemGlyph != null && button == MouseButtons.Left)
				{
					ISelectionService selectionService = this.GetSelectionService(item);
					if (selectionService == null)
					{
						return false;
					}
					ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
					if (toolStripItem == item)
					{
						if (this._timer != null)
						{
							this._timer.Enabled = false;
							this._timer.Tick -= this.OnDoubleClickTimerTick;
							this._timer.Dispose();
							this._timer = null;
						}
						if (toolStripItem != null)
						{
							ToolStripItemDesigner itemDesigner = toolStripItemGlyph.ItemDesigner;
							if (itemDesigner != null && itemDesigner.IsEditorActive)
							{
								return false;
							}
							itemDesigner.DoDefaultAction();
						}
						this.doubleClickFired = false;
						this.mouseUpFired = false;
					}
				}
			}
			else
			{
				this.mouseUpFired = true;
			}
			return false;
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x00092D38 File Offset: 0x00091D38
		public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
		{
			ToolStripItemGlyph toolStripItemGlyph = g as ToolStripItemGlyph;
			ToolStripItem item = toolStripItemGlyph.Item;
			ISelectionService selectionService = this.GetSelectionService(item);
			BehaviorService behaviorService = this.GetBehaviorService(item);
			ToolStripKeyboardHandlingService keyBoardHandlingService = this.GetKeyBoardHandlingService(item);
			if (button == MouseButtons.Left && keyBoardHandlingService != null && keyBoardHandlingService.TemplateNodeActive && keyBoardHandlingService.ActiveTemplateNode.IsSystemContextMenuDisplayed)
			{
				return false;
			}
			IDesignerHost designerHost = (IDesignerHost)item.Site.GetService(typeof(IDesignerHost));
			ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
			ICollection collection = null;
			if (selectionService != null)
			{
				collection = selectionService.GetSelectedComponents();
			}
			ArrayList arrayList = new ArrayList(collection);
			if (arrayList.Count == 0 && keyBoardHandlingService != null && keyBoardHandlingService.SelectedDesignerControl != null)
			{
				arrayList.Add(keyBoardHandlingService.SelectedDesignerControl);
			}
			if (keyBoardHandlingService != null)
			{
				keyBoardHandlingService.SelectedDesignerControl = null;
				if (keyBoardHandlingService.TemplateNodeActive)
				{
					keyBoardHandlingService.ActiveTemplateNode.CommitAndSelect();
					if (toolStripItem != null && toolStripItem == item)
					{
						selectionService.SetSelectedComponents(null, SelectionTypes.Replace);
					}
				}
			}
			if (selectionService == null || this.MouseHandlerPresent(item))
			{
				return false;
			}
			if (toolStripItemGlyph != null && button == MouseButtons.Left)
			{
				ToolStripItem toolStripItem2 = selectionService.PrimarySelection as ToolStripItem;
				this.SetParentDesignerValuesForDragDrop(item, true, mouseLoc);
				if (toolStripItem2 != null && toolStripItem2 == item)
				{
					if (toolStripItem2 != null)
					{
						ToolStripItemDesigner itemDesigner = toolStripItemGlyph.ItemDesigner;
						if (itemDesigner != null && itemDesigner.IsEditorActive)
						{
							return false;
						}
					}
					bool flag = (Control.ModifierKeys & (Keys.Shift | Keys.Control)) > Keys.None;
					if (flag)
					{
						selectionService.SetSelectedComponents(new IComponent[] { toolStripItem2 }, SelectionTypes.Remove);
						return false;
					}
					if (toolStripItem2 is ToolStripMenuItem)
					{
						this._timer = new Timer();
						this._timer.Interval = SystemInformation.DoubleClickTime;
						this._timer.Tick += this.OnDoubleClickTimerTick;
						this._timer.Enabled = true;
						this.selectedGlyph = toolStripItemGlyph;
					}
				}
				else
				{
					bool flag2 = (Control.ModifierKeys & Keys.Shift) > Keys.None;
					if (!selectionService.GetComponentSelected(item))
					{
						this.mouseUpFired = false;
						this.doubleClickFired = false;
						if (flag2 && toolStripItem2 != null && this.CommonParent(toolStripItem2, item))
						{
							ToolStrip toolStrip;
							if (item.IsOnOverflow)
							{
								toolStrip = item.Owner;
							}
							else
							{
								toolStrip = item.GetCurrentParent();
							}
							int num = Math.Min(toolStrip.Items.IndexOf(toolStripItem2), toolStrip.Items.IndexOf(item));
							int num2 = Math.Max(toolStrip.Items.IndexOf(toolStripItem2), toolStrip.Items.IndexOf(item));
							int num3 = num2 - num + 1;
							if (num3 == 2)
							{
								selectionService.SetSelectedComponents(new IComponent[] { item });
							}
							else
							{
								object[] array = new object[num3];
								int num4 = 0;
								for (int i = num; i <= num2; i++)
								{
									array[num4++] = toolStrip.Items[i];
								}
								selectionService.SetSelectedComponents(new IComponent[] { toolStrip }, SelectionTypes.Replace);
								ToolStripDesigner.shiftState = true;
								selectionService.SetSelectedComponents(array, SelectionTypes.Replace);
							}
						}
						else
						{
							if (item.IsOnDropDown && ToolStripDesigner.shiftState)
							{
								ToolStripDesigner.shiftState = false;
								if (behaviorService != null)
								{
									behaviorService.Invalidate(item.Owner.Bounds);
								}
							}
							selectionService.SetSelectedComponents(new IComponent[] { item }, SelectionTypes.Auto);
						}
						if (keyBoardHandlingService != null)
						{
							keyBoardHandlingService.ShiftPrimaryItem = item;
						}
					}
					else if (flag2 || (Control.ModifierKeys & Keys.Control) > Keys.None)
					{
						selectionService.SetSelectedComponents(new IComponent[] { item }, SelectionTypes.Remove);
					}
				}
			}
			if (toolStripItemGlyph != null && button == MouseButtons.Right && !selectionService.GetComponentSelected(item))
			{
				selectionService.SetSelectedComponents(new IComponent[] { item });
			}
			ToolStripDesignerUtils.InvalidateSelection(arrayList, item, item.Site, false);
			return false;
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x000930EC File Offset: 0x000920EC
		public override bool OnMouseEnter(Glyph g)
		{
			ToolStripItemGlyph toolStripItemGlyph = g as ToolStripItemGlyph;
			if (toolStripItemGlyph != null)
			{
				ToolStripItem item = toolStripItemGlyph.Item;
				if (this.MouseHandlerPresent(item))
				{
					return false;
				}
				ISelectionService selectionService = this.GetSelectionService(item);
				if (selectionService != null && !selectionService.GetComponentSelected(item))
				{
					this.PaintInsertionMark(item);
				}
			}
			return false;
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x00093134 File Offset: 0x00092134
		public override bool OnMouseLeave(Glyph g)
		{
			ToolStripItemGlyph toolStripItemGlyph = g as ToolStripItemGlyph;
			if (toolStripItemGlyph != null)
			{
				ToolStripItem item = toolStripItemGlyph.Item;
				if (this.MouseHandlerPresent(item))
				{
					return false;
				}
				ISelectionService selectionService = this.GetSelectionService(item);
				if (selectionService != null && !selectionService.GetComponentSelected(item))
				{
					this.ClearInsertionMark(item);
				}
			}
			return false;
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x0009317C File Offset: 0x0009217C
		public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
		{
			bool flag = false;
			ToolStripItemGlyph toolStripItemGlyph = g as ToolStripItemGlyph;
			ToolStripItem item = toolStripItemGlyph.Item;
			ISelectionService selectionService = this.GetSelectionService(item);
			if (selectionService == null || item.Site == null || this.MouseHandlerPresent(item))
			{
				return false;
			}
			if (!selectionService.GetComponentSelected(item))
			{
				this.PaintInsertionMark(item);
				flag = false;
			}
			if (button == MouseButtons.Left && toolStripItemGlyph != null && toolStripItemGlyph.ItemDesigner != null && !toolStripItemGlyph.ItemDesigner.IsEditorActive)
			{
				Rectangle empty = Rectangle.Empty;
				IDesignerHost designerHost = (IDesignerHost)item.Site.GetService(typeof(IDesignerHost));
				if (item.Placement == ToolStripItemPlacement.Overflow || (item.Placement == ToolStripItemPlacement.Main && !item.IsOnDropDown))
				{
					ToolStripItemDesigner itemDesigner = toolStripItemGlyph.ItemDesigner;
					ToolStrip mainToolStrip = itemDesigner.GetMainToolStrip();
					ToolStripDesigner toolStripDesigner = designerHost.GetDesigner(mainToolStrip) as ToolStripDesigner;
					if (toolStripDesigner != null)
					{
						empty = toolStripDesigner.DragBoxFromMouseDown;
					}
				}
				else if (item.IsOnDropDown)
				{
					ToolStripDropDown toolStripDropDown = item.Owner as ToolStripDropDown;
					if (toolStripDropDown != null)
					{
						ToolStripItem ownerItem = toolStripDropDown.OwnerItem;
						ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(ownerItem) as ToolStripItemDesigner;
						if (toolStripItemDesigner != null)
						{
							empty = toolStripItemDesigner.dragBoxFromMouseDown;
						}
					}
				}
				if (empty != Rectangle.Empty && !empty.Contains(mouseLoc.X, mouseLoc.Y))
				{
					if (this._timer != null)
					{
						this._timer.Enabled = false;
						this._timer.Tick -= this.OnDoubleClickTimerTick;
						this._timer.Dispose();
						this._timer = null;
					}
					try
					{
						ArrayList arrayList = new ArrayList();
						ICollection selectedComponents = selectionService.GetSelectedComponents();
						foreach (object obj in selectedComponents)
						{
							IComponent component = (IComponent)obj;
							ToolStripItem toolStripItem = component as ToolStripItem;
							if (toolStripItem != null)
							{
								arrayList.Add(toolStripItem);
							}
						}
						ToolStripItem toolStripItem2 = selectionService.PrimarySelection as ToolStripItem;
						if (toolStripItem2 != null)
						{
							ToolStrip owner = toolStripItem2.Owner;
							ToolStripItemDataObject toolStripItemDataObject = new ToolStripItemDataObject(arrayList, toolStripItem2, owner);
							this.DropSource.QueryContinueDrag += this.QueryContinueDrag;
							ToolStripDropDownItem toolStripDropDownItem = item as ToolStripDropDownItem;
							if (toolStripDropDownItem != null)
							{
								ToolStripMenuItemDesigner toolStripMenuItemDesigner = designerHost.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
								if (toolStripMenuItemDesigner != null)
								{
									toolStripMenuItemDesigner.InitializeBodyGlyphsForItems(false, toolStripDropDownItem);
									toolStripDropDownItem.HideDropDown();
								}
							}
							else if (item.IsOnDropDown && !item.IsOnOverflow)
							{
								ToolStripDropDown toolStripDropDown2 = item.GetCurrentParent() as ToolStripDropDown;
								ToolStripDropDownItem toolStripDropDownItem2 = toolStripDropDown2.OwnerItem as ToolStripDropDownItem;
								selectionService.SetSelectedComponents(new IComponent[] { toolStripDropDownItem2 }, SelectionTypes.Replace);
							}
							this.DropSource.DoDragDrop(toolStripItemDataObject, DragDropEffects.All);
						}
					}
					finally
					{
						this.DropSource.QueryContinueDrag -= this.QueryContinueDrag;
						this.SetParentDesignerValuesForDragDrop(item, false, Point.Empty);
						ToolStripDesigner.dragItem = null;
						this.dropSource = null;
					}
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x0009349C File Offset: 0x0009249C
		public override void OnDragDrop(Glyph g, DragEventArgs e)
		{
			ToolStripItem dragItem = ToolStripDesigner.dragItem;
			if (e.Data is ToolStripItemDataObject && dragItem != null)
			{
				ToolStripItemDataObject toolStripItemDataObject = (ToolStripItemDataObject)e.Data;
				ToolStripItem toolStripItem = toolStripItemDataObject.PrimarySelection;
				IDesignerHost designerHost = (IDesignerHost)dragItem.Site.GetService(typeof(IDesignerHost));
				if (dragItem != toolStripItem && designerHost != null)
				{
					ArrayList arrayList = toolStripItemDataObject.DragComponents;
					ToolStrip toolStrip = dragItem.GetCurrentParent();
					int num = -1;
					bool flag = e.Effect == DragDropEffects.Copy;
					string text2;
					if (arrayList.Count == 1)
					{
						string text = TypeDescriptor.GetComponentName(arrayList[0]);
						if (text == null || text.Length == 0)
						{
							text = arrayList[0].GetType().Name;
						}
						text2 = SR.GetString(flag ? "BehaviorServiceCopyControl" : "BehaviorServiceMoveControl", new object[] { text });
					}
					else
					{
						text2 = SR.GetString(flag ? "BehaviorServiceCopyControls" : "BehaviorServiceMoveControls", new object[] { arrayList.Count });
					}
					DesignerTransaction designerTransaction = designerHost.CreateTransaction(text2);
					try
					{
						IComponentChangeService componentChangeService = (IComponentChangeService)dragItem.Site.GetService(typeof(IComponentChangeService));
						if (componentChangeService != null)
						{
							ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
							if (toolStripDropDown != null)
							{
								ToolStripItem ownerItem = toolStripDropDown.OwnerItem;
								componentChangeService.OnComponentChanging(ownerItem, TypeDescriptor.GetProperties(ownerItem)["DropDownItems"]);
							}
							else
							{
								componentChangeService.OnComponentChanging(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"]);
							}
						}
						if (flag)
						{
							if (toolStripItem != null)
							{
								num = arrayList.IndexOf(toolStripItem);
							}
							ToolStripKeyboardHandlingService keyBoardHandlingService = this.GetKeyBoardHandlingService(toolStripItem);
							if (keyBoardHandlingService != null)
							{
								keyBoardHandlingService.CopyInProgress = true;
							}
							arrayList = DesignerUtils.CopyDragObjects(arrayList, dragItem.Site) as ArrayList;
							if (keyBoardHandlingService != null)
							{
								keyBoardHandlingService.CopyInProgress = false;
							}
							if (num != -1)
							{
								toolStripItem = arrayList[num] as ToolStripItem;
							}
						}
						if (e.Effect == DragDropEffects.Move || flag)
						{
							ISelectionService selectionService = this.GetSelectionService(dragItem);
							if (selectionService != null)
							{
								if (toolStrip is ToolStripOverflow)
								{
									toolStrip = ((ToolStripOverflow)toolStrip).OwnerItem.Owner;
								}
								int num2 = toolStrip.Items.IndexOf(ToolStripDesigner.dragItem);
								if (num2 != -1)
								{
									int num3 = 0;
									if (toolStripItem != null)
									{
										num3 = toolStrip.Items.IndexOf(toolStripItem);
									}
									if (num3 != -1 && num2 > num3)
									{
										num2--;
									}
									foreach (object obj in arrayList)
									{
										ToolStripItem toolStripItem2 = (ToolStripItem)obj;
										toolStrip.Items.Insert(num2, toolStripItem2);
									}
								}
								selectionService.SetSelectedComponents(new IComponent[] { toolStripItem }, SelectionTypes.Replace | SelectionTypes.Click);
							}
						}
						if (componentChangeService != null)
						{
							ToolStripDropDown toolStripDropDown2 = toolStrip as ToolStripDropDown;
							if (toolStripDropDown2 != null)
							{
								ToolStripItem ownerItem2 = toolStripDropDown2.OwnerItem;
								componentChangeService.OnComponentChanged(ownerItem2, TypeDescriptor.GetProperties(ownerItem2)["DropDownItems"], null, null);
							}
							else
							{
								componentChangeService.OnComponentChanged(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"], null, null);
							}
							if (flag)
							{
								if (toolStripDropDown2 != null)
								{
									ToolStripItem ownerItem3 = toolStripDropDown2.OwnerItem;
									componentChangeService.OnComponentChanging(ownerItem3, TypeDescriptor.GetProperties(ownerItem3)["DropDownItems"]);
									componentChangeService.OnComponentChanged(ownerItem3, TypeDescriptor.GetProperties(ownerItem3)["DropDownItems"], null, null);
								}
								else
								{
									componentChangeService.OnComponentChanging(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"]);
									componentChangeService.OnComponentChanged(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"], null, null);
								}
							}
						}
						foreach (object obj2 in arrayList)
						{
							ToolStripItem toolStripItem3 = (ToolStripItem)obj2;
							if (toolStripItem3 is ToolStripDropDownItem)
							{
								ToolStripMenuItemDesigner toolStripMenuItemDesigner = designerHost.GetDesigner(toolStripItem3) as ToolStripMenuItemDesigner;
								if (toolStripMenuItemDesigner != null)
								{
									toolStripMenuItemDesigner.InitializeDropDown();
								}
							}
							ToolStripDropDown toolStripDropDown3 = toolStripItem3.GetCurrentParent() as ToolStripDropDown;
							if (toolStripDropDown3 != null && !(toolStripDropDown3 is ToolStripOverflow))
							{
								ToolStripDropDownItem toolStripDropDownItem = toolStripDropDown3.OwnerItem as ToolStripDropDownItem;
								if (toolStripDropDownItem != null)
								{
									ToolStripMenuItemDesigner toolStripMenuItemDesigner2 = designerHost.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
									if (toolStripMenuItemDesigner2 != null)
									{
										toolStripMenuItemDesigner2.InitializeBodyGlyphsForItems(false, toolStripDropDownItem);
										toolStripMenuItemDesigner2.InitializeBodyGlyphsForItems(true, toolStripDropDownItem);
									}
								}
							}
						}
						BehaviorService behaviorService = this.GetBehaviorService(dragItem);
						if (behaviorService != null)
						{
							behaviorService.SyncSelection();
						}
					}
					catch (Exception ex)
					{
						if (designerTransaction != null)
						{
							designerTransaction.Cancel();
							designerTransaction = null;
						}
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
					}
					finally
					{
						if (designerTransaction != null)
						{
							designerTransaction.Commit();
							designerTransaction = null;
						}
					}
				}
			}
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x00093994 File Offset: 0x00092994
		public override void OnDragEnter(Glyph g, DragEventArgs e)
		{
			ToolStripItemGlyph toolStripItemGlyph = g as ToolStripItemGlyph;
			ToolStripItem item = toolStripItemGlyph.Item;
			ToolStripItemDataObject toolStripItemDataObject = e.Data as ToolStripItemDataObject;
			if (toolStripItemDataObject == null)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			if (toolStripItemDataObject.Owner == item.Owner)
			{
				this.PaintInsertionMark(item);
				ToolStripDesigner.dragItem = item;
				e.Effect = DragDropEffects.Move;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x000939F0 File Offset: 0x000929F0
		public override void OnDragLeave(Glyph g, EventArgs e)
		{
			ToolStripItemGlyph toolStripItemGlyph = g as ToolStripItemGlyph;
			this.ClearInsertionMark(toolStripItemGlyph.Item);
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00093A10 File Offset: 0x00092A10
		public override void OnDragOver(Glyph g, DragEventArgs e)
		{
			ToolStripItemGlyph toolStripItemGlyph = g as ToolStripItemGlyph;
			ToolStripItem item = toolStripItemGlyph.Item;
			if (e.Data is ToolStripItemDataObject)
			{
				this.PaintInsertionMark(item);
				e.Effect = ((Control.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00093A60 File Offset: 0x00092A60
		private void PaintInsertionMark(ToolStripItem item)
		{
			if (ToolStripDesigner.LastCursorPosition != Point.Empty && ToolStripDesigner.LastCursorPosition == Cursor.Position)
			{
				return;
			}
			ToolStripKeyboardHandlingService keyBoardHandlingService = this.GetKeyBoardHandlingService(item);
			if (keyBoardHandlingService != null && keyBoardHandlingService.TemplateNodeActive)
			{
				return;
			}
			if (item != null && item.Site != null)
			{
				ToolStripDesigner.LastCursorPosition = Cursor.Position;
				IDesignerHost designerHost = (IDesignerHost)item.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					Rectangle paintingBounds = ToolStripItemBehavior.GetPaintingBounds(designerHost, item);
					BehaviorService behaviorService = this.GetBehaviorService(item);
					if (behaviorService != null)
					{
						Graphics adornerWindowGraphics = behaviorService.AdornerWindowGraphics;
						try
						{
							using (Pen pen = new Pen(new SolidBrush(Color.Black)))
							{
								pen.DashStyle = DashStyle.Dot;
								adornerWindowGraphics.DrawRectangle(pen, paintingBounds);
							}
						}
						finally
						{
							adornerWindowGraphics.Dispose();
						}
					}
				}
			}
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x00093B4C File Offset: 0x00092B4C
		private void QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (e.Action == DragAction.Continue)
			{
				return;
			}
			if (e.EscapePressed)
			{
				e.Action = DragAction.Cancel;
				ToolStripItem toolStripItem = sender as ToolStripItem;
				this.SetParentDesignerValuesForDragDrop(toolStripItem, false, Point.Empty);
				ISelectionService selectionService = this.GetSelectionService(toolStripItem);
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new IComponent[] { toolStripItem }, SelectionTypes.Auto);
				}
				ToolStripDesigner.dragItem = null;
			}
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x00093BAC File Offset: 0x00092BAC
		private void SetParentDesignerValuesForDragDrop(ToolStripItem glyphItem, bool setValues, Point mouseLoc)
		{
			if (glyphItem.Site == null)
			{
				return;
			}
			Size size = new Size(1, 1);
			IDesignerHost designerHost = (IDesignerHost)glyphItem.Site.GetService(typeof(IDesignerHost));
			if (glyphItem.Placement == ToolStripItemPlacement.Overflow || (glyphItem.Placement == ToolStripItemPlacement.Main && !glyphItem.IsOnDropDown))
			{
				ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(glyphItem) as ToolStripItemDesigner;
				ToolStrip mainToolStrip = toolStripItemDesigner.GetMainToolStrip();
				ToolStripDesigner toolStripDesigner = designerHost.GetDesigner(mainToolStrip) as ToolStripDesigner;
				if (toolStripDesigner != null)
				{
					if (setValues)
					{
						toolStripDesigner.IndexOfItemUnderMouseToDrag = mainToolStrip.Items.IndexOf(glyphItem);
						toolStripDesigner.DragBoxFromMouseDown = (this.dragBoxFromMouseDown = new Rectangle(new Point(mouseLoc.X - size.Width / 2, mouseLoc.Y - size.Height / 2), size));
						return;
					}
					toolStripDesigner.IndexOfItemUnderMouseToDrag = -1;
					toolStripDesigner.DragBoxFromMouseDown = (this.dragBoxFromMouseDown = Rectangle.Empty);
					return;
				}
			}
			else if (glyphItem.IsOnDropDown)
			{
				ToolStripDropDown toolStripDropDown = glyphItem.Owner as ToolStripDropDown;
				if (toolStripDropDown != null)
				{
					ToolStripItem ownerItem = toolStripDropDown.OwnerItem;
					ToolStripItemDesigner toolStripItemDesigner2 = designerHost.GetDesigner(ownerItem) as ToolStripItemDesigner;
					if (toolStripItemDesigner2 != null)
					{
						if (setValues)
						{
							toolStripItemDesigner2.indexOfItemUnderMouseToDrag = toolStripDropDown.Items.IndexOf(glyphItem);
							toolStripItemDesigner2.dragBoxFromMouseDown = (this.dragBoxFromMouseDown = new Rectangle(new Point(mouseLoc.X - size.Width / 2, mouseLoc.Y - size.Height / 2), size));
							return;
						}
						toolStripItemDesigner2.indexOfItemUnderMouseToDrag = -1;
						toolStripItemDesigner2.dragBoxFromMouseDown = (this.dragBoxFromMouseDown = Rectangle.Empty);
					}
				}
			}
		}

		// Token: 0x04001543 RID: 5443
		private const int GLYPHBORDER = 1;

		// Token: 0x04001544 RID: 5444
		private const int GLYPHINSET = 2;

		// Token: 0x04001545 RID: 5445
		internal Rectangle dragBoxFromMouseDown = Rectangle.Empty;

		// Token: 0x04001546 RID: 5446
		private Timer _timer;

		// Token: 0x04001547 RID: 5447
		private ToolStripItemGlyph selectedGlyph;

		// Token: 0x04001548 RID: 5448
		private bool doubleClickFired;

		// Token: 0x04001549 RID: 5449
		private bool mouseUpFired;

		// Token: 0x0400154A RID: 5450
		private Control dropSource;

		// Token: 0x0400154B RID: 5451
		private IEventHandlerService eventSvc;
	}
}
