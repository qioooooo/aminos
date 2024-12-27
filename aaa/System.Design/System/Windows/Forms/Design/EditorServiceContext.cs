using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200022E RID: 558
	internal class EditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x0600154D RID: 5453 RVA: 0x0006F111 File Offset: 0x0006E111
		internal EditorServiceContext(ComponentDesigner designer)
		{
			this._designer = designer;
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0006F120 File Offset: 0x0006E120
		internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
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
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0006F172 File Offset: 0x0006E172
		internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText)
			: this(designer, prop)
		{
			this._designer.Verbs.Add(new DesignerVerb(newVerbText, new EventHandler(this.OnEditItems)));
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0006F1A0 File Offset: 0x0006E1A0
		public static object EditValue(ComponentDesigner designer, object objectToChange, string propName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(objectToChange)[propName];
			EditorServiceContext editorServiceContext = new EditorServiceContext(designer, propertyDescriptor);
			UITypeEditor uitypeEditor = propertyDescriptor.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
			object value = propertyDescriptor.GetValue(objectToChange);
			object obj = uitypeEditor.EditValue(editorServiceContext, editorServiceContext, value);
			if (obj != value)
			{
				try
				{
					propertyDescriptor.SetValue(objectToChange, obj);
				}
				catch (CheckoutException)
				{
				}
			}
			return obj;
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x0006F210 File Offset: 0x0006E210
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

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0006F23B File Offset: 0x0006E23B
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

		// Token: 0x06001553 RID: 5459 RVA: 0x0006F266 File Offset: 0x0006E266
		void ITypeDescriptorContext.OnComponentChanged()
		{
			this.ChangeService.OnComponentChanged(this._designer.Component, this._targetProperty, null, null);
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0006F288 File Offset: 0x0006E288
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

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001555 RID: 5461 RVA: 0x0006F2D8 File Offset: 0x0006E2D8
		object ITypeDescriptorContext.Instance
		{
			get
			{
				return this._designer.Component;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0006F2E5 File Offset: 0x0006E2E5
		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return this._targetProperty;
			}
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0006F2F0 File Offset: 0x0006E2F0
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

		// Token: 0x06001558 RID: 5464 RVA: 0x0006F343 File Offset: 0x0006E343
		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0006F345 File Offset: 0x0006E345
		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0006F348 File Offset: 0x0006E348
		DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
		{
			IUIService iuiservice = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				return iuiservice.ShowDialog(dialog);
			}
			return dialog.ShowDialog(this._designer.Component as IWin32Window);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0006F38C File Offset: 0x0006E38C
		private void OnEditItems(object sender, EventArgs e)
		{
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

		// Token: 0x0400127E RID: 4734
		private ComponentDesigner _designer;

		// Token: 0x0400127F RID: 4735
		private IComponentChangeService _componentChangeSvc;

		// Token: 0x04001280 RID: 4736
		private PropertyDescriptor _targetProperty;
	}
}
