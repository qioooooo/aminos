using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001AA RID: 426
	internal class CollectionEditVerbManager : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x0600104F RID: 4175 RVA: 0x0004A8A4 File Offset: 0x000498A4
		internal CollectionEditVerbManager(string text, ComponentDesigner designer, PropertyDescriptor prop, bool addToDesignerVerbs)
		{
			this._designer = designer;
			this._targetProperty = prop;
			if (prop == null)
			{
				prop = TypeDescriptor.GetDefaultProperty(designer.Component);
				if (prop != null && typeof(ICollection).IsAssignableFrom(prop.PropertyType))
				{
					this._targetProperty = prop;
				}
			}
			if (text == null)
			{
				text = SR.GetString("ToolStripItemCollectionEditorVerb");
			}
			this._editItemsVerb = new DesignerVerb(text, new EventHandler(this.OnEditItems));
			if (addToDesignerVerbs)
			{
				this._designer.Verbs.Add(this._editItemsVerb);
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x0004A938 File Offset: 0x00049938
		private IComponentChangeService ChangeService
		{
			get
			{
				if (this._componentChangeSvc == null)
				{
					this._componentChangeSvc = (IComponentChangeService)((IServiceProvider)this).GetService(typeof(IComponentChangeService));
				}
				return this._componentChangeSvc;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x0004A963 File Offset: 0x00049963
		IContainer ITypeDescriptorContext.Container
		{
			get
			{
				if (this._designer.Component.Site != null)
				{
					return this._designer.Component.Site.Container;
				}
				return null;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x0004A98E File Offset: 0x0004998E
		public DesignerVerb EditItemsVerb
		{
			get
			{
				return this._editItemsVerb;
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0004A996 File Offset: 0x00049996
		void ITypeDescriptorContext.OnComponentChanged()
		{
			this.ChangeService.OnComponentChanged(this._designer.Component, this._targetProperty, null, null);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0004A9B8 File Offset: 0x000499B8
		bool ITypeDescriptorContext.OnComponentChanging()
		{
			try
			{
				this.ChangeService.OnComponentChanging(this._designer.Component, this._targetProperty);
			}
			catch (CheckoutException ex)
			{
				if (ex == CheckoutException.Canceled)
				{
					return false;
				}
				throw;
			}
			return true;
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x0004AA08 File Offset: 0x00049A08
		object ITypeDescriptorContext.Instance
		{
			get
			{
				return this._designer.Component;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0004AA15 File Offset: 0x00049A15
		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return this._targetProperty;
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0004AA20 File Offset: 0x00049A20
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == typeof(ITypeDescriptorContext) || serviceType == typeof(IWindowsFormsEditorService))
			{
				return this;
			}
			if (this._designer.Component.Site != null)
			{
				return this._designer.Component.Site.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0004AA73 File Offset: 0x00049A73
		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0004AA75 File Offset: 0x00049A75
		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0004AA78 File Offset: 0x00049A78
		DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
		{
			IUIService iuiservice = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				return iuiservice.ShowDialog(dialog);
			}
			return dialog.ShowDialog(this._designer.Component as IWin32Window);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0004AABC File Offset: 0x00049ABC
		private void OnEditItems(object sender, EventArgs e)
		{
			DesignerActionUIService designerActionUIService = (DesignerActionUIService)((IServiceProvider)this).GetService(typeof(DesignerActionUIService));
			if (designerActionUIService != null)
			{
				designerActionUIService.HideUI(this._designer.Component);
			}
			object value = this._targetProperty.GetValue(this._designer.Component);
			if (value == null)
			{
				return;
			}
			CollectionEditor collectionEditor = TypeDescriptor.GetEditor(value, typeof(UITypeEditor)) as CollectionEditor;
			if (collectionEditor != null)
			{
				collectionEditor.EditValue(this, this, value);
			}
		}

		// Token: 0x04001058 RID: 4184
		private ComponentDesigner _designer;

		// Token: 0x04001059 RID: 4185
		private IComponentChangeService _componentChangeSvc;

		// Token: 0x0400105A RID: 4186
		private PropertyDescriptor _targetProperty;

		// Token: 0x0400105B RID: 4187
		private DesignerVerb _editItemsVerb;
	}
}
