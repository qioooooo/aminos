using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001A4 RID: 420
	internal class BindingNavigatorDesigner : ToolStripDesigner
	{
		// Token: 0x06001021 RID: 4129 RVA: 0x000492F8 File Offset: 0x000482F8
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

		// Token: 0x06001022 RID: 4130 RVA: 0x0004934C File Offset: 0x0004834C
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

		// Token: 0x06001023 RID: 4131 RVA: 0x000493A0 File Offset: 0x000483A0
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

		// Token: 0x06001024 RID: 4132 RVA: 0x0004942C File Offset: 0x0004842C
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

		// Token: 0x06001025 RID: 4133 RVA: 0x000494C8 File Offset: 0x000484C8
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

		// Token: 0x06001026 RID: 4134 RVA: 0x00049528 File Offset: 0x00048528
		private void SiteItems(IDesignerHost host, ToolStripItemCollection items)
		{
			foreach (object obj in items)
			{
				ToolStripItem toolStripItem = (ToolStripItem)obj;
				this.SiteItem(host, toolStripItem);
			}
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00049580 File Offset: 0x00048580
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

		// Token: 0x06001028 RID: 4136 RVA: 0x00049634 File Offset: 0x00048634
		private void ComponentChangeSvc_ComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			BindingNavigator bindingNavigator = (BindingNavigator)base.Component;
			if (e.Component != null && e.Component == bindingNavigator.CountItem && e.Member != null && e.Member.Name == "Text")
			{
				bindingNavigator.CountItemFormat = bindingNavigator.CountItem.Text;
			}
		}

		// Token: 0x04001042 RID: 4162
		private static string[] itemNames = new string[] { "MovePreviousItem", "MoveFirstItem", "MoveNextItem", "MoveLastItem", "AddNewItem", "DeleteItem", "PositionItem", "CountItem" };
	}
}
