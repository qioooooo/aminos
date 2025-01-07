using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class DesignBindingValueUIHandler : IDisposable
	{
		internal Bitmap DataBitmap
		{
			get
			{
				if (this.dataBitmap == null)
				{
					this.dataBitmap = new Bitmap(typeof(DesignBindingValueUIHandler), "BoundProperty.bmp");
					this.dataBitmap.MakeTransparent();
				}
				return this.dataBitmap;
			}
		}

		internal void OnGetUIValueItem(ITypeDescriptorContext context, PropertyDescriptor propDesc, ArrayList valueUIItemList)
		{
			if (context.Instance is Control)
			{
				Control control = (Control)context.Instance;
				foreach (object obj in control.DataBindings)
				{
					Binding binding = (Binding)obj;
					if ((binding.DataSource is IListSource || binding.DataSource is IList || binding.DataSource is Array) && binding.PropertyName.Equals(propDesc.Name))
					{
						valueUIItemList.Add(new DesignBindingValueUIHandler.LocalUIItem(this, binding));
					}
				}
			}
		}

		private void OnPropertyValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor descriptor, PropertyValueUIItem invokedItem)
		{
			DesignBindingValueUIHandler.LocalUIItem localUIItem = (DesignBindingValueUIHandler.LocalUIItem)invokedItem;
			IServiceProvider serviceProvider = null;
			Control control = localUIItem.Binding.Control;
			if (control.Site != null)
			{
				serviceProvider = (IServiceProvider)control.Site.GetService(typeof(IServiceProvider));
			}
			if (serviceProvider != null)
			{
				AdvancedBindingPropertyDescriptor.advancedBindingEditor.EditValue(context, serviceProvider, control.DataBindings);
			}
		}

		public void Dispose()
		{
			if (this.dataBitmap != null)
			{
				this.dataBitmap.Dispose();
			}
		}

		private Bitmap dataBitmap;

		private class LocalUIItem : PropertyValueUIItem
		{
			internal LocalUIItem(DesignBindingValueUIHandler handler, Binding binding)
				: base(handler.DataBitmap, new PropertyValueUIItemInvokeHandler(handler.OnPropertyValueUIItemInvoke), DesignBindingValueUIHandler.LocalUIItem.GetToolTip(binding))
			{
				this.binding = binding;
			}

			internal Binding Binding
			{
				get
				{
					return this.binding;
				}
			}

			private static string GetToolTip(Binding binding)
			{
				string text = "";
				if (binding.DataSource is IComponent)
				{
					IComponent component = (IComponent)binding.DataSource;
					if (component.Site != null)
					{
						text = component.Site.Name;
					}
				}
				if (text.Length == 0)
				{
					text = "(List)";
				}
				return text + " - " + binding.BindingMemberInfo.BindingMember;
			}

			private Binding binding;
		}
	}
}
