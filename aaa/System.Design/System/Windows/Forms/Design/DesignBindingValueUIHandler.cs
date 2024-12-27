using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200020E RID: 526
	internal class DesignBindingValueUIHandler : IDisposable
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x00064F94 File Offset: 0x00063F94
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

		// Token: 0x060013D6 RID: 5078 RVA: 0x00064FCC File Offset: 0x00063FCC
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

		// Token: 0x060013D7 RID: 5079 RVA: 0x00065084 File Offset: 0x00064084
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

		// Token: 0x060013D8 RID: 5080 RVA: 0x000650DF File Offset: 0x000640DF
		public void Dispose()
		{
			if (this.dataBitmap != null)
			{
				this.dataBitmap.Dispose();
			}
		}

		// Token: 0x040011CF RID: 4559
		private Bitmap dataBitmap;

		// Token: 0x0200020F RID: 527
		private class LocalUIItem : PropertyValueUIItem
		{
			// Token: 0x060013DA RID: 5082 RVA: 0x000650FC File Offset: 0x000640FC
			internal LocalUIItem(DesignBindingValueUIHandler handler, Binding binding)
				: base(handler.DataBitmap, new PropertyValueUIItemInvokeHandler(handler.OnPropertyValueUIItemInvoke), DesignBindingValueUIHandler.LocalUIItem.GetToolTip(binding))
			{
				this.binding = binding;
			}

			// Token: 0x17000336 RID: 822
			// (get) Token: 0x060013DB RID: 5083 RVA: 0x00065123 File Offset: 0x00064123
			internal Binding Binding
			{
				get
				{
					return this.binding;
				}
			}

			// Token: 0x060013DC RID: 5084 RVA: 0x0006512C File Offset: 0x0006412C
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

			// Token: 0x040011D0 RID: 4560
			private Binding binding;
		}
	}
}
