using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002C0 RID: 704
	internal class ToolStripItemDesigner : ComponentDesigner
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001A84 RID: 6788 RVA: 0x00090483 File Offset: 0x0008F483
		// (set) Token: 0x06001A85 RID: 6789 RVA: 0x0009049C File Offset: 0x0008F49C
		internal bool AutoSize
		{
			get
			{
				return (bool)base.ShadowProperties["AutoSize"];
			}
			set
			{
				bool flag = (bool)base.ShadowProperties["AutoSize"];
				base.ShadowProperties["AutoSize"] = value;
				if (value != flag)
				{
					this.ToolStripItem.AutoSize = value;
				}
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001A86 RID: 6790 RVA: 0x000904E5 File Offset: 0x0008F4E5
		// (set) Token: 0x06001A87 RID: 6791 RVA: 0x000904FC File Offset: 0x0008F4FC
		private string AccessibleName
		{
			get
			{
				return (string)base.ShadowProperties["AccessibleName"];
			}
			set
			{
				base.ShadowProperties["AccessibleName"] = value;
			}
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x0009050F File Offset: 0x0008F50F
		internal override bool CanBeAssociatedWith(IDesigner parentDesigner)
		{
			return parentDesigner is ToolStripDesigner;
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x0009051C File Offset: 0x0008F51C
		private ContextMenuStrip DesignerContextMenu
		{
			get
			{
				BaseContextMenuStrip baseContextMenuStrip = new BaseContextMenuStrip(base.Component.Site, this.ToolStripItem);
				if (this.selSvc.SelectionCount > 1)
				{
					baseContextMenuStrip.GroupOrdering.Clear();
					baseContextMenuStrip.GroupOrdering.AddRange(new string[] { "Code", "Selection", "Edit", "Properties" });
				}
				else
				{
					baseContextMenuStrip.GroupOrdering.Clear();
					baseContextMenuStrip.GroupOrdering.AddRange(new string[] { "Code", "Custom", "Selection", "Edit", "Properties" });
					baseContextMenuStrip.Text = "CustomContextMenu";
					if (this.toolStripItemCustomMenuItemCollection == null)
					{
						this.toolStripItemCustomMenuItemCollection = new ToolStripItemCustomMenuItemCollection(base.Component.Site, this.ToolStripItem);
					}
					foreach (object obj in this.toolStripItemCustomMenuItemCollection)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj;
						baseContextMenuStrip.Groups["Custom"].Items.Add(toolStripItem);
					}
				}
				if (this.toolStripItemCustomMenuItemCollection != null)
				{
					this.toolStripItemCustomMenuItemCollection.RefreshItems();
				}
				baseContextMenuStrip.Populated = false;
				return baseContextMenuStrip;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001A8A RID: 6794 RVA: 0x00090688 File Offset: 0x0008F688
		// (set) Token: 0x06001A8B RID: 6795 RVA: 0x00090690 File Offset: 0x0008F690
		internal virtual ToolStripTemplateNode Editor
		{
			get
			{
				return this._editorNode;
			}
			set
			{
				this._editorNode = value;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x00090699 File Offset: 0x0008F699
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

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x000906B4 File Offset: 0x0008F6B4
		// (set) Token: 0x06001A8E RID: 6798 RVA: 0x000906BC File Offset: 0x0008F6BC
		internal bool IsEditorActive
		{
			get
			{
				return this.isEditorActive;
			}
			set
			{
				this.isEditorActive = value;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x000906C5 File Offset: 0x0008F6C5
		// (set) Token: 0x06001A90 RID: 6800 RVA: 0x000906CD File Offset: 0x0008F6CD
		internal bool InternalCreate
		{
			get
			{
				return this.internalCreate;
			}
			set
			{
				this.internalCreate = value;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x000906D8 File Offset: 0x0008F6D8
		protected IComponent ImmediateParent
		{
			get
			{
				if (this.ToolStripItem == null)
				{
					return null;
				}
				ToolStrip currentParent = this.ToolStripItem.GetCurrentParent();
				if (currentParent == null)
				{
					return this.ToolStripItem.Owner;
				}
				return currentParent;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x0009070B File Offset: 0x0008F70B
		// (set) Token: 0x06001A93 RID: 6803 RVA: 0x00090724 File Offset: 0x0008F724
		private ToolStripItemOverflow Overflow
		{
			get
			{
				return (ToolStripItemOverflow)base.ShadowProperties["Overflow"];
			}
			set
			{
				if (this.ToolStripItem.IsOnOverflow)
				{
					ToolStrip owner = this.ToolStripItem.Owner;
					if (owner.OverflowButton.DropDown.Visible)
					{
						owner.OverflowButton.HideDropDown();
					}
				}
				if (this.ToolStripItem is ToolStripDropDownItem)
				{
					ToolStripDropDownItem toolStripDropDownItem = this.ToolStripItem as ToolStripDropDownItem;
					toolStripDropDownItem.HideDropDown();
				}
				if (value != this.ToolStripItem.Overflow)
				{
					this.ToolStripItem.Overflow = value;
					base.ShadowProperties["Overflow"] = value;
				}
				BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
				if (behaviorService != null)
				{
					behaviorService.SyncSelection();
				}
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x000907D8 File Offset: 0x0008F7D8
		protected override IComponent ParentComponent
		{
			get
			{
				if (this.ToolStripItem != null)
				{
					if (this.ToolStripItem.IsOnDropDown && !this.ToolStripItem.IsOnOverflow)
					{
						ToolStripDropDown toolStripDropDown = this.ImmediateParent as ToolStripDropDown;
						if (toolStripDropDown != null)
						{
							if (toolStripDropDown.IsAutoGenerated)
							{
								return toolStripDropDown.OwnerItem;
							}
							return toolStripDropDown;
						}
					}
					return this.GetMainToolStrip();
				}
				return null;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x0009082F File Offset: 0x0008F82F
		public ToolStripItem ToolStripItem
		{
			get
			{
				return (ToolStripItem)base.Component;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x0009083C File Offset: 0x0008F83C
		// (set) Token: 0x06001A97 RID: 6807 RVA: 0x00090853 File Offset: 0x0008F853
		protected bool Visible
		{
			get
			{
				return (bool)base.ShadowProperties["Visible"];
			}
			set
			{
				base.ShadowProperties["Visible"] = value;
				this.currentVisible = value;
			}
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00090874 File Offset: 0x0008F874
		internal ArrayList AddParentTree()
		{
			ArrayList arrayList = new ArrayList();
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				IComponent rootComponent = designerHost.RootComponent;
				Component component = this.ToolStripItem;
				if (component != null && rootComponent != null)
				{
					while (component != rootComponent)
					{
						if (component is ToolStripItem)
						{
							ToolStripItem toolStripItem = component as ToolStripItem;
							if (toolStripItem.IsOnDropDown)
							{
								if (toolStripItem.IsOnOverflow)
								{
									arrayList.Add(toolStripItem.Owner);
									component = toolStripItem.Owner;
								}
								else
								{
									ToolStripDropDown toolStripDropDown = toolStripItem.Owner as ToolStripDropDown;
									if (toolStripDropDown != null)
									{
										ToolStripItem ownerItem = toolStripDropDown.OwnerItem;
										if (ownerItem != null)
										{
											arrayList.Add(ownerItem);
											component = ownerItem;
										}
									}
								}
							}
							else
							{
								if (toolStripItem.Owner.Site != null)
								{
									arrayList.Add(toolStripItem.Owner);
								}
								component = toolStripItem.Owner;
							}
						}
						else if (component is Control)
						{
							Control control = component as Control;
							Control parent = control.Parent;
							if (parent.Site != null)
							{
								arrayList.Add(parent);
							}
							component = parent;
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x0009098B File Offset: 0x0008F98B
		private void CreateDummyNode()
		{
			this._editorNode = new ToolStripTemplateNode(this.ToolStripItem, this.ToolStripItem.Text, this.ToolStripItem.Image);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000909B4 File Offset: 0x0008F9B4
		internal virtual void CommitEdit(Type type, string text, bool commit, bool enterKeyPressed, bool tabKeyPressed)
		{
			ToolStripItem toolStripItem = null;
			SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
			BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
			ToolStrip toolStrip = this.ImmediateParent as ToolStrip;
			toolStrip.SuspendLayout();
			this.HideDummyNode();
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			ToolStripDesigner toolStripDesigner = (ToolStripDesigner)designerHost.GetDesigner(this.ToolStripItem.Owner);
			if (toolStripDesigner != null && toolStripDesigner.EditManager != null)
			{
				toolStripDesigner.EditManager.ActivateEditor(null, false);
			}
			if (toolStrip is MenuStrip && type == typeof(ToolStripSeparator))
			{
				IDesignerHost designerHost2 = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost2 != null)
				{
					IUIService iuiservice = (IUIService)designerHost2.GetService(typeof(IUIService));
					if (iuiservice != null)
					{
						iuiservice.ShowError(SR.GetString("ToolStripSeparatorError"));
						commit = false;
						if (this.selSvc != null)
						{
							this.selSvc.SetSelectedComponents(new object[] { toolStrip });
						}
					}
				}
			}
			if (commit)
			{
				if (this.dummyItemAdded)
				{
					try
					{
						this.RemoveItem();
						toolStripItem = toolStripDesigner.AddNewItem(type, text, enterKeyPressed, false);
						goto IL_0206;
					}
					finally
					{
						if (toolStripDesigner.NewItemTransaction != null)
						{
							toolStripDesigner.NewItemTransaction.Commit();
							toolStripDesigner.NewItemTransaction = null;
						}
					}
				}
				DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("ToolStripItemPropertyChangeTransaction"));
				try
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.ToolStripItem)["Text"];
					string text2 = (string)propertyDescriptor.GetValue(this.ToolStripItem);
					if (propertyDescriptor != null && text != text2)
					{
						propertyDescriptor.SetValue(this.ToolStripItem, text);
					}
					if (enterKeyPressed && this.selSvc != null)
					{
						this.SelectNextItem(this.selSvc, enterKeyPressed, toolStripDesigner);
					}
				}
				catch (Exception ex)
				{
					if (designerTransaction != null)
					{
						designerTransaction.Cancel();
						designerTransaction = null;
					}
					if (selectionManager != null)
					{
						selectionManager.Refresh();
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
				IL_0206:
				this.dummyItemAdded = false;
			}
			else if (this.dummyItemAdded)
			{
				this.dummyItemAdded = false;
				this.RemoveItem();
				if (toolStripDesigner.NewItemTransaction != null)
				{
					toolStripDesigner.NewItemTransaction.Cancel();
					toolStripDesigner.NewItemTransaction = null;
				}
			}
			toolStrip.ResumeLayout();
			if (toolStripItem != null && !toolStripItem.IsOnDropDown)
			{
				ToolStripDropDownItem toolStripDropDownItem = toolStripItem as ToolStripDropDownItem;
				if (toolStripDropDownItem != null)
				{
					ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)designerHost.GetDesigner(toolStripItem);
					Rectangle glyphBounds = toolStripItemDesigner.GetGlyphBounds();
					Control control = designerHost.RootComponent as Control;
					if (control != null && behaviorService != null)
					{
						Rectangle rectangle = behaviorService.ControlRectInAdornerWindow(control);
						if (!ToolStripDesigner.IsGlyphTotallyVisible(glyphBounds, rectangle))
						{
							toolStripDropDownItem.HideDropDown();
						}
					}
				}
			}
			if (selectionManager != null)
			{
				selectionManager.Refresh();
			}
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00090C9C File Offset: 0x0008FC9C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._editorNode != null)
				{
					this._editorNode.CloseEditor();
					this._editorNode = null;
				}
				if (this.ToolStripItem != null)
				{
					this.ToolStripItem.Paint -= this.OnItemPaint;
				}
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRename -= this.OnComponentRename;
				}
				if (this.selSvc != null)
				{
					this.selSvc.SelectionChanged -= this.OnSelectionChanged;
				}
				if (this.bodyGlyph != null)
				{
					ToolStripAdornerWindowService toolStripAdornerWindowService = (ToolStripAdornerWindowService)this.GetService(typeof(ToolStripAdornerWindowService));
					if (toolStripAdornerWindowService != null && toolStripAdornerWindowService.DropDownAdorner.Glyphs.Contains(this.bodyGlyph))
					{
						toolStripAdornerWindowService.DropDownAdorner.Glyphs.Remove(this.bodyGlyph);
					}
				}
				if (this.toolStripItemCustomMenuItemCollection != null && this.toolStripItemCustomMenuItemCollection.Count > 0)
				{
					foreach (object obj in this.toolStripItemCustomMenuItemCollection)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj;
						toolStripItem.Dispose();
					}
					this.toolStripItemCustomMenuItemCollection.Clear();
				}
				this.toolStripItemCustomMenuItemCollection = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00090E00 File Offset: 0x0008FE00
		protected virtual Component GetOwnerForActionList()
		{
			if (this.ToolStripItem.Placement != ToolStripItemPlacement.Main)
			{
				return this.ToolStripItem.Owner;
			}
			return this.ToolStripItem.GetCurrentParent();
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00090E26 File Offset: 0x0008FE26
		internal virtual ToolStrip GetMainToolStrip()
		{
			return this.ToolStripItem.Owner;
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00090E34 File Offset: 0x0008FE34
		public Rectangle GetGlyphBounds()
		{
			BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
			Rectangle rectangle = Rectangle.Empty;
			if (behaviorService != null && this.ImmediateParent != null)
			{
				Point point = behaviorService.ControlToAdornerWindow((Control)this.ImmediateParent);
				rectangle = this.ToolStripItem.Bounds;
				rectangle.Offset(point);
			}
			return rectangle;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00090E90 File Offset: 0x0008FE90
		private void FireComponentChanging(ToolStripDropDownItem parent)
		{
			if (parent != null)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null && parent.Site != null)
				{
					componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["DropDownItems"]);
				}
				foreach (object obj in parent.DropDownItems)
				{
					ToolStripItem toolStripItem = (ToolStripItem)obj;
					ToolStripDropDownItem toolStripDropDownItem = toolStripItem as ToolStripDropDownItem;
					if (toolStripDropDownItem != null && toolStripDropDownItem.DropDownItems.Count > 1)
					{
						this.FireComponentChanging(toolStripDropDownItem);
					}
				}
			}
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00090F44 File Offset: 0x0008FF44
		private void FireComponentChanged(ToolStripDropDownItem parent)
		{
			if (parent != null)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null && parent.Site != null)
				{
					componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["DropDownItems"], null, null);
				}
				foreach (object obj in parent.DropDownItems)
				{
					ToolStripItem toolStripItem = (ToolStripItem)obj;
					ToolStripDropDownItem toolStripDropDownItem = toolStripItem as ToolStripDropDownItem;
					if (toolStripDropDownItem != null && toolStripDropDownItem.DropDownItems.Count > 1)
					{
						this.FireComponentChanged(toolStripDropDownItem);
					}
				}
			}
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00090FF8 File Offset: 0x0008FFF8
		public void GetGlyphs(ref GlyphCollection glyphs, Behavior standardBehavior)
		{
			if (this.ImmediateParent != null)
			{
				Rectangle glyphBounds = this.GetGlyphBounds();
				ToolStripDesignerUtils.GetAdjustedBounds(this.ToolStripItem, ref glyphBounds);
				BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
				if (behaviorService.ControlRectInAdornerWindow((Control)this.ImmediateParent).Contains(glyphBounds.Left, glyphBounds.Top))
				{
					if (this.ToolStripItem.IsOnDropDown)
					{
						ToolStrip toolStrip = this.ToolStripItem.GetCurrentParent();
						if (toolStrip == null)
						{
							toolStrip = this.ToolStripItem.Owner;
						}
						if (toolStrip != null && toolStrip.Visible)
						{
							glyphs.Add(new MiniLockedBorderGlyph(glyphBounds, SelectionBorderGlyphType.Top, standardBehavior, true));
							glyphs.Add(new MiniLockedBorderGlyph(glyphBounds, SelectionBorderGlyphType.Bottom, standardBehavior, true));
							glyphs.Add(new MiniLockedBorderGlyph(glyphBounds, SelectionBorderGlyphType.Left, standardBehavior, true));
							glyphs.Add(new MiniLockedBorderGlyph(glyphBounds, SelectionBorderGlyphType.Right, standardBehavior, true));
							return;
						}
					}
					else
					{
						glyphs.Add(new MiniLockedBorderGlyph(glyphBounds, SelectionBorderGlyphType.Top, standardBehavior, true));
						glyphs.Add(new MiniLockedBorderGlyph(glyphBounds, SelectionBorderGlyphType.Bottom, standardBehavior, true));
						glyphs.Add(new MiniLockedBorderGlyph(glyphBounds, SelectionBorderGlyphType.Left, standardBehavior, true));
						glyphs.Add(new MiniLockedBorderGlyph(glyphBounds, SelectionBorderGlyphType.Right, standardBehavior, true));
					}
				}
			}
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x00091128 File Offset: 0x00090128
		internal ToolStripDropDown GetFirstDropDown(ToolStripItem currentItem)
		{
			if (currentItem.Owner is ToolStripDropDown)
			{
				ToolStripDropDown toolStripDropDown = currentItem.Owner as ToolStripDropDown;
				while (toolStripDropDown.OwnerItem != null && toolStripDropDown.OwnerItem.Owner is ToolStripDropDown)
				{
					toolStripDropDown = toolStripDropDown.OwnerItem.Owner as ToolStripDropDown;
				}
				return toolStripDropDown;
			}
			return null;
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x0009117E File Offset: 0x0009017E
		private void HideDummyNode()
		{
			this.ToolStripItem.AutoSize = this.AutoSize;
			if (this._editorNode != null)
			{
				this._editorNode.CloseEditor();
				this._editorNode = null;
			}
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000911AC File Offset: 0x000901AC
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.AutoSize = this.ToolStripItem.AutoSize;
			this.Visible = true;
			this.currentVisible = this.Visible;
			this.AccessibleName = this.ToolStripItem.AccessibleName;
			this.ToolStripItem.Paint += this.OnItemPaint;
			this.ToolStripItem.AccessibleName = this.ToolStripItem.Name;
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRename += this.OnComponentRename;
			}
			this.selSvc = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (this.selSvc != null)
			{
				this.selSvc.SelectionChanged += this.OnSelectionChanged;
			}
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00091288 File Offset: 0x00090288
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			if (!this.internalCreate)
			{
				ISite site = base.Component.Site;
				if (site != null && base.Component is ToolStripDropDownItem)
				{
					if (defaultValues == null)
					{
						defaultValues = new Hashtable();
					}
					defaultValues["Text"] = site.Name;
					IComponent component = base.Component;
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.ToolStripItem)["Text"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType.Equals(typeof(string)))
					{
						string text = (string)propertyDescriptor.GetValue(component);
						if (text == null || text.Length == 0)
						{
							propertyDescriptor.SetValue(component, site.Name);
						}
					}
				}
			}
			base.InitializeNewComponent(defaultValues);
			if (base.Component is ToolStripTextBox || base.Component is ToolStripComboBox)
			{
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(base.Component)["Text"];
				if (propertyDescriptor2 != null && propertyDescriptor2.PropertyType == typeof(string) && !propertyDescriptor2.IsReadOnly && propertyDescriptor2.IsBrowsable)
				{
					propertyDescriptor2.SetValue(base.Component, "");
				}
			}
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x000913AC File Offset: 0x000903AC
		internal virtual ToolStripItem MorphCurrentItem(Type t)
		{
			ToolStripItem toolStripItem = null;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost == null)
			{
				return toolStripItem;
			}
			DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("ToolStripMorphingItemTransaction"));
			ToolStrip toolStrip = (ToolStrip)this.ImmediateParent;
			if (toolStrip is ToolStripOverflow)
			{
				toolStrip = this.ToolStripItem.Owner;
			}
			ToolStripMenuItemDesigner toolStripMenuItemDesigner = null;
			int num = toolStrip.Items.IndexOf(this.ToolStripItem);
			string name = this.ToolStripItem.Name;
			ToolStripItem toolStripItem2 = null;
			if (this.ToolStripItem.IsOnDropDown)
			{
				ToolStripDropDown toolStripDropDown = this.ImmediateParent as ToolStripDropDown;
				if (toolStripDropDown != null)
				{
					toolStripItem2 = toolStripDropDown.OwnerItem;
					if (toolStripItem2 != null)
					{
						toolStripMenuItemDesigner = (ToolStripMenuItemDesigner)designerHost.GetDesigner(toolStripItem2);
					}
				}
			}
			try
			{
				ToolStripDesigner._autoAddNewItems = false;
				ComponentSerializationService componentSerializationService = this.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
				if (componentSerializationService != null)
				{
					SerializationStore serializationStore = componentSerializationService.CreateStore();
					componentSerializationService.Serialize(serializationStore, base.Component);
					SerializationStore serializationStore2 = null;
					ToolStripDropDownItem toolStripDropDownItem = this.ToolStripItem as ToolStripDropDownItem;
					if (toolStripDropDownItem != null && typeof(ToolStripDropDownItem).IsAssignableFrom(t))
					{
						toolStripDropDownItem.HideDropDown();
						serializationStore2 = componentSerializationService.CreateStore();
						this.SerializeDropDownItems(toolStripDropDownItem, ref serializationStore2, componentSerializationService);
						serializationStore2.Close();
					}
					serializationStore.Close();
					IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						if (toolStrip.Site != null)
						{
							componentChangeService.OnComponentChanging(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"]);
						}
						else if (toolStripItem2 != null)
						{
							componentChangeService.OnComponentChanging(toolStripItem2, TypeDescriptor.GetProperties(toolStripItem2)["DropDownItems"]);
							componentChangeService.OnComponentChanged(toolStripItem2, TypeDescriptor.GetProperties(toolStripItem2)["DropDownItems"], null, null);
						}
					}
					this.FireComponentChanging(toolStripDropDownItem);
					toolStrip.Items.Remove(this.ToolStripItem);
					designerHost.DestroyComponent(this.ToolStripItem);
					ToolStripItem toolStripItem3 = (ToolStripItem)designerHost.CreateComponent(t, name);
					if (toolStripItem3 is ToolStripDropDownItem && serializationStore2 != null)
					{
						componentSerializationService.Deserialize(serializationStore2);
					}
					componentSerializationService.DeserializeTo(serializationStore, designerHost.Container, false, true);
					toolStripItem = (ToolStripItem)designerHost.Container.Components[name];
					if (toolStripItem.Image == null && toolStripItem is ToolStripButton)
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
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(toolStripItem)["Image"];
						if (propertyDescriptor != null && image != null)
						{
							propertyDescriptor.SetValue(toolStripItem, image);
						}
						PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(toolStripItem)["DisplayStyle"];
						if (propertyDescriptor2 != null)
						{
							propertyDescriptor2.SetValue(toolStripItem, ToolStripItemDisplayStyle.Image);
						}
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(toolStripItem)["ImageTransparentColor"];
						if (propertyDescriptor3 != null)
						{
							propertyDescriptor3.SetValue(toolStripItem, Color.Magenta);
						}
					}
					toolStrip.Items.Insert(num, toolStripItem);
					if (componentChangeService != null)
					{
						if (toolStrip.Site != null)
						{
							componentChangeService.OnComponentChanged(toolStrip, TypeDescriptor.GetProperties(toolStrip)["Items"], null, null);
						}
						else if (toolStripItem2 != null)
						{
							componentChangeService.OnComponentChanging(toolStripItem2, TypeDescriptor.GetProperties(toolStripItem2)["DropDownItems"]);
							componentChangeService.OnComponentChanged(toolStripItem2, TypeDescriptor.GetProperties(toolStripItem2)["DropDownItems"], null, null);
						}
					}
					this.FireComponentChanged(toolStripDropDownItem);
					if (toolStripItem.IsOnDropDown && toolStripMenuItemDesigner != null)
					{
						toolStripMenuItemDesigner.RemoveItemBodyGlyph(toolStripItem);
						toolStripMenuItemDesigner.AddItemBodyGlyph(toolStripItem);
					}
					ToolStripDesigner._autoAddNewItems = true;
					if (toolStripItem != null)
					{
						if (toolStripItem is ToolStripSeparator)
						{
							toolStrip.PerformLayout();
						}
						BehaviorService behaviorService = (BehaviorService)toolStripItem.Site.GetService(typeof(BehaviorService));
						if (behaviorService != null)
						{
							behaviorService.Invalidate();
						}
						ISelectionService selectionService = (ISelectionService)toolStripItem.Site.GetService(typeof(ISelectionService));
						if (selectionService != null)
						{
							selectionService.SetSelectedComponents(new object[] { toolStripItem }, SelectionTypes.Replace);
						}
					}
				}
			}
			catch
			{
				designerHost.Container.Add(this.ToolStripItem);
				toolStrip.Items.Insert(num, this.ToolStripItem);
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
			return toolStripItem;
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x0009182C File Offset: 0x0009082C
		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			if (e.Component == this.ToolStripItem)
			{
				this.ToolStripItem.AccessibleName = e.NewName;
			}
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x00091850 File Offset: 0x00090850
		private void OnItemPaint(object sender, PaintEventArgs e)
		{
			ToolStripDropDown toolStripDropDown = this.ToolStripItem.GetCurrentParent() as ToolStripDropDown;
			if (toolStripDropDown != null && this.selSvc != null && !this.IsEditorActive && this.ToolStripItem.Equals(this.selSvc.PrimarySelection))
			{
				BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
				if (behaviorService != null)
				{
					Point point = behaviorService.ControlToAdornerWindow((Control)this.ImmediateParent);
					Rectangle bounds = this.ToolStripItem.Bounds;
					bounds.Offset(point);
					bounds.Inflate(2, 2);
					behaviorService.ProcessPaintMessage(bounds);
				}
			}
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x000918E8 File Offset: 0x000908E8
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			ISelectionService selectionService = sender as ISelectionService;
			if (selectionService == null)
			{
				return;
			}
			ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
			ToolStripItem.ToolStripItemAccessibleObject toolStripItemAccessibleObject = this.ToolStripItem.AccessibilityObject as ToolStripItem.ToolStripItemAccessibleObject;
			if (toolStripItemAccessibleObject != null)
			{
				toolStripItemAccessibleObject.AddState(AccessibleStates.None);
				ToolStrip mainToolStrip = this.GetMainToolStrip();
				if (selectionService.GetComponentSelected(this.ToolStripItem))
				{
					ToolStrip toolStrip = this.ImmediateParent as ToolStrip;
					int num = 0;
					if (toolStrip != null)
					{
						num = toolStrip.Items.IndexOf(toolStripItem);
					}
					toolStripItemAccessibleObject.AddState(AccessibleStates.Selected);
					if (mainToolStrip != null)
					{
						UnsafeNativeMethods.NotifyWinEvent(32775, new HandleRef(toolStrip, toolStrip.Handle), -4, num + 1);
					}
					if (toolStripItem == this.ToolStripItem)
					{
						toolStripItemAccessibleObject.AddState(AccessibleStates.Focused);
						if (mainToolStrip != null)
						{
							UnsafeNativeMethods.NotifyWinEvent(32773, new HandleRef(toolStrip, toolStrip.Handle), -4, num + 1);
						}
					}
				}
			}
			if (toolStripItem != null && this.ToolStripItem != null && toolStripItem.IsOnDropDown && this.ToolStripItem.Equals(toolStripItem) && !(this.ToolStripItem is ToolStripMenuItem))
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					ToolStripDropDown toolStripDropDown = toolStripItem.Owner as ToolStripDropDown;
					if (toolStripDropDown != null && !toolStripDropDown.Visible)
					{
						ToolStripDropDownItem toolStripDropDownItem = toolStripDropDown.OwnerItem as ToolStripDropDownItem;
						if (toolStripDropDownItem != null)
						{
							ToolStripMenuItemDesigner toolStripMenuItemDesigner = (ToolStripMenuItemDesigner)designerHost.GetDesigner(toolStripDropDownItem);
							if (toolStripMenuItemDesigner != null)
							{
								toolStripMenuItemDesigner.InitializeDropDown();
							}
							SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
							if (selectionManager != null)
							{
								selectionManager.Refresh();
							}
						}
					}
				}
			}
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x00091A7C File Offset: 0x00090A7C
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "AutoSize", "AccessibleName", "Visible", "Overflow" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(ToolStripItemDesigner), propertyDescriptor, array2);
				}
			}
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x00091B00 File Offset: 0x00090B00
		public void RemoveItem()
		{
			this.dummyItemAdded = false;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost == null)
			{
				return;
			}
			ToolStrip toolStrip = (ToolStrip)this.ImmediateParent;
			if (toolStrip is ToolStripOverflow)
			{
				toolStrip = this.ParentComponent as ToolStrip;
			}
			toolStrip.Items.Remove(this.ToolStripItem);
			designerHost.DestroyComponent(this.ToolStripItem);
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x00091B6B File Offset: 0x00090B6B
		private void ResetAutoSize()
		{
			base.ShadowProperties["AutoSize"] = false;
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00091B83 File Offset: 0x00090B83
		private void RestoreAutoSize()
		{
			this.ToolStripItem.AutoSize = (bool)base.ShadowProperties["AutoSize"];
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00091BA5 File Offset: 0x00090BA5
		private void ResetVisible()
		{
			this.Visible = true;
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x00091BAE File Offset: 0x00090BAE
		private void RestoreOverflow()
		{
			this.ToolStripItem.Overflow = (ToolStripItemOverflow)base.ShadowProperties["Overflow"];
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00091BD0 File Offset: 0x00090BD0
		private void ResetOverflow()
		{
			this.ToolStripItem.Overflow = ToolStripItemOverflow.AsNeeded;
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x00091BDE File Offset: 0x00090BDE
		private void ResetAccessibleName()
		{
			base.ShadowProperties["AccessibleName"] = null;
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x00091BF1 File Offset: 0x00090BF1
		private void RestoreAccessibleName()
		{
			this.ToolStripItem.AccessibleName = (string)base.ShadowProperties["AccessibleName"];
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00091C14 File Offset: 0x00090C14
		internal void SelectNextItem(ISelectionService service, bool enterKeyPressed, ToolStripDesigner designer)
		{
			ToolStripDropDownItem toolStripDropDownItem = this.ToolStripItem as ToolStripDropDownItem;
			if (toolStripDropDownItem != null)
			{
				this.SetSelection(enterKeyPressed);
				return;
			}
			ToolStrip toolStrip = (ToolStrip)this.ImmediateParent;
			if (toolStrip is ToolStripOverflow)
			{
				toolStrip = this.ToolStripItem.Owner;
			}
			int num = toolStrip.Items.IndexOf(this.ToolStripItem);
			ToolStripItem toolStripItem = toolStrip.Items[num + 1];
			ToolStripKeyboardHandlingService toolStripKeyboardHandlingService = (ToolStripKeyboardHandlingService)this.GetService(typeof(ToolStripKeyboardHandlingService));
			if (toolStripKeyboardHandlingService != null)
			{
				if (toolStripItem == designer.EditorNode)
				{
					toolStripKeyboardHandlingService.SelectedDesignerControl = toolStripItem;
					this.selSvc.SetSelectedComponents(null, SelectionTypes.Replace);
					return;
				}
				toolStripKeyboardHandlingService.SelectedDesignerControl = null;
				this.selSvc.SetSelectedComponents(new object[] { toolStripItem });
			}
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00091CD8 File Offset: 0x00090CD8
		private void SerializeDropDownItems(ToolStripDropDownItem parent, ref SerializationStore _serializedDataForDropDownItems, ComponentSerializationService _serializationService)
		{
			foreach (object obj in parent.DropDownItems)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				if (!(toolStripItem is DesignerToolStripControlHost))
				{
					_serializationService.Serialize(_serializedDataForDropDownItems, toolStripItem);
					ToolStripDropDownItem toolStripDropDownItem = toolStripItem as ToolStripDropDownItem;
					if (toolStripDropDownItem != null)
					{
						this.SerializeDropDownItems(toolStripDropDownItem, ref _serializedDataForDropDownItems, _serializationService);
					}
				}
			}
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x00091D50 File Offset: 0x00090D50
		internal void SetItemVisible(bool toolStripSelected, ToolStripDesigner designer)
		{
			if (toolStripSelected)
			{
				if (!this.currentVisible)
				{
					this.ToolStripItem.Visible = true;
					if (designer != null && !designer.FireSyncSelection)
					{
						designer.FireSyncSelection = true;
						return;
					}
				}
			}
			else if (!this.currentVisible)
			{
				this.ToolStripItem.Visible = this.currentVisible;
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00091DA0 File Offset: 0x00090DA0
		private bool ShouldSerializeVisible()
		{
			return !this.Visible;
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00091DAB File Offset: 0x00090DAB
		private bool ShouldSerializeAutoSize()
		{
			return base.ShadowProperties.Contains("AutoSize");
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x00091DBD File Offset: 0x00090DBD
		private bool ShouldSerializeAccessibleName()
		{
			return base.ShadowProperties["AccessibleName"] != null;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00091DD5 File Offset: 0x00090DD5
		private bool ShouldSerializeOverflow()
		{
			return base.ShadowProperties["Overflow"] != null;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x00091DF0 File Offset: 0x00090DF0
		internal virtual void ShowEditNode(bool clicked)
		{
			if (this.ToolStripItem is ToolStripMenuItem)
			{
				if (this._editorNode == null)
				{
					this.CreateDummyNode();
				}
				IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
				ToolStrip toolStrip = this.ImmediateParent as ToolStrip;
				if (toolStrip != null)
				{
					ToolStripDesigner toolStripDesigner = (ToolStripDesigner)designerHost.GetDesigner(toolStrip);
					BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
					Point point = behaviorService.ControlToAdornerWindow(toolStrip);
					Rectangle bounds = this.ToolStripItem.Bounds;
					bounds.Offset(point);
					this.ToolStripItem.AutoSize = false;
					this._editorNode.SetWidth(this.ToolStripItem.Text);
					if (toolStrip.Orientation == Orientation.Horizontal)
					{
						this.ToolStripItem.Width = this._editorNode.EditorToolStrip.Width + 2;
					}
					else
					{
						this.ToolStripItem.Height = this._editorNode.EditorToolStrip.Height;
					}
					if (!this.dummyItemAdded)
					{
						behaviorService.SyncSelection();
					}
					if (this.ToolStripItem.Placement != ToolStripItemPlacement.None)
					{
						Rectangle rectangle = this.ToolStripItem.Bounds;
						rectangle.Offset(point);
						if (toolStrip.Orientation == Orientation.Horizontal)
						{
							rectangle.X++;
							rectangle.Y += (this.ToolStripItem.Height - this._editorNode.EditorToolStrip.Height) / 2;
							rectangle.Y++;
						}
						else
						{
							rectangle.X += (this.ToolStripItem.Width - this._editorNode.EditorToolStrip.Width) / 2;
							rectangle.X++;
						}
						this._editorNode.Bounds = rectangle;
						rectangle = Rectangle.Union(bounds, rectangle);
						behaviorService.Invalidate(rectangle);
						if (toolStripDesigner != null && toolStripDesigner.EditManager != null)
						{
							toolStripDesigner.EditManager.ActivateEditor(this.ToolStripItem, clicked);
						}
						SelectionManager selectionManager = (SelectionManager)this.GetService(typeof(SelectionManager));
						if (this.bodyGlyph != null)
						{
							selectionManager.BodyGlyphAdorner.Glyphs.Remove(this.bodyGlyph);
							return;
						}
					}
					else
					{
						this.ToolStripItem.AutoSize = this.AutoSize;
						if (this.ToolStripItem is ToolStripDropDownItem)
						{
							ToolStripDropDownItem toolStripDropDownItem = this.ToolStripItem as ToolStripDropDownItem;
							if (toolStripDropDownItem != null)
							{
								toolStripDropDownItem.HideDropDown();
							}
							this.selSvc.SetSelectedComponents(new object[] { this.ImmediateParent });
						}
					}
				}
			}
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00092080 File Offset: 0x00091080
		internal virtual bool SetSelection(bool enterKeyPressed)
		{
			return false;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x00092084 File Offset: 0x00091084
		internal override void ShowContextMenu(int x, int y)
		{
			ToolStripKeyboardHandlingService toolStripKeyboardHandlingService = (ToolStripKeyboardHandlingService)this.GetService(typeof(ToolStripKeyboardHandlingService));
			if (toolStripKeyboardHandlingService != null)
			{
				if (!toolStripKeyboardHandlingService.ContextMenuShownByKeyBoard)
				{
					BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
					Point point = Point.Empty;
					if (behaviorService != null)
					{
						point = behaviorService.ScreenToAdornerWindow(new Point(x, y));
					}
					if (this.GetGlyphBounds().Contains(point))
					{
						this.DesignerContextMenu.Show(x, y);
						return;
					}
				}
				else
				{
					toolStripKeyboardHandlingService.ContextMenuShownByKeyBoard = false;
					this.DesignerContextMenu.Show(x, y);
				}
			}
		}

		// Token: 0x04001526 RID: 5414
		private const int GLYPHBORDER = 1;

		// Token: 0x04001527 RID: 5415
		private const int GLYPHINSET = 2;

		// Token: 0x04001528 RID: 5416
		private ToolStripTemplateNode _editorNode;

		// Token: 0x04001529 RID: 5417
		private bool isEditorActive;

		// Token: 0x0400152A RID: 5418
		private bool internalCreate;

		// Token: 0x0400152B RID: 5419
		private ISelectionService selSvc;

		// Token: 0x0400152C RID: 5420
		private bool currentVisible;

		// Token: 0x0400152D RID: 5421
		private Rectangle lastInsertionMarkRect = Rectangle.Empty;

		// Token: 0x0400152E RID: 5422
		internal ControlBodyGlyph bodyGlyph;

		// Token: 0x0400152F RID: 5423
		internal bool dummyItemAdded;

		// Token: 0x04001530 RID: 5424
		internal Rectangle dragBoxFromMouseDown = Rectangle.Empty;

		// Token: 0x04001531 RID: 5425
		internal int indexOfItemUnderMouseToDrag = -1;

		// Token: 0x04001532 RID: 5426
		private ToolStripItemCustomMenuItemCollection toolStripItemCustomMenuItemCollection;
	}
}
