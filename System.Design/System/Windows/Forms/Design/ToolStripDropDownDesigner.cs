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
	internal class ToolStripDropDownDesigner : ComponentDesigner
	{
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

		public override ICollection AssociatedComponents
		{
			get
			{
				return ((ToolStrip)base.Component).Items;
			}
		}

		public ToolStripMenuItem DesignerMenuItem
		{
			get
			{
				return this.menuItem;
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

		internal void AddSelectionGlyphs()
		{
			SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
			if (selectionManager != null)
			{
				this.AddSelectionGlyphs(selectionManager, this.selSvc);
			}
		}

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

		private void OnSelectionChanging(object sender, EventArgs e)
		{
			ISelectionService selectionService = (ISelectionService)sender;
			bool flag = this.IsContextMenuStripItemSelected(selectionService) || base.Component.Equals(selectionService.PrimarySelection);
			if (this.selected && !flag)
			{
				this.HideMenu();
			}
		}

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

		public void ResetSettingsKey()
		{
			IPersistComponentSettings persistComponentSettings = base.Component as IPersistComponentSettings;
			if (persistComponentSettings != null)
			{
				this.SettingsKey = null;
			}
		}

		private void ResetAutoClose()
		{
			base.ShadowProperties["AutoClose"] = true;
		}

		private void RestoreAutoClose()
		{
			this.dropDown.AutoClose = (bool)base.ShadowProperties["AutoClose"];
		}

		private void ResetAllowDrop()
		{
			base.ShadowProperties["AllowDrop"] = false;
		}

		private void RestoreAllowDrop()
		{
			this.dropDown.AutoClose = (bool)base.ShadowProperties["AllowDrop"];
		}

		private void ResetRightToLeft()
		{
			this.RightToLeft = RightToLeft.No;
		}

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

		private bool ShouldSerializeSettingsKey()
		{
			IPersistComponentSettings persistComponentSettings = base.Component as IPersistComponentSettings;
			return persistComponentSettings != null && persistComponentSettings.SaveSettings && this.SettingsKey != null;
		}

		private bool ShouldSerializeAutoClose()
		{
			bool flag = (bool)base.ShadowProperties["AutoClose"];
			return !flag;
		}

		private bool ShouldSerializeAllowDrop()
		{
			return this.AllowDrop;
		}

		private bool ShouldSerializeRightToLeft()
		{
			return this.RightToLeft != RightToLeft.No;
		}

		private void OnUndone(object source, EventArgs e)
		{
			if (this.selSvc != null && base.Component.Equals(this.selSvc.PrimarySelection))
			{
				this.HideMenu();
				this.ShowMenu();
			}
		}

		private ISelectionService selSvc;

		private MenuStrip designMenu;

		private ToolStripMenuItem menuItem;

		private IDesignerHost host;

		private ToolStripDropDown dropDown;

		private bool selected;

		private ControlBodyGlyph dummyToolStripGlyph;

		private uint _editingCollection;

		private MainMenu parentMenu;

		private FormDocumentDesigner parentFormDesigner;

		internal ToolStripMenuItem currentParent;

		private INestedContainer nestedContainer;

		private UndoEngine undoEngine;

		internal class ContextMenuStripBehavior : Behavior
		{
			internal ContextMenuStripBehavior(ToolStripMenuItem menuItem)
			{
				this.item = menuItem;
			}

			public override bool OnMouseUp(Glyph g, MouseButtons button)
			{
				return button == MouseButtons.Left;
			}

			private ToolStripMenuItem item;
		}
	}
}
