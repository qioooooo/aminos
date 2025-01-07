using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class BindingNavigatorDesigner : ToolStripDesigner
	{
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoved += this.ComponentChangeSvc_ComponentRemoved;
				componentChangeService.ComponentChanged += this.ComponentChangeSvc_ComponentChanged;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRemoved -= this.ComponentChangeSvc_ComponentRemoved;
					componentChangeService.ComponentChanged -= this.ComponentChangeSvc_ComponentChanged;
				}
			}
			base.Dispose(disposing);
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			BindingNavigator bindingNavigator = (BindingNavigator)base.Component;
			IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
			try
			{
				ToolStripDesigner._autoAddNewItems = false;
				bindingNavigator.SuspendLayout();
				bindingNavigator.AddStandardItems();
				this.SiteItems(designerHost, bindingNavigator.Items);
				this.RaiseItemsChanged();
				bindingNavigator.ResumeLayout();
				bindingNavigator.ShowItemToolTips = true;
			}
			finally
			{
				ToolStripDesigner._autoAddNewItems = true;
			}
		}

		private void RaiseItemsChanged()
		{
			BindingNavigator bindingNavigator = (BindingNavigator)base.Component;
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				MemberDescriptor memberDescriptor = TypeDescriptor.GetProperties(bindingNavigator)["Items"];
				componentChangeService.OnComponentChanging(bindingNavigator, memberDescriptor);
				componentChangeService.OnComponentChanged(bindingNavigator, memberDescriptor, null, null);
				foreach (string text in BindingNavigatorDesigner.itemNames)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(bindingNavigator)[text];
					if (propertyDescriptor != null)
					{
						componentChangeService.OnComponentChanging(bindingNavigator, propertyDescriptor);
						componentChangeService.OnComponentChanged(bindingNavigator, propertyDescriptor, null, null);
					}
				}
			}
		}

		private void SiteItem(IDesignerHost host, ToolStripItem item)
		{
			if (item is DesignerToolStripControlHost)
			{
				return;
			}
			host.Container.Add(item, DesignerUtils.GetUniqueSiteName(host, item.Name));
			item.Name = item.Site.Name;
			ToolStripDropDownItem toolStripDropDownItem = item as ToolStripDropDownItem;
			if (toolStripDropDownItem != null && toolStripDropDownItem.HasDropDownItems)
			{
				this.SiteItems(host, toolStripDropDownItem.DropDownItems);
			}
		}

		private void SiteItems(IDesignerHost host, ToolStripItemCollection items)
		{
			foreach (object obj in items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				this.SiteItem(host, toolStripItem);
			}
		}

		private void ComponentChangeSvc_ComponentRemoved(object sender, ComponentEventArgs e)
		{
			ToolStripItem toolStripItem = e.Component as ToolStripItem;
			if (toolStripItem != null)
			{
				BindingNavigator bindingNavigator = (BindingNavigator)base.Component;
				if (toolStripItem == bindingNavigator.MoveFirstItem)
				{
					bindingNavigator.MoveFirstItem = null;
					return;
				}
				if (toolStripItem == bindingNavigator.MovePreviousItem)
				{
					bindingNavigator.MovePreviousItem = null;
					return;
				}
				if (toolStripItem == bindingNavigator.MoveNextItem)
				{
					bindingNavigator.MoveNextItem = null;
					return;
				}
				if (toolStripItem == bindingNavigator.MoveLastItem)
				{
					bindingNavigator.MoveLastItem = null;
					return;
				}
				if (toolStripItem == bindingNavigator.PositionItem)
				{
					bindingNavigator.PositionItem = null;
					return;
				}
				if (toolStripItem == bindingNavigator.CountItem)
				{
					bindingNavigator.CountItem = null;
					return;
				}
				if (toolStripItem == bindingNavigator.AddNewItem)
				{
					bindingNavigator.AddNewItem = null;
					return;
				}
				if (toolStripItem == bindingNavigator.DeleteItem)
				{
					bindingNavigator.DeleteItem = null;
				}
			}
		}

		private void ComponentChangeSvc_ComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			BindingNavigator bindingNavigator = (BindingNavigator)base.Component;
			if (e.Component != null && e.Component == bindingNavigator.CountItem && e.Member != null && e.Member.Name == "Text")
			{
				bindingNavigator.CountItemFormat = bindingNavigator.CountItem.Text;
			}
		}

		private static string[] itemNames = new string[] { "MovePreviousItem", "MoveFirstItem", "MoveNextItem", "MoveLastItem", "AddNewItem", "DeleteItem", "PositionItem", "CountItem" };
	}
}
