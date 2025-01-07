using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class ToolStripItemCustomMenuItemCollection : CustomMenuItemCollection
	{
		public ToolStripItemCustomMenuItemCollection(IServiceProvider provider, Component currentItem)
		{
			this.serviceProvider = provider;
			this.currentItem = currentItem as ToolStripItem;
			this.PopulateList();
		}

		private ToolStrip ParentTool
		{
			get
			{
				return this.currentItem.Owner;
			}
		}

		private ToolStripMenuItem CreatePropertyBasedItem(string text, string propertyName, string imageName)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text);
			bool flag = this.IsPropertyBrowsable(propertyName);
			toolStripMenuItem.Visible = flag;
			if (flag)
			{
				if (!string.IsNullOrEmpty(imageName))
				{
					toolStripMenuItem.Image = new Bitmap(typeof(ToolStripMenuItem), imageName);
					toolStripMenuItem.ImageTransparentColor = Color.Magenta;
				}
				IUIService iuiservice = this.serviceProvider.GetService(typeof(IUIService)) as IUIService;
				if (iuiservice != null)
				{
					toolStripMenuItem.DropDown.Renderer = (ToolStripProfessionalRenderer)iuiservice.Styles["VsRenderer"];
					toolStripMenuItem.DropDown.Font = (Font)iuiservice.Styles["DialogFont"];
				}
			}
			return toolStripMenuItem;
		}

		private ToolStripMenuItem CreateEnumValueItem(string propertyName, string name, object value)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(name);
			toolStripMenuItem.Tag = new ToolStripItemCustomMenuItemCollection.EnumValueDescription(propertyName, value);
			toolStripMenuItem.Click += this.OnEnumValueChanged;
			return toolStripMenuItem;
		}

		private ToolStripMenuItem CreateBooleanItem(string text, string propertyName)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text);
			bool flag = this.IsPropertyBrowsable(propertyName);
			toolStripMenuItem.Visible = flag;
			toolStripMenuItem.Tag = propertyName;
			toolStripMenuItem.CheckOnClick = true;
			toolStripMenuItem.Click += this.OnBooleanValueChanged;
			return toolStripMenuItem;
		}

		private void PopulateList()
		{
			ToolStripItem toolStripItem = this.currentItem;
			if (!(toolStripItem is ToolStripControlHost) && !(toolStripItem is ToolStripSeparator))
			{
				this.imageToolStripMenuItem = new ToolStripMenuItem();
				this.imageToolStripMenuItem.Text = SR.GetString("ToolStripItemContextMenuSetImage");
				this.imageToolStripMenuItem.Image = new Bitmap(typeof(ToolStripMenuItem), "image.bmp");
				this.imageToolStripMenuItem.ImageTransparentColor = Color.Magenta;
				this.imageToolStripMenuItem.Click += this.OnImageToolStripMenuItemClick;
				this.enabledToolStripMenuItem = this.CreateBooleanItem("E&nabled", "Enabled");
				base.AddRange(new ToolStripItem[] { this.imageToolStripMenuItem, this.enabledToolStripMenuItem });
				if (toolStripItem is ToolStripMenuItem)
				{
					this.checkedToolStripMenuItem = this.CreateBooleanItem("C&hecked", "Checked");
					this.showShortcutKeysToolStripMenuItem = this.CreateBooleanItem("ShowShortcut&Keys", "ShowShortcutKeys");
					base.AddRange(new ToolStripItem[] { this.checkedToolStripMenuItem, this.showShortcutKeysToolStripMenuItem });
				}
				else
				{
					if (toolStripItem is ToolStripLabel)
					{
						this.isLinkToolStripMenuItem = this.CreateBooleanItem("IsLin&k", "IsLink");
						base.Add(this.isLinkToolStripMenuItem);
					}
					if (toolStripItem is ToolStripStatusLabel)
					{
						this.springToolStripMenuItem = this.CreateBooleanItem("Sprin&g", "Spring");
						base.Add(this.springToolStripMenuItem);
					}
					this.leftToolStripMenuItem = this.CreateEnumValueItem("Alignment", "Left", ToolStripItemAlignment.Left);
					this.rightToolStripMenuItem = this.CreateEnumValueItem("Alignment", "Right", ToolStripItemAlignment.Right);
					this.noneStyleToolStripMenuItem = this.CreateEnumValueItem("DisplayStyle", "None", ToolStripItemDisplayStyle.None);
					this.textStyleToolStripMenuItem = this.CreateEnumValueItem("DisplayStyle", "Text", ToolStripItemDisplayStyle.Text);
					this.imageStyleToolStripMenuItem = this.CreateEnumValueItem("DisplayStyle", "Image", ToolStripItemDisplayStyle.Image);
					this.imageTextStyleToolStripMenuItem = this.CreateEnumValueItem("DisplayStyle", "ImageAndText", ToolStripItemDisplayStyle.ImageAndText);
					this.alignmentToolStripMenuItem = this.CreatePropertyBasedItem("Ali&gnment", "Alignment", "alignment.bmp");
					this.alignmentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.leftToolStripMenuItem, this.rightToolStripMenuItem });
					this.displayStyleToolStripMenuItem = this.CreatePropertyBasedItem("Displa&yStyle", "DisplayStyle", "displaystyle.bmp");
					this.displayStyleToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.noneStyleToolStripMenuItem, this.textStyleToolStripMenuItem, this.imageStyleToolStripMenuItem, this.imageTextStyleToolStripMenuItem });
					base.AddRange(new ToolStripItem[] { this.alignmentToolStripMenuItem, this.displayStyleToolStripMenuItem });
				}
				this.toolStripSeparator1 = new ToolStripSeparator();
				base.Add(this.toolStripSeparator1);
			}
			this.convertToolStripMenuItem = new ToolStripMenuItem();
			this.convertToolStripMenuItem.Text = SR.GetString("ToolStripItemContextMenuConvertTo");
			this.convertToolStripMenuItem.DropDown = ToolStripDesignerUtils.GetNewItemDropDown(this.ParentTool, this.currentItem, new EventHandler(this.AddNewItemClick), true, this.serviceProvider);
			this.insertToolStripMenuItem = new ToolStripMenuItem();
			this.insertToolStripMenuItem.Text = SR.GetString("ToolStripItemContextMenuInsert");
			this.insertToolStripMenuItem.DropDown = ToolStripDesignerUtils.GetNewItemDropDown(this.ParentTool, this.currentItem, new EventHandler(this.AddNewItemClick), false, this.serviceProvider);
			base.AddRange(new ToolStripItem[] { this.convertToolStripMenuItem, this.insertToolStripMenuItem });
			if (this.currentItem is ToolStripDropDownItem)
			{
				IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(this.currentItem) as ToolStripItemDesigner;
					if (toolStripItemDesigner != null)
					{
						this.verbManager = new CollectionEditVerbManager(SR.GetString("ToolStripDropDownItemCollectionEditorVerb"), toolStripItemDesigner, TypeDescriptor.GetProperties(this.currentItem)["DropDownItems"], false);
						this.editItemsToolStripMenuItem = new ToolStripMenuItem();
						this.editItemsToolStripMenuItem.Text = SR.GetString("ToolStripDropDownItemCollectionEditorVerb");
						this.editItemsToolStripMenuItem.Click += this.OnEditItemsMenuItemClick;
						this.editItemsToolStripMenuItem.Image = new Bitmap(typeof(ToolStripMenuItem), "editdropdownlist.bmp");
						this.editItemsToolStripMenuItem.ImageTransparentColor = Color.Magenta;
						base.Add(this.editItemsToolStripMenuItem);
					}
				}
			}
		}

		private void OnEditItemsMenuItemClick(object sender, EventArgs e)
		{
			if (this.verbManager != null)
			{
				this.verbManager.EditItemsVerb.Invoke();
			}
		}

		private void OnImageToolStripMenuItemClick(object sender, EventArgs e)
		{
			IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				ToolStripItemDesigner toolStripItemDesigner = designerHost.GetDesigner(this.currentItem) as ToolStripItemDesigner;
				if (toolStripItemDesigner != null)
				{
					try
					{
						EditorServiceContext.EditValue(toolStripItemDesigner, this.currentItem, "Image");
					}
					catch (InvalidOperationException ex)
					{
						IUIService iuiservice = (IUIService)this.serviceProvider.GetService(typeof(IUIService));
						iuiservice.ShowError(ex.Message);
					}
				}
			}
		}

		private void OnBooleanValueChanged(object sender, EventArgs e)
		{
			ToolStripItem toolStripItem = sender as ToolStripItem;
			if (toolStripItem != null)
			{
				string text = toolStripItem.Tag as string;
				if (text != null)
				{
					bool flag = (bool)this.GetProperty(text);
					this.ChangeProperty(text, !flag);
				}
			}
		}

		private void OnEnumValueChanged(object sender, EventArgs e)
		{
			ToolStripItem toolStripItem = sender as ToolStripItem;
			if (toolStripItem != null)
			{
				ToolStripItemCustomMenuItemCollection.EnumValueDescription enumValueDescription = toolStripItem.Tag as ToolStripItemCustomMenuItemCollection.EnumValueDescription;
				if (enumValueDescription != null && !string.IsNullOrEmpty(enumValueDescription.PropertyName))
				{
					this.ChangeProperty(enumValueDescription.PropertyName, enumValueDescription.Value);
				}
			}
		}

		private void AddNewItemClick(object sender, EventArgs e)
		{
			ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = (ItemTypeToolStripMenuItem)sender;
			Type itemType = itemTypeToolStripMenuItem.ItemType;
			if (itemTypeToolStripMenuItem.ConvertTo)
			{
				this.MorphToolStripItem(itemType);
				return;
			}
			this.InsertItem(itemType);
		}

		private void MorphToolStripItem(Type t)
		{
			if (t != this.currentItem.GetType())
			{
				IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
				ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)designerHost.GetDesigner(this.currentItem);
				toolStripItemDesigner.MorphCurrentItem(t);
			}
		}

		private void InsertItem(Type t)
		{
			ToolStripMenuItem toolStripMenuItem = this.currentItem as ToolStripMenuItem;
			if (toolStripMenuItem != null)
			{
				this.InsertMenuItem(t);
				return;
			}
			this.InsertStripItem(t);
		}

		private void InsertStripItem(Type t)
		{
			StatusStrip statusStrip = this.ParentTool as StatusStrip;
			if (statusStrip != null)
			{
				this.InsertIntoStatusStrip(statusStrip, t);
				return;
			}
			this.InsertToolStripItem(t);
		}

		private void InsertMenuItem(Type t)
		{
			MenuStrip menuStrip = this.ParentTool as MenuStrip;
			if (menuStrip != null)
			{
				this.InsertIntoMainMenu(menuStrip, t);
				return;
			}
			this.InsertIntoDropDown((ToolStripDropDown)this.currentItem.Owner, t);
		}

		private void TryCancelTransaction(ref DesignerTransaction transaction)
		{
			if (transaction != null)
			{
				try
				{
					transaction.Cancel();
					transaction = null;
				}
				catch
				{
				}
			}
		}

		private void InsertIntoDropDown(ToolStripDropDown parent, Type t)
		{
			IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			int num = parent.Items.IndexOf(this.currentItem);
			if (parent != null)
			{
				ToolStripDropDownItem toolStripDropDownItem = parent.OwnerItem as ToolStripDropDownItem;
				if (toolStripDropDownItem != null && (toolStripDropDownItem.DropDownDirection == ToolStripDropDownDirection.AboveLeft || toolStripDropDownItem.DropDownDirection == ToolStripDropDownDirection.AboveRight))
				{
					num++;
				}
			}
			DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("ToolStripAddingItem"));
			try
			{
				IComponent component = designerHost.CreateComponent(t);
				IDesigner designer = designerHost.GetDesigner(component);
				if (designer is ComponentDesigner)
				{
					((ComponentDesigner)designer).InitializeNewComponent(null);
				}
				parent.Items.Insert(num, (ToolStripItem)component);
				ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new object[] { component }, SelectionTypes.Replace);
				}
			}
			catch (Exception ex)
			{
				if (parent != null && parent.OwnerItem != null && parent.OwnerItem.Owner != null)
				{
					ToolStripDesigner toolStripDesigner = designerHost.GetDesigner(parent.OwnerItem.Owner) as ToolStripDesigner;
					if (toolStripDesigner != null)
					{
						toolStripDesigner.CancelPendingMenuItemTransaction();
					}
				}
				this.TryCancelTransaction(ref designerTransaction);
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

		private void InsertIntoMainMenu(MenuStrip parent, Type t)
		{
			IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			int num = parent.Items.IndexOf(this.currentItem);
			DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("ToolStripAddingItem"));
			try
			{
				IComponent component = designerHost.CreateComponent(t);
				IDesigner designer = designerHost.GetDesigner(component);
				if (designer is ComponentDesigner)
				{
					((ComponentDesigner)designer).InitializeNewComponent(null);
				}
				parent.Items.Insert(num, (ToolStripItem)component);
				ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new object[] { component }, SelectionTypes.Replace);
				}
			}
			catch (Exception ex)
			{
				this.TryCancelTransaction(ref designerTransaction);
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

		private void InsertIntoStatusStrip(StatusStrip parent, Type t)
		{
			IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			int num = parent.Items.IndexOf(this.currentItem);
			DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("ToolStripAddingItem"));
			try
			{
				IComponent component = designerHost.CreateComponent(t);
				IDesigner designer = designerHost.GetDesigner(component);
				if (designer is ComponentDesigner)
				{
					((ComponentDesigner)designer).InitializeNewComponent(null);
				}
				parent.Items.Insert(num, (ToolStripItem)component);
				ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new object[] { component }, SelectionTypes.Replace);
				}
			}
			catch (Exception ex)
			{
				this.TryCancelTransaction(ref designerTransaction);
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

		private void InsertToolStripItem(Type t)
		{
			IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			ToolStrip parentTool = this.ParentTool;
			int num = parentTool.Items.IndexOf(this.currentItem);
			DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("ToolStripAddingItem"));
			try
			{
				IComponent component = designerHost.CreateComponent(t);
				IDesigner designer = designerHost.GetDesigner(component);
				if (designer is ComponentDesigner)
				{
					((ComponentDesigner)designer).InitializeNewComponent(null);
				}
				if (component is ToolStripButton || component is ToolStripSplitButton || component is ToolStripDropDownButton)
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
					this.ChangeProperty(component, "Image", image);
					this.ChangeProperty(component, "DisplayStyle", ToolStripItemDisplayStyle.Image);
					this.ChangeProperty(component, "ImageTransparentColor", Color.Magenta);
				}
				parentTool.Items.Insert(num, (ToolStripItem)component);
				ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
				if (selectionService != null)
				{
					selectionService.SetSelectedComponents(new object[] { component }, SelectionTypes.Replace);
				}
			}
			catch (Exception ex2)
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
				if (ClientUtils.IsCriticalException(ex2))
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

		private bool IsPropertyBrowsable(string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.currentItem)[propertyName];
			if (propertyDescriptor != null)
			{
				BrowsableAttribute browsableAttribute = propertyDescriptor.Attributes[typeof(BrowsableAttribute)] as BrowsableAttribute;
				if (browsableAttribute != null)
				{
					return browsableAttribute.Browsable;
				}
			}
			return true;
		}

		private object GetProperty(string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.currentItem)[propertyName];
			if (propertyDescriptor != null)
			{
				return propertyDescriptor.GetValue(this.currentItem);
			}
			return null;
		}

		protected void ChangeProperty(string propertyName, object value)
		{
			this.ChangeProperty(this.currentItem, propertyName, value);
		}

		protected void ChangeProperty(IComponent target, string propertyName, object value)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(target)[propertyName];
			try
			{
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(target, value);
				}
			}
			catch (InvalidOperationException ex)
			{
				IUIService iuiservice = (IUIService)this.serviceProvider.GetService(typeof(IUIService));
				iuiservice.ShowError(ex.Message);
			}
		}

		private void RefreshAlignment()
		{
			ToolStripItemAlignment toolStripItemAlignment = (ToolStripItemAlignment)this.GetProperty("Alignment");
			this.leftToolStripMenuItem.Checked = toolStripItemAlignment == ToolStripItemAlignment.Left;
			this.rightToolStripMenuItem.Checked = toolStripItemAlignment == ToolStripItemAlignment.Right;
		}

		private void RefreshDisplayStyle()
		{
			ToolStripItemDisplayStyle toolStripItemDisplayStyle = (ToolStripItemDisplayStyle)this.GetProperty("DisplayStyle");
			this.noneStyleToolStripMenuItem.Checked = toolStripItemDisplayStyle == ToolStripItemDisplayStyle.None;
			this.textStyleToolStripMenuItem.Checked = toolStripItemDisplayStyle == ToolStripItemDisplayStyle.Text;
			this.imageStyleToolStripMenuItem.Checked = toolStripItemDisplayStyle == ToolStripItemDisplayStyle.Image;
			this.imageTextStyleToolStripMenuItem.Checked = toolStripItemDisplayStyle == ToolStripItemDisplayStyle.ImageAndText;
		}

		public override void RefreshItems()
		{
			base.RefreshItems();
			ToolStripItem toolStripItem = this.currentItem;
			if (!(toolStripItem is ToolStripControlHost) && !(toolStripItem is ToolStripSeparator))
			{
				this.enabledToolStripMenuItem.Checked = (bool)this.GetProperty("Enabled");
				if (toolStripItem is ToolStripMenuItem)
				{
					this.checkedToolStripMenuItem.Checked = (bool)this.GetProperty("Checked");
					this.showShortcutKeysToolStripMenuItem.Checked = (bool)this.GetProperty("ShowShortcutKeys");
					return;
				}
				if (toolStripItem is ToolStripLabel)
				{
					this.isLinkToolStripMenuItem.Checked = (bool)this.GetProperty("IsLink");
				}
				this.RefreshAlignment();
				this.RefreshDisplayStyle();
			}
		}

		private ToolStripItem currentItem;

		private IServiceProvider serviceProvider;

		private ToolStripMenuItem imageToolStripMenuItem;

		private ToolStripMenuItem enabledToolStripMenuItem;

		private ToolStripMenuItem isLinkToolStripMenuItem;

		private ToolStripMenuItem springToolStripMenuItem;

		private ToolStripMenuItem checkedToolStripMenuItem;

		private ToolStripMenuItem showShortcutKeysToolStripMenuItem;

		private ToolStripMenuItem alignmentToolStripMenuItem;

		private ToolStripMenuItem displayStyleToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem convertToolStripMenuItem;

		private ToolStripMenuItem insertToolStripMenuItem;

		private ToolStripMenuItem leftToolStripMenuItem;

		private ToolStripMenuItem rightToolStripMenuItem;

		private ToolStripMenuItem noneStyleToolStripMenuItem;

		private ToolStripMenuItem textStyleToolStripMenuItem;

		private ToolStripMenuItem imageStyleToolStripMenuItem;

		private ToolStripMenuItem imageTextStyleToolStripMenuItem;

		private ToolStripMenuItem editItemsToolStripMenuItem;

		private CollectionEditVerbManager verbManager;

		private class EnumValueDescription
		{
			public EnumValueDescription(string propertyName, object value)
			{
				this.PropertyName = propertyName;
				this.Value = value;
			}

			public string PropertyName;

			public object Value;
		}
	}
}
