using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002BE RID: 702
	internal class ToolStripDropDownDesigner : ComponentDesigner
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0008F424 File Offset: 0x0008E424
		// (set) Token: 0x06001A5E RID: 6750 RVA: 0x0008F43B File Offset: 0x0008E43B
		private bool AutoClose
		{
			get
			{
				return (bool)base.ShadowProperties["AutoClose"];
			}
			set
			{
				base.ShadowProperties["AutoClose"] = value;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001A5F RID: 6751 RVA: 0x0008F453 File Offset: 0x0008E453
		// (set) Token: 0x06001A60 RID: 6752 RVA: 0x0008F46A File Offset: 0x0008E46A
		private bool AllowDrop
		{
			get
			{
				return (bool)base.ShadowProperties["AllowDrop"];
			}
			set
			{
				base.ShadowProperties["AllowDrop"] = value;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001A61 RID: 6753 RVA: 0x0008F484 File Offset: 0x0008E484
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				ContextMenuStripActionList contextMenuStripActionList = new ContextMenuStripActionList(this);
				if (contextMenuStripActionList != null)
				{
					designerActionListCollection.Add(contextMenuStripActionList);
				}
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

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001A62 RID: 6754 RVA: 0x0008F4E9 File Offset: 0x0008E4E9
		public override ICollection AssociatedComponents
		{
			get
			{
				return ((ToolStrip)base.Component).Items;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0008F4FB File Offset: 0x0008E4FB
		public ToolStripMenuItem DesignerMenuItem
		{
			get
			{
				return this.menuItem;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x0008F503 File Offset: 0x0008E503
		// (set) Token: 0x06001A65 RID: 6757 RVA: 0x0008F511 File Offset: 0x0008E511
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

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001A66 RID: 6758 RVA: 0x0008F533 File Offset: 0x0008E533
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

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001A67 RID: 6759 RVA: 0x0008F54E File Offset: 0x0008E54E
		// (set) Token: 0x06001A68 RID: 6760 RVA: 0x0008F55C File Offset: 0x0008E55C
		private RightToLeft RightToLeft
		{
			get
			{
				return this.dropDown.RightToLeft;
			}
			set
			{
				if (this.menuItem != null && this.designMenu != null && value != this.RightToLeft)
				{
					Rectangle rectangle = Rectangle.Empty;
					try
					{
						rectangle = this.dropDown.Bounds;
						this.menuItem.HideDropDown();
						this.designMenu.RightToLeft = value;
						this.dropDown.RightToLeft = value;
					}
					finally
					{
						BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
						if (behaviorService != null && rectangle != Rectangle.Empty)
						{
							behaviorService.Invalidate(rectangle);
						}
						ToolStripMenuItemDesigner toolStripMenuItemDesigner = (ToolStripMenuItemDesigner)this.host.GetDesigner(this.menuItem);
						if (toolStripMenuItemDesigner != null)
						{
							toolStripMenuItemDesigner.InitializeDropDown();
						}
					}
				}
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x0008F620 File Offset: 0x0008E620
		// (set) Token: 0x06001A6A RID: 6762 RVA: 0x0008F728 File Offset: 0x0008E728
		private string SettingsKey
		{
			get
			{
				if (string.IsNullOrEmpty((string)base.ShadowProperties["SettingsKey"]))
				{
					IPersistComponentSettings persistComponentSettings = base.Component as IPersistComponentSettings;
					if (persistComponentSettings != null && this.host != null)
					{
						if (persistComponentSettings.SettingsKey == null)
						{
							IComponent rootComponent = this.host.RootComponent;
							if (rootComponent != null && rootComponent != persistComponentSettings)
							{
								base.ShadowProperties["SettingsKey"] = string.Format(CultureInfo.CurrentCulture, "{0}.{1}", new object[]
								{
									rootComponent.Site.Name,
									base.Component.Site.Name
								});
							}
							else
							{
								base.ShadowProperties["SettingsKey"] = base.Component.Site.Name;
							}
						}
						persistComponentSettings.SettingsKey = base.ShadowProperties["SettingsKey"] as string;
						return persistComponentSettings.SettingsKey;
					}
				}
				return base.ShadowProperties["SettingsKey"] as string;
			}
			set
			{
				base.ShadowProperties["SettingsKey"] = value;
				IPersistComponentSettings persistComponentSettings = base.Component as IPersistComponentSettings;
				if (persistComponentSettings != null)
				{
					persistComponentSettings.SettingsKey = value;
				}
			}
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0008F75C File Offset: 0x0008E75C
		private void AddSelectionGlyphs(SelectionManager selMgr, ISelectionService selectionService)
		{
			ICollection selectedComponents = selectionService.GetSelectedComponents();
			GlyphCollection glyphCollection = new GlyphCollection();
			foreach (object obj in selectedComponents)
			{
				ToolStripItem toolStripItem = obj as ToolStripItem;
				if (toolStripItem != null)
				{
					ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)this.host.GetDesigner(toolStripItem);
					if (toolStripItemDesigner != null)
					{
						toolStripItemDesigner.GetGlyphs(ref glyphCollection, new ResizeBehavior(toolStripItem.Site));
					}
				}
			}
			if (glyphCollection.Count > 0)
			{
				selMgr.SelectionGlyphAdorner.Glyphs.AddRange(glyphCollection);
			}
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0008F808 File Offset: 0x0008E808
		internal void AddSelectionGlyphs()
		{
			SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
			if (selectionManager != null)
			{
				this.AddSelectionGlyphs(selectionManager, this.selSvc);
			}
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0008F83C File Offset: 0x0008E83C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.selSvc != null)
				{
					this.selSvc.SelectionChanged -= this.OnSelectionChanged;
					this.selSvc.SelectionChanging -= this.OnSelectionChanging;
				}
				this.DisposeMenu();
				if (this.designMenu != null)
				{
					this.designMenu.Dispose();
					this.designMenu = null;
				}
				if (this.dummyToolStripGlyph != null)
				{
					this.dummyToolStripGlyph = null;
				}
				if (this.undoEngine != null)
				{
					this.undoEngine.Undone -= this.OnUndone;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0008F8DC File Offset: 0x0008E8DC
		private void DisposeMenu()
		{
			this.HideMenu();
			Control control = this.host.RootComponent as Control;
			if (control != null)
			{
				if (this.designMenu != null)
				{
					control.Controls.Remove(this.designMenu);
				}
				if (this.menuItem != null)
				{
					if (this.nestedContainer != null)
					{
						this.nestedContainer.Dispose();
						this.nestedContainer = null;
					}
					this.menuItem.Dispose();
					this.menuItem = null;
				}
			}
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0008F950 File Offset: 0x0008E950
		private void HideMenu()
		{
			if (this.menuItem == null)
			{
				return;
			}
			if (this.parentMenu != null && this.parentFormDesigner != null)
			{
				this.parentFormDesigner.Menu = this.parentMenu;
			}
			this.selected = false;
			Control control = this.host.RootComponent as Control;
			if (control != null)
			{
				this.menuItem.DropDown.AutoClose = true;
				this.menuItem.HideDropDown();
				this.menuItem.Visible = false;
				this.designMenu.Visible = false;
				ToolStripAdornerWindowService toolStripAdornerWindowService = (ToolStripAdornerWindowService)this.GetService(typeof(ToolStripAdornerWindowService));
				if (toolStripAdornerWindowService != null)
				{
					toolStripAdornerWindowService.Invalidate();
				}
				BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
				if (behaviorService != null)
				{
					if (this.dummyToolStripGlyph != null)
					{
						SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
						if (selectionManager != null)
						{
							if (selectionManager.BodyGlyphAdorner.Glyphs.Contains(this.dummyToolStripGlyph))
							{
								selectionManager.BodyGlyphAdorner.Glyphs.Remove(this.dummyToolStripGlyph);
							}
							selectionManager.Refresh();
						}
					}
					this.dummyToolStripGlyph = null;
				}
				if (this.menuItem != null)
				{
					ToolStripMenuItemDesigner toolStripMenuItemDesigner = this.host.GetDesigner(this.menuItem) as ToolStripMenuItemDesigner;
					if (toolStripMenuItemDesigner != null)
					{
						toolStripMenuItemDesigner.UnHookEvents();
						toolStripMenuItemDesigner.RemoveTypeHereNode(this.menuItem);
					}
				}
			}
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0008FAA8 File Offset: 0x0008EAA8
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if ((ToolStripKeyboardHandlingService)this.GetService(typeof(ToolStripKeyboardHandlingService)) == null)
			{
				ToolStripKeyboardHandlingService toolStripKeyboardHandlingService = new ToolStripKeyboardHandlingService(component.Site);
			}
			if ((ISupportInSituService)this.GetService(typeof(ISupportInSituService)) == null)
			{
				ISupportInSituService supportInSituService = new ToolStripInSituService(base.Component.Site);
			}
			this.dropDown = (ToolStripDropDown)base.Component;
			this.dropDown.Visible = false;
			this.AutoClose = this.dropDown.AutoClose;
			this.AllowDrop = this.dropDown.AllowDrop;
			this.selSvc = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (this.selSvc != null)
			{
				if (this.host != null && !this.host.Loading)
				{
					this.selSvc.SetSelectedComponents(new IComponent[] { this.host.RootComponent }, SelectionTypes.Replace);
				}
				this.selSvc.SelectionChanging += this.OnSelectionChanging;
				this.selSvc.SelectionChanged += this.OnSelectionChanged;
			}
			this.designMenu = new MenuStrip();
			this.designMenu.Visible = false;
			this.designMenu.AutoSize = false;
			this.designMenu.Dock = DockStyle.Top;
			Control control = this.host.RootComponent as Control;
			if (control != null)
			{
				this.menuItem = new ToolStripMenuItem();
				this.menuItem.BackColor = SystemColors.Window;
				this.menuItem.Name = base.Component.Site.Name;
				this.menuItem.Text = ((this.dropDown != null) ? this.dropDown.GetType().Name : this.menuItem.Name);
				this.designMenu.Items.Add(this.menuItem);
				control.Controls.Add(this.designMenu);
				this.designMenu.SendToBack();
				this.nestedContainer = this.GetService(typeof(INestedContainer)) as INestedContainer;
				if (this.nestedContainer != null)
				{
					this.nestedContainer.Add(this.menuItem, "ContextMenuStrip");
				}
			}
			new EditorServiceContext(this, TypeDescriptor.GetProperties(base.Component)["Items"], SR.GetString("ToolStripItemCollectionEditorVerb"));
			if (this.undoEngine == null)
			{
				this.undoEngine = this.GetService(typeof(UndoEngine)) as UndoEngine;
				if (this.undoEngine != null)
				{
					this.undoEngine.Undone += this.OnUndone;
				}
			}
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0008FD70 File Offset: 0x0008ED70
		private bool IsContextMenuStripItemSelected(ISelectionService selectionService)
		{
			bool flag = false;
			if (this.menuItem == null)
			{
				return flag;
			}
			ToolStripDropDown toolStripDropDown = null;
			IComponent component = (IComponent)selectionService.PrimarySelection;
			if (component == null && this.dropDown.Visible)
			{
				ToolStripKeyboardHandlingService toolStripKeyboardHandlingService = (ToolStripKeyboardHandlingService)this.GetService(typeof(ToolStripKeyboardHandlingService));
				if (toolStripKeyboardHandlingService != null)
				{
					component = (IComponent)toolStripKeyboardHandlingService.SelectedDesignerControl;
				}
			}
			if (component is ToolStripDropDownItem)
			{
				ToolStripDropDownItem toolStripDropDownItem = component as ToolStripDropDownItem;
				if (toolStripDropDownItem != null && toolStripDropDownItem == this.menuItem)
				{
					toolStripDropDown = this.menuItem.DropDown;
				}
				else
				{
					ToolStripMenuItemDesigner toolStripMenuItemDesigner = (ToolStripMenuItemDesigner)this.host.GetDesigner(component);
					if (toolStripMenuItemDesigner != null)
					{
						toolStripDropDown = toolStripMenuItemDesigner.GetFirstDropDown((ToolStripDropDownItem)component);
					}
				}
			}
			else if (component is ToolStripItem)
			{
				ToolStripDropDown toolStripDropDown2 = ((ToolStripItem)component).GetCurrentParent() as ToolStripDropDown;
				if (toolStripDropDown2 == null)
				{
					toolStripDropDown2 = ((ToolStripItem)component).Owner as ToolStripDropDown;
				}
				if (toolStripDropDown2 != null && toolStripDropDown2.Visible)
				{
					ToolStripItem ownerItem = toolStripDropDown2.OwnerItem;
					if (ownerItem != null && ownerItem == this.menuItem)
					{
						toolStripDropDown = this.menuItem.DropDown;
					}
					else
					{
						ToolStripMenuItemDesigner toolStripMenuItemDesigner2 = (ToolStripMenuItemDesigner)this.host.GetDesigner(ownerItem);
						if (toolStripMenuItemDesigner2 != null)
						{
							toolStripDropDown = toolStripMenuItemDesigner2.GetFirstDropDown((ToolStripDropDownItem)ownerItem);
						}
					}
				}
			}
			if (toolStripDropDown != null)
			{
				ToolStripItem ownerItem2 = toolStripDropDown.OwnerItem;
				if (ownerItem2 == this.menuItem)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0008FECC File Offset: 0x0008EECC
		private void OnSelectionChanging(object sender, EventArgs e)
		{
			ISelectionService selectionService = (ISelectionService)sender;
			bool flag = this.IsContextMenuStripItemSelected(selectionService) || base.Component.Equals(selectionService.PrimarySelection);
			if (this.selected && !flag)
			{
				this.HideMenu();
			}
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x0008FF10 File Offset: 0x0008EF10
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (base.Component == null || this.menuItem == null)
			{
				return;
			}
			ISelectionService selectionService = (ISelectionService)sender;
			if (selectionService.GetComponentSelected(this.menuItem))
			{
				selectionService.SetSelectedComponents(new IComponent[] { base.Component }, SelectionTypes.Replace);
			}
			if (base.Component.Equals(selectionService.PrimarySelection) && this.selected)
			{
				return;
			}
			bool flag = this.IsContextMenuStripItemSelected(selectionService) || base.Component.Equals(selectionService.PrimarySelection);
			if (flag)
			{
				if (!this.dropDown.Visible)
				{
					this.ShowMenu();
				}
				SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
				if (selectionManager != null)
				{
					if (this.dummyToolStripGlyph != null)
					{
						selectionManager.BodyGlyphAdorner.Glyphs.Insert(0, this.dummyToolStripGlyph);
					}
					this.AddSelectionGlyphs(selectionManager, selectionService);
				}
			}
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x0008FFEC File Offset: 0x0008EFEC
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "AutoClose", "SettingsKey", "RightToLeft", "AllowDrop" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(ToolStripDropDownDesigner), propertyDescriptor, array2);
				}
			}
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00090070 File Offset: 0x0008F070
		public void ResetSettingsKey()
		{
			IPersistComponentSettings persistComponentSettings = base.Component as IPersistComponentSettings;
			if (persistComponentSettings != null)
			{
				this.SettingsKey = null;
			}
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00090093 File Offset: 0x0008F093
		private void ResetAutoClose()
		{
			base.ShadowProperties["AutoClose"] = true;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x000900AB File Offset: 0x0008F0AB
		private void RestoreAutoClose()
		{
			this.dropDown.AutoClose = (bool)base.ShadowProperties["AutoClose"];
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x000900CD File Offset: 0x0008F0CD
		private void ResetAllowDrop()
		{
			base.ShadowProperties["AllowDrop"] = false;
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000900E5 File Offset: 0x0008F0E5
		private void RestoreAllowDrop()
		{
			this.dropDown.AutoClose = (bool)base.ShadowProperties["AllowDrop"];
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x00090107 File Offset: 0x0008F107
		private void ResetRightToLeft()
		{
			this.RightToLeft = RightToLeft.No;
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x00090110 File Offset: 0x0008F110
		public void ShowMenu()
		{
			if (this.menuItem == null)
			{
				return;
			}
			Control parent = this.designMenu.Parent;
			Form form = parent as Form;
			if (form != null)
			{
				this.parentFormDesigner = this.host.GetDesigner(form) as FormDocumentDesigner;
				if (this.parentFormDesigner != null && this.parentFormDesigner.Menu != null)
				{
					this.parentMenu = this.parentFormDesigner.Menu;
					this.parentFormDesigner.Menu = null;
				}
			}
			this.selected = true;
			this.designMenu.Visible = true;
			this.designMenu.BringToFront();
			this.menuItem.Visible = true;
			if (this.currentParent != null && this.currentParent != this.menuItem)
			{
				ToolStripMenuItemDesigner toolStripMenuItemDesigner = this.host.GetDesigner(this.currentParent) as ToolStripMenuItemDesigner;
				if (toolStripMenuItemDesigner != null)
				{
					toolStripMenuItemDesigner.RemoveTypeHereNode(this.currentParent);
				}
			}
			this.menuItem.DropDown = this.dropDown;
			this.menuItem.DropDown.OwnerItem = this.menuItem;
			if (this.dropDown.Items.Count > 0)
			{
				ToolStripItem[] array = new ToolStripItem[this.dropDown.Items.Count];
				this.dropDown.Items.CopyTo(array, 0);
				foreach (ToolStripItem toolStripItem in array)
				{
					if (toolStripItem is DesignerToolStripControlHost)
					{
						this.dropDown.Items.Remove(toolStripItem);
					}
				}
			}
			ToolStripMenuItemDesigner toolStripMenuItemDesigner2 = (ToolStripMenuItemDesigner)this.host.GetDesigner(this.menuItem);
			BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
			if (behaviorService != null)
			{
				if (toolStripMenuItemDesigner2 != null && parent != null)
				{
					Rectangle rectangle = behaviorService.ControlRectInAdornerWindow(parent);
					Rectangle rectangle2 = behaviorService.ControlRectInAdornerWindow(this.designMenu);
					if (ToolStripDesigner.IsGlyphTotallyVisible(rectangle2, rectangle))
					{
						toolStripMenuItemDesigner2.InitializeDropDown();
					}
				}
				if (this.dummyToolStripGlyph == null)
				{
					Point point = behaviorService.ControlToAdornerWindow(this.designMenu);
					Rectangle bounds = this.designMenu.Bounds;
					bounds.Offset(point);
					this.dummyToolStripGlyph = new ControlBodyGlyph(bounds, Cursor.Current, this.menuItem, new ToolStripDropDownDesigner.ContextMenuStripBehavior(this.menuItem));
					SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
					if (selectionManager != null)
					{
						selectionManager.BodyGlyphAdorner.Glyphs.Insert(0, this.dummyToolStripGlyph);
					}
				}
				ToolStripKeyboardHandlingService toolStripKeyboardHandlingService = (ToolStripKeyboardHandlingService)this.GetService(typeof(ToolStripKeyboardHandlingService));
				if (toolStripKeyboardHandlingService != null)
				{
					int num = this.dropDown.Items.Count - 1;
					if (num >= 0)
					{
						toolStripKeyboardHandlingService.SelectedDesignerControl = this.dropDown.Items[num];
					}
				}
			}
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x000903C0 File Offset: 0x0008F3C0
		private bool ShouldSerializeSettingsKey()
		{
			IPersistComponentSettings persistComponentSettings = base.Component as IPersistComponentSettings;
			return persistComponentSettings != null && persistComponentSettings.SaveSettings && this.SettingsKey != null;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000903F4 File Offset: 0x0008F3F4
		private bool ShouldSerializeAutoClose()
		{
			bool flag = (bool)base.ShadowProperties["AutoClose"];
			return !flag;
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x0009041B File Offset: 0x0008F41B
		private bool ShouldSerializeAllowDrop()
		{
			return this.AllowDrop;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00090423 File Offset: 0x0008F423
		private bool ShouldSerializeRightToLeft()
		{
			return this.RightToLeft != RightToLeft.No;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00090431 File Offset: 0x0008F431
		private void OnUndone(object source, EventArgs e)
		{
			if (this.selSvc != null && base.Component.Equals(this.selSvc.PrimarySelection))
			{
				this.HideMenu();
				this.ShowMenu();
			}
		}

		// Token: 0x04001518 RID: 5400
		private ISelectionService selSvc;

		// Token: 0x04001519 RID: 5401
		private MenuStrip designMenu;

		// Token: 0x0400151A RID: 5402
		private ToolStripMenuItem menuItem;

		// Token: 0x0400151B RID: 5403
		private IDesignerHost host;

		// Token: 0x0400151C RID: 5404
		private ToolStripDropDown dropDown;

		// Token: 0x0400151D RID: 5405
		private bool selected;

		// Token: 0x0400151E RID: 5406
		private ControlBodyGlyph dummyToolStripGlyph;

		// Token: 0x0400151F RID: 5407
		private uint _editingCollection;

		// Token: 0x04001520 RID: 5408
		private MainMenu parentMenu;

		// Token: 0x04001521 RID: 5409
		private FormDocumentDesigner parentFormDesigner;

		// Token: 0x04001522 RID: 5410
		internal ToolStripMenuItem currentParent;

		// Token: 0x04001523 RID: 5411
		private INestedContainer nestedContainer;

		// Token: 0x04001524 RID: 5412
		private UndoEngine undoEngine;

		// Token: 0x020002BF RID: 703
		internal class ContextMenuStripBehavior : Behavior
		{
			// Token: 0x06001A82 RID: 6786 RVA: 0x00090467 File Offset: 0x0008F467
			internal ContextMenuStripBehavior(ToolStripMenuItem menuItem)
			{
				this.item = menuItem;
			}

			// Token: 0x06001A83 RID: 6787 RVA: 0x00090476 File Offset: 0x0008F476
			public override bool OnMouseUp(Glyph g, MouseButtons button)
			{
				return button == MouseButtons.Left;
			}

			// Token: 0x04001525 RID: 5413
			private ToolStripMenuItem item;
		}
	}
}
