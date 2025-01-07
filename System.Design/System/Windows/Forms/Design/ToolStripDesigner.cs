using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class ToolStripDesigner : ControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				if (this._actionLists == null)
				{
					this._actionLists = new ToolStripActionList(this);
				}
				designerActionListCollection.Add(this._actionLists);
				DesignerVerbCollection verbs = this.Verbs;
				if (verbs != null && verbs.Count != 0)
				{
					DesignerVerb[] array = new DesignerVerb[verbs.Count];
					verbs.CopyTo(array, 0);
					designerActionListCollection.Add(new DesignerActionVerbList(array));
				}
				return designerActionListCollection;
			}
		}

		private Rectangle AddItemRect
		{
			get
			{
				Rectangle rectangle = default(Rectangle);
				if (this._miniToolStrip == null)
				{
					return rectangle;
				}
				return this._miniToolStrip.Bounds;
			}
		}

		private bool AllowDrop
		{
			get
			{
				return (bool)base.ShadowProperties["AllowDrop"];
			}
			set
			{
				if (value && this.AllowItemReorder)
				{
					throw new ArgumentException(SR.GetString("ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue"));
				}
				base.ShadowProperties["AllowDrop"] = value;
			}
		}

		private bool AllowItemReorder
		{
			get
			{
				return (bool)base.ShadowProperties["AllowItemReorder"];
			}
			set
			{
				if (value && this.AllowDrop)
				{
					throw new ArgumentException(SR.GetString("ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue"));
				}
				base.ShadowProperties["AllowItemReorder"] = value;
			}
		}

		public override ICollection AssociatedComponents
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this.ToolStrip.Items)
				{
					ToolStripItem toolStripItem = (ToolStripItem)obj;
					if (!(toolStripItem is DesignerToolStripControlHost))
					{
						arrayList.Add(toolStripItem);
					}
				}
				return arrayList;
			}
		}

		public bool CacheItems
		{
			get
			{
				return this.cacheItems;
			}
			set
			{
				this.cacheItems = value;
			}
		}

		private bool CanAddItems
		{
			get
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(this.ToolStrip)[typeof(InheritanceAttribute)];
				return inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel == InheritanceLevel.NotInherited;
			}
		}

		internal override bool ControlSupportsSnaplines
		{
			get
			{
				return !(this.ToolStrip.Parent is ToolStripPanel);
			}
		}

		private ContextMenuStrip DesignerContextMenu
		{
			get
			{
				if (this.toolStripContextMenu == null)
				{
					this.toolStripContextMenu = new BaseContextMenuStrip(this.ToolStrip.Site, this.ToolStrip);
					this.toolStripContextMenu.Text = "CustomContextMenu";
				}
				return this.toolStripContextMenu;
			}
		}

		public bool DontCloseOverflow
		{
			get
			{
				return this.dontCloseOverflow;
			}
			set
			{
				this.dontCloseOverflow = value;
			}
		}

		public Rectangle DragBoxFromMouseDown
		{
			get
			{
				return this.dragBoxFromMouseDown;
			}
			set
			{
				this.dragBoxFromMouseDown = value;
			}
		}

		internal bool EditingCollection
		{
			get
			{
				return this._editingCollection != 0U;
			}
			set
			{
				if (value)
				{
					this._editingCollection += 1U;
					return;
				}
				this._editingCollection -= 1U;
			}
		}

		public ToolStripEditorManager EditManager
		{
			get
			{
				return this.editManager;
			}
		}

		internal ToolStripTemplateNode Editor
		{
			get
			{
				return this.tn;
			}
		}

		public DesignerToolStripControlHost EditorNode
		{
			get
			{
				return this.editorNode;
			}
		}

		internal ToolStrip EditorToolStrip
		{
			get
			{
				return this._miniToolStrip;
			}
			set
			{
				this._miniToolStrip = value;
				this._miniToolStrip.Parent = this.ToolStrip;
				this.LayoutToolStrip();
			}
		}

		public bool FireSyncSelection
		{
			get
			{
				return this.fireSyncSelection;
			}
			set
			{
				this.fireSyncSelection = value;
			}
		}

		public int IndexOfItemUnderMouseToDrag
		{
			get
			{
				return this.indexOfItemUnderMouseToDrag;
			}
			set
			{
				this.indexOfItemUnderMouseToDrag = value;
			}
		}

		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (base.InheritanceAttribute == InheritanceAttribute.Inherited)
				{
					return InheritanceAttribute.InheritedReadOnly;
				}
				return base.InheritanceAttribute;
			}
		}

		public DesignerTransaction InsertTansaction
		{
			get
			{
				return this._insertMenuItemTransaction;
			}
			set
			{
				this._insertMenuItemTransaction = value;
			}
		}

		private bool IsToolStripOrItemSelected
		{
			get
			{
				return this.toolStripSelected;
			}
		}

		public ArrayList Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ArrayList();
				}
				return this.items;
			}
		}

		public DesignerTransaction NewItemTransaction
		{
			get
			{
				return this.newItemTransaction;
			}
			set
			{
				this.newItemTransaction = value;
			}
		}

		private Rectangle OverFlowButtonRect
		{
			get
			{
				Rectangle rectangle = default(Rectangle);
				if (this.ToolStrip.OverflowButton.Visible)
				{
					return this.ToolStrip.OverflowButton.Bounds;
				}
				return rectangle;
			}
		}

		internal ISelectionService SelectionService
		{
			get
			{
				if (this._selectionSvc == null)
				{
					this._selectionSvc = (ISelectionService)this.GetService(typeof(ISelectionService));
				}
				return this._selectionSvc;
			}
		}

		public bool SupportEditing
		{
			get
			{
				WindowsFormsDesignerOptionService windowsFormsDesignerOptionService = this.GetService(typeof(DesignerOptionService)) as WindowsFormsDesignerOptionService;
				return windowsFormsDesignerOptionService == null || windowsFormsDesignerOptionService.CompatibilityOptions.EnableInSituEditing;
			}
		}

		protected ToolStrip ToolStrip
		{
			get
			{
				return (ToolStrip)base.Component;
			}
		}

		private ToolStripKeyboardHandlingService KeyboardHandlingService
		{
			get
			{
				if (this.keyboardHandlingService == null)
				{
					this.keyboardHandlingService = (ToolStripKeyboardHandlingService)this.GetService(typeof(ToolStripKeyboardHandlingService));
					if (this.keyboardHandlingService == null)
					{
						this.keyboardHandlingService = new ToolStripKeyboardHandlingService(base.Component.Site);
					}
				}
				return this.keyboardHandlingService;
			}
		}

		internal override bool SerializePerformLayout
		{
			get
			{
				return true;
			}
		}

		internal bool Visible
		{
			get
			{
				return this.currentVisible;
			}
			set
			{
				this.currentVisible = value;
				if (this.ToolStrip.Visible != value && !this.SelectionService.GetComponentSelected(this.ToolStrip))
				{
					this.Control.Visible = value;
				}
			}
		}

		private void AddBodyGlyphsForOverflow()
		{
			foreach (object obj in this.ToolStrip.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				if (!(toolStripItem is DesignerToolStripControlHost) && toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					this.AddItemBodyGlyph(toolStripItem);
				}
			}
		}

		private void AddItemBodyGlyph(ToolStripItem item)
		{
			if (item != null)
			{
				ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)this.host.GetDesigner(item);
				if (toolStripItemDesigner != null)
				{
					Rectangle glyphBounds = toolStripItemDesigner.GetGlyphBounds();
					Behavior behavior = new ToolStripItemBehavior();
					ToolStripItemGlyph toolStripItemGlyph = new ToolStripItemGlyph(item, toolStripItemDesigner, glyphBounds, behavior);
					toolStripItemDesigner.bodyGlyph = toolStripItemGlyph;
					if (this.toolStripAdornerWindowService != null)
					{
						this.toolStripAdornerWindowService.DropDownAdorner.Glyphs.Add(toolStripItemGlyph);
					}
				}
			}
		}

		private ToolStripItem AddNewItem(Type t)
		{
			this.NewItemTransaction = this.host.CreateTransaction(SR.GetString("ToolStripCreatingNewItemTransaction"));
			IComponent component = null;
			try
			{
				this._addingItem = true;
				this.ToolStrip.SuspendLayout();
				ToolStripItemDesigner toolStripItemDesigner = null;
				try
				{
					component = this.host.CreateComponent(t);
					toolStripItemDesigner = this.host.GetDesigner(component) as ToolStripItemDesigner;
					toolStripItemDesigner.InternalCreate = true;
					if (toolStripItemDesigner != null)
					{
						toolStripItemDesigner.InitializeNewComponent(null);
					}
				}
				finally
				{
					if (toolStripItemDesigner != null)
					{
						toolStripItemDesigner.InternalCreate = false;
					}
					this.ToolStrip.ResumeLayout();
				}
			}
			catch (Exception ex)
			{
				if (this.NewItemTransaction != null)
				{
					this.NewItemTransaction.Cancel();
					this.NewItemTransaction = null;
				}
				CheckoutException ex2 = ex as CheckoutException;
				if (ex2 == null || !ex2.Equals(CheckoutException.Canceled))
				{
					throw;
				}
			}
			finally
			{
				this._addingItem = false;
			}
			return component as ToolStripItem;
		}

		internal ToolStripItem AddNewItem(Type t, string text, bool enterKeyPressed, bool tabKeyPressed)
		{
			DesignerTransaction designerTransaction = this.host.CreateTransaction(SR.GetString("ToolStripAddingItem", new object[] { t.Name }));
			ToolStripItem toolStripItem = null;
			try
			{
				this._addingItem = true;
				this.ToolStrip.SuspendLayout();
				IComponent component = this.host.CreateComponent(t, ToolStripDesigner.NameFromText(text, t, base.Component.Site));
				ToolStripItemDesigner toolStripItemDesigner = this.host.GetDesigner(component) as ToolStripItemDesigner;
				try
				{
					if (!string.IsNullOrEmpty(text))
					{
						toolStripItemDesigner.InternalCreate = true;
					}
					if (toolStripItemDesigner != null)
					{
						toolStripItemDesigner.InitializeNewComponent(null);
					}
				}
				finally
				{
					toolStripItemDesigner.InternalCreate = false;
				}
				toolStripItem = component as ToolStripItem;
				if (toolStripItem != null)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(toolStripItem)["Text"];
					if (propertyDescriptor != null && !string.IsNullOrEmpty(text))
					{
						propertyDescriptor.SetValue(toolStripItem, text);
					}
					if (toolStripItem is ToolStripButton || toolStripItem is ToolStripSplitButton || toolStripItem is ToolStripDropDownButton)
					{
						Image image = null;
						try
						{
							image = new Bitmap(typeof(ToolStripButton), "blank.bmp");
						}
						catch (Exception ex)
						{
							if (ClientUtils.IsCriticalException(ex))
							{
								throw;
							}
						}
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(toolStripItem)["Image"];
						if (propertyDescriptor2 != null && image != null)
						{
							propertyDescriptor2.SetValue(toolStripItem, image);
						}
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(toolStripItem)["DisplayStyle"];
						if (propertyDescriptor3 != null)
						{
							propertyDescriptor3.SetValue(toolStripItem, ToolStripItemDisplayStyle.Image);
						}
						PropertyDescriptor propertyDescriptor4 = TypeDescriptor.GetProperties(toolStripItem)["ImageTransparentColor"];
						if (propertyDescriptor4 != null)
						{
							propertyDescriptor4.SetValue(toolStripItem, Color.Magenta);
						}
					}
				}
				this.ToolStrip.ResumeLayout();
				if (!tabKeyPressed)
				{
					if (enterKeyPressed)
					{
						if (!toolStripItemDesigner.SetSelection(enterKeyPressed) && this.KeyboardHandlingService != null)
						{
							this.KeyboardHandlingService.SelectedDesignerControl = this.editorNode;
							this.SelectionService.SetSelectedComponents(null, SelectionTypes.Replace);
						}
					}
					else
					{
						this.KeyboardHandlingService.SelectedDesignerControl = null;
						this.SelectionService.SetSelectedComponents(new IComponent[] { toolStripItem }, SelectionTypes.Replace);
						this.editorNode.RefreshSelectionGlyph();
					}
				}
				else if (this.keyboardHandlingService != null)
				{
					this.KeyboardHandlingService.SelectedDesignerControl = this.editorNode;
					this.SelectionService.SetSelectedComponents(null, SelectionTypes.Replace);
				}
				if (toolStripItemDesigner != null && toolStripItem.Placement != ToolStripItemPlacement.Overflow)
				{
					Rectangle glyphBounds = toolStripItemDesigner.GetGlyphBounds();
					SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
					Behavior behavior = new ToolStripItemBehavior();
					ToolStripItemGlyph toolStripItemGlyph = new ToolStripItemGlyph(toolStripItem, toolStripItemDesigner, glyphBounds, behavior);
					selectionManager.BodyGlyphAdorner.Glyphs.Insert(0, toolStripItemGlyph);
				}
				else if (toolStripItemDesigner != null && toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					this.RemoveBodyGlyphsForOverflow();
					this.AddBodyGlyphsForOverflow();
				}
			}
			catch (Exception ex2)
			{
				this.ToolStrip.ResumeLayout();
				if (this._pendingTransaction != null)
				{
					this._pendingTransaction.Cancel();
					this._pendingTransaction = null;
				}
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
				CheckoutException ex3 = ex2 as CheckoutException;
				if (ex3 != null && ex3 != CheckoutException.Canceled)
				{
					throw;
				}
			}
			finally
			{
				if (this._pendingTransaction != null)
				{
					this._pendingTransaction.Cancel();
					this._pendingTransaction = null;
					if (designerTransaction != null)
					{
						designerTransaction.Cancel();
					}
				}
				else if (designerTransaction != null)
				{
					designerTransaction.Commit();
					designerTransaction = null;
				}
				this._addingItem = false;
			}
			return toolStripItem;
		}

		internal void AddNewTemplateNode(ToolStrip wb)
		{
			this.tn = new ToolStripTemplateNode(base.Component, SR.GetString("ToolStripDesignerTemplateNodeEnterText"), null);
			this._miniToolStrip = this.tn.EditorToolStrip;
			int width = this.tn.EditorToolStrip.Width;
			this.editorNode = new DesignerToolStripControlHost(this.tn.EditorToolStrip);
			this.tn.ControlHost = this.editorNode;
			this.editorNode.Width = width;
			this.ToolStrip.Items.Add(this.editorNode);
			this.editorNode.Visible = false;
		}

		internal void CancelPendingMenuItemTransaction()
		{
			if (this._insertMenuItemTransaction != null)
			{
				this._insertMenuItemTransaction.Cancel();
			}
		}

		private bool CheckIfItemSelected()
		{
			bool flag = false;
			object obj = this.SelectionService.PrimarySelection;
			if (obj == null)
			{
				obj = (IComponent)this.KeyboardHandlingService.SelectedDesignerControl;
			}
			ToolStripItem toolStripItem = obj as ToolStripItem;
			if (toolStripItem != null)
			{
				if (toolStripItem.Placement == ToolStripItemPlacement.Overflow && toolStripItem.Owner == this.ToolStrip)
				{
					if (this.ToolStrip.CanOverflow && !this.ToolStrip.OverflowButton.DropDown.Visible)
					{
						this.ToolStrip.OverflowButton.ShowDropDown();
					}
					flag = true;
				}
				else
				{
					if (!this.ItemParentIsOverflow(toolStripItem) && this.ToolStrip.OverflowButton.DropDown.Visible)
					{
						this.ToolStrip.OverflowButton.HideDropDown();
					}
					if (toolStripItem.Owner == this.ToolStrip)
					{
						flag = true;
					}
					else if (toolStripItem is DesignerToolStripControlHost)
					{
						if (toolStripItem.IsOnDropDown && toolStripItem.Placement != ToolStripItemPlacement.Overflow)
						{
							ToolStripDropDown toolStripDropDown = (ToolStripDropDown)((DesignerToolStripControlHost)obj).GetCurrentParent();
							if (toolStripDropDown != null)
							{
								ToolStripItem ownerItem = toolStripDropDown.OwnerItem;
								ToolStripMenuItemDesigner toolStripMenuItemDesigner = (ToolStripMenuItemDesigner)this.host.GetDesigner(ownerItem);
								ToolStripDropDown firstDropDown = toolStripMenuItemDesigner.GetFirstDropDown((ToolStripDropDownItem)ownerItem);
								ToolStripItem toolStripItem2 = ((firstDropDown == null) ? ownerItem : firstDropDown.OwnerItem);
								if (toolStripItem2 != null && toolStripItem2.Owner == this.ToolStrip)
								{
									flag = true;
								}
							}
						}
					}
					else if (toolStripItem.IsOnDropDown && toolStripItem.Placement != ToolStripItemPlacement.Overflow)
					{
						ToolStripItem ownerItem2 = ((ToolStripDropDown)toolStripItem.Owner).OwnerItem;
						if (ownerItem2 != null)
						{
							ToolStripMenuItemDesigner toolStripMenuItemDesigner2 = (ToolStripMenuItemDesigner)this.host.GetDesigner(ownerItem2);
							ToolStripDropDown toolStripDropDown2 = ((toolStripMenuItemDesigner2 == null) ? null : toolStripMenuItemDesigner2.GetFirstDropDown((ToolStripDropDownItem)ownerItem2));
							ToolStripItem toolStripItem3 = ((toolStripDropDown2 == null) ? ownerItem2 : toolStripDropDown2.OwnerItem);
							if (toolStripItem3 != null && toolStripItem3.Owner == this.ToolStrip)
							{
								flag = true;
							}
						}
					}
				}
			}
			return flag;
		}

		internal bool Commit()
		{
			if (this.tn != null && this.tn.Active)
			{
				this.tn.Commit(false, false);
				this.editorNode.Width = this.tn.EditorToolStrip.Width;
			}
			else
			{
				ToolStripDropDownItem toolStripDropDownItem = this.SelectionService.PrimarySelection as ToolStripDropDownItem;
				if (toolStripDropDownItem != null)
				{
					ToolStripMenuItemDesigner toolStripMenuItemDesigner = this.host.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
					if (toolStripMenuItemDesigner != null && toolStripMenuItemDesigner.IsEditorActive)
					{
						toolStripMenuItemDesigner.Commit();
						return true;
					}
				}
				else if (this.KeyboardHandlingService != null)
				{
					ToolStripItem toolStripItem = this.KeyboardHandlingService.SelectedDesignerControl as ToolStripItem;
					if (toolStripItem != null && toolStripItem.IsOnDropDown)
					{
						ToolStripDropDown toolStripDropDown = toolStripItem.GetCurrentParent() as ToolStripDropDown;
						if (toolStripDropDown != null)
						{
							ToolStripDropDownItem toolStripDropDownItem2 = toolStripDropDown.OwnerItem as ToolStripDropDownItem;
							if (toolStripDropDownItem2 != null)
							{
								ToolStripMenuItemDesigner toolStripMenuItemDesigner2 = this.host.GetDesigner(toolStripDropDownItem2) as ToolStripMenuItemDesigner;
								if (toolStripMenuItemDesigner2 != null && toolStripMenuItemDesigner2.IsEditorActive)
								{
									toolStripMenuItemDesigner2.Commit();
									return true;
								}
							}
						}
					}
					else
					{
						ToolStripItem toolStripItem2 = this.SelectionService.PrimarySelection as ToolStripItem;
						if (toolStripItem2 != null)
						{
							ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)this.host.GetDesigner(toolStripItem2);
							if (toolStripItemDesigner != null && toolStripItemDesigner.IsEditorActive)
							{
								toolStripItemDesigner.Editor.Commit(false, false);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private void Control_HandleCreated(object sender, EventArgs e)
		{
			this.Control.HandleCreated -= this.Control_HandleCreated;
			this.InitializeNewItemDropDown();
			this.ToolStrip.OverflowButton.DropDown.Closing += this.OnOverflowDropDownClosing;
			this.ToolStrip.OverflowButton.DropDownOpening += this.OnOverFlowDropDownOpening;
			this.ToolStrip.OverflowButton.DropDownOpened += this.OnOverFlowDropDownOpened;
			this.ToolStrip.OverflowButton.DropDownClosed += this.OnOverFlowDropDownClosed;
			this.ToolStrip.OverflowButton.DropDown.Resize += this.OnOverflowDropDownResize;
			this.ToolStrip.OverflowButton.DropDown.Paint += this.OnOverFlowDropDownPaint;
			this.ToolStrip.Move += this.OnToolStripMove;
			this.ToolStrip.VisibleChanged += this.OnToolStripVisibleChanged;
			this.ToolStrip.ItemAdded += this.OnItemAdded;
		}

		private void ComponentChangeSvc_ComponentAdded(object sender, ComponentEventArgs e)
		{
			if (this.toolStripSelected && e.Component is ToolStrip)
			{
				this.toolStripSelected = false;
			}
			ToolStripItem toolStripItem = e.Component as ToolStripItem;
			try
			{
				if (toolStripItem != null && this._addingItem && !toolStripItem.IsOnDropDown)
				{
					this._addingItem = false;
					if (this.CacheItems)
					{
						this.items.Add(toolStripItem);
					}
					else
					{
						int count = this.ToolStrip.Items.Count;
						try
						{
							base.RaiseComponentChanging(TypeDescriptor.GetProperties(base.Component)["Items"]);
							ToolStripItem toolStripItem2 = this.SelectionService.PrimarySelection as ToolStripItem;
							if (toolStripItem2 != null)
							{
								if (toolStripItem2.Owner == this.ToolStrip)
								{
									int num = this.ToolStrip.Items.IndexOf(toolStripItem2);
									this.ToolStrip.Items.Insert(num, toolStripItem);
								}
							}
							else if (count > 0)
							{
								this.ToolStrip.Items.Insert(count - 1, toolStripItem);
							}
							else
							{
								this.ToolStrip.Items.Add(toolStripItem);
							}
						}
						finally
						{
							base.RaiseComponentChanged(TypeDescriptor.GetProperties(base.Component)["Items"], null, null);
						}
					}
				}
			}
			catch
			{
				if (this._pendingTransaction != null)
				{
					this._pendingTransaction.Cancel();
					this._pendingTransaction = null;
					this._insertMenuItemTransaction = null;
				}
			}
			finally
			{
				if (this._pendingTransaction != null)
				{
					this._pendingTransaction.Commit();
					this._pendingTransaction = null;
					this._insertMenuItemTransaction = null;
				}
			}
		}

		private void ComponentChangeSvc_ComponentAdding(object sender, ComponentEventArgs e)
		{
			if (this.KeyboardHandlingService != null && this.KeyboardHandlingService.CopyInProgress)
			{
				return;
			}
			object obj = this.SelectionService.PrimarySelection;
			if (obj == null && this.keyboardHandlingService != null)
			{
				obj = this.KeyboardHandlingService.SelectedDesignerControl;
			}
			ToolStripItem toolStripItem = obj as ToolStripItem;
			if (toolStripItem != null && toolStripItem.Owner != this.ToolStrip)
			{
				return;
			}
			ToolStripItem toolStripItem2 = e.Component as ToolStripItem;
			if (toolStripItem2 != null && toolStripItem2.Owner != null && toolStripItem2.Owner.Site == null)
			{
				return;
			}
			if (this._insertMenuItemTransaction == null && ToolStripDesigner._autoAddNewItems && toolStripItem2 != null && !this._addingItem && this.IsToolStripOrItemSelected && !this.EditingCollection)
			{
				this._addingItem = true;
				if (this._pendingTransaction == null)
				{
					this._insertMenuItemTransaction = (this._pendingTransaction = this.host.CreateTransaction(SR.GetString("ToolStripDesignerTransactionAddingItem")));
				}
			}
		}

		private void ComponentChangeSvc_ComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			ToolStripItem toolStripItem = e.Component as ToolStripItem;
			if (toolStripItem != null)
			{
				ToolStrip owner = toolStripItem.Owner;
				if (owner == this.ToolStrip && e.Member != null && e.Member.Name == "Overflow")
				{
					ToolStripItemOverflow toolStripItemOverflow = (ToolStripItemOverflow)e.OldValue;
					ToolStripItemOverflow toolStripItemOverflow2 = (ToolStripItemOverflow)e.NewValue;
					if (toolStripItemOverflow != ToolStripItemOverflow.Always && toolStripItemOverflow2 == ToolStripItemOverflow.Always && this.ToolStrip.CanOverflow && !this.ToolStrip.OverflowButton.DropDown.Visible)
					{
						this.ToolStrip.OverflowButton.ShowDropDown();
					}
				}
			}
		}

		private void ComponentChangeSvc_ComponentRemoved(object sender, ComponentEventArgs e)
		{
			if (e.Component is ToolStripItem && ((ToolStripItem)e.Component).Owner == base.Component)
			{
				ToolStripItem toolStripItem = (ToolStripItem)e.Component;
				int num = this.ToolStrip.Items.IndexOf(toolStripItem);
				try
				{
					if (num != -1)
					{
						this.ToolStrip.Items.Remove(toolStripItem);
						base.RaiseComponentChanged(TypeDescriptor.GetProperties(base.Component)["Items"], null, null);
					}
				}
				finally
				{
					if (this._pendingTransaction != null)
					{
						this._pendingTransaction.Commit();
						this._pendingTransaction = null;
					}
				}
				if (this.ToolStrip.Items.Count > 1)
				{
					num = Math.Min(this.ToolStrip.Items.Count - 1, num);
					num = Math.Max(0, num);
				}
				else
				{
					num = -1;
				}
				this.LayoutToolStrip();
				if (toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					this.RemoveBodyGlyphsForOverflow();
					this.AddBodyGlyphsForOverflow();
				}
				if (this.toolStripAdornerWindowService != null && this.boundsToInvalidate != Rectangle.Empty)
				{
					this.toolStripAdornerWindowService.Invalidate(this.boundsToInvalidate);
					base.BehaviorService.Invalidate(this.boundsToInvalidate);
				}
				if (this.KeyboardHandlingService.CutOrDeleteInProgress)
				{
					IComponent component2;
					if (num != -1)
					{
						IComponent component = this.ToolStrip.Items[num];
						component2 = component;
					}
					else
					{
						component2 = this.ToolStrip;
					}
					IComponent component3 = component2;
					if (component3 != null)
					{
						if (component3 is DesignerToolStripControlHost)
						{
							if (this.KeyboardHandlingService != null)
							{
								this.KeyboardHandlingService.SelectedDesignerControl = component3;
							}
							this.SelectionService.SetSelectedComponents(null, SelectionTypes.Replace);
							return;
						}
						this.SelectionService.SetSelectedComponents(new IComponent[] { component3 }, SelectionTypes.Replace);
					}
				}
			}
		}

		private void ComponentChangeSvc_ComponentRemoving(object sender, ComponentEventArgs e)
		{
			if (e.Component is ToolStripItem && ((ToolStripItem)e.Component).Owner == base.Component)
			{
				try
				{
					this._pendingTransaction = this.host.CreateTransaction(SR.GetString("ToolStripDesignerTransactionRemovingItem"));
					base.RaiseComponentChanging(TypeDescriptor.GetProperties(base.Component)["Items"]);
					ToolStripDropDownItem toolStripDropDownItem = e.Component as ToolStripDropDownItem;
					if (toolStripDropDownItem != null)
					{
						toolStripDropDownItem.HideDropDown();
						this.boundsToInvalidate = toolStripDropDownItem.DropDown.Bounds;
					}
				}
				catch
				{
					if (this._pendingTransaction != null)
					{
						this._pendingTransaction.Cancel();
						this._pendingTransaction = null;
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.disposed = true;
				if (this.items != null)
				{
					this.items = null;
				}
				if (this.undoEngine != null)
				{
					this.undoEngine.Undoing -= this.OnUndoing;
					this.undoEngine.Undone -= this.OnUndone;
				}
				if (this.componentChangeSvc != null)
				{
					this.componentChangeSvc.ComponentRemoved -= this.ComponentChangeSvc_ComponentRemoved;
					this.componentChangeSvc.ComponentRemoving -= this.ComponentChangeSvc_ComponentRemoving;
					this.componentChangeSvc.ComponentAdded -= this.ComponentChangeSvc_ComponentAdded;
					this.componentChangeSvc.ComponentAdding -= this.ComponentChangeSvc_ComponentAdding;
					this.componentChangeSvc.ComponentChanged -= this.ComponentChangeSvc_ComponentChanged;
				}
				if (this._selectionSvc != null)
				{
					this._selectionSvc.SelectionChanged -= this.selSvc_SelectionChanged;
					this._selectionSvc.SelectionChanging -= this.selSvc_SelectionChanging;
					this._selectionSvc = null;
				}
				base.EnableDragDrop(false);
				if (this.editManager != null)
				{
					this.editManager.CloseManager();
					this.editManager = null;
				}
				if (this.tn != null)
				{
					this.tn.RollBack();
					this.tn.CloseEditor();
					this.tn = null;
				}
				if (this._miniToolStrip != null)
				{
					this._miniToolStrip.Dispose();
					this._miniToolStrip = null;
				}
				if (this.editorNode != null)
				{
					this.editorNode.Dispose();
					this.editorNode = null;
				}
				if (this.ToolStrip != null)
				{
					this.ToolStrip.OverflowButton.DropDown.Closing -= this.OnOverflowDropDownClosing;
					this.ToolStrip.OverflowButton.DropDownOpening -= this.OnOverFlowDropDownOpening;
					this.ToolStrip.OverflowButton.DropDownOpened -= this.OnOverFlowDropDownOpened;
					this.ToolStrip.OverflowButton.DropDownClosed -= this.OnOverFlowDropDownClosed;
					this.ToolStrip.OverflowButton.DropDown.Resize -= this.OnOverflowDropDownResize;
					this.ToolStrip.OverflowButton.DropDown.Paint -= this.OnOverFlowDropDownPaint;
					this.ToolStrip.Move -= this.OnToolStripMove;
					this.ToolStrip.VisibleChanged -= this.OnToolStripVisibleChanged;
					this.ToolStrip.ItemAdded -= this.OnItemAdded;
					this.ToolStrip.Resize -= this.ToolStrip_Resize;
					this.ToolStrip.DockChanged -= this.ToolStrip_Resize;
					this.ToolStrip.LayoutCompleted -= this.ToolStrip_LayoutCompleted;
				}
				if (this.toolStripContextMenu != null)
				{
					this.toolStripContextMenu.Dispose();
					this.toolStripContextMenu = null;
				}
				this.RemoveBodyGlyphsForOverflow();
				if (this.ToolStrip.OverflowButton.DropDown.Visible)
				{
					this.ToolStrip.OverflowButton.HideDropDown();
				}
				if (this.toolStripAdornerWindowService != null)
				{
					this.toolStripAdornerWindowService = null;
				}
			}
			base.Dispose(disposing);
		}

		public override void DoDefaultAction()
		{
			if (this.InheritanceAttribute != InheritanceAttribute.InheritedReadOnly)
			{
				IComponent component = this.SelectionService.PrimarySelection as IComponent;
				if (component == null && this.KeyboardHandlingService != null)
				{
					component = (IComponent)this.KeyboardHandlingService.SelectedDesignerControl;
				}
				if (component is ToolStripItem && this.host != null)
				{
					IDesigner designer = this.host.GetDesigner(component);
					if (designer != null)
					{
						designer.DoDefaultAction();
						return;
					}
				}
				base.DoDefaultAction();
			}
		}

		protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
		{
			if (!this.ToolStrip.IsHandleCreated)
			{
				return null;
			}
			SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
			if (selectionManager != null && this.ToolStrip != null && this.CanAddItems && this.ToolStrip.Visible)
			{
				base.BehaviorService.ControlToAdornerWindow(this.ToolStrip);
				object primarySelection = this.SelectionService.PrimarySelection;
				Behavior behavior = new ToolStripItemBehavior();
				if (this.ToolStrip.Items.Count > 0)
				{
					ToolStripItem[] array = new ToolStripItem[this.ToolStrip.Items.Count];
					this.ToolStrip.Items.CopyTo(array, 0);
					foreach (ToolStripItem toolStripItem in array)
					{
						if (toolStripItem != null)
						{
							ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)this.host.GetDesigner(toolStripItem);
							if (toolStripItem != primarySelection && toolStripItemDesigner != null && toolStripItemDesigner.IsEditorActive)
							{
								toolStripItemDesigner.Editor.Commit(false, false);
							}
						}
					}
				}
				IMenuEditorService menuEditorService = (IMenuEditorService)this.GetService(typeof(IMenuEditorService));
				if (menuEditorService == null || (menuEditorService != null && !menuEditorService.IsActive()))
				{
					foreach (object obj in this.ToolStrip.Items)
					{
						ToolStripItem toolStripItem2 = (ToolStripItem)obj;
						if (!(toolStripItem2 is DesignerToolStripControlHost) && toolStripItem2.Placement == ToolStripItemPlacement.Main)
						{
							ToolStripItemDesigner toolStripItemDesigner2 = (ToolStripItemDesigner)this.host.GetDesigner(toolStripItem2);
							if (toolStripItemDesigner2 != null)
							{
								bool flag = toolStripItem2 == primarySelection;
								if (flag)
								{
									((ToolStripItemBehavior)behavior).dragBoxFromMouseDown = this.dragBoxFromMouseDown;
								}
								if (!flag)
								{
									toolStripItem2.AutoSize = toolStripItemDesigner2 == null || toolStripItemDesigner2.AutoSize;
								}
								Rectangle glyphBounds = toolStripItemDesigner2.GetGlyphBounds();
								Control parent = this.ToolStrip.Parent;
								Rectangle rectangle = base.BehaviorService.ControlRectInAdornerWindow(parent);
								if (ToolStripDesigner.IsGlyphTotallyVisible(glyphBounds, rectangle) && toolStripItem2.Visible)
								{
									ToolStripItemGlyph toolStripItemGlyph = new ToolStripItemGlyph(toolStripItem2, toolStripItemDesigner2, glyphBounds, behavior);
									toolStripItemDesigner2.bodyGlyph = toolStripItemGlyph;
									selectionManager.BodyGlyphAdorner.Glyphs.Add(toolStripItemGlyph);
								}
							}
						}
					}
				}
			}
			return base.GetControlGlyph(selectionType);
		}

		public override GlyphCollection GetGlyphs(GlyphSelectionType selType)
		{
			GlyphCollection glyphCollection = new GlyphCollection();
			ICollection selectedComponents = this.SelectionService.GetSelectedComponents();
			foreach (object obj in selectedComponents)
			{
				if (obj is ToolStrip)
				{
					GlyphCollection glyphs = base.GetGlyphs(selType);
					glyphCollection.AddRange(glyphs);
				}
				else
				{
					ToolStripItem toolStripItem = obj as ToolStripItem;
					if (toolStripItem != null && toolStripItem.Visible)
					{
						ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)this.host.GetDesigner(toolStripItem);
						if (toolStripItemDesigner != null)
						{
							toolStripItemDesigner.GetGlyphs(ref glyphCollection, this.StandardBehavior);
						}
					}
				}
			}
			if ((this.SelectionRules & SelectionRules.Moveable) != SelectionRules.None && this.InheritanceAttribute != InheritanceAttribute.InheritedReadOnly && selType != GlyphSelectionType.NotSelected)
			{
				Point point = base.BehaviorService.ControlToAdornerWindow((Control)base.Component);
				Rectangle rectangle = new Rectangle(point, ((Control)base.Component).Size);
				int num = (int)((double)DesignerUtils.CONTAINERGRABHANDLESIZE * 0.5);
				if (rectangle.Width < 2 * DesignerUtils.CONTAINERGRABHANDLESIZE)
				{
					num = -1 * num;
				}
				ContainerSelectorBehavior containerSelectorBehavior = new ContainerSelectorBehavior(this.ToolStrip, base.Component.Site, true);
				ContainerSelectorGlyph containerSelectorGlyph = new ContainerSelectorGlyph(rectangle, DesignerUtils.CONTAINERGRABHANDLESIZE, num, containerSelectorBehavior);
				glyphCollection.Insert(0, containerSelectorGlyph);
			}
			return glyphCollection;
		}

		protected override bool GetHitTest(Point point)
		{
			point = this.Control.PointToClient(point);
			return (this._miniToolStrip != null && this._miniToolStrip.Visible && this.AddItemRect.Contains(point)) || this.OverFlowButtonRect.Contains(point) || base.GetHitTest(point);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			this.host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (this.host != null)
			{
				this.componentChangeSvc = (IComponentChangeService)this.host.GetService(typeof(IComponentChangeService));
			}
			if (this.undoEngine == null)
			{
				this.undoEngine = this.GetService(typeof(UndoEngine)) as UndoEngine;
				if (this.undoEngine != null)
				{
					this.undoEngine.Undoing += this.OnUndoing;
					this.undoEngine.Undone += this.OnUndone;
				}
			}
			this.editManager = new ToolStripEditorManager(component);
			if (this.Control.IsHandleCreated)
			{
				this.InitializeNewItemDropDown();
			}
			else
			{
				this.Control.HandleCreated += this.Control_HandleCreated;
			}
			if (this.componentChangeSvc != null)
			{
				this.componentChangeSvc.ComponentRemoved += this.ComponentChangeSvc_ComponentRemoved;
				this.componentChangeSvc.ComponentRemoving += this.ComponentChangeSvc_ComponentRemoving;
				this.componentChangeSvc.ComponentAdded += this.ComponentChangeSvc_ComponentAdded;
				this.componentChangeSvc.ComponentAdding += this.ComponentChangeSvc_ComponentAdding;
				this.componentChangeSvc.ComponentChanged += this.ComponentChangeSvc_ComponentChanged;
			}
			this.toolStripAdornerWindowService = (ToolStripAdornerWindowService)this.GetService(typeof(ToolStripAdornerWindowService));
			this.SelectionService.SelectionChanging += this.selSvc_SelectionChanging;
			this.SelectionService.SelectionChanged += this.selSvc_SelectionChanged;
			this.ToolStrip.Resize += this.ToolStrip_Resize;
			this.ToolStrip.DockChanged += this.ToolStrip_Resize;
			this.ToolStrip.LayoutCompleted += this.ToolStrip_LayoutCompleted;
			this.ToolStrip.OverflowButton.DropDown.TopLevel = false;
			if (this.CanAddItems)
			{
				new EditorServiceContext(this, TypeDescriptor.GetProperties(base.Component)["Items"], SR.GetString("ToolStripItemCollectionEditorVerb"));
				this.keyboardHandlingService = (ToolStripKeyboardHandlingService)this.GetService(typeof(ToolStripKeyboardHandlingService));
				if (this.keyboardHandlingService == null)
				{
					this.keyboardHandlingService = new ToolStripKeyboardHandlingService(base.Component.Site);
				}
				if ((ISupportInSituService)this.GetService(typeof(ISupportInSituService)) == null)
				{
					ISupportInSituService supportInSituService = new ToolStripInSituService(base.Component.Site);
				}
			}
			this.toolStripSelected = true;
			if (this.keyboardHandlingService != null)
			{
				this.KeyboardHandlingService.SelectedDesignerControl = null;
			}
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			Control control = defaultValues["Parent"] as Control;
			Form form = this.host.RootComponent as Form;
			MainMenu mainMenu = null;
			FormDocumentDesigner formDocumentDesigner = null;
			if (form != null)
			{
				formDocumentDesigner = this.host.GetDesigner(form) as FormDocumentDesigner;
				if (formDocumentDesigner != null && formDocumentDesigner.Menu != null)
				{
					mainMenu = formDocumentDesigner.Menu;
					formDocumentDesigner.Menu = null;
				}
			}
			ToolStripPanel toolStripPanel = control as ToolStripPanel;
			if (toolStripPanel == null && control is ToolStripContentPanel)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.ToolStrip)["Dock"];
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(this.ToolStrip, DockStyle.None);
				}
			}
			if (toolStripPanel == null || this.ToolStrip is MenuStrip)
			{
				base.InitializeNewComponent(defaultValues);
			}
			if (formDocumentDesigner != null)
			{
				if (mainMenu != null)
				{
					formDocumentDesigner.Menu = mainMenu;
				}
				if (this.ToolStrip is MenuStrip)
				{
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(form)["MainMenuStrip"];
					if (propertyDescriptor2 != null && propertyDescriptor2.GetValue(form) == null)
					{
						propertyDescriptor2.SetValue(form, this.ToolStrip as MenuStrip);
					}
				}
			}
			if (toolStripPanel != null)
			{
				if (!(this.ToolStrip is MenuStrip))
				{
					PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(toolStripPanel)["Controls"];
					if (this.componentChangeSvc != null)
					{
						this.componentChangeSvc.OnComponentChanging(toolStripPanel, propertyDescriptor3);
					}
					toolStripPanel.Join(this.ToolStrip, toolStripPanel.Rows.Length);
					if (this.componentChangeSvc != null)
					{
						this.componentChangeSvc.OnComponentChanged(toolStripPanel, propertyDescriptor3, toolStripPanel.Controls, toolStripPanel.Controls);
					}
					PropertyDescriptor propertyDescriptor4 = TypeDescriptor.GetProperties(this.ToolStrip)["Location"];
					if (this.componentChangeSvc != null)
					{
						this.componentChangeSvc.OnComponentChanging(this.ToolStrip, propertyDescriptor4);
						this.componentChangeSvc.OnComponentChanged(this.ToolStrip, propertyDescriptor4, null, null);
						return;
					}
				}
			}
			else if (control != null)
			{
				if (this.ToolStrip is MenuStrip)
				{
					int num = -1;
					foreach (object obj in control.Controls)
					{
						Control control2 = (Control)obj;
						if (control2 is ToolStrip && control2 != this.ToolStrip)
						{
							num = control.Controls.IndexOf(control2);
						}
					}
					if (num == -1)
					{
						num = control.Controls.Count - 1;
					}
					control.Controls.SetChildIndex(this.ToolStrip, num);
					return;
				}
				int num2 = -1;
				foreach (object obj2 in control.Controls)
				{
					Control control3 = (Control)obj2;
					MenuStrip menuStrip = control3 as MenuStrip;
					if (control3 is ToolStrip && menuStrip == null)
					{
						return;
					}
					if (menuStrip != null)
					{
						num2 = control.Controls.IndexOf(control3);
						break;
					}
				}
				if (num2 == -1)
				{
					num2 = control.Controls.Count;
				}
				control.Controls.SetChildIndex(this.ToolStrip, num2 - 1);
			}
		}

		private void InitializeNewItemDropDown()
		{
			if (!this.CanAddItems || !this.SupportEditing)
			{
				return;
			}
			ToolStrip toolStrip = (ToolStrip)base.Component;
			this.AddNewTemplateNode(toolStrip);
			this.selSvc_SelectionChanged(null, EventArgs.Empty);
		}

		internal static bool IsGlyphTotallyVisible(Rectangle itemBounds, Rectangle parentBounds)
		{
			return parentBounds.Contains(itemBounds);
		}

		private bool ItemParentIsOverflow(ToolStripItem item)
		{
			ToolStripDropDown toolStripDropDown = item.Owner as ToolStripDropDown;
			if (toolStripDropDown != null)
			{
				while (toolStripDropDown != null && !(toolStripDropDown is ToolStripOverflow))
				{
					if (toolStripDropDown.OwnerItem != null)
					{
						toolStripDropDown = toolStripDropDown.OwnerItem.GetCurrentParent() as ToolStripDropDown;
					}
					else
					{
						toolStripDropDown = null;
					}
				}
			}
			return toolStripDropDown is ToolStripOverflow;
		}

		private void LayoutToolStrip()
		{
			if (!this.disposed)
			{
				this.ToolStrip.PerformLayout();
			}
		}

		internal static string NameFromText(string text, Type componentType, IServiceProvider serviceProvider, bool adjustCapitalization)
		{
			string text2 = ToolStripDesigner.NameFromText(text, componentType, serviceProvider);
			if (adjustCapitalization)
			{
				string text3 = ToolStripDesigner.NameFromText(null, typeof(ToolStripMenuItem), serviceProvider);
				if (!string.IsNullOrEmpty(text3) && char.IsUpper(text3[0]))
				{
					text2 = char.ToUpper(text2[0], CultureInfo.InvariantCulture) + text2.Substring(1);
				}
			}
			return text2;
		}

		internal static string NameFromText(string text, Type componentType, IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				return null;
			}
			INameCreationService nameCreationService = serviceProvider.GetService(typeof(INameCreationService)) as INameCreationService;
			IContainer container = (IContainer)serviceProvider.GetService(typeof(IContainer));
			if (nameCreationService == null || container == null)
			{
				return null;
			}
			string text2 = nameCreationService.CreateName(container, componentType);
			if (text == null || text.Length == 0 || text == "-")
			{
				return text2;
			}
			string name = componentType.Name;
			StringBuilder stringBuilder = new StringBuilder(text.Length + name.Length);
			bool flag = false;
			foreach (char c in text)
			{
				if (flag)
				{
					if (char.IsLower(c))
					{
						c = char.ToUpper(c, CultureInfo.CurrentCulture);
					}
					flag = false;
				}
				if (char.IsLetterOrDigit(c))
				{
					if (stringBuilder.Length == 0)
					{
						if (char.IsDigit(c))
						{
							goto IL_011D;
						}
						if (char.IsLower(c) != char.IsLower(text2[0]))
						{
							if (char.IsLower(c))
							{
								c = char.ToUpper(c, CultureInfo.CurrentCulture);
							}
							else
							{
								c = char.ToLower(c, CultureInfo.CurrentCulture);
							}
						}
					}
					stringBuilder.Append(c);
				}
				else if (char.IsWhiteSpace(c))
				{
					flag = true;
				}
				IL_011D:;
			}
			if (stringBuilder.Length == 0)
			{
				return text2;
			}
			stringBuilder.Append(name);
			string text3 = stringBuilder.ToString();
			if (container.Components[text3] != null)
			{
				string text4 = text3;
				int num = 1;
				while (!nameCreationService.IsValidName(text4) || container.Components[text4] != null)
				{
					text4 = text3 + num.ToString(CultureInfo.InvariantCulture);
					num++;
				}
				return text4;
			}
			if (!nameCreationService.IsValidName(text3))
			{
				return text2;
			}
			return text3;
		}

		protected override void OnContextMenu(int x, int y)
		{
			Component component = this.SelectionService.PrimarySelection as Component;
			if (component is ToolStrip)
			{
				this.DesignerContextMenu.Show(x, y);
			}
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			base.OnDragEnter(de);
			this.SetDragDropEffects(de);
		}

		protected override void OnDragOver(DragEventArgs de)
		{
			base.OnDragOver(de);
			this.SetDragDropEffects(de);
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			base.OnDragDrop(de);
			bool flag = false;
			ToolStrip toolStrip = this.ToolStrip;
			NativeMethods.POINT point = new NativeMethods.POINT(de.X, de.Y);
			NativeMethods.MapWindowPoints(IntPtr.Zero, toolStrip.Handle, point, 1);
			Point point2 = new Point(point.x, point.y);
			if (this.ToolStrip.Orientation == Orientation.Horizontal)
			{
				if (this.ToolStrip.RightToLeft == RightToLeft.Yes)
				{
					if (point2.X >= toolStrip.Items[0].Bounds.X)
					{
						flag = true;
					}
				}
				else if (point2.X <= toolStrip.Items[0].Bounds.X)
				{
					flag = true;
				}
			}
			else if (point2.Y <= toolStrip.Items[0].Bounds.Y)
			{
				flag = true;
			}
			ToolStripItemDataObject toolStripItemDataObject = de.Data as ToolStripItemDataObject;
			if (toolStripItemDataObject != null && toolStripItemDataObject.Owner == toolStrip)
			{
				ArrayList arrayList = toolStripItemDataObject.DragComponents;
				ToolStripItem toolStripItem = toolStripItemDataObject.PrimarySelection;
				int num = -1;
				bool flag2 = de.Effect == DragDropEffects.Copy;
				string text2;
				if (arrayList.Count == 1)
				{
					string text = TypeDescriptor.GetComponentName(arrayList[0]);
					if (text == null || text.Length == 0)
					{
						text = arrayList[0].GetType().Name;
					}
					text2 = SR.GetString(flag2 ? "BehaviorServiceCopyControl" : "BehaviorServiceMoveControl", new object[] { text });
				}
				else
				{
					text2 = SR.GetString(flag2 ? "BehaviorServiceCopyControls" : "BehaviorServiceMoveControls", new object[] { arrayList.Count });
				}
				DesignerTransaction designerTransaction = this.host.CreateTransaction(text2);
				try
				{
					IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.OnComponentChanging(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"]);
					}
					if (flag2)
					{
						if (toolStripItem != null)
						{
							num = arrayList.IndexOf(toolStripItem);
						}
						if (this.KeyboardHandlingService != null)
						{
							this.KeyboardHandlingService.CopyInProgress = true;
						}
						arrayList = DesignerUtils.CopyDragObjects(arrayList, base.Component.Site) as ArrayList;
						if (this.KeyboardHandlingService != null)
						{
							this.KeyboardHandlingService.CopyInProgress = false;
						}
						if (num != -1)
						{
							toolStripItem = arrayList[num] as ToolStripItem;
						}
					}
					if (de.Effect == DragDropEffects.Move || flag2)
					{
						for (int i = 0; i < arrayList.Count; i++)
						{
							if (flag)
							{
								toolStrip.Items.Insert(0, arrayList[i] as ToolStripItem);
							}
							else
							{
								toolStrip.Items.Add(arrayList[i] as ToolStripItem);
							}
						}
						ToolStripDropDownItem toolStripDropDownItem = toolStripItem as ToolStripDropDownItem;
						if (toolStripDropDownItem != null)
						{
							ToolStripMenuItemDesigner toolStripMenuItemDesigner = this.host.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
							if (toolStripMenuItemDesigner != null)
							{
								toolStripMenuItemDesigner.InitializeDropDown();
							}
						}
						this.SelectionService.SetSelectedComponents(new IComponent[] { toolStripItem }, SelectionTypes.Replace | SelectionTypes.Click);
					}
					if (componentChangeService != null)
					{
						componentChangeService.OnComponentChanged(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"], null, null);
					}
					if (flag2 && componentChangeService != null)
					{
						componentChangeService.OnComponentChanging(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"]);
						componentChangeService.OnComponentChanged(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"], null, null);
					}
					base.BehaviorService.SyncSelection();
				}
				catch
				{
					if (designerTransaction != null)
					{
						designerTransaction.Cancel();
						designerTransaction = null;
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

		private void OnItemAdded(object sender, ToolStripItemEventArgs e)
		{
			if (this.editorNode != null && e.Item != this.editorNode)
			{
				int num = this.ToolStrip.Items.IndexOf(this.editorNode);
				if (num == -1 || num != this.ToolStrip.Items.Count - 1)
				{
					this.ToolStrip.ItemAdded -= this.OnItemAdded;
					this.ToolStrip.SuspendLayout();
					this.ToolStrip.Items.Add(this.editorNode);
					this.ToolStrip.ResumeLayout();
					this.ToolStrip.ItemAdded += this.OnItemAdded;
				}
			}
			this.LayoutToolStrip();
		}

		protected override void OnMouseDragMove(int x, int y)
		{
			if (!this.SelectionService.GetComponentSelected(this.ToolStrip))
			{
				base.OnMouseDragMove(x, y);
			}
		}

		private void OnOverflowDropDownClosing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			e.Cancel = e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;
		}

		private void OnOverFlowDropDownClosed(object sender, EventArgs e)
		{
			ToolStripDropDownItem toolStripDropDownItem = sender as ToolStripDropDownItem;
			if (this.toolStripAdornerWindowService != null && toolStripDropDownItem != null)
			{
				this.toolStripAdornerWindowService.Invalidate(toolStripDropDownItem.DropDown.Bounds);
				this.RemoveBodyGlyphsForOverflow();
			}
			ToolStripItem toolStripItem = this.SelectionService.PrimarySelection as ToolStripItem;
			if (toolStripItem != null && toolStripItem.IsOnOverflow)
			{
				ToolStripItem nextItem = this.ToolStrip.GetNextItem(this.ToolStrip.OverflowButton, ArrowDirection.Left);
				if (nextItem != null)
				{
					this.SelectionService.SetSelectedComponents(new IComponent[] { nextItem }, SelectionTypes.Replace);
				}
			}
		}

		private void OnOverFlowDropDownOpened(object sender, EventArgs e)
		{
			if (this.editorNode != null)
			{
				this.editorNode.Control.Visible = true;
				this.editorNode.Visible = true;
			}
			ToolStripDropDownItem toolStripDropDownItem = sender as ToolStripDropDownItem;
			if (toolStripDropDownItem != null)
			{
				this.RemoveBodyGlyphsForOverflow();
				this.AddBodyGlyphsForOverflow();
			}
			ToolStripItem toolStripItem = this.SelectionService.PrimarySelection as ToolStripItem;
			if (toolStripItem == null || (toolStripItem != null && !toolStripItem.IsOnOverflow))
			{
				ToolStripItem nextItem = toolStripDropDownItem.DropDown.GetNextItem(null, ArrowDirection.Down);
				if (nextItem != null)
				{
					this.SelectionService.SetSelectedComponents(new IComponent[] { nextItem }, SelectionTypes.Replace);
					base.BehaviorService.Invalidate(base.BehaviorService.ControlRectInAdornerWindow(this.ToolStrip));
				}
			}
		}

		private void OnOverFlowDropDownPaint(object sender, PaintEventArgs e)
		{
			foreach (object obj in this.ToolStrip.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				if (toolStripItem.Visible && toolStripItem.IsOnOverflow && this.SelectionService.GetComponentSelected(toolStripItem))
				{
					ToolStripItemDesigner toolStripItemDesigner = this.host.GetDesigner(toolStripItem) as ToolStripItemDesigner;
					if (toolStripItemDesigner != null)
					{
						Rectangle glyphBounds = toolStripItemDesigner.GetGlyphBounds();
						ToolStripDesignerUtils.GetAdjustedBounds(toolStripItem, ref glyphBounds);
						glyphBounds.Inflate(2, 2);
						BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
						if (behaviorService != null)
						{
							behaviorService.ProcessPaintMessage(glyphBounds);
						}
					}
				}
			}
		}

		private void OnOverFlowDropDownOpening(object sender, EventArgs e)
		{
			ToolStripDropDownItem toolStripDropDownItem = sender as ToolStripDropDownItem;
			if (toolStripDropDownItem.DropDown.TopLevel)
			{
				toolStripDropDownItem.DropDown.TopLevel = false;
			}
			if (this.toolStripAdornerWindowService != null)
			{
				this.ToolStrip.SuspendLayout();
				toolStripDropDownItem.DropDown.Parent = this.toolStripAdornerWindowService.ToolStripAdornerWindowControl;
				this.ToolStrip.ResumeLayout();
			}
		}

		private void OnOverflowDropDownResize(object sender, EventArgs e)
		{
			ToolStripDropDown toolStripDropDown = sender as ToolStripDropDown;
			if (toolStripDropDown.Visible)
			{
				this.RemoveBodyGlyphsForOverflow();
				this.AddBodyGlyphsForOverflow();
			}
			if (this.toolStripAdornerWindowService != null && toolStripDropDown != null)
			{
				this.toolStripAdornerWindowService.Invalidate();
			}
		}

		protected override void OnSetCursor()
		{
			if (this.toolboxService == null)
			{
				this.toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
			}
			if (this.toolboxService == null || !this.toolboxService.SetCursor() || this.InheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly))
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void OnUndone(object source, EventArgs e)
		{
			if (this.editorNode != null && this.ToolStrip.Items.IndexOf(this.editorNode) == -1)
			{
				this.ToolStrip.Items.Add(this.editorNode);
			}
			if (this.undoingCalled)
			{
				this.ToolStrip.ResumeLayout(true);
				this.ToolStrip.PerformLayout();
				ToolStripDropDownItem toolStripDropDownItem = this.SelectionService.PrimarySelection as ToolStripDropDownItem;
				if (toolStripDropDownItem != null)
				{
					ToolStripMenuItemDesigner toolStripMenuItemDesigner = this.host.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
					if (toolStripMenuItemDesigner != null)
					{
						toolStripMenuItemDesigner.InitializeBodyGlyphsForItems(false, toolStripDropDownItem);
						toolStripMenuItemDesigner.InitializeBodyGlyphsForItems(true, toolStripDropDownItem);
					}
				}
				this.undoingCalled = false;
			}
			base.BehaviorService.SyncSelection();
		}

		private void OnUndoing(object source, EventArgs e)
		{
			if (this.CheckIfItemSelected() || this.SelectionService.GetComponentSelected(this.ToolStrip))
			{
				this.undoingCalled = true;
				this.ToolStrip.SuspendLayout();
			}
		}

		private void OnToolStripMove(object sender, EventArgs e)
		{
			if (this.SelectionService.GetComponentSelected(this.ToolStrip))
			{
				base.BehaviorService.SyncSelection();
			}
		}

		private void OnToolStripVisibleChanged(object sender, EventArgs e)
		{
			ToolStrip toolStrip = sender as ToolStrip;
			if (toolStrip != null && !toolStrip.Visible)
			{
				SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
				Glyph[] array = new Glyph[selectionManager.BodyGlyphAdorner.Glyphs.Count];
				selectionManager.BodyGlyphAdorner.Glyphs.CopyTo(array, 0);
				foreach (Glyph glyph in array)
				{
					if (glyph is ToolStripItemGlyph)
					{
						selectionManager.BodyGlyphAdorner.Glyphs.Remove(glyph);
					}
				}
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "Visible", "AllowDrop", "AllowItemReorder" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(ToolStripDesigner), propertyDescriptor, array2);
				}
			}
		}

		private void RemoveBodyGlyphsForOverflow()
		{
			foreach (object obj in this.ToolStrip.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				if (!(toolStripItem is DesignerToolStripControlHost) && toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)this.host.GetDesigner(toolStripItem);
					if (toolStripItemDesigner != null)
					{
						ControlBodyGlyph bodyGlyph = toolStripItemDesigner.bodyGlyph;
						if (bodyGlyph != null && this.toolStripAdornerWindowService != null && this.toolStripAdornerWindowService.DropDownAdorner.Glyphs.Contains(bodyGlyph))
						{
							this.toolStripAdornerWindowService.DropDownAdorner.Glyphs.Remove(bodyGlyph);
						}
					}
				}
			}
		}

		internal void RollBack()
		{
			if (this.tn != null)
			{
				this.tn.RollBack();
				this.editorNode.Width = this.tn.EditorToolStrip.Width;
			}
		}

		private void ResetVisible()
		{
			this.Visible = true;
		}

		private void SetDragDropEffects(DragEventArgs de)
		{
			ToolStripItemDataObject toolStripItemDataObject = de.Data as ToolStripItemDataObject;
			if (toolStripItemDataObject != null)
			{
				if (toolStripItemDataObject.Owner != this.ToolStrip)
				{
					de.Effect = DragDropEffects.None;
					return;
				}
				de.Effect = ((Control.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
			}
		}

		private void selSvc_SelectionChanging(object sender, EventArgs e)
		{
			if (this.toolStripSelected && this.tn != null && this.tn.Active)
			{
				this.tn.Commit(false, false);
			}
			if (!this.CheckIfItemSelected() && !this.SelectionService.GetComponentSelected(this.ToolStrip))
			{
				this.ToolStrip.Visible = this.currentVisible;
				if (!this.currentVisible && this.parentNotVisible)
				{
					this.ToolStrip.Parent.Visible = this.currentVisible;
					this.parentNotVisible = false;
				}
				if (this.ToolStrip.OverflowButton.DropDown.Visible)
				{
					this.ToolStrip.OverflowButton.HideDropDown();
				}
				if (this.editorNode != null)
				{
					this.editorNode.Visible = false;
				}
				this.ShowHideToolStripItems(false);
				this.toolStripSelected = false;
			}
		}

		private void selSvc_SelectionChanged(object sender, EventArgs e)
		{
			if (this._miniToolStrip != null && this.host != null)
			{
				bool flag = this.CheckIfItemSelected();
				bool flag2 = flag || this.SelectionService.GetComponentSelected(this.ToolStrip);
				if (flag2)
				{
					if (this.SelectionService.GetComponentSelected(this.ToolStrip) && !this.DontCloseOverflow && this.ToolStrip.OverflowButton.DropDown.Visible)
					{
						this.ToolStrip.OverflowButton.HideDropDown();
					}
					this.ShowHideToolStripItems(true);
					this.currentVisible = this.Control.Visible && this.currentVisible;
					if (!this.currentVisible)
					{
						this.Control.Visible = true;
						if (this.ToolStrip.Parent is ToolStripPanel && !this.ToolStrip.Parent.Visible)
						{
							this.parentNotVisible = true;
							this.ToolStrip.Parent.Visible = true;
						}
						base.BehaviorService.SyncSelection();
					}
					if (this.editorNode != null && (this.SelectionService.PrimarySelection == this.ToolStrip || flag))
					{
						bool flag3 = this.FireSyncSelection;
						ToolStripPanel toolStripPanel = this.ToolStrip.Parent as ToolStripPanel;
						try
						{
							if (toolStripPanel != null)
							{
								toolStripPanel.LocationChanged += this.OnToolStripMove;
							}
							this.FireSyncSelection = true;
							this.editorNode.Visible = true;
						}
						finally
						{
							this.FireSyncSelection = flag3;
							if (toolStripPanel != null)
							{
								toolStripPanel.LocationChanged -= this.OnToolStripMove;
							}
						}
					}
					if (!(this.SelectionService.PrimarySelection is ToolStripItem) && this.KeyboardHandlingService != null)
					{
						ToolStripItem toolStripItem = this.KeyboardHandlingService.SelectedDesignerControl as ToolStripItem;
					}
					this.toolStripSelected = true;
				}
			}
		}

		private bool ShouldSerializeVisible()
		{
			return !this.Visible;
		}

		private bool ShouldSerializeAllowDrop()
		{
			return (bool)base.ShadowProperties["AllowDrop"];
		}

		private bool ShouldSerializeAllowItemReorder()
		{
			return (bool)base.ShadowProperties["AllowItemReorder"];
		}

		internal void ShowEditNode(bool clicked)
		{
			if (this.ToolStrip is MenuStrip)
			{
				if (this.KeyboardHandlingService != null)
				{
					this.KeyboardHandlingService.ResetActiveTemplateNodeSelectionState();
				}
				try
				{
					ToolStripItem toolStripItem = this.AddNewItem(typeof(ToolStripMenuItem));
					if (toolStripItem != null)
					{
						ToolStripItemDesigner toolStripItemDesigner = this.host.GetDesigner(toolStripItem) as ToolStripItemDesigner;
						if (toolStripItemDesigner != null)
						{
							toolStripItemDesigner.dummyItemAdded = true;
							((ToolStripMenuItemDesigner)toolStripItemDesigner).InitializeDropDown();
							try
							{
								this.addingDummyItem = true;
								toolStripItemDesigner.ShowEditNode(clicked);
							}
							finally
							{
								this.addingDummyItem = false;
							}
						}
					}
				}
				catch (InvalidOperationException ex)
				{
					IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
					iuiservice.ShowError(ex.Message);
					if (this.KeyboardHandlingService != null)
					{
						this.KeyboardHandlingService.ResetActiveTemplateNodeSelectionState();
					}
				}
			}
		}

		private void ShowHideToolStripItems(bool toolStripSelected)
		{
			foreach (object obj in this.ToolStrip.Items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				if (!(toolStripItem is DesignerToolStripControlHost))
				{
					ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)this.host.GetDesigner(toolStripItem);
					if (toolStripItemDesigner != null)
					{
						toolStripItemDesigner.SetItemVisible(toolStripSelected, this);
					}
				}
			}
			if (this.FireSyncSelection)
			{
				base.BehaviorService.SyncSelection();
				this.FireSyncSelection = false;
			}
		}

		private void ToolStrip_LayoutCompleted(object sender, EventArgs e)
		{
			if (this.FireSyncSelection)
			{
				base.BehaviorService.SyncSelection();
			}
		}

		private void ToolStrip_Resize(object sender, EventArgs e)
		{
			if (!this.addingDummyItem && !this.disposed && (this.CheckIfItemSelected() || this.SelectionService.GetComponentSelected(this.ToolStrip)))
			{
				if (this._miniToolStrip != null && this._miniToolStrip.Visible)
				{
					this.LayoutToolStrip();
				}
				base.BehaviorService.SyncSelection();
			}
		}

		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 123)
			{
				if (msg != 513 && msg != 516)
				{
					base.WndProc(ref m);
					return;
				}
				this.Commit();
				base.WndProc(ref m);
				return;
			}
			else
			{
				int num = NativeMethods.Util.SignedLOWORD(m.LParam);
				int num2 = NativeMethods.Util.SignedHIWORD(m.LParam);
				bool hitTest = this.GetHitTest(new Point(num, num2));
				if (hitTest)
				{
					return;
				}
				base.WndProc(ref m);
				return;
			}
		}

		private const int GLYPHBORDER = 2;

		internal static Point LastCursorPosition = Point.Empty;

		internal static bool _autoAddNewItems = true;

		internal static ToolStripItem dragItem = null;

		internal static bool shiftState = false;

		internal static bool editTemplateNode = false;

		private DesignerToolStripControlHost editorNode;

		private ToolStripEditorManager editManager;

		private ToolStrip _miniToolStrip;

		private DesignerTransaction _insertMenuItemTransaction;

		private Rectangle dragBoxFromMouseDown = Rectangle.Empty;

		private int indexOfItemUnderMouseToDrag = -1;

		private ToolStripTemplateNode tn;

		private ISelectionService _selectionSvc;

		private uint _editingCollection;

		private DesignerTransaction _pendingTransaction;

		private bool _addingItem;

		private Rectangle boundsToInvalidate = Rectangle.Empty;

		private bool currentVisible = true;

		private ToolStripActionList _actionLists;

		private ToolStripAdornerWindowService toolStripAdornerWindowService;

		private IDesignerHost host;

		private IComponentChangeService componentChangeSvc;

		private UndoEngine undoEngine;

		private bool undoingCalled;

		private IToolboxService toolboxService;

		private ContextMenuStrip toolStripContextMenu;

		private bool toolStripSelected;

		private bool cacheItems;

		private ArrayList items;

		private bool disposed;

		private DesignerTransaction newItemTransaction;

		private bool fireSyncSelection;

		private ToolStripKeyboardHandlingService keyboardHandlingService;

		private bool parentNotVisible;

		private bool dontCloseOverflow;

		private bool addingDummyItem;
	}
}
