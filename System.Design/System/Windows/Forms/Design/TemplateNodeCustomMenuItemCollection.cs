using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class TemplateNodeCustomMenuItemCollection : CustomMenuItemCollection
	{
		public TemplateNodeCustomMenuItemCollection(IServiceProvider provider, Component currentItem)
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

		private void PopulateList()
		{
			this.insertToolStripMenuItem = new ToolStripMenuItem();
			this.insertToolStripMenuItem.Text = SR.GetString("ToolStripItemContextMenuInsert");
			this.insertToolStripMenuItem.DropDown = ToolStripDesignerUtils.GetNewItemDropDown(this.ParentTool, this.currentItem, new EventHandler(this.AddNewItemClick), false, this.serviceProvider);
			base.Add(this.insertToolStripMenuItem);
		}

		private void AddNewItemClick(object sender, EventArgs e)
		{
			ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = (ItemTypeToolStripMenuItem)sender;
			Type itemType = itemTypeToolStripMenuItem.ItemType;
			this.InsertItem(itemType);
		}

		private void InsertItem(Type t)
		{
			this.InsertToolStripItem(t);
		}

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

		private ToolStripItem currentItem;

		private IServiceProvider serviceProvider;

		private ToolStripMenuItem insertToolStripMenuItem;
	}
}
