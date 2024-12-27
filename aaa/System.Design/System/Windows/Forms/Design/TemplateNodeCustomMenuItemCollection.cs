using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002A6 RID: 678
	internal class TemplateNodeCustomMenuItemCollection : CustomMenuItemCollection
	{
		// Token: 0x0600197F RID: 6527 RVA: 0x00089749 File Offset: 0x00088749
		public TemplateNodeCustomMenuItemCollection(IServiceProvider provider, Component currentItem)
		{
			this.serviceProvider = provider;
			this.currentItem = currentItem as ToolStripItem;
			this.PopulateList();
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0008976A File Offset: 0x0008876A
		private ToolStrip ParentTool
		{
			get
			{
				return this.currentItem.Owner;
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00089778 File Offset: 0x00088778
		private void PopulateList()
		{
			this.insertToolStripMenuItem = new ToolStripMenuItem();
			this.insertToolStripMenuItem.Text = SR.GetString("ToolStripItemContextMenuInsert");
			this.insertToolStripMenuItem.DropDown = ToolStripDesignerUtils.GetNewItemDropDown(this.ParentTool, this.currentItem, new EventHandler(this.AddNewItemClick), false, this.serviceProvider);
			base.Add(this.insertToolStripMenuItem);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x000897E4 File Offset: 0x000887E4
		private void AddNewItemClick(object sender, EventArgs e)
		{
			ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = (ItemTypeToolStripMenuItem)sender;
			Type itemType = itemTypeToolStripMenuItem.ItemType;
			this.InsertItem(itemType);
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00089806 File Offset: 0x00088806
		private void InsertItem(Type t)
		{
			this.InsertToolStripItem(t);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00089810 File Offset: 0x00088810
		private void InsertToolStripItem(Type t)
		{
			IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
			ToolStrip parentTool = this.ParentTool;
			int num = parentTool.Items.IndexOf(this.currentItem);
			DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("ToolStripAddingItem"));
			try
			{
				ToolStripDesigner._autoAddNewItems = false;
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
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["Image"];
					if (propertyDescriptor != null && image != null)
					{
						propertyDescriptor.SetValue(component, image);
					}
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(component)["DisplayStyle"];
					if (propertyDescriptor2 != null)
					{
						propertyDescriptor2.SetValue(component, ToolStripItemDisplayStyle.Image);
					}
					PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(component)["ImageTransparentColor"];
					if (propertyDescriptor3 != null)
					{
						propertyDescriptor3.SetValue(component, Color.Magenta);
					}
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
				ToolStripDesigner._autoAddNewItems = true;
				ToolStripDropDown toolStripDropDown = parentTool as ToolStripDropDown;
				if (toolStripDropDown != null && toolStripDropDown.Visible)
				{
					ToolStripDropDownItem toolStripDropDownItem = toolStripDropDown.OwnerItem as ToolStripDropDownItem;
					if (toolStripDropDownItem != null)
					{
						ToolStripMenuItemDesigner toolStripMenuItemDesigner = designerHost.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
						if (toolStripMenuItemDesigner != null)
						{
							toolStripMenuItemDesigner.ResetGlyphs(toolStripDropDownItem);
						}
					}
				}
			}
		}

		// Token: 0x0400149D RID: 5277
		private ToolStripItem currentItem;

		// Token: 0x0400149E RID: 5278
		private IServiceProvider serviceProvider;

		// Token: 0x0400149F RID: 5279
		private ToolStripMenuItem insertToolStripMenuItem;
	}
}
