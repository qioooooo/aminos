using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200025E RID: 606
	internal class ListBoxDesigner : ControlDesigner
	{
		// Token: 0x060016FD RID: 5885 RVA: 0x0007685C File Offset: 0x0007585C
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

		// Token: 0x060016FE RID: 5886 RVA: 0x000768B0 File Offset: 0x000758B0
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

		// Token: 0x060016FF RID: 5887 RVA: 0x00076908 File Offset: 0x00075908
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

		// Token: 0x06001700 RID: 5888 RVA: 0x0007695D File Offset: 0x0007595D
		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			if (e.Component == base.Component)
			{
				this.UpdateControlName(e.NewName);
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x0007697C File Offset: 0x0007597C
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

		// Token: 0x06001702 RID: 5890 RVA: 0x000769E8 File Offset: 0x000759E8
		protected override void OnCreateHandle()
		{
			base.OnCreateHandle();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Name"];
			if (propertyDescriptor != null)
			{
				this.UpdateControlName(propertyDescriptor.GetValue(base.Component).ToString());
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00076A2C File Offset: 0x00075A2C
		private void UpdateControlName(string name)
		{
			ListBox listBox = (ListBox)this.Control;
			if (listBox.IsHandleCreated && listBox.Items.Count == 0)
			{
				NativeMethods.SendMessage(listBox.Handle, 388, 0, 0);
				NativeMethods.SendMessage(listBox.Handle, 384, 0, name);
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001704 RID: 5892 RVA: 0x00076A80 File Offset: 0x00075A80
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

		// Token: 0x04001312 RID: 4882
		private DesignerActionListCollection _actionLists;
	}
}
