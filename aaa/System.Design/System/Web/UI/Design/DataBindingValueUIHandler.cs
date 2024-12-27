using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000348 RID: 840
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataBindingValueUIHandler
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000B5437 File Offset: 0x000B4437
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

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001FAC RID: 8108 RVA: 0x000B546C File Offset: 0x000B446C
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

		// Token: 0x06001FAD RID: 8109 RVA: 0x000B548C File Offset: 0x000B448C
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

		// Token: 0x06001FAE RID: 8110 RVA: 0x000B54D4 File Offset: 0x000B44D4
		private void OnValueUIItemInvoke(ITypeDescriptorContext context, PropertyDescriptor propDesc, PropertyValueUIItem invokedItem)
		{
		}

		// Token: 0x040017BE RID: 6078
		private Bitmap dataBindingBitmap;

		// Token: 0x040017BF RID: 6079
		private string dataBindingToolTip;

		// Token: 0x02000349 RID: 841
		private class DataBindingUIItem : PropertyValueUIItem
		{
			// Token: 0x06001FB0 RID: 8112 RVA: 0x000B54DE File Offset: 0x000B44DE
			public DataBindingUIItem(DataBindingValueUIHandler handler)
				: base(handler.DataBindingBitmap, new PropertyValueUIItemInvokeHandler(handler.OnValueUIItemInvoke), handler.DataBindingToolTip)
			{
			}
		}
	}
}
