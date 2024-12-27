using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002B9 RID: 697
	internal class ToolStripItemCustomMenuItemCollection : CustomMenuItemCollection
	{
		// Token: 0x06001A25 RID: 6693 RVA: 0x0008D77E File Offset: 0x0008C77E
		public ToolStripItemCustomMenuItemCollection(IServiceProvider provider, Component currentItem)
		{
			this.serviceProvider = provider;
			this.currentItem = currentItem as ToolStripItem;
			this.PopulateList();
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x0008D79F File Offset: 0x0008C79F
		private ToolStrip ParentTool
		{
			get
			{
				return this.currentItem.Owner;
			}
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x0008D7AC File Offset: 0x0008C7AC
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

		// Token: 0x06001A28 RID: 6696 RVA: 0x0008D860 File Offset: 0x0008C860
		private ToolStripMenuItem CreateEnumValueItem(string propertyName, string name, object value)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(name);
			toolStripMenuItem.Tag = new ToolStripItemCustomMenuItemCollection.EnumValueDescription(propertyName, value);
			toolStripMenuItem.Click += this.OnEnumValueChanged;
			return toolStripMenuItem;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0008D894 File Offset: 0x0008C894
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

		// Token: 0x06001A2A RID: 6698 RVA: 0x0008D8D8 File Offset: 0x0008C8D8
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

		// Token: 0x06001A2B RID: 6699 RVA: 0x0008DD85 File Offset: 0x0008CD85
		private void OnEditItemsMenuItemClick(object sender, EventArgs e)
		{
			if (this.verbManager != null)
			{
				this.verbManager.EditItemsVerb.Invoke();
			}
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x0008DDA0 File Offset: 0x0008CDA0
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

		// Token: 0x06001A2D RID: 6701 RVA: 0x0008DE30 File Offset: 0x0008CE30
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

		// Token: 0x06001A2E RID: 6702 RVA: 0x0008DE74 File Offset: 0x0008CE74
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

		// Token: 0x06001A2F RID: 6703 RVA: 0x0008DEBC File Offset: 0x0008CEBC
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

		// Token: 0x06001A30 RID: 6704 RVA: 0x0008DEF0 File Offset: 0x0008CEF0
		private void MorphToolStripItem(Type t)
		{
			if (t != this.currentItem.GetType())
			{
				IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
				ToolStripItemDesigner toolStripItemDesigner = (ToolStripItemDesigner)designerHost.GetDesigner(this.currentItem);
				toolStripItemDesigner.MorphCurrentItem(t);
			}
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0008DF40 File Offset: 0x0008CF40
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

		// Token: 0x06001A32 RID: 6706 RVA: 0x0008DF6C File Offset: 0x0008CF6C
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

		// Token: 0x06001A33 RID: 6707 RVA: 0x0008DF98 File Offset: 0x0008CF98
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

		// Token: 0x06001A34 RID: 6708 RVA: 0x0008DFD4 File Offset: 0x0008CFD4
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

		// Token: 0x06001A35 RID: 6709 RVA: 0x0008E004 File Offset: 0x0008D004
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

		// Token: 0x06001A36 RID: 6710 RVA: 0x0008E16C File Offset: 0x0008D16C
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

		// Token: 0x06001A37 RID: 6711 RVA: 0x0008E270 File Offset: 0x0008D270
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

		// Token: 0x06001A38 RID: 6712 RVA: 0x0008E374 File Offset: 0x0008D374
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

		// Token: 0x06001A39 RID: 6713 RVA: 0x0008E534 File Offset: 0x0008D534
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

		// Token: 0x06001A3A RID: 6714 RVA: 0x0008E57C File Offset: 0x0008D57C
		private object GetProperty(string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.currentItem)[propertyName];
			if (propertyDescriptor != null)
			{
				return propertyDescriptor.GetValue(this.currentItem);
			}
			return null;
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0008E5AC File Offset: 0x0008D5AC
		protected void ChangeProperty(string propertyName, object value)
		{
			this.ChangeProperty(this.currentItem, propertyName, value);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x0008E5BC File Offset: 0x0008D5BC
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

		// Token: 0x06001A3D RID: 6717 RVA: 0x0008E620 File Offset: 0x0008D620
		private void RefreshAlignment()
		{
			ToolStripItemAlignment toolStripItemAlignment = (ToolStripItemAlignment)this.GetProperty("Alignment");
			this.leftToolStripMenuItem.Checked = toolStripItemAlignment == ToolStripItemAlignment.Left;
			this.rightToolStripMenuItem.Checked = toolStripItemAlignment == ToolStripItemAlignment.Right;
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0008E664 File Offset: 0x0008D664
		private void RefreshDisplayStyle()
		{
			ToolStripItemDisplayStyle toolStripItemDisplayStyle = (ToolStripItemDisplayStyle)this.GetProperty("DisplayStyle");
			this.noneStyleToolStripMenuItem.Checked = toolStripItemDisplayStyle == ToolStripItemDisplayStyle.None;
			this.textStyleToolStripMenuItem.Checked = toolStripItemDisplayStyle == ToolStripItemDisplayStyle.Text;
			this.imageStyleToolStripMenuItem.Checked = toolStripItemDisplayStyle == ToolStripItemDisplayStyle.Image;
			this.imageTextStyleToolStripMenuItem.Checked = toolStripItemDisplayStyle == ToolStripItemDisplayStyle.ImageAndText;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0008E6D0 File Offset: 0x0008D6D0
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

		// Token: 0x040014EA RID: 5354
		private ToolStripItem currentItem;

		// Token: 0x040014EB RID: 5355
		private IServiceProvider serviceProvider;

		// Token: 0x040014EC RID: 5356
		private ToolStripMenuItem imageToolStripMenuItem;

		// Token: 0x040014ED RID: 5357
		private ToolStripMenuItem enabledToolStripMenuItem;

		// Token: 0x040014EE RID: 5358
		private ToolStripMenuItem isLinkToolStripMenuItem;

		// Token: 0x040014EF RID: 5359
		private ToolStripMenuItem springToolStripMenuItem;

		// Token: 0x040014F0 RID: 5360
		private ToolStripMenuItem checkedToolStripMenuItem;

		// Token: 0x040014F1 RID: 5361
		private ToolStripMenuItem showShortcutKeysToolStripMenuItem;

		// Token: 0x040014F2 RID: 5362
		private ToolStripMenuItem alignmentToolStripMenuItem;

		// Token: 0x040014F3 RID: 5363
		private ToolStripMenuItem displayStyleToolStripMenuItem;

		// Token: 0x040014F4 RID: 5364
		private ToolStripSeparator toolStripSeparator1;

		// Token: 0x040014F5 RID: 5365
		private ToolStripMenuItem convertToolStripMenuItem;

		// Token: 0x040014F6 RID: 5366
		private ToolStripMenuItem insertToolStripMenuItem;

		// Token: 0x040014F7 RID: 5367
		private ToolStripMenuItem leftToolStripMenuItem;

		// Token: 0x040014F8 RID: 5368
		private ToolStripMenuItem rightToolStripMenuItem;

		// Token: 0x040014F9 RID: 5369
		private ToolStripMenuItem noneStyleToolStripMenuItem;

		// Token: 0x040014FA RID: 5370
		private ToolStripMenuItem textStyleToolStripMenuItem;

		// Token: 0x040014FB RID: 5371
		private ToolStripMenuItem imageStyleToolStripMenuItem;

		// Token: 0x040014FC RID: 5372
		private ToolStripMenuItem imageTextStyleToolStripMenuItem;

		// Token: 0x040014FD RID: 5373
		private ToolStripMenuItem editItemsToolStripMenuItem;

		// Token: 0x040014FE RID: 5374
		private CollectionEditVerbManager verbManager;

		// Token: 0x020002BA RID: 698
		private class EnumValueDescription
		{
			// Token: 0x06001A40 RID: 6720 RVA: 0x0008E789 File Offset: 0x0008D789
			public EnumValueDescription(string propertyName, object value)
			{
				this.PropertyName = propertyName;
				this.Value = value;
			}

			// Token: 0x040014FF RID: 5375
			public string PropertyName;

			// Token: 0x04001500 RID: 5376
			public object Value;
		}
	}
}
