using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataBindingValueUIHandler
	{
		private Bitmap DataBindingBitmap
		{
			get
			{
				if (this.dataBindingBitmap == null)
				{
					this.dataBindingBitmap = new Bitmap(typeof(DataBindingValueUIHandler), "DataBindingGlyph.bmp");
					this.dataBindingBitmap.MakeTransparent();
				}
				return this.dataBindingBitmap;
			}
		}

		private string DataBindingToolTip
		{
			get
			{
				if (this.dataBindingToolTip == null)
				{
					this.dataBindingToolTip = SR.GetString("DataBindingGlyph_ToolTip");
				}
				return this.dataBindingToolTip;
			}
		}

		public void OnGetUIValueItem(ITypeDescriptorContext context, PropertyDescriptor propDesc, ArrayList valueUIItemList)
		{
			Control control = context.Instance as Control;
			if (control != null)
			{
				IDataBindingsAccessor dataBindingsAccessor = control;
				if (dataBindingsAccessor.HasDataBindings)
				{
					DataBinding dataBinding = dataBindingsAccessor.DataBindings[propDesc.Name];
					if (dataBinding != null)
					{
						valueUIItemList.Add(new DataBindingValueUIHandler.DataBindingUIItem(this));
					}
				}
			}
		}

		private void OnValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor propDesc, PropertyValueUIItem invokedItem)
		{
		}

		private Bitmap dataBindingBitmap;

		private string dataBindingToolTip;

		private class DataBindingUIItem : PropertyValueUIItem
		{
			public DataBindingUIItem(DataBindingValueUIHandler handler)
				: base(handler.DataBindingBitmap, new PropertyValueUIItemInvokeHandler(handler.OnValueUIItemInvoke), handler.DataBindingToolTip)
			{
			}
		}
	}
}
