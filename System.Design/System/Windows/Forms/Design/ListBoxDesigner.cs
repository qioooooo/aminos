using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class ListBoxDesigner : ControlDesigner
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRename -= this.OnComponentRename;
					componentChangeService.ComponentChanged -= this.OnComponentChanged;
				}
			}
			base.Dispose(disposing);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRename += this.OnComponentRename;
				componentChangeService.ComponentChanged += this.OnComponentChanged;
			}
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			((ListBox)base.Component).FormattingEnabled = true;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Name"];
			if (propertyDescriptor != null)
			{
				this.UpdateControlName(propertyDescriptor.GetValue(base.Component).ToString());
			}
		}

		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			if (e.Component == base.Component)
			{
				this.UpdateControlName(e.NewName);
			}
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Component == base.Component && e.Member != null && e.Member.Name == "Items")
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Name"];
				if (propertyDescriptor != null)
				{
					this.UpdateControlName(propertyDescriptor.GetValue(base.Component).ToString());
				}
			}
		}

		protected override void OnCreateHandle()
		{
			base.OnCreateHandle();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Name"];
			if (propertyDescriptor != null)
			{
				this.UpdateControlName(propertyDescriptor.GetValue(base.Component).ToString());
			}
		}

		private void UpdateControlName(string name)
		{
			ListBox listBox = (ListBox)this.Control;
			if (listBox.IsHandleCreated && listBox.Items.Count == 0)
			{
				NativeMethods.SendMessage(listBox.Handle, 388, 0, 0);
				NativeMethods.SendMessage(listBox.Handle, 384, 0, name);
			}
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					if (base.Component is CheckedListBox)
					{
						this._actionLists.Add(new ListControlUnboundActionList(this));
					}
					else
					{
						this._actionLists.Add(new ListControlBoundActionList(this));
					}
				}
				return this._actionLists;
			}
		}

		private DesignerActionListCollection _actionLists;
	}
}
