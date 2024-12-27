using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000215 RID: 533
	internal class DesignerContextDescriptor : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x06001416 RID: 5142 RVA: 0x00066696 File Offset: 0x00065696
		public DesignerContextDescriptor(Component component, PropertyDescriptor imageProperty, IDesignerHost host)
		{
			this._component = component;
			this._propertyDescriptor = imageProperty;
			this._host = host;
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x000666B4 File Offset: 0x000656B4
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

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x00066715 File Offset: 0x00065715
		IContainer ITypeDescriptorContext.Container
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x00066718 File Offset: 0x00065718
		object ITypeDescriptorContext.Instance
		{
			get
			{
				return this._component;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x00066720 File Offset: 0x00065720
		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return this._propertyDescriptor;
			}
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00066728 File Offset: 0x00065728
		void ITypeDescriptorContext.OnComponentChanged()
		{
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0006672A File Offset: 0x0006572A
		bool ITypeDescriptorContext.OnComponentChanging()
		{
			return false;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0006672D File Offset: 0x0006572D
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == typeof(IWindowsFormsEditorService))
			{
				return this;
			}
			return this._host.GetService(serviceType);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0006674A File Offset: 0x0006574A
		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0006674C File Offset: 0x0006574C
		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00066750 File Offset: 0x00065750
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

		// Token: 0x040011ED RID: 4589
		private Component _component;

		// Token: 0x040011EE RID: 4590
		private PropertyDescriptor _propertyDescriptor;

		// Token: 0x040011EF RID: 4591
		private IDesignerHost _host;
	}
}
