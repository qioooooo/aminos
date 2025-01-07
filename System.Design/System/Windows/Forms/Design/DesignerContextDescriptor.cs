using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	internal class DesignerContextDescriptor : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
	{
		public DesignerContextDescriptor(Component component, PropertyDescriptor imageProperty, IDesignerHost host)
		{
			this._component = component;
			this._propertyDescriptor = imageProperty;
			this._host = host;
		}

		public Image OpenImageCollection()
		{
			object value = this._propertyDescriptor.GetValue(this._component);
			if (this._propertyDescriptor != null)
			{
				Image image = null;
				UITypeEditor uitypeEditor = this._propertyDescriptor.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
				if (uitypeEditor != null)
				{
					image = (Image)uitypeEditor.EditValue(this, this, value);
				}
				if (image != null)
				{
					return image;
				}
			}
			return (Image)value;
		}

		IContainer ITypeDescriptorContext.Container
		{
			get
			{
				return null;
			}
		}

		object ITypeDescriptorContext.Instance
		{
			get
			{
				return this._component;
			}
		}

		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return this._propertyDescriptor;
			}
		}

		void ITypeDescriptorContext.OnComponentChanged()
		{
		}

		bool ITypeDescriptorContext.OnComponentChanging()
		{
			return false;
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == typeof(IWindowsFormsEditorService))
			{
				return this;
			}
			return this._host.GetService(serviceType);
		}

		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
		{
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			IUIService iuiservice = (IUIService)((IServiceProvider)this).GetService(typeof(IUIService));
			DialogResult dialogResult;
			if (iuiservice != null)
			{
				dialogResult = iuiservice.ShowDialog(dialog);
			}
			else
			{
				dialogResult = dialog.ShowDialog(this._component as IWin32Window);
			}
			if (focus != IntPtr.Zero)
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(null, focus));
			}
			return dialogResult;
		}

		private Component _component;

		private PropertyDescriptor _propertyDescriptor;

		private IDesignerHost _host;
	}
}
