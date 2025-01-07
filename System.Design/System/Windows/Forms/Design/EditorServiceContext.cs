using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class EditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
	{
		internal EditorServiceContext(ComponentDesigner designer)
		{
			this._designer = designer;
		}

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

		internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText)
			: this(designer, prop)
		{
			this._designer.Verbs.Add(new DesignerVerb(newVerbText, new EventHandler(this.OnEditItems)));
		}

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

		void ITypeDescriptorContext.OnComponentChanged()
		{
			this.ChangeService.OnComponentChanged(this._designer.Component, this._targetProperty, null, null);
		}

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

		object ITypeDescriptorContext.Instance
		{
			get
			{
				return this._designer.Component;
			}
		}

		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return this._targetProperty;
			}
		}

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

		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
		{
			IUIService iuiservice = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
			if (iuiservice != null)
			{
				return iuiservice.ShowDialog(dialog);
			}
			return dialog.ShowDialog(this._designer.Component as IWin32Window);
		}

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

		private ComponentDesigner _designer;

		private IComponentChangeService _componentChangeSvc;

		private PropertyDescriptor _targetProperty;
	}
}
